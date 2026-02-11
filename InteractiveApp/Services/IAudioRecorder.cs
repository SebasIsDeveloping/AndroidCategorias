using System.Threading.Tasks;

namespace InteractiveApp.Services;

public interface IAudioRecorder
{
    Task<byte[]> RecordAsync(int seconds);
}
