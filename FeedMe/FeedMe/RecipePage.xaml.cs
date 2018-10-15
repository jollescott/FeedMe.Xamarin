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

            /*Label_Ingridients.Margin = Constants.padding;
            Label_Ingridients.FontSize = Constants.textSize1;
            Label_Ingridients.Text = meal.ingredients;*/

            Label lbl = new Label();

            lbl.Text = dot + meal.ingredients[0];
            lbl.FontSize = Constants.textSize1;
            lbl.Margin = Constants.padding;

            /*Label lbl2 = new Label();

            lbl2.Text = dot + meal.ingredients[1];
            lbl2.FontSize = Constants.textSize1;
            lbl2.Margin = Constants.padding;*/

            Stack_Ingridients.Children.Add(lbl);
            //Stack_Ingridients.Children.Add(lbl2);


        }
    }
}