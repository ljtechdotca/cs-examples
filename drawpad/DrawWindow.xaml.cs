
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
    public DrawWindow(Screen Screen)
    {
        InitializeComponent();

        this.Title = Screen.DeviceName;
        this.Width = Screen.WorkingArea.Width;
        this.Height = Screen.WorkingArea.Height;
        this.Left = Screen.Bounds.X;
        this.Top = Screen.Bounds.Y;
        InkCanvas.DefaultDrawingAttributes.FitToCurve = true;
        KeyUp += CheckKey_KeyUp;
        this.Show();
    }

    private void CheckKey_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Z && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && InkCanvas.Strokes.Count > 0)
        {
            InkCanvas.Strokes.RemoveAt(InkCanvas.Strokes.Count - 1);
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
}

