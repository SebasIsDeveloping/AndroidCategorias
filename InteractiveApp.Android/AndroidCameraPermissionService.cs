using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using InteractiveApp.Services;

namespace InteractiveApp.Android;

public class AndroidCameraPermissionService
    : ICameraPermissionService
{
    private const int RequestCode = 2001;

    private readonly Activity _activity;
    private TaskCompletionSource<bool>? _tcs;

    public AndroidCameraPermissionService(Activity activity)
    {
        _activity = activity;
    }

    public Task<bool> EnsurePermissionAsync()
    {
        if (ContextCompat.CheckSelfPermission(
                _activity,
                Manifest.Permission.Camera) == Permission.Granted)
        {
            return Task.FromResult(true);
        }

        _tcs = new TaskCompletionSource<bool>();

        ActivityCompat.RequestPermissions(
            _activity,
            new[] { Manifest.Permission.Camera },
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