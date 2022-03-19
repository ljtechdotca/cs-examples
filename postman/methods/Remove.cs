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
    }
}