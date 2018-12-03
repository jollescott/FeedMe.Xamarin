using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FeedMe
{
    class Constants
    {
        public static Color backgroundColor = Color.FromRgb(230, 230, 230);
        public static Color navigationBarColor = Color.FromRgb(230, 230, 230);
        public static Color mainColor1 = Color.FromHex("#00CC66");
        public static Color mainColor2 = Color.FromHex("#00CC66");

        public static Color textColor1 = Color.FromRgb(0, 0, 0);
        public static Color textColor2 = Color.FromHex("#00CC66");
        public static Color textColor3 = Color.White;

        public static Color listBackgroundColor1 = Color.FromRgb(230, 230, 230);
        public static Color listBackgroundColor2 = Color.FromRgb(200, 200, 200);

        public static double fontSize1 = Device.GetNamedSize(NamedSize.Large, typeof(Label));
        public static double fontSize2 = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
        public static double fontSize3 = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        public static double fontSize4 = Device.GetNamedSize(NamedSize.Micro, typeof(Label));

        public static int buttonCornerRadius = 10;

        public static int padding1 = 20;
        public static int padding2 = 15;
        public static int padding3 = 10;

        public static int textListMargin = 3;

        public static int navigationBarPadding = 5;
        public static int navigationBarHeight = 30;

        public static string server_adress = "https://ramsey.azurewebsites.net/ingredient/suggest?search=";
        public static string server_adress_recipe = "https://ramsey.azurewebsites.net/recipe/suggest";

    }
}
