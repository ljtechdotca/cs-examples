using System.Windows;
using System;
using System.Web;
using System.Windows.Controls;

namespace postman
{
    public partial class MainWindow : Window
    {
        void ChangeUrl(object sender, RoutedEventArgs args)
        {
            if (!Uri.IsWellFormedUriString(Url.Text, UriKind.Absolute)) return;
            if (sender is not TextBox textBox) return;
            // string encodedUrl = HttpUtility.UrlEncode(Url.Text);
            // epic:  [00:10] !%KanawanagasakiYoko: HttpUtility.UrlEncode is used to encode keys and values of Query!
            // epic: [00:10] !%KanawanagasakiYoko: if you apply HttpUtility.UrlEncode to whole url it will do this: https%3a%2f%2fljtech.ca%2f%3fhello%3dwolrd to https://ljtech.ca/?hello=wolrd

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
                bool active = true;
                string key = parsedQueryParam as string;
                string value = parsedQueryParams[parsedQueryParam as string];
                if (key is null)
                {
                    QueryParamsCollection.Add(new Record(active, value, value));
                }
                else
                {
                    QueryParamsCollection.Add(new Record(active, key, value));
                }
            }

            args.Handled = true;
        }
    }
}