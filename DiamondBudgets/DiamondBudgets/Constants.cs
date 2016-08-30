using System;
using Xamarin.Forms;

namespace DiamondBudgets
{
    public static class Constants
    {
        // Replace strings with your mobile services and gateway URLs.
        public static string ApplicationURL = @"https://diamondbudgetapp.azurewebsites.net";

        public static Color PrimaryColor
        {
            get
            {
                return Color.FromHex("2196F3"); // Primary Color
            }
        }
        public static Color DarkPrimaryColor
        {
            get
            {
                return Color.FromHex("1976D2"); // Dark Primary Color
            }
        }
        public static Color LightPrimaryColor
        {
            get
            {
                return Color.FromHex("BBDEFB"); // Light Primary Color
            }
        }
        public static Color AccentColor
        {
            get
            {
                return Color.FromHex("FF5722"); // Accent Color
            }
        }
        public static Color DarkTextColor
        {
            get
            {
                return Color.FromHex("212121"); // Primary Text Color
            }
        }
        public static Color SecondaryTextColor
        {
            get
            {
                return Color.FromHex("757575"); // Secondary Text Color
            }
        }
        public static Color DividerColor
        {
            get
            {
                return Color.FromHex("BDBDBD"); // Divider Text Color
            }
        }
        public static Color LightTextColor
        {
            get
            {
                return Color.FromHex("FFFFFF"); // Icon Color
            }
        }

    }
}
