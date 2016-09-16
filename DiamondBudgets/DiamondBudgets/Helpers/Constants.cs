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
                return Color.FromHex("FFFFFF"); // Primary Color
            }
        }
        public static Color DarkPrimaryColor
        {
            get
            {
                return Color.FromHex("F7941D"); // Dark Primary Color
            }
        }
        public static Color LightPrimaryColor
        {
            get
            {
                return Color.FromHex("FF961E"); // Light Primary Color
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
                return Color.FromHex("414143"); // Primary Text Color
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
