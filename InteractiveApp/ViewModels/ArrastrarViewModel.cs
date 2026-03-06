using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace InteractiveApp.ViewModels;

public partial class ArrastrarViewModel : ViewModelBase
{
    [ObservableProperty] private bool isLevelOk;
    
    [RelayCommand]
    private void DragDelta(VectorEventArgs e)
    {
        if (e.Source is not Thumb)
        {
            return;
        }
        
        var thumb = (Thumb)e.Source;
        if (thumb.RenderTransform is not TranslateTransform t)
        {
            t = new TranslateTransform();
            thumb.RenderTransform = t;
        }
        
        t.X += e.Vector.X;
        t.Y += e.Vector.Y;
    }
    
    [RelayCommand]
    public void DragCompleted(VectorEventArgs e)
    {
        if (e.Source is not Thumb thumb)
            return;

        var window = TopLevel.GetTopLevel(thumb);

        var thumbRect = GetRect(thumb, window);
        if (thumbRect == null)
            return;

        var view = thumb.FindAncestorOfType<UserControl>();
        if (view == null)
            return;

        var huecoV = view.FindControl<Border>("HuecoV");
        var huecoC = view.FindControl<Border>("HuecoC");
        var huecoP = view.FindControl<Border>("HuecoP");

        var rectV = GetRect(huecoV, window);
        var rectC = GetRect(huecoC, window);
        var rectP = GetRect(huecoP, window);

        if (rectV != null && thumbRect.Value.Intersects(rectV.Value) && thumb.Name == "I1")
        {
            thumb.IsVisible = false;
        }
        else if (rectC != null && thumbRect.Value.Intersects(rectC.Value) && thumb.Name == "I2")
        {
            thumb.IsVisible = false;
        }
        else if (rectP != null && thumbRect.Value.Intersects(rectP.Value) && thumb.Name == "I3")
        {
            thumb.IsVisible = false;
        }
        else if (rectP != null && thumbRect.Value.Intersects(rectP.Value) && thumb.Name == "I4")
        {
            thumb.IsVisible = false;
        }
        else if (rectP != null && thumbRect.Value.Intersects(rectP.Value) && thumb.Name == "I5")
        {
            thumb.IsVisible = false;
        }
        else if (rectP != null && thumbRect.Value.Intersects(rectP.Value) && thumb.Name == "I6")
        {
            thumb.IsVisible = false;
        }
    }
    
    private Rect? GetRect(Control control, TopLevel window)
    {
        var pos = control.TranslatePoint(new Point(0,0), window);
        if (pos is null)
            return null;

        return new Rect(pos.Value, control.Bounds.Size);
    }
    
    [RelayCommand]
    public void Scroll(VectorEventArgs e)
    {
        if (IsLevelOk)
        {
            return;
        }
        
        if (e.Source is not Thumb thumb)
            return;

        if (thumb.RenderTransform is not TranslateTransform t)
        {
            t = new TranslateTransform();
            thumb.RenderTransform = t;
        }

        t.X += e.Vector.X;
        t.Y += e.Vector.Y;
    }
}