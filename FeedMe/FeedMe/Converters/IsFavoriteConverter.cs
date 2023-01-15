﻿using System.Globalization;

namespace FeedMe.Converters;

public class IsFavoriteConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var favorite = (bool)value;
        return favorite ? "md-favorite" : "md-favorite-border";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}