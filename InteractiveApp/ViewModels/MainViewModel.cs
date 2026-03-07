using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InteractiveApp.Services;


namespace InteractiveApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private NavegationService navegationService = new();
    public MainViewModel()
    {
        
    }
    
    
}