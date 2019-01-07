using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FeedMe.User
{
    class User
    {
        private static ISettings AppSettings =>
    CrossSettings.Current;

        public static string SavedIngredinets
        {
            get => AppSettings.GetValueOrDefault(nameof(SavedIngredinets), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(SavedIngredinets), value);
        }
    }
}
