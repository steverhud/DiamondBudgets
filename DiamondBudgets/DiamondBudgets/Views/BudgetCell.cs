using System;
using Xamarin.Forms;

namespace DiamondBudgets
{
    public class BudgetCell : ViewCell
    {
        public BudgetCell()
        {
            Grid layoutGrid = new Grid()
            {
                RowDefinitions =
                        {
                            new RowDefinition { Height = new GridLength(10, GridUnitType.Star) },
                            new RowDefinition { Height = new GridLength(10, GridUnitType.Star) }
                        },

                ColumnDefinitions =
                        {
                            new ColumnDefinition {Width = new GridLength(App.ScreenWidth * .50, GridUnitType.Star) },
                            new ColumnDefinition {Width = new GridLength(App.ScreenWidth * .25, GridUnitType.Star) },
                            new ColumnDefinition {Width = new GridLength(App.ScreenWidth * .25, GridUnitType.Star) },
                        },
            };

            Label accountDescription = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Start,
            };
            accountDescription.SetBinding(Label.TextProperty, "Description");

            Label account = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Start
            };
            account.SetBinding(Label.TextProperty, "Account");

            Label budgetAmount = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.End,
            };
            budgetAmount.SetBinding(Label.TextProperty, "Amount");

            Label actualAmount = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.End,
            };
            actualAmount.SetBinding(Label.TextProperty, "ActualAmount");
            actualAmount.SetBinding(Label.TextColorProperty, "ActualAmountColor");

            layoutGrid.Children.Add(accountDescription, 0, 0);
            layoutGrid.Children.Add(account, 0, 1);
            layoutGrid.Children.Add(budgetAmount, 1, 0);
            layoutGrid.Children.Add(actualAmount, 2, 0);

            View = layoutGrid;
        }
    }
}
