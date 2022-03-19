using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace postman
{
    partial class MainWindow : Window
    {
        void Check(object sender, RoutedEventArgs args)
        {
            preventTextChanged = true;

            if (sender is not CheckBox checkBox) return;

            var param = checkBox.Tag as Param;

            if (checkBox.Name == "CheckParam")
            {
            int index = QueryParamsCollection.IndexOf(param);
                if (index == -1) return;
                string query = "?" + string.Join("&", QueryParamsCollection.Where(item => item.active).Select(item => ($"{item.key}={item.value}")));

                string domain = Url.Text.Split("?")[0];
                Url.Text = $"{domain}{query}";
                args.Handled = true;
            }
            if (checkBox.Name == "CheckHeader")
            {
                Console.WriteLine("Hello this is Check SPeaking.");
            }
        }
    }
}