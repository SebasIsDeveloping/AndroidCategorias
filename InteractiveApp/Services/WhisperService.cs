using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace InteractiveApp.Services;

public class WhisperService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "http://192.168.29.29:9010/inference";

    public WhisperService()
    {
        var handler = new System.Net.Http.HttpClientHandler();

        _httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    public async Task<string> TranscribeAudioAsync(byte[] audio, string language = "es")
    {
        try
        {
            using var form = new MultipartFormDataContent();

            var fileContent = new ByteArrayContent(audio);
            fileContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");

            form.Add(fileContent, "file", "audio.wav");

            form.Add(new StringContent("0.0"), "temperature");
            form.Add(new StringContent(language), "language");
            form.Add(new StringContent("text"), "response_format");

            var response = await _httpClient.PostAsync(ApiUrl, form);

            var raw = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return raw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Whisper error:\n{ex}");
            return ex.ToString(); 
        }
    }


}