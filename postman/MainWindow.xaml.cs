using System.Windows;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using System.Linq;
using System.Windows.Controls;

namespace postman
{
    public record Param(string key, string value);

    public partial class MainWindow : Window
    {
        static readonly HttpClient client = new HttpClient();
        private ObservableCollection<Param> QueryParamsCollection = new();
        private bool preventTextChanged = false;

        public MainWindow()
        {
            InitializeComponent();
            Url.Text = "https://ljtech.ca/api/github";
            Response.Text = @"{
    ""status"": 200, 
    ""data"": ""Hello World"",
}";
            QueryParams.ItemsSource = QueryParamsCollection;
        }

        void ChangeUrl(object sender, RoutedEventArgs args)
        {
            if (sender is not TextBox textBox || !Uri.IsWellFormedUriString(Url.Text, UriKind.Absolute)) return;
            if (preventTextChanged)
            {
                preventTextChanged = false;
                return;
            }
            string text = textBox.Text;
            string name = textBox.Name;
            Uri uri = new Uri(Url.Text);
            var parsedQueryParams = HttpUtility.ParseQueryString(uri.Query);
            QueryParamsCollection.Clear();
            foreach (var parsedQueryParam in parsedQueryParams)
            {
                QueryParamsCollection.Add(new Param(parsedQueryParam as string, parsedQueryParams[parsedQueryParam as string]));
            }

            args.Handled = true;
        }

        void ChangeKey(object sender, RoutedEventArgs args)
        {
            if (!Uri.IsWellFormedUriString(Url.Text, UriKind.Absolute)) return;
            if (sender is not TextBox textBox) return;
            if (textBox.Tag is not Param tag) return;
            preventTextChanged = true;
            int index = QueryParamsCollection.IndexOf(tag);
            if (index == -1) return;
            QueryParamsCollection[index] = tag with { key = textBox.Text };
            string query = "?" + string.Join("&", QueryParamsCollection.Select(item => ($"{item.key}={item.value}")));
            string domain = Url.Text.Split("?")[0];
            Url.Text = $"{domain}{query}";
            args.Handled = true;
        }

        void ChangeValue(object sender, RoutedEventArgs args)
        {
            if (!Uri.IsWellFormedUriString(Url.Text, UriKind.Absolute)) return;
            if (sender is not TextBox textBox) return;
            if (textBox.Tag is not Param tag) return;
            preventTextChanged = true;
            int index = QueryParamsCollection.IndexOf(tag);
            if (index == -1) return;
            QueryParamsCollection[index] = tag with { value = textBox.Text };
            string query = "?" + string.Join("&", QueryParamsCollection.Select(item => ($"{item.key}={item.value}")));
            string domain = Url.Text.Split("?")[0];
            Url.Text = $"{domain}{query}";
            args.Handled = true;
        }

        void Add(object sender, RoutedEventArgs args)
        {
            Param newParam = new Param("", "");
            if (QueryParamsCollection.IndexOf(newParam) != -1) return;
            QueryParamsCollection.Add(new Param("", ""));
            string query = "?" + string.Join("&", QueryParamsCollection.Select(item => ($"{item.key}={item.value}")));
            string domain = Url.Text.Split("?")[0];
            Url.Text = $"{domain}{query}";
        }

        void Remove(object sender, RoutedEventArgs args)
        {
            Button button = (Button)sender;
            Param tag = button.Tag as Param;
            QueryParamsCollection.Remove(tag as Param);
            string query = "";
            string domain = Url.Text.Split("?")[0];
            if (QueryParamsCollection.Count > 0)
            {
                query = "?" + string.Join("&", QueryParamsCollection.Select(item => ($"{item.key}={item.value}")));
            }
            Url.Text = $"{domain}{query}";
        }

        async void Submit(object sender, RoutedEventArgs args)
        {
            if (!Uri.IsWellFormedUriString(Url.Text, UriKind.Absolute))
            {
                Console.WriteLine($@"Bad URL: ""{Url.Text}""");
                return;
            };
            Uri uri = new Uri(Url.Text);
            var parsedQueryParams = HttpUtility.ParseQueryString(uri.Query);
            QueryParamsCollection.Clear();
            foreach (var parsedQueryParam in parsedQueryParams)
            {
                QueryParamsCollection.Add(new Param(parsedQueryParam as string, parsedQueryParams[parsedQueryParam as string]));
            }
            HttpResponseMessage response = null;

            // task : change form
            var form = new FormUrlEncodedContent(new Dictionary<string, string> { { "name", "lj" }, { "description", "human" } });

            switch (Method.Text)
            {
                case "GET":
                    response = await client.GetAsync(Url.Text);
                    break;
                case "POST":
                    response = await client.PostAsync(Url.Text, form);
                    break;
                case "PUT":
                    response = await client.PutAsync(Url.Text, form);
                    break;
                case "DELETE":
                    response = await client.DeleteAsync(Url.Text);
                    break;
                default:
                    Response.Text = "Bad Method";
                    break;
            }
            if (response == null)
            {
                Response.Text = "No Response";
                Console.WriteLine("No Response");
                return;
            }
            string mediaType = response.Content.Headers.ContentType?.MediaType ?? "";
            string responseBody = await response.Content.ReadAsStringAsync();
            switch (mediaType)
            {
                case "application/json":
                    var temp = JsonConvert.DeserializeObject(responseBody);
                    Response.Text = temp?.ToString();
                    break;
                case "text/html":
                    Response.Text = responseBody;
                    break;
                case "image/gif":
                    Response.Text = responseBody;
                    break;
                default:
                    Response.Text = "Bad Media Type";
                    break;
            }
        }
    }
}
