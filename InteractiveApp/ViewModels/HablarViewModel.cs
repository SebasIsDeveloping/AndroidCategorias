using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
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
    [ObservableProperty] private bool _isRecord;
    [ObservableProperty] private bool _isLevelOk = false;
    [ObservableProperty] private bool _finNivel = true;
    [ObservableProperty] private bool _primeraPreguntaVisible = true;
    [ObservableProperty] private bool _segundaPreguntaVisible = false;

    [ObservableProperty] private Bitmap _photoPath =
        new Bitmap(AssetLoader.Open(new Uri("avares://InteractiveApp/Assets/img/postre_hablar_silueta.png")));

    public string[] ingredientes = new[] { "fresa", "pollo", "brócoli" };

    public string[] ingredientesImg = new[] { "postre_hablar_silueta.png", "comida_hablar_silueta.png", "vegano_hablar_silueta.png" };

    public string[] categorias = new[] { "postre", "normal", "vegano" };
    public string[] categoriasImg = new[] { "postre_hablar.png", "comida_hablar.png", "vegano_hablar.png" };

    private int _nivelActual = 0;

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
            IsRecord = true;
            var audio = await AppServices.AudioRecorder.RecordAsync(3);

            Text = "Escuchando…";

            Text = await AppServices.SttService.TranscribeAsync(audio);

            if (!string.IsNullOrWhiteSpace(Text) && Text.ToLower().Contains(ingredientes[_nivelActual]))
            {
                PrimeraPreguntaVisible = false;
                SegundaPreguntaVisible = true;
                PhotoPath = new Bitmap(
                    AssetLoader.Open(new Uri("avares://InteractiveApp/Assets/img/" + categoriasImg[_nivelActual])));
            }
            else
            {
                Text = "Prueba de nuevo";
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
            IsRecord = true;
            var audio = await AppServices.AudioRecorder.RecordAsync(3);

            Text = "Escuchando…";

            Text = await AppServices.SttService.TranscribeAsync(audio);

            if (!string.IsNullOrWhiteSpace(Text) && Text.ToLower().Contains(categorias[_nivelActual]))
            {
                IsLevelOk = true;
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
        _navegationService.NavigateTo(NavegationService.INICIO_VIEW);
    }

    [RelayCommand]
    private void SiguienteNivel()
    {
        _nivelActual++;
        IsLevelOk = false;
        PrimeraPreguntaVisible = true;
        SegundaPreguntaVisible = false;
        Text = "";
        PhotoPath = new Bitmap(
            AssetLoader.Open(new Uri("avares://InteractiveApp/Assets/img/" + ingredientesImg[_nivelActual])));
        if (_nivelActual == 2) FinNivel = false;
    }
}