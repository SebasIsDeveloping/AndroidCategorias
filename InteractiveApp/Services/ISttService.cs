using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InteractiveApp.Services;

public interface ISttService
{
    Task<string> TranscribeAsync(byte[] audio);
}
