using Android;
using Android.App;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using System.Threading.Tasks;
using InteractiveApp.Services;

namespace InteractiveApp.Android;

public class AndroidMicrophonePermissionService
    : IMicrophonePermissionService
{
    private const int RequestCode = 1001;

    private readonly Activity _activity;
    private TaskCompletionSource<bool>? _tcs;

    public AndroidMicrophonePermissionService(Activity activity)
    {
        _activity = activity;
    }

    public Task<bool> EnsurePermissionAsync()
    {
        if (ContextCompat.CheckSelfPermission(
                _activity,
                Manifest.Permission.RecordAudio) == Permission.Granted)
        {
            return Task.FromResult(true);
        }

        _tcs = new TaskCompletionSource<bool>();

        ActivityCompat.RequestPermissions(
            _activity,
            new[] { Manifest.Permission.RecordAudio },
            RequestCode);

        return _tcs.Task;
    }


    public void OnRequestPermissionsResult(
        int requestCode,
        Permission[] grantResults)
    {
        if (requestCode != RequestCode)
            return;

        _tcs?.TrySetResult(
            grantResults.Length > 0 &&
            grantResults[0] == Permission.Granted);

        _tcs = null;
    }
}