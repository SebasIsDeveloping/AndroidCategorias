using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InteractiveApp.Services;


namespace InteractiveApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    [ObservableProperty] private string _text = "App para reconocimiento de voz";
    [ObservableProperty] private string _photoPath = string.Empty;
    [ObservableProperty] private bool _isRecord;

    public MainViewModel()
    {

    }
    
    [RelayCommand]
    public async Task TakePhotoAsync()
    {
        if (!await AppServices.CameraPermission.EnsurePermissionAsync())
        {
            Text = "Sin permiso cámara";
            return;
        }

        var photo = await AppServices.Camera.TakePhotoAsync();

        if (photo == null)
        {
            Text = "Cancelado";
            PhotoPath = null;
            return;
        }

        Text = photo;
        PhotoPath = photo;
    }



    [RelayCommand]
    public async Task Record()
    {
        try
        {
            if (!await AppServices.MicrophonePermission.EnsurePermissionAsync())
            {
                Text = "Permiso denegado";
                return;
            }
            AppServices.AudioPlayer
                .PlayFromAsset("avares://InteractiveApp/Assets/record.mp3");

            Text = "Preparando…";
            IsRecord =  true;
            var audio = await AppServices.AudioRecorder.RecordAsync(3);
    
            Text = "Escuchando…";
            
            //Text = await AppServices.WhisperService.TranscribeAudioAsync(audio);
            Text = await AppServices.SttService.TranscribeAsync(audio);
            
            AppServices.AudioPlayer
                .PlayFromAsset("avares://InteractiveApp/Assets/endrecord.mp3");
            IsRecord = false;
        }
        catch (Exception ex)
        {
            Text = "ERROR: " + ex.GetType().Name;
            Console.WriteLine(ex);
        }
    }
    
    
}