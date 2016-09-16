using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DiamondBudgets
{
    public partial class App : Application
    {
        public UserAppSettings userAppSettings;

        public App()
        {
            InitializeComponent();

            userAppSettings = new UserAppSettings();
            userAppSettings.RestoreState(Current.Properties);

            var md = new MasterDetailPage();

            md.Master = new MenuPage(md, userAppSettings);
            md.Detail = new NavigationPage(new BudgetCatagoryList()) { BarBackgroundColor = Constants.DarkPrimaryColor };

            MainPage = md;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            userAppSettings.SaveState(Current.Properties);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        public static object UIContext { get; set; }

        public static double ScreenWidth { get; set; }

    }
}
