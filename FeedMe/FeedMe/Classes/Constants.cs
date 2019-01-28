using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FeedMe
{
    class Constants
    {
        public class AppColor
        {
            public static Color backgroundColor = Color.FromHex("#F2F2F2");
            public static Color navigationBarColor = Color.FromHex("#00CC66");

            public static Color green = Color.FromHex("#00CC66");
            public static Color white = Color.FromHex("#FFFFFF");
            public static Color gray = Color.FromHex("#777777");
            public static Color lightGray = Color.FromHex("#E6E6E6");

            public static Color text_defult = Color.FromHex("#282828");

            public static Color text_green = Color.FromHex("#00CC66");
            public static Color text_black = Color.FromHex("#202020");
            public static Color text_gray = Color.FromHex("#777777");
            public static Color text_white = Color.White;
            public static Color text_link = Color.FromHex("#0000EE");
        }

        public static int textHeight = 50;


        public static double fontSize1double = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2;
        public static double fontSize1 = Device.GetNamedSize(NamedSize.Large, typeof(Label));
        public static double fontSize2 = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
        public static double fontSize3 = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        public static double fontSize4 = Device.GetNamedSize(NamedSize.Micro, typeof(Label));

        public static int cornerRadius1 = 15;
        public static int cornerRadius2 = 5;


        public static int padding1 = 20;
        public static int padding2 = 15;
        public static int padding3 = 10;

        public static int textListMargin = 3;

        public static string AddIngredientCheckIcon = "md-check-box-outline-blank";
        public static string DeleteIngredientCheckIcon = "md-check-box";
        public static string ExcludeIngredientCheckIcon = "md-block";
        public static string AddIngredientIcon = "md-add";
        public static string DeleteIngredientIcon = "md-clear";

        //public static int navigationBarPadding = 5;
        //public static int navigationBarHeight = 30;

        //public static string ingredient_search = "https://ramsey.azurewebsites.net/ingredient/suggest?search=";
        //public static string recipe_suggest = "https://ramsey.azurewebsites.net/recipe/suggest";
        //public static string recipe_retrive = "https://ramsey.azurewebsites.net/recipe/retrieve?id=";

        //public static string myIngredientsSave = "myIngredients";
    }
}
