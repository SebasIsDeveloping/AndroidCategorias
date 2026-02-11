using System;
using System.IO;
using Android.App;
using Android.Media;
using Avalonia.Platform;
using InteractiveApp.Services;

namespace InteractiveApp.Android.Resources;

public class AndroidAudioPlayer: IAudioPlayer
{
    public void PlayFromAsset(string avaresPath)
    {
        using var stream =
            AssetLoader.Open(new Uri(avaresPath));

        var tempPath = Path.Combine(
            Application.Context.CacheDir!.AbsolutePath,
            "temp.mp3");

        using (var file = File.Create(tempPath))
            stream.CopyTo(file);

        var player = new MediaPlayer();
        player.SetDataSource(tempPath);
        player.Prepare();
        player.Start();
    }
}