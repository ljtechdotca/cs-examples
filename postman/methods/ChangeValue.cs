using System.Windows;
using System.Web;
using System.Linq;
using System.Windows.Controls;
using System;

namespace postman
{
    public partial class MainWindow : Window
    {
        void ChangeValue(object sender, RoutedEventArgs args)
        {
            if (sender is not TextBox textBox) return;
            if (textBox.Tag is not Record tag) return;
            if (textBox.Name == "ValueParam")
            {
                // string encodedUrl = HttpUtility.UrlEncode(Url.Text);
                // epic:  [00:10] !%KanawanagasakiYoko: HttpUtility.UrlEncode is used to encode keys and values of Query!
                // epic: [00:10] !%KanawanagasakiYoko: if you apply HttpUtility.UrlEncode to whole url it will do this: https%3a%2f%2fljtech.ca%2f%3fhello%3dwolrd to https://ljtech.ca/?hello=wolrd                preventTextChanged = true;
                int index = QueryParamsCollection.IndexOf(tag);
                if (index == -1) return;
                QueryParamsCollection[index] = tag with { value = textBox.Text };
                string query = "?" + string.Join("&", QueryParamsCollection.Select(item => ($"{item.key}={item.value}")));
                string domain = Url.Text.Split("?")[0];
                Url.Text = $"{domain}{query}";
                args.Handled = true;
            }
            if (textBox.Name == "ValueHeader")
            {
                Console.WriteLine("Hello World, This is Change Value Speaking");
            }
        }
    }
}