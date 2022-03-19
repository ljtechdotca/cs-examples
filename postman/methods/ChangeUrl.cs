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
            // if (!Uri.IsWellFormedUriString(Url.Text, UriKind.Absolute)) return;
            if (sender is not TextBox textBox) return;
            string encodedUrl = HttpUtility.UrlEncode(Url.Text);

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
                QueryParamsCollection.Add(new Param(true, parsedQueryParam as string, parsedQueryParams[parsedQueryParam as string]));
            }

            args.Handled = true;
        }
    }
}