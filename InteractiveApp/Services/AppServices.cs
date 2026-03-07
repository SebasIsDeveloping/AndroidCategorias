using InteractiveApp.Services;

namespace InteractiveApp.Services;

public class AppServices
{
    public static IAudioRecorder AudioRecorder { get; set; }
    public static ISttService SttService { get; set; }
    public static IMicrophonePermissionService MicrophonePermission { get; set; }
    public static ImageUploadService ImageUpload { get; set; }
    public static WhisperService WhisperService { get; set; }
    public static IAudioPlayer AudioPlayer { get; set; } = null!;
}
