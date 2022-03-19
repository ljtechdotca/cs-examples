using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Net.Http.Headers;

namespace postman
{
    public partial class MainWindow : Window
    {
        void Add(object sender, RoutedEventArgs args)
        {
            if (sender is not Button button) return;

            Param newParam = new Param(true, "", "");
            if (button.Name == "AddParam")
            {
                if (QueryParamsCollection.IndexOf(newParam) != -1) return;
                QueryParamsCollection.Add(new Param(true, "", ""));
                string query = "?" + string.Join("&", QueryParamsCollection.Where(item => item.active).Select(item => ($"{item.key}={item.value}"))); string domain = Url.Text.Split("?")[0];
                Url.Text = $"{domain}{query}";

            }
            if (button.Name == "AddHeader")
            {
                if (HeadersCollection.IndexOf(newParam) != -1) return;
                HeadersCollection.Add(new Param(true, "", ""));
                client.DefaultRequestHeaders.Add("x", "x");
            }
        }
    }
}