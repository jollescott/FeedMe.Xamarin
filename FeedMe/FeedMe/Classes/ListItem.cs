using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace FeedMe.Classes
{
    internal class ListItem
    {
        public string Name { get; init; } = "None";
        public string IconSource { get; set; } = "md-remove-shopping-cart";
        public Color Color { get; init; } = Colors.HotPink;
    }
}
