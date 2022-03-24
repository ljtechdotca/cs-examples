using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace drawpad;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public enum EditingMode
    {
        Erase,
        Ink,
    }

    public Collection<DrawWindow> DrawWindows = new();
    public BrushWindow BrushWindow;
    public ColorWindow ColorWindow;
    public EditingMode CurrentEditingMode = EditingMode.Ink;

    public MainWindow()
    {
        this.BrushWindow = new BrushWindow(this, new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0)), 50);
        this.ColorWindow = new ColorWindow(this, new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0)));
        InitializeComponent();
        DrawWindow_Clear(null, null);
        PrimaryColorSwatch.Background = new SolidColorBrush(ColorWindow.PrimaryColor);
    }

    private void DrawWindow_Clear(object? sender, EventArgs? e)
    {
        if (DrawWindows.Count > 0)
        {
            foreach (var DrawWindow in DrawWindows)
            {
                DrawWindow.Close();
            }
            DrawWindows.Clear();
        }
        foreach (var Screen in Screen.AllScreens)
        {
            DrawWindow drawWindow = new DrawWindow(Screen);
            ColorWindow.SetColors();
            DrawWindows.Add(drawWindow);
        }
    }

    private void BrushWindow_Open(object sender, RoutedEventArgs e)
    {
        BrushWindow.Show();
    }

    private void ColorWindow_Open(object sender, RoutedEventArgs e)
    {
        ColorWindow.Show();
    }

    private void EraserMode_Click(object sender, RoutedEventArgs e)
    {
        CurrentEditingMode = EditingMode.Erase;
        EraserMode.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        InkMode.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        foreach (var DrawWindow in DrawWindows)
        {
            DrawWindow.InkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }
    }

    private void InkMode_Click(object sender, RoutedEventArgs e)
    {
        CurrentEditingMode = EditingMode.Ink;
        EraserMode.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        InkMode.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        foreach (var DrawWindow in DrawWindows)
        {
            DrawWindow.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }
    }

    private void MainWindow_Closing(object sender, CancelEventArgs e)
    {
        foreach (var DrawWindow in DrawWindows)
        {
            DrawWindow.Close();
        }
        BrushWindow.Close();
        ColorWindow.Close();
        DrawWindows.Clear();
    }
}

