using CommunityToolkit.Mvvm.Input;
using InteractiveApp.Services;

namespace InteractiveApp.ViewModels;

public partial class MenuViewModel : ViewModelBase
{
    private NavegationService _navegationService;
    
    public MenuViewModel(NavegationService navegationService)
    {
        _navegationService =  navegationService;
    }

    public MenuViewModel() { }
    
    [RelayCommand]
    private void NavegateToArrastrar()
    {
        AppServices.AudioPlayer
            .PlayFromAsset("avares://InteractiveApp/Assets/audio/tap.mp3");
        _navegationService.NavigateTo(NavegationService.ARRASTRAR_VIEW);
    }
    
    [RelayCommand]
    private void NavegateToHablar()
    {
        AppServices.AudioPlayer
            .PlayFromAsset("avares://InteractiveApp/Assets/audio/tap.mp3");
        _navegationService.NavigateTo(NavegationService.HABLAR_VIEW);
    }
}