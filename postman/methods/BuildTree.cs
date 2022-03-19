using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;

namespace postman
{
    partial class MainWindow : Window
    {
        void BuildTree(JToken token)
        {
            ResponseTreeCollection.Clear();
            TreeViewItem CreateTreeViewItem(JToken token)
            {
                TreeViewItem RootItem = new TreeViewItem();
                RootItem.IsExpanded = false;
                switch (token.Type)
                {
                    case JTokenType.Property:
                        RootItem = CreateTreeViewItem((token as JProperty).Value);
                        RootItem.Header = $"{(token as JProperty).Name}";
                        break;

                    case JTokenType.Array:
                        RootItem.Header = "Array";
                        int index = 0;
                        foreach (JToken value in (token as JArray))
                        {
                            var NestedItem = CreateTreeViewItem(value);
                            NestedItem.Header = $"{index}: {NestedItem.Header}";
                            RootItem.Items.Add(NestedItem);
                            index++;
                        }
                        break;

                    case JTokenType.Object:
                        RootItem.Header = "Object";
                        foreach (KeyValuePair<string, JToken> item in (token as JObject))
                        {
                            var NestedItem = CreateTreeViewItem(item.Value);
                            NestedItem.Header = $"{item.Key}: {NestedItem.Header}";
                            RootItem.Items.Add(NestedItem);
                        }
                        break;

                    default:
                        RootItem.Header = $"{token}";

                        break;
                }

                return RootItem;
            }
            ResponseTreeCollection.Add(CreateTreeViewItem(token));
        }
    }
}