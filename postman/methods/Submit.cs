using System;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace postman
{
    partial class MainWindow : Window
    {
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
                QueryParamsCollection.Add(new Param(true, parsedQueryParam as string, parsedQueryParams[parsedQueryParam as string]));
            }
            HttpResponseMessage response = null;

            // task : change form
            // var form = new FormUrlEncodedContent(new Dictionary<string, string> { { "name", "lj" }, { "description", "human" } });
            // A container for name/value tuples encoded using application/x-www-form-urlencoded MIME type.

            string serializedJson = JsonConvert.SerializeObject(new TextRange(RequestBody.ContentStart, RequestBody.ContentEnd).Text);
            StringContent json = new StringContent(serializedJson, Encoding.UTF8, "application/json");

            switch (Method.Text)
            {
                case "GET":
                    response = await client.GetAsync(Url.Text);
                    break;
                case "POST":
                    response = await client.PostAsync(Url.Text, json);
                    break;
                case "PUT":
                    response = await client.PutAsync(Url.Text, json);
                    break;
                case "DELETE":
                    response = await client.DeleteAsync(Url.Text);
                    break;
                default:
                    ResponseRaw.Text = "Bad Method";
                    break;
            }
            if (response == null)
            {
                ResponseRaw.Text = "No Response";
                Console.WriteLine("No Response");
                return;
            }
            string mediaType = response.Content.Headers.ContentType?.MediaType ?? "";
            string responseBody = await response.Content.ReadAsStringAsync();
            switch (mediaType)
            {
                case "application/json":
                    var token = JsonConvert.DeserializeObject<JToken>(responseBody);
                    BuildTree(token);
                    ResponseRaw.Text = token.ToString();
                    break;
                case "text/html":
                    ResponseRaw.Text = responseBody;
                    break;
                case "image/gif":
                    ResponseRaw.Text = responseBody;
                    break;
                default:
                    ResponseRaw.Text = "Bad Media Type";
                    break;
            }
        }
    }
}