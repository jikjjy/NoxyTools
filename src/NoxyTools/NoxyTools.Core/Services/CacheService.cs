using NoxyTools.Core.Model;
using System;
using System.IO;
using Noxypedia;
using Noxypedia.Model;
using Noxypedia.Utils;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace NoxyTools.Core.Services
{
    public class CacheService
    {
        /// <summary>
        /// 개별 이미지가 준비됐을 때 발생합니다. 인자는 해당 이미지의 URL입니다.
        /// 백그라운드 스레드에서 호출될 수 있으므로 구독자는 Dispatcher로 마샬링해야 합니다.
        /// </summary>
        public event EventHandler<string>? ImageReady;
        public NoxypediaSet NoxypediaData { get; private set; }
        private readonly ConcurrentDictionary<string, InternalImageSet> mImageData = new ConcurrentDictionary<string, InternalImageSet>();
        private readonly string mBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Noxy Tools", @"cache");
        private readonly string mClipImagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Noxy Tools", @"cache", @"cilpImage");
        private string mNoxypediaCacheFile => Path.Combine(mBasePath, @"noxypedia_rawdata.cache");

        public void LoadNoxypediaData(ConfigService config, string datFilePath)
        {
            checkDirectory(mBasePath);
            checkDirectory(mClipImagePath);

            // 설치 디렉토리의 noxypedia.dat 버전 확인
            int datVersion = NoxypediaDataFile.ReadVersion(datFilePath);
            bool loadedFromCache = false;

            // 캐시 시도: 버전이 일치하면 캐시 사용
            if (File.Exists(mNoxypediaCacheFile) && datVersion > 0)
            {
                try
                {
                    var (cachedData, cachedVersion) = NoxypediaDataFile.Load(mNoxypediaCacheFile);
                    if (cachedVersion >= datVersion)
                    {
                        NoxypediaData = cachedData;
                        loadedFromCache = true;
                    }
                }
                catch
                {
                    // 손상된/이전 포맷 캐시 삭제 후 .dat에서 다시 로드
                    try { File.Delete(mNoxypediaCacheFile); } catch { }
                }
            }

            // 캐시 미스: noxypedia.dat에서 로드
            if (!loadedFromCache)
            {
                if (!File.Exists(datFilePath))
                {
                    NoxypediaData = new NoxypediaSet();
                    Console.WriteLine($"[CacheService] noxypedia.dat 파일이 없습니다: {datFilePath}");
                    return;
                }

                var (data, version) = NoxypediaDataFile.Load(datFilePath);
                NoxypediaData = data;
                datVersion    = version;

                // 빠른 다음 실행을 위해 캐시 저장 (실패해도 비치명)
                try { NoxypediaDataFile.Save(NoxypediaData, datVersion, mNoxypediaCacheFile); }
                catch (Exception ex) { Console.WriteLine($"[CacheService] 캐시 저장 실패: {ex.Message}"); }
            }

            config.NoxypediaDataVersion = datVersion;

            // URL → 엔트리 등록만 수행 (다운로드는 RequestImage 호출 시에만)
            var imageUrls = NoxypediaData.Items.Select(item => (BaseModel)item)
                .Concat(NoxypediaData.Creeps)
                .Concat(NoxypediaData.Locations)
                .Concat(NoxypediaData.Regions)
                .SelectMany(item => item.ClipImages.Values)
                .Select(item => item.SourceURL)
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .Distinct();

            mImageData.Clear();
            foreach (var url in imageUrls)
            {
                string key = createMD5(url);
                mImageData[key] = new InternalImageSet { URL = url };
            }
        }

        /// <summary>
        /// 이미지를 즉시 반환합니다. 아직 로드되지 않은 경우 null을 반환합니다.
        /// 이미지가 필요하면 <see cref="RequestImage"/>를 먼저 호출하세요.
        /// </summary>
        public BitmapSource? GetImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            string key = createMD5(url);
            if (!mImageData.TryGetValue(key, out var entry)) return null;
            if (!entry.IsLoaded) return null;
            return entry.Image;
        }

        /// <summary>
        /// 지정한 URL의 이미지를 비동기로 요청합니다.
        /// 디스크 캐시 또는 원격에서 로드 완료 시 <see cref="ImageReady"/> 이벤트가 발생합니다.
        /// 이미 로드 중이거나 완료된 경우에는 중복 요청하지 않습니다.
        /// </summary>
        public void RequestImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            string key = createMD5(url);

            // 등록되지 않은 URL이면 새로 등록
            var entry = mImageData.GetOrAdd(key, _ => new InternalImageSet { URL = url });

            // Interlocked CAS: 0→1 교환 성공한 스레드만 작업 시작
            if (Interlocked.CompareExchange(ref entry.IsRequested, 1, 0) != 0) return;

            _ = Task.Run(async () =>
            {
                string fileName = Path.Combine(mClipImagePath, $"{key}.jpg");
                try
                {
                    BitmapSource? bitmap = null;

                    if (File.Exists(fileName))
                    {
                        // 디스크 캐시 히트
                        var bytes = await File.ReadAllBytesAsync(fileName).ConfigureAwait(false);
                        using var ms = new MemoryStream(bytes);
                        bitmap = createBitmapSource(ms);
                    }
                    else
                    {
                        // 원격 다운로드
                        using var ms = new MemoryStream();
                        bool ok = await DownloadRemoteImageUtils
                            .DownloadRemoteImageFileAsync(url, ms)
                            .ConfigureAwait(false);

                        if (ok && ms.Length > 0)
                        {
                            // 디스크 캐시에 저장
                            try
                            {
                                var bytes = ms.ToArray();
                                await File.WriteAllBytesAsync(fileName, bytes).ConfigureAwait(false);
                            }
                            catch { /* 캐시 저장 실패는 비치명 */ }

                            ms.Position = 0;
                            bitmap = createBitmapSource(ms);
                        }
                        else
                        {
                            Console.WriteLine($"[CacheService] 이미지 다운로드 실패. URL={url}");
                        }
                    }

                    if (bitmap != null)
                    {
                        entry.Image    = bitmap;
                        entry.IsLoaded = true;
                        ImageReady?.Invoke(this, url);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CacheService] 이미지 로드 오류. URL={url} / {ex.Message}");
                }
            });
        }

        private static BitmapSource createBitmapSource(MemoryStream ms)
        {
            ms.Position = 0;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            bitmap.Freeze(); // 백그라운드 스레드에서 UI 스레드로 전달 가능하게
            return bitmap;
        }

        private static void checkDirectory(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }

        private static string createMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
