using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DiamondBudgets
{
    public class PercentageBar : ViewCell
    {
        Label barAmount;
        Label barLabel;
        decimal percentage = new decimal();

        public ListView Host { get; set; }

        public PercentageBar()
        {
            ContextActions.Add(new MenuItem() { Text = "% Complete" });
            ContextActions.Add(new MenuItem() { Text = "Variance" });
            ContextActions.Add(new MenuItem() { Text = "YTD Actual" });
            ContextActions.Add(new MenuItem() { Text = "Budget Amount" });

            Grid myGrid = new Grid
            {
                //Padding = new Thickness(0, 5, 0, 0),
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(10, GridUnitType.Auto) },
                    new RowDefinition {Height = new GridLength(10, GridUnitType.Star) },
                },

                HorizontalOptions = LayoutOptions.Fill,
            };

            Grid innerGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(App.ScreenWidth * .75, GridUnitType.Star) },
                    new ColumnDefinition {Width = new GridLength(App.ScreenWidth * .25, GridUnitType.Star) },
                },
                VerticalOptions = LayoutOptions.Fill,
            };

            barLabel = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
            };

            barAmount = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
            };

            barLabel.SetBinding(Label.TextProperty, "BarLabel");
            barAmount.SetBinding(Label.TextProperty, "Percentage");
            innerGrid.SetBinding(StackLayout.BackgroundColorProperty, "BarColor");

            innerGrid.Children.Add(barLabel, 0, 0);
            innerGrid.Children.Add(barAmount, 1, 0);
            myGrid.Children.Add(innerGrid, 0, 1);

            View = myGrid;
        }

        //protected override void OnTapped()
        //{
        //    base.OnTapped();
        //    Host.SelectedItem.ToString();
        //}

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                percentage = Convert.ToDecimal(barAmount.Text);
                if (percentage == 0)
                    barAmount.Text = "0%";
                else
                    barAmount.Text = string.Format("{0:#,#.##}", (percentage * 100)) + "%";

            }
        }
    }
}
