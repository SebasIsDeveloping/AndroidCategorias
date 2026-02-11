using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;
using InteractiveApp.Services;
using Java.IO;

namespace InteractiveApp.Android;

public class AndroidCameraService : ICameraService
{
    private readonly Activity _activity;

    private TaskCompletionSource<string?>? _tcs;
    private string? _photoPath;

    public AndroidCameraService(Activity activity)
    {
        _activity = activity;
    }

    public Task<string?> TakePhotoAsync()
    {
        _tcs = new TaskCompletionSource<string?>();

        var intent = new Intent(MediaStore.ActionImageCapture);

        var photoFile = File.CreateTempFile(
            "photo_",
            ".jpg",
            _activity.GetExternalFilesDir(null));

        _photoPath = photoFile.AbsolutePath;

        var uri = AndroidX.Core.Content.FileProvider.GetUriForFile(
            _activity,
            _activity.PackageName + ".fileprovider",
            photoFile);

        intent.PutExtra(MediaStore.ExtraOutput, uri);

        _activity.StartActivityForResult(intent, 1234);

        return _tcs.Task;
    }

    public void OnResult(bool success)
    {
        _tcs?.SetResult(success ? _photoPath : null);
    }
}