using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecipePage : ContentPage
	{
        Meal meal;
        public RecipePage (Meal meal_)
		{
            InitializeComponent();
            meal = meal_;
            XamlSetup();
		}

        void XamlSetup()
        {
            Label_RecipeName.Margin = Constants.padding;
            Label_RecipeName.Text = meal.name;
            Label_RecipeName.FontSize = Constants.menuSize1;

            string dot = "● ";

            Label[] labels = new Label[meal.ingredients.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label();
                labels[i].Text = dot + meal.ingredients[i];
                labels[i].FontSize = Constants.textSize1;
                labels[i].Margin = Constants.textListMargin;
                Stack_Ingridients.Children.Add(labels[i]);
            }

        }
    }
}