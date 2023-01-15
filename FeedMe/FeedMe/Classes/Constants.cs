namespace FeedMe.Classes
{
    internal abstract class Constants
    {
        public abstract class AppColor
        {
            public static Color BackgroundColor = Color.FromHex("#F2F2F2");
            public static Color NavigationBarColor = Color.FromHex("#00CC66");

            public static readonly Color Green = Color.FromHex("#00CC66");
            public static Color White = Color.FromHex("#FFFFFF");
            public static Color Gray = Color.FromHex("#777777");
            public static readonly Color LightGray = Color.FromHex("#E6E6E6");

            public static readonly Color TextDefault = Color.FromHex("#282828");

            public static readonly Color TextGreen = Color.FromHex("#00CC66");
            public static readonly Color TextBlack = Color.FromHex("#202020");
            public static readonly Color TextGray = Color.FromHex("#777777");
            public static readonly Color TextWhite = Colors.White;
            public static readonly Color TextLink = Color.FromHex("#0000EE");
        }

        public const int TextHeight = 50;

        public static readonly double FontSize1double = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2;
        public static double fontSize1 = Device.GetNamedSize(NamedSize.Large, typeof(Label));
        public static double fontSize2 = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
        public static double fontSize3 = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        public static double fontSize4 = Device.GetNamedSize(NamedSize.Micro, typeof(Label));

        public static int CornerRadius1 = 15;
        public static int CornerRadius2 = 5;


        public static int Padding1 = 20;
        public const int Padding2 = 15;
        public const int Padding3 = 10;

        public const int TextListMargin = 3;

        public const string AddIngredientCheckIcon = "md-check-box-outline-blank";
        public const string DeleteIngredientCheckIcon = "md-check-box";
        public const string ExcludeIngredientCheckIcon = "md-block";
        public static string AddIngredientIcon = "md-add";
        public const string DeleteIngredientIcon = "md-clear";
    }
}
