using System.Windows;
using System.Web;
using System.Linq;
using System.Windows.Controls;
using System;

namespace postman
{
    public partial class MainWindow : Window
    {
        void ChangeKey(object sender, RoutedEventArgs args)
        {
            if (sender is not TextBox textBox) return;
            if (textBox.Tag is not Param tag) return;
            if (textBox.Name == "KeyParam")
            {
                string encodedUrl = HttpUtility.UrlEncode(Url.Text);
                preventTextChanged = true;
                int index = QueryParamsCollection.IndexOf(tag);
                if (index == -1) return;
                QueryParamsCollection[index] = tag with { key = textBox.Text };
                string query = "?" + string.Join("&", QueryParamsCollection.Select(item => ($"{item.key}={item.value}")));
                string domain = encodedUrl.Split("?")[0];
                Url.Text = $"{domain}{query}";
                args.Handled = true;
            }
            if (textBox.Name == "KeyHeader")
            {
                Console.WriteLine("Hello World");
            }
        }

    }
}