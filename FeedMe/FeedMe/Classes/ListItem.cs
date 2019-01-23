using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FeedMe.Classes
{
    class ListItem
    {
        public string Name { get; set; } = "None";
        public string IconSource { get; set; } = "md-remove-shopping-cart";
        public Color Color { get; set; } = Color.HotPink;
    }
}
