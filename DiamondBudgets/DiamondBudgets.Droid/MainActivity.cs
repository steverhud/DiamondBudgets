using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DiamondBudgets.Droid
{
    [Activity(Label = "Diamond Budgets", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            App.ScreenWidth = (Resources.DisplayMetrics.WidthPixels + 0.5F);

            global::Xamarin.Forms.Forms.Init(this, bundle);
//            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            App.UIContext = this;
            LoadApplication(new App());
        }
    }
}

