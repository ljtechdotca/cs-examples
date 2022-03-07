using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System;

namespace notepad
{
    public partial class Notepad : Window
    {
        ObservableCollection<Item> Notes { get; set; } = new();
        int? Index = null;
        public Notepad()
        {
            Item[] items = new Item[0];
            try
            {
                items = JsonSerializer.Deserialize<Item[]>(File.ReadAllText("notes.json"));
                foreach (var item in items) Notes.Add(item);
            }
            catch
            {
                if (!File.Exists("notes.json"))
                {
                    File.WriteAllText("notes.json", "[]");
                }
                else
                {
                    File.Delete("notes.json");
                    File.WriteAllText("notes.json", "[]");
                }
            }
            InitializeComponent();
            Items.ItemsSource = Notes;
        }
        void Submit(object sender, RoutedEventArgs e)
        {
            Notes.Add(new Item(Name.Text, Description.Text, Deadline.Text));
            File.WriteAllText("notes.json", JsonSerializer.Serialize(Notes));
            Name.Text = "";
            Description.Text = "";
            Deadline.Text = "";
        }
        void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Index is not null)
                {
                    Notes[Index.Value] = (new Item(Name.Text, Description.Text, Deadline.Text));
                    File.WriteAllText("notes.json", JsonSerializer.Serialize(Notes));
                    Create.Visibility = Visibility.Visible;
                    Update.Visibility = Visibility.Collapsed;
                    Name.Text = "";
                    Description.Text = "";
                    Deadline.Text = "";
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }
        void Cancel(object sender, RoutedEventArgs e)
        {
            Create.Visibility = Visibility.Visible;
            Update.Visibility = Visibility.Collapsed;
            Name.Text = "";
            Description.Text = "";
            Deadline.Text = "";
        }
        void Note_OnDelete(object sender, EventArgs e)
        {
            if (sender is Note note)
            {
                Notes.Remove(note.Item);
                File.WriteAllText("notes.json", JsonSerializer.Serialize(Notes));
            };
        }
        void Note_OnEdit(object sender, EventArgs e)
        {
            if (sender is Note note)
            {
                Create.Visibility = Visibility.Collapsed;
                Update.Visibility = Visibility.Visible;
                Name.Text = note.Item.name;
                Description.Text = note.Item.description;
                Deadline.Text = note.Item.deadline;
                Index = Notes.IndexOf(note.Item);
            };
        }
    }
}
