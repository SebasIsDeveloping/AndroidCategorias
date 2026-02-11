using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace InteractiveApp.Services;

public class ImageUploadService
{
    private readonly HttpClient _http = new();

    private const string WebhookUrl =
        "http://192.168.88.141/webhook/upload";

    public async Task<string> SendImageAsync(string path)
    {
        using var form = new MultipartFormDataContent();

        var bytes = await File.ReadAllBytesAsync(path);

        var fileContent = new ByteArrayContent(bytes);
        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue("image/jpeg");

        form.Add(fileContent, "file", Path.GetFileName(path));

        var response = await _http.PostAsync(WebhookUrl, form);

        var result = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();

        return result;
    }
}