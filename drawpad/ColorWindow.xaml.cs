using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace drawpad;

/// <summary>
/// Interaction logic for ColorWindow.xaml
/// </summary>
public partial class ColorWindow : Window
{

    public Color PrimaryColor;
    public MainWindow MainWindow;

    public ColorWindow(MainWindow MainWindow, SolidColorBrush SolidBrushColor)
    {
        this.MainWindow = MainWindow;
        InitializeComponent();
        PrimaryColor = SolidBrushColor.Color;
        ColorPicker.Color = PrimaryColor;
    }

    public void SetColors()
    {
        foreach (var DrawWindow in MainWindow.DrawWindows)
        {
            DrawWindow.InkCanvas.DefaultDrawingAttributes.Color = PrimaryColor;
        }
    }

    private void ColorPicker_MouseUp(object sender, EventArgs e)
    {
        PrimaryColor = ColorPicker.Color;
        MainWindow.PrimaryColorSwatch.Background = new SolidColorBrush(PrimaryColor);
        MainWindow.BrushWindow.BrushEllipse.Fill = new SolidColorBrush(PrimaryColor);
        SetColors();
    }

    private void ColorWindow_Closing(object sender, CancelEventArgs e)
    {
        MainWindow.ColorWindow = new ColorWindow(MainWindow, (SolidColorBrush)MainWindow.PrimaryColorSwatch.Background);
    }
}