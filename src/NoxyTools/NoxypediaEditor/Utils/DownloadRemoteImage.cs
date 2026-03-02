namespace NoxypediaEditor
{
    public static partial class Utils
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static bool DownloadRemoteImageFile(string url, Stream stream)
        {
            return DownloadRemoteImageFileAsync(url, stream).GetAwaiter().GetResult();
        }

        private static async Task<bool> DownloadRemoteImageFileAsync(string url, Stream stream)
        {
            using var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                return false;

            string contentType = response.Content.Headers.ContentType?.MediaType;
            if (contentType == null || !contentType.StartsWith("image", System.StringComparison.OrdinalIgnoreCase))
                return false;

            await response.Content.CopyToAsync(stream).ConfigureAwait(false);
            return true;
        }
    }
}
