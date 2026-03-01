using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NoxyTools.Core.Model;

/// <summary>GitHub Releases API의 최신 릴리즈 응답 모델</summary>
public class GitHubRelease
{
    /// <summary>버전 태그 (예: "v0.2.0")</summary>
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; } = string.Empty;

    /// <summary>릴리즈 제목</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>변경 내역 (Markdown)</summary>
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>게시 일시</summary>
    [JsonPropertyName("published_at")]
    public DateTime PublishedAt { get; set; }

    /// <summary>프리릴리즈 여부</summary>
    [JsonPropertyName("prerelease")]
    public bool Prerelease { get; set; }

    /// <summary>HTML 페이지 URL</summary>
    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; } = string.Empty;

    /// <summary>릴리즈에 첨부된 파일 목록</summary>
    [JsonPropertyName("assets")]
    public List<GitHubReleaseAsset> Assets { get; set; } = new();

    /// <summary>TagName에서 "v" prefix를 제거한 순수 버전 문자열 (예: "0.2.0")</summary>
    public string VersionString => TagName.TrimStart('v');
}

/// <summary>GitHub Release에 첨부된 파일(Asset) 정보</summary>
public class GitHubReleaseAsset
{
    /// <summary>파일명 (예: "NoxyToolsSetup.exe")</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>직접 다운로드 URL</summary>
    [JsonPropertyName("browser_download_url")]
    public string BrowserDownloadUrl { get; set; } = string.Empty;

    /// <summary>파일 크기 (bytes)</summary>
    [JsonPropertyName("size")]
    public long Size { get; set; }

    /// <summary>Content-Type</summary>
    [JsonPropertyName("content_type")]
    public string ContentType { get; set; } = string.Empty;
}
