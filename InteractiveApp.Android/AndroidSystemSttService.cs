using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Speech;
using InteractiveApp.Services;


namespace InteractiveApp.Android;

public class AndroidSystemSttService : ISttService
{
    private readonly Activity _activity;
    private TaskCompletionSource<string>? _tcs;

    public AndroidSystemSttService(Activity activity)
    {
        _activity = activity;
    }

    public Task<string> TranscribeAsync(byte[] pcmAudio)
    {
        _tcs = new TaskCompletionSource<string>();

        var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
        intent.PutExtra(RecognizerIntent.ExtraLanguageModel,
            RecognizerIntent.LanguageModelFreeForm);
        intent.PutExtra(RecognizerIntent.ExtraLanguage, "es-ES");
        intent.PutExtra(RecognizerIntent.ExtraPrompt, "Habla ahora");

        _activity.StartActivityForResult(intent, 2001);

        return _tcs.Task;
    }

    public void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        if (requestCode != 2001)
            return;

        if (resultCode == Result.Ok && data != null)
        {
            var results =
                data.GetStringArrayListExtra(
                    RecognizerIntent.ExtraResults);

            _tcs?.TrySetResult(results?[0] ?? "");
        }
        else
        {
            _tcs?.TrySetResult("");
        }

        _tcs = null;
    }
}