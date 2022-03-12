using System.Windows;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using System.Windows.Controls;

namespace postman
{
    public record Param(string key, string value);

    public partial class MainWindow : Window
    {
        static readonly HttpClient client = new HttpClient();
        ObservableCollection<Param> QueryParamsCollection = new();

        public MainWindow()
        {
            InitializeComponent();
            Url.Text = "https://ljtech.ca/api/github";
            Response.Text = @"
                {
                    ""status"": 200, 
                    ""data"": ""Hello World"",
                }
            ";
            QueryParams.ItemsSource = QueryParamsCollection;
        }

        void UrlChange(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox textBox || !Uri.IsWellFormedUriString(Url.Text, UriKind.Absolute)) return;
            string text = textBox.Text;
            string name = textBox.Name;
            Uri uri = new Uri(Url.Text);
            var parsedQueryParams = HttpUtility.ParseQueryString(uri.Query);
            QueryParamsCollection.Clear();
            foreach (var parsedQueryParam in parsedQueryParams)
            {
                QueryParamsCollection.Add(new Param(parsedQueryParam as string, parsedQueryParams[parsedQueryParam as string]));
            }
        }

        void KeyChange(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox textBox) return;
            string text = textBox.Text;
            string name = textBox.Name;
            string tag = textBox.Tag.ToString();
            Console.WriteLine(text);
            Console.WriteLine(name);
            Console.WriteLine(tag);
        }

        void ValueChange(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox textBox) return;
            string text = textBox.Text;
            string name = textBox.Name;
            string tag = textBox.Tag.ToString();
            Console.WriteLine(text);
            Console.WriteLine(name);
            Console.WriteLine(tag);
        }

        void Add(object sender, RoutedEventArgs e)
        {
            QueryParamsCollection.Add(new Param("", ""));
            if (Url.Text.Contains("?"))
            {
                Url.Text += "&=";
            }
            else
            {
                Url.Text += "?=";
            }
        }

        void Remove(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Param tag = button.Tag as Param;
            string target = "";
            if (Url.Text.Contains("?"))
            {
                target = $"?{tag.key}={tag.value}";
            }
            else
            {
                target = $"&{tag.key}={tag.value}";
            }
            Url.Text = Url.Text.Replace(target, "");
            QueryParamsCollection.Remove(tag as Param);
        }

        async void Submit(object sender, RoutedEventArgs e)
        {
            if (!Uri.IsWellFormedUriString(Url.Text, UriKind.Absolute))
            {
                Console.WriteLine($@"Bad URL: ""{Url.Text}""");
                return;
            };

            Uri uri = new Uri(Url.Text);

            Console.WriteLine($"fetching {Url.Text}");


            var parsedQueryParams = HttpUtility.ParseQueryString(uri.Query);
            foreach (var parsedQueryParam in parsedQueryParams)
            {
                QueryParamsCollection.Add(new Param(parsedQueryParam as string, parsedQueryParams[parsedQueryParam as string]));
            }

            HttpResponseMessage response = null;

            // task : figure this form out
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
