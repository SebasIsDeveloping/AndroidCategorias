using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Avalonia;
using Avalonia.Android;
using InteractiveApp.Android.Resources;
using InteractiveApp.Services;

namespace InteractiveApp.Android;

[Activity(
    Label = "InteractiveApp.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }
    
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        AppServices.AudioPlayer = new AndroidAudioPlayer();
        AppServices.MicrophonePermission =
            new AndroidMicrophonePermissionService(this);
        AppServices.SttService =
            new AndroidSystemSttService(this);
        AppServices.AudioRecorder = new AndroidAudioRecorder();
        AppServices.ImageUpload = new ImageUploadService();
        AppServices.WhisperService = new WhisperService();
    }
    
    // protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    // {
    //     base.OnActivityResult(requestCode, resultCode, data);
    //
    //     if (requestCode == 1234)
    //     {
    //         var camera = AppServices.Camera as AndroidCameraService;
    //         camera?.OnResult(resultCode == Result.Ok);
    //     }else if (AppServices.SttService is AndroidSystemSttService sys)
    //     {
    //         sys.OnActivityResult(requestCode, resultCode, data);
    //     }
    // }
    
    public override void OnRequestPermissionsResult(
        int requestCode,
        string[] permissions,
        Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        if (AppServices.MicrophonePermission
            is AndroidMicrophonePermissionService mic)
        {
            mic.OnRequestPermissionsResult(requestCode, grantResults);
        }
    }

}