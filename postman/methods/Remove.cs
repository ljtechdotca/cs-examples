using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace postman
{
    partial class MainWindow : Window
    {
        void Remove(object sender, RoutedEventArgs args)
        {
            Button button = (Button)sender;
            Record tag = button.Tag as Record;
            QueryParamsCollection.Remove(tag as Record);
            string query = "";
            string domain = Url.Text.Split("?")[0];
            if (QueryParamsCollection.Count > 0)
            {
                query = "?" + string.Join("&", QueryParamsCollection.Select(item => ($"{item.key}={item.value}")));
            }
            Url.Text = $"{domain}{query}";
        }
    }
}