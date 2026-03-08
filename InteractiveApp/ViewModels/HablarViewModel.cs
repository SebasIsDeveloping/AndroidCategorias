using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InteractiveApp.Services;

namespace InteractiveApp.ViewModels;

public partial class HablarViewModel : ViewModelBase
{    
    private NavegationService _navegationService;
    public HablarViewModel(NavegationService navegationService)
    {
        _navegationService = navegationService;
    }
    public HablarViewModel()
    {
        IsLevelOk = false;
    }

    [ObservableProperty] private string _text = "";
    [ObservableProperty] private string _photoPath = string.Empty;
    [ObservableProperty] private bool _isRecord;
    [ObservableProperty] private bool _isLevelOk = false;
    [ObservableProperty] private bool _segundaPreguntaVisible = false;
    [ObservableProperty] private bool _primeraPreguntaVisible = false;
    
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
        
            if (!string.IsNullOrWhiteSpace(Text) && Text.ToLower().Contains("pollo"))
            {
                PrimeraPreguntaVisible = false; 
                SegundaPreguntaVisible = true; 
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
        
            if (!string.IsNullOrWhiteSpace(Text) && Text.ToLower().Contains("perro"))
            {
                IsLevelOk= true;
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