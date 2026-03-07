using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InteractiveApp.Views;

namespace InteractiveApp.ViewModels;

public partial class ArrastrarViewModel : ViewModelBase
{
    [ObservableProperty] private bool isLevelOk;
    
    private Point _posI1, _posI2, _posI3, _posI4, _posI5, _posI6;
    private readonly string[] _thumbs = { "I1", "I2", "I3", "I4", "I5", "I6" };
    private int _correctos = 0;
    private int _nivelActual = 1;
    private HashSet<string> _colocados = new();
    private UserControl? _ultimaVista;
    
    private void CambiarTags(UserControl view)
    {
        var I1 = view.FindControl<Thumb>("I1");
        var I2 = view.FindControl<Thumb>("I2");
        var I3 = view.FindControl<Thumb>("I3");
        var I4 = view.FindControl<Thumb>("I4");
        var I5 = view.FindControl<Thumb>("I5");
        var I6 = view.FindControl<Thumb>("I6");

        switch (_nivelActual)
        {
            case 1:
                I1.Tag = "HuecoP";
                I2.Tag = "HuecoC";
                I3.Tag = "HuecoV";
                I4.Tag = "HuecoP";
                I5.Tag = "HuecoC";
                I6.Tag = "HuecoV";
                break;

            case 2:
                I1.Tag = "HuecoV";
                I2.Tag = "HuecoP";
                I3.Tag = "HuecoC";
                I4.Tag = "HuecoV";
                I5.Tag = "HuecoP";
                I6.Tag = "HuecoC";
                break;

            case 3:
                I1.Tag = "HuecoC";
                I2.Tag = "HuecoV";
                I3.Tag = "HuecoP";
                I4.Tag = "HuecoC";
                I5.Tag = "HuecoV";
                I6.Tag = "HuecoP";
                break;
        }
    }

    [RelayCommand]
    private void DragDelta(VectorEventArgs e)
    {
        if (e.Source is not Thumb thumb) return;
        
        if (thumb.RenderTransform is not TranslateTransform t)
        {
            t = new TranslateTransform();
            thumb.RenderTransform = t;
        }

        switch (thumb.Name)
        {
            case "I1": if (_posI1 == default) _posI1 = new Point(t.X, t.Y); break;
            case "I2": if (_posI2 == default) _posI2 = new Point(t.X, t.Y); break;
            case "I3": if (_posI3 == default) _posI3 = new Point(t.X, t.Y); break;
            case "I4": if (_posI4 == default) _posI4 = new Point(t.X, t.Y); break;
            case "I5": if (_posI5 == default) _posI5 = new Point(t.X, t.Y); break;
            case "I6": if (_posI6 == default) _posI6 = new Point(t.X, t.Y); break;
        }

        t.X += e.Vector.X;
        t.Y += e.Vector.Y;
    }

    [RelayCommand]
    public void DragCompleted(VectorEventArgs e)
    {
        if (e.Source is not Thumb thumb) return;
        
        var view = thumb.FindAncestorOfType<UserControl>();
        if (view == null) return;
        _ultimaVista = view;

        var window = TopLevel.GetTopLevel(thumb);
        var thumbRect = GetRect(thumb, window);
        if (thumbRect == null) return;

        var huecoV = view.FindControl<Border>("HuecoV");
        var huecoC = view.FindControl<Border>("HuecoC");
        var huecoP = view.FindControl<Border>("HuecoP");

        var rectV = GetRect(huecoV, window);
        var rectC = GetRect(huecoC, window);
        var rectP = GetRect(huecoP, window);

        string categoria = (string)thumb.Tag;

        bool correcto = categoria switch
        {
            "HuecoV" => rectV?.Intersects(thumbRect.Value) == true,
            "HuecoC" => rectC?.Intersects(thumbRect.Value) == true,
            "HuecoP" => rectP?.Intersects(thumbRect.Value) == true,
            _ => false
        };

        if (correcto)
        {
            if (!_colocados.Contains(thumb.Name))
            {
                _colocados.Add(thumb.Name);
                _correctos++;
            }

            thumb.IsVisible = false;

            if (_correctos == 6)
                IsLevelOk = true;
        }
        else
        {
            VolverAPosicionInicial(thumb);
        }
    }

    private void VolverAPosicionInicial(Thumb thumb)
    {
        if (thumb.RenderTransform is not TranslateTransform t)
        {
            t = new TranslateTransform();
            thumb.RenderTransform = t;
        }

        switch (thumb.Name)
        {
            case "I1": t.X = _posI1.X; t.Y = _posI1.Y; break;
            case "I2": t.X = _posI2.X; t.Y = _posI2.Y; break;
            case "I3": t.X = _posI3.X; t.Y = _posI3.Y; break;
            case "I4": t.X = _posI4.X; t.Y = _posI4.Y; break;
            case "I5": t.X = _posI5.X; t.Y = _posI5.Y; break;
            case "I6": t.X = _posI6.X; t.Y = _posI6.Y; break;
        }
    }

    private Rect? GetRect(Control control, TopLevel window)
    {
        var pos = control.TranslatePoint(new Point(0, 0), window);
        if (pos is null) return null;
        return new Rect(pos.Value, control.Bounds.Size);
    }

    [RelayCommand]
    public void Scroll(VectorEventArgs e)
    {
        if (IsLevelOk) return;
        if (e.Source is not Thumb thumb) return;

        if (thumb.RenderTransform is not TranslateTransform t)
        {
            t = new TranslateTransform();
            thumb.RenderTransform = t;
        }

        t.X += e.Vector.X;
        t.Y += e.Vector.Y;
    }
    
    [RelayCommand]
    private void VolverMenu()
    {
        
    }

    [RelayCommand]
    private void SiguienteNivel()
    {
        var view = _ultimaVista; 
        if (view == null) return;

        _nivelActual++;

        if (_nivelActual > 3)
            _nivelActual = 1;

        ReiniciarNivel(view);
    }
    
    private void ReiniciarNivel(UserControl view)
    {
        string[] thumbs = { "I1", "I2", "I3", "I4", "I5", "I6" };

        foreach (var nombre in thumbs)
        {
            var thumb = view.FindControl<Thumb>(nombre);
            if (thumb == null) continue;

            thumb.IsVisible = true;
            VolverAPosicionInicial(thumb);
        }

        CambiarTags(view);

        _correctos = 0;
        _colocados.Clear();
        IsLevelOk = false;
    }
    
    private void ReiniciarThumb(Thumb? thumb, Point pos)
    {
        if (thumb == null) return;

        thumb.IsVisible = true;

        if (thumb.RenderTransform is not TranslateTransform t)
        {
            t = new TranslateTransform();
            thumb.RenderTransform = t;
        }

        t.X = pos.X;
        t.Y = pos.Y;
    }
}