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

    public MenuViewModel()
    {
        
    }
    
    [RelayCommand]
    private void NavegateToArrastrar()
    {
        _navegationService.NavigateTo(NavegationService.ARRASTRAR_VIEW);
    }
}