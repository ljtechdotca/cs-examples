using System.Windows.Controls;
using System.Windows;
using System;

namespace notepad
{
    public record Item(string name, string description, string deadline);

    public partial class Note : UserControl
    {
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(Item), typeof(Note));

        public Item Item
        {
            get => (Item)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        public event EventHandler OnEdit;
        public event EventHandler OnDelete;

        public Note()
        {
            InitializeComponent();
        }

        void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            OnEdit?.Invoke(this, EventArgs.Empty);
        }

        void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            OnDelete?.Invoke(this, EventArgs.Empty);
        }

        // public event EventHandler OnEdit;
    }
}