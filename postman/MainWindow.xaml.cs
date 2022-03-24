using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;

namespace postman
{
    public record Record(bool active, string key, string value);

    public partial class MainWindow : Window
    {
        static readonly HttpClient client = new HttpClient();
        private ObservableCollection<Record> QueryParamsCollection = new();
        private ObservableCollection<Record> HeadersCollection = new();
        private ObservableCollection<TreeViewItem> ResponseTreeCollection = new();
        private bool preventTextChanged = false;

        public MainWindow()
        {
            InitializeComponent();
            Url.Text = "https://ljtech.ca/api/github";
            ResponseTree.ItemsSource = ResponseTreeCollection;
            QueryParams.ItemsSource = QueryParamsCollection;
            Headers.ItemsSource = HeadersCollection;
            ResponseRaw.Text = @"{
    ""status"": 200, 
    ""data"": [""Hello World"", ""Lorem Ipsum""],
    ""payload"": { ""array"": [""Hello World"", ""Lorem Ipsum""], ""title"": ""Lorem Ipsum Dolor Set Amet"" },
}";
            RequestBody.Inlines.Add(@"{
    ""id"": 123, 
    ""name"": ""Lorem Ipsum"",
    ""payload"": { ""array"": [""Hello World"", ""Lorem Ipsum""], ""title"": ""Lorem Ipsum Dolor Set Amet"" },
}");
            BuildTree(JsonConvert.DeserializeObject<JToken>(ResponseRaw.Text));
        }

        void ChildWindow(object sender, RoutedEventArgs args) {
            ChildWindow window = new ChildWindow();
            window.Name = "Response";
            window.Owner = Application.Current.MainWindow;
            window.Show();
            window.Content = ResponseRaw.Text;
            
        }

    }
}
