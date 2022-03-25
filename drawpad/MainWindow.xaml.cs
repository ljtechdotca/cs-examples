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

    public SolidColorBrush solidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 255));
    public double brushSize = 16;

    public Collection<DrawWindow> drawWindows = new();
    public SettingsWindow settingsWindow;
    public EditingMode currentEditingMode = EditingMode.Ink;

    public MainWindow()
    {

        settingsWindow = new SettingsWindow(this, solidColorBrush, brushSize);
        settingsWindow.Show();

        InitializeComponent();

        InkMode.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
        ColorSwatch.Background = solidColorBrush;

        DrawWindows_Init(solidColorBrush);
    }

    private void DrawWindows_Clear(object sender, RoutedEventArgs e)
    {

    }

    public void DrawWindows_Init(SolidColorBrush solidColorBrush)
    {
        if (drawWindows.Count > 0)
        {
            foreach (var DrawWindow in drawWindows)
            {
                DrawWindow.Close();
            }
            drawWindows.Clear();
        }
        foreach (var screen in Screen.AllScreens)
        {
            DrawWindow drawWindow = new DrawWindow(screen, brushSize, solidColorBrush);
            drawWindows.Add(drawWindow);
        }
    }

    public void DrawAttributes_Color(SolidColorBrush solidColorBrush)
    {
        foreach (var drawWindow in drawWindows)
        {
            drawWindow.InkCanvas.DefaultDrawingAttributes.Color = solidColorBrush.Color;
        }
    }

    public void DrawAttributes_Size()
    {
        foreach (var drawWindow in drawWindows)
        {
            drawWindow.InkCanvas.DefaultDrawingAttributes.Width = brushSize;
            drawWindow.InkCanvas.DefaultDrawingAttributes.Height = brushSize;
        }
    }

    private void SettingsWindow_Open(object sender, RoutedEventArgs e)
    {
        settingsWindow.Close();
        this.settingsWindow = new SettingsWindow(this, solidColorBrush, brushSize);
        settingsWindow.Show();
    }

    private void EraserMode_Click(object sender, RoutedEventArgs e)
    {
        currentEditingMode = EditingMode.Erase;
        EraserMode.Foreground = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
        InkMode.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        foreach (var DrawWindow in drawWindows)
        {
            DrawWindow.InkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }
    }

    private void InkMode_Click(object sender, RoutedEventArgs e)
    {
        currentEditingMode = EditingMode.Ink;
        EraserMode.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        InkMode.Foreground = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
        foreach (var DrawWindow in drawWindows)
        {
            DrawWindow.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }
    }

    private void MainWindow_Closing(object sender, CancelEventArgs e)
    {
        foreach (var DrawWindow in drawWindows)
        {
            DrawWindow.Close();
        }
        settingsWindow.Close();
        drawWindows.Clear();
    }
}

