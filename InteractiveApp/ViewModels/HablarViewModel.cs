using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InteractiveApp.Services;

namespace InteractiveApp.ViewModels;

public partial class HablarViewModel : ViewModelBase
{
    public HablarViewModel()
    {

    }

    [ObservableProperty] private string _text = "App para reconocimiento de voz";
    [ObservableProperty] private string _photoPath = string.Empty;
    [ObservableProperty] private bool _isRecord;
    [ObservableProperty] private bool isLevelOk;
    [ObservableProperty] private bool preguntaVisible;
    
    [RelayCommand]
    public async Task Record1()
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
        
            Text = await AppServices.SttService.TranscribeAsync(audio);
        
            if (!string.IsNullOrWhiteSpace(Text) && Text.ToLower().Contains("perro"))
            {
                PreguntaVisible = true; 
            }
        
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
        
    
    [RelayCommand]
    public async Task Record2()
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
        
            Text = await AppServices.SttService.TranscribeAsync(audio);
        
            if (!string.IsNullOrWhiteSpace(Text) && Text.ToLower().Contains("postre"))
            {
                PreguntaVisible = true; 
            }
        
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
    
    [RelayCommand]
    private void VolverMenu()
    {

    }

    [RelayCommand]
    private void SiguienteNivel()
    {
        
    }
    
}