using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentAvalonia.UI.Controls;
using InteractiveApp.ViewModels;
using InteractiveApp.Views;

namespace InteractiveApp.Services;

public partial class NavegationService: ObservableObject
{
    public const string INICIO_VIEW = "Inicio";
    public const string ARRASTRAR_VIEW = "Arrastrar";
    public const string HABLAR_VIEW = "Hablar";

    [ObservableProperty] private ContentControl currentView;
    [ObservableProperty] private NavigationViewItem selectMenuItem;
    [ObservableProperty] private ObservableCollection<NavigationViewItem> menuItems = new();
    
    private NavigationViewItem inicioView;
    private NavigationViewItem arrastrarView;
    private NavigationViewItem hablarView;
    private NavigationViewItem cerrarView;

    public NavegationService()
    {
        inicioView = new NavigationViewItem
        {
            Tag = INICIO_VIEW
        };
        
        arrastrarView = new NavigationViewItem
        {
            Tag = ARRASTRAR_VIEW
        };
            
        hablarView = new NavigationViewItem
        {
            Tag = HABLAR_VIEW
        };
        
        MenuItems.Add(inicioView);
        MenuItems.Add(arrastrarView);
        MenuItems.Add(hablarView);
        NavigateTo(INICIO_VIEW);
    }
    
    partial void OnSelectMenuItemChanged(NavigationViewItem item)
    {
        if (item.Tag == null)
            return;

        NavigateTo(item.Tag.ToString());
    }
    
    public void NavigateTo(string tag)
    {
        if (tag.Equals(INICIO_VIEW))
        {
            if (CurrentView is MenuView) return;
            
            MenuView inicioVista = new MenuView();
            inicioVista.DataContext = new MenuViewModel(this);
            CurrentView = inicioVista;
            SelectMenuItem = inicioView;
        }
        else if (tag.Equals(ARRASTRAR_VIEW))
        {
            ArrastrarView arrastrarVista = new ArrastrarView();
            arrastrarVista.DataContext = new ArrastrarViewModel(this);
            CurrentView = arrastrarVista;
            SelectMenuItem = arrastrarView;
        }
        else if (tag.Equals(HABLAR_VIEW))
        {
            HablarView arrastrarVista = new HablarView();
            arrastrarVista.DataContext = new HablarViewModel(this);
            CurrentView = arrastrarVista;
            SelectMenuItem = hablarView;
        }
    }
}