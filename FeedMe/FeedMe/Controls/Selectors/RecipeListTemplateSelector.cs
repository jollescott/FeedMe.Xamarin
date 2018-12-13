using FeedMe.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FeedMe.Controls.Selectors
{
    public class RecipeListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RecipeTemplate { get; set; }
        public DataTemplate AdTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return (item as RecipeMetaModel).IsAd ? AdTemplate : RecipeTemplate;
        }
    }
}
