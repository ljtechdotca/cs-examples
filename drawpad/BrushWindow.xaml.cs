using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace drawpad;

/// <summary>
/// Interaction logic for BrushWindow.xaml
/// </summary>
public partial class BrushWindow : Window
{
    public MainWindow MainWindow;

    public BrushWindow(MainWindow MainWindow, SolidColorBrush SolidColorBrush, double Size)
    {
        this.MainWindow = MainWindow;
        InitializeComponent();
        BrushEllipse.Width = BrushSizeSlider.Value;
        BrushEllipse.Height = BrushSizeSlider.Value;
        BrushEllipse.Fill = SolidColorBrush;
        BrushEllipse.Width = Size;
        BrushEllipse.Height = Size;
        Canvas.SetLeft(BrushEllipse, 50 - Size / 2);
        Canvas.SetTop(BrushEllipse, 50 - Size / 2);
    }

    private void BrushSizeSlider_ValueChanged(object sender, EventArgs e)
    {
        BrushEllipse.Width = BrushSizeSlider.Value;
        BrushEllipse.Height = BrushSizeSlider.Value;
        Canvas.SetLeft(BrushEllipse, 50 - BrushSizeSlider.Value / 2);
        Canvas.SetTop(BrushEllipse, 50 - BrushSizeSlider.Value / 2);
        foreach (var DrawWindow in MainWindow.DrawWindows)
        {
            DrawWindow.InkCanvas.DefaultDrawingAttributes.Width = BrushSizeSlider.Value;
            DrawWindow.InkCanvas.DefaultDrawingAttributes.Height = BrushSizeSlider.Value;
        }
    }

    private void BrushWindow_Closing(object sender, CancelEventArgs e)
    {
        var blahblah = MainWindow.PrimaryColorSwatch.Background;
        MainWindow.BrushWindow = new BrushWindow(MainWindow, (SolidColorBrush)MainWindow.PrimaryColorSwatch.Background, MainWindow.DrawWindows[0].InkCanvas.DefaultDrawingAttributes.Width);
    }
}