using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace drawpad;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    public MainWindow mainWindow;
    public ObservableCollection<SolidColorBrush> colorSwatches = new();

    public SettingsWindow(MainWindow mainWindow, SolidColorBrush solidColorBrush, double brushSize)
    {
        this.mainWindow = mainWindow;

        InitializeComponent();

        ColorPicker.Color = solidColorBrush.Color;
        BrushSizeSlider.Value = brushSize;
        BrushEllipse.Fill = mainWindow.solidColorBrush;
        BrushEllipseSize_Set(brushSize);
        // ColorSwatches_Init();
        for (int i = 0; i < 13; i++)
        {
            colorSwatches.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)(255 / (i + 1)), (byte)(255 / (i + 1)), (byte)(255 / (i + 1)))));
        }
        ColorSwatches_Add(solidColorBrush);
    }

    // private void ColorSwatches_Init()
    // {
    //     for (int i = 0; i < 13; i++)
    //     {
    //         colorSwatches.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)(255 / i + 1), (byte)(255 / i + 1), (byte)(255 / i + 1))));
    //     }
    // }

    public void BrushEllipseSize_Set(double brushSize)
    {
        BrushEllipse.Width = brushSize;
        BrushEllipse.Height = brushSize;
        Canvas.SetLeft(BrushEllipse, 50 - brushSize / 2);
        Canvas.SetTop(BrushEllipse, 50 - brushSize / 2);
    }

    private void BrushSizeSlider_ValueChanged(object sender, EventArgs e)
    {
        mainWindow.brushSize = BrushSizeSlider.Value;
        BrushEllipseSize_Set(BrushSizeSlider.Value);
        mainWindow.DrawAttributes_Size();
    }

    public void ColorSwatches_Add(SolidColorBrush solidColorBrush)
    {
        if (colorSwatches.Contains(solidColorBrush)) return;
        if (colorSwatches.Count > 14)
        {
            colorSwatches.RemoveAt(0);
        }
        colorSwatches.Add(solidColorBrush);
        ColorSwatches.ItemsSource = colorSwatches;
    }

    public void ColorPicker_Set(SolidColorBrush solidColorBrush)
    {
        mainWindow.solidColorBrush = solidColorBrush;
        mainWindow.ColorSwatch.Background = solidColorBrush;
        BrushEllipse.Fill = solidColorBrush;
        mainWindow.DrawAttributes_Color(solidColorBrush);
    }

    private void ColorPicker_MouseUp(object sender, MouseButtonEventArgs e)
    {

        SolidColorBrush newSolidBrush = new SolidColorBrush(ColorPicker.Color);
        ColorPicker_Set(newSolidBrush);
        ColorSwatches_Add(newSolidBrush);
        mainWindow.DrawAttributes_Color(mainWindow.solidColorBrush);
    }

    private void ColorSwatch_Set(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;
        Console.WriteLine(button.Background.GetType());
        if (button.Background is not SolidColorBrush brush) return;
        Console.WriteLine(brush.Color);
        ColorPicker_Set(brush);
    }
}