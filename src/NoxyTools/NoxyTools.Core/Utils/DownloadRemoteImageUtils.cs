using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NoxyTools.Core
{
    public static class DownloadRemoteImageUtils
    {
        // 앱 수명 동안 재사용하는 단일 HttpClient 인스턴스
        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = true,
            MaxAutomaticRedirections = 5,
        })
        {
            Timeout = TimeSpan.FromSeconds(30),
        };

        /// <summary>
        /// [동기] 원격 이미지 파일을 다운로드하여 stream에 씁니다. (레거시 호환용)
        /// </summary>
        public static bool DownloadRemoteImageFile(string url, Stream stream)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            bool bImage = response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase);
            if (
                (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Redirect)
                && bImage
                )
            {
                using (Stream inputStream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        stream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// [비동기] 원격 이미지 파일을 다운로드하여 stream에 씁니다.
        /// Content-Type이 "image"로 시작하지 않으면 false를 반환합니다.
        /// </summary>
        public static async Task<bool> DownloadRemoteImageFileAsync(
            string url, Stream stream, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync(url,
                HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode) return false;

            var contentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;
            if (!contentType.StartsWith("image", StringComparison.OrdinalIgnoreCase)) return false;

            await response.Content.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
