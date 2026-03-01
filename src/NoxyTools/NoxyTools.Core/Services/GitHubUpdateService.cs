using NoxyTools.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NoxyTools.Core.Services;

/// <summary>
/// GitHub Releases API를 사용하여 버전 확인 및 업데이트 파일 다운로드를 담당하는 서비스.
/// NoxyTools.Core에 위치하여 WPF/WinForms 프로젝트 모두에서 공유 가능.
/// </summary>
public class GitHubUpdateService : IDisposable
{
    private const string GITHUB_API_BASE = "https://api.github.com";
    private const string USER_AGENT      = "NoxyTools-Updater/1.0";

    private readonly string _repoOwner;
    private readonly string _repoName;
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <param name="repoOwner">GitHub 리포지토리 소유자 (예: "MyAccount")</param>
    /// <param name="repoName">리포지토리 이름 (예: "NoxyTools")</param>
    /// <param name="githubToken">Private 리포 접근 시 Personal Access Token (선택)</param>
    public GitHubUpdateService(string repoOwner, string repoName, string? githubToken = null)
    {
        _repoOwner = repoOwner;
        _repoName  = repoName;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(USER_AGENT);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        _httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

        if (!string.IsNullOrWhiteSpace(githubToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", githubToken);
        }
    }

    // ─────────────────────────────────────────────────────────────
    // 버전 확인
    // ─────────────────────────────────────────────────────────────

    /// <summary>
    /// GitHub 최신 릴리즈 정보를 조회합니다.
        /// 네트워크 오류, 404(릴리즈 없음), 403/429(Rate Limit) 시 null을 반환합니다.
        /// 오류 원인은 <paramref name="errorReason"/>으로 전달됩니다.
        /// </summary>
        public async Task<GitHubRelease?> GetLatestReleaseAsync(
            CancellationToken ct = default,
            Action<string>? onError = null)
        {
            var url = $"{GITHUB_API_BASE}/repos/{_repoOwner}/{_repoName}/releases/latest";
            try
            {
                var response = await _httpClient.GetAsync(url, ct);

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                    (int)response.StatusCode == 429)
                {
                    // GitHub API Rate Limit
                    var resetHeader = response.Headers.Contains("X-RateLimit-Reset")
                        ? response.Headers.GetValues("X-RateLimit-Reset").FirstOrDefault()
                        : null;
                    string resetMsg = resetHeader != null &&
                        long.TryParse(resetHeader, out var resetEpoch)
                            ? $" (초기화: {DateTimeOffset.FromUnixTimeSeconds(resetEpoch):HH:mm:ss})"
                            : string.Empty;
                    onError?.Invoke($"GitHub API 요청 한도 초과{resetMsg}. 잠시 후 다시 시도하세요.");
                    return null;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    onError?.Invoke("릴리즈 정보를 찾을 수 없습니다.");
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    onError?.Invoke($"GitHub API 오류: {(int)response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync(ct);
                return JsonSerializer.Deserialize<GitHubRelease>(json, _jsonOptions);
            }
            catch (HttpRequestException)
            {
                onError?.Invoke("네트워크에 연결할 수 없습니다. 인터넷 연결을 확인하세요.");
                return null;
            }
            catch (TaskCanceledException) when (!ct.IsCancellationRequested)
            {
                onError?.Invoke("요청 시간이 초과되었습니다.");
                return null;
            }
            catch (TaskCanceledException)
            {
                return null;  // 사용자가 취소한 경우
            }
            catch (JsonException)
            {
                onError?.Invoke("버전 정보 형식이 올바르지 않습니다.");
                return null;
            }
        }

    // ─────────────────────────────────────────────────────────────
    // 버전 비교
    // ─────────────────────────────────────────────────────────────

    /// <summary>현재 버전보다 릴리즈가 최신인지 확인합니다.</summary>
    public bool IsUpdateAvailable(Version currentVersion, GitHubRelease release)
    {
        if (release == null) return false;
        return Version.TryParse(release.VersionString, out var latest) && latest > currentVersion;
    }

    // ─────────────────────────────────────────────────────────────
    // 다운로드 (재시도 + 이어받기)
    // ─────────────────────────────────────────────────────────────

    /// <summary>
    /// GitHub Release Asset을 재시도(최대 <paramref name="maxRetries"/>회) 및
    /// 이어받기(Range 헤더)를 지원하여 다운로드합니다.
    /// </summary>
    public async Task DownloadAssetAsync(
        string downloadUrl,
        string destPath,
        IProgress<int>? progress = null,
        int maxRetries = 3,
        CancellationToken ct = default)
    {
        var dir = Path.GetDirectoryName(destPath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        var tempPath = destPath + ".part";

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                long resumeOffset = File.Exists(tempPath) ? new FileInfo(tempPath).Length : 0;
                var request = new HttpRequestMessage(HttpMethod.Get, downloadUrl);
                if (resumeOffset > 0)
                    request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(resumeOffset, null);

                using var response = await _httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead, ct);

                // 416: .part 파일 offset이 서버 파일 크기를 초과 → 처음부터 재시도
                if (response.StatusCode == System.Net.HttpStatusCode.RequestedRangeNotSatisfiable)
                {
                    if (File.Exists(tempPath)) File.Delete(tempPath);
                    resumeOffset = 0;
                    var retryRequest = new HttpRequestMessage(HttpMethod.Get, downloadUrl);
                    using var retryResponse = await _httpClient.SendAsync(
                        retryRequest, HttpCompletionOption.ResponseHeadersRead, ct);
                    retryResponse.EnsureSuccessStatusCode();
                    await WriteStreamAsync(retryResponse, tempPath, destPath, 0, progress, ct);
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    resumeOffset = 0;  // 서버가 전체 재전송 (Range 미지원)

                response.EnsureSuccessStatusCode();
                await WriteStreamAsync(response, tempPath, destPath, resumeOffset, progress, ct);
                return;  // 성공
            }
            catch (OperationCanceledException) { throw; }
            catch when (attempt < maxRetries)
            {
                // 지수 백오프: 2s / 4s …
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), ct);
            }
        }

        // 마지막 시도 – 예외 전파
        if (File.Exists(tempPath)) File.Delete(tempPath);  // 손상된 .part 정리 후 처음부터
        var lastReq = new HttpRequestMessage(HttpMethod.Get, downloadUrl);
        using var lastResp = await _httpClient.SendAsync(
            lastReq, HttpCompletionOption.ResponseHeadersRead, ct);
        lastResp.EnsureSuccessStatusCode();
        await WriteStreamAsync(lastResp, tempPath, destPath, 0, progress, ct);
    }

    private static async Task WriteStreamAsync(
        HttpResponseMessage response,
        string tempPath,
        string destPath,
        long resumeOffset,
        IProgress<int>? progress,
        CancellationToken ct)
    {
        var totalBytes = (response.Content.Headers.ContentLength ?? -1L) + resumeOffset;

        using var srcStream  = await response.Content.ReadAsStreamAsync(ct);
        using var destStream = new FileStream(tempPath,
            resumeOffset > 0 ? FileMode.Append : FileMode.Create,
            FileAccess.Write, FileShare.None, 81920, true);

        var buffer = new byte[81920];
        long bytesRead = resumeOffset;
        int read;

        while ((read = await srcStream.ReadAsync(buffer, ct)) > 0)
        {
            await destStream.WriteAsync(buffer.AsMemory(0, read), ct);
            bytesRead += read;
            if (totalBytes > 0)
                progress?.Report((int)(bytesRead * 100L / totalBytes));
        }

        progress?.Report(100);
        await destStream.FlushAsync(ct);
        destStream.Close();

        if (File.Exists(destPath)) File.Delete(destPath);
        File.Move(tempPath, destPath);
    }

    // ─────────────────────────────────────────────────────────────
    // 유틸리티
    // ─────────────────────────────────────────────────────────────

    /// <summary>릴리즈에서 특정 이름의 Asset을 찾아 반환합니다.</summary>
    public static GitHubReleaseAsset? FindAsset(GitHubRelease release, string assetName)
    {
        return release?.Assets?.Find(a =>
            string.Equals(a.Name, assetName, StringComparison.OrdinalIgnoreCase));
    }

    public void Dispose() => _httpClient.Dispose();
}
