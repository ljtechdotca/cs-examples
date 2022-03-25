
using System.Windows;
using System;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Ink;
using System.Windows.Input;

namespace drawpad;

/// <summary>
/// Interaction logic for DrawWindow.xaml
/// </summary>
public partial class DrawWindow : Window
{
    StrokeCollection redoStrokes = new();

    public DrawWindow(Screen screen, double brushSize, SolidColorBrush solidColorBrush)
    {
        this.Title = screen.DeviceName;
        this.Width = screen.WorkingArea.Width;
        this.Height = screen.WorkingArea.Height;
        this.Left = screen.Bounds.X;
        this.Top = screen.Bounds.Y;

        InitializeComponent();
        InkCanvas.DefaultDrawingAttributes.Height = brushSize;
        InkCanvas.DefaultDrawingAttributes.Width = brushSize;
        InkCanvas.DefaultDrawingAttributes.Color = solidColorBrush.Color;
        InkCanvas.DefaultDrawingAttributes.FitToCurve = true;
        KeyUp += CheckKey_KeyUp;
        this.Show();
    }

    private void CheckKey_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
        {
            if (e.Key == Key.Z && InkCanvas.Strokes.Count > 0)
            {
                redoStrokes.Add(InkCanvas.Strokes[InkCanvas.Strokes.Count - 1]);
                InkCanvas.Strokes.RemoveAt(InkCanvas.Strokes.Count - 1);
            }
            if (e.Key == Key.Y && redoStrokes.Count > 0)
            {
                InkCanvas.Strokes.Add(redoStrokes[redoStrokes.Count - 1]);
                redoStrokes.RemoveAt(redoStrokes.Count - 1);
            }
        }
    }

    private void DrawWindow_Activated(object sender, EventArgs e)
    {
        SolidColorBrush transparentBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(1, 0, 0, 0));
        this.Background = transparentBrush;
        InkCanvas.Background = transparentBrush;
    }

    private void DrawWindow_Deactivated(object sender, EventArgs e)
    {
        SolidColorBrush transparentBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        this.Background = transparentBrush;
        InkCanvas.Background = transparentBrush;
    }

    public void InkCanvasAttributes_Color(Color color)
    {
        InkCanvas.DefaultDrawingAttributes.Color = color;
    }

    public void InkCanvasAttributes_Size(double size)
    {
        InkCanvas.DefaultDrawingAttributes.Height = size;
        InkCanvas.DefaultDrawingAttributes.Width = size;
    }
}

