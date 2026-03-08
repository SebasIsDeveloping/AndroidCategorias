using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Speech;
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
    
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
         if (AppServices.SttService is AndroidSystemSttService sys)
        {
            sys.OnActivityResult(requestCode, resultCode, data);
        }
    }
    
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
    
    // protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    // {
    //     base.OnActivityResult(requestCode, resultCode, data);
    //
    //     // 2001 es el número que pusiste en StartActivityForResult
    //     if (requestCode == 2001) 
    //     {
    //         if (resultCode == Result.Ok && data != null)
    //         {
    //             // Extraemos el texto que ha entendido Google
    //             var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
    //             if (matches != null && matches.Count > 0)
    //             {
    //                 string textoReconocido = matches[0];
    //             
    //                 // IMPORTANTE: Aquí le pasamos el texto a tu servicio para que desbloquee el await
    //                 // Sustituye "AppServices.SttService" por la forma en la que accedes a tu servicio normalmente
    //                 AppServices.SttService.CompletarTranscripcion(textoReconocido);
    //                 return;
    //             }
    //         }
    //     
    //         // Si no entendió nada o el usuario canceló
    //         AppServices.SttService.CancelarTranscripcion();
    //     }
    // }

}