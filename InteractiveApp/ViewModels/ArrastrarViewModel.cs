using System;
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

namespace InteractiveApp.ViewModels;

public partial class ArrastrarViewModel : ViewModelBase
{
    [ObservableProperty] private bool isLevelOk;
    
    private Point _posI1, _posI2, _posI3, _posI4, _posI5, _posI6;
    private int _correctos = 0;

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

        var window = TopLevel.GetTopLevel(thumb);
        var thumbRect = GetRect(thumb, window);
        if (thumbRect == null) return;

        var view = thumb.FindAncestorOfType<UserControl>();
        if (view == null) return;
        
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
            thumb.IsVisible = false;
            _correctos++;

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
}