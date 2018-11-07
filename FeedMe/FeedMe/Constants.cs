using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FeedMe
{
    class Constants
    {
        public static Color backgroundColor = Color.White;
        public static Color navigationBarColor = Color.White;
        public static Color mainColor1 = Color.FromHex("#8ACC9B");
        public static Color mainColor2 = Color.FromHex("#8AFF9B");

        public static Color textColor1 = Color.FromRgb(0, 0, 0);
        public static Color textColor2 = Color.FromHex("#8AFF9B");
        public static Color textColor3 = Color.White;

        public static Color listBackground1 = Color.White;
        public static Color listBackground2 = Color.FromRgb(200, 200, 200);

        public static double fontSize1 = Device.GetNamedSize(NamedSize.Large, typeof(Label));
        public static double fontSize2 = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
        public static double fontSize3 = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        public static double fontSize4 = Device.GetNamedSize(NamedSize.Micro, typeof(Label));

        public static int padding = 10;

        public static int textListMargin = 3;

        public static int navigationBarPadding = 5;
        public static int navigationBarHeight = 30;

        public static string server_adress = "https://gusteausharp.azurewebsites.net/ingredient/suggest?search="; 
    }
}
