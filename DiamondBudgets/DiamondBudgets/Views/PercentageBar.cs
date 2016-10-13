using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DiamondBudgets
{
    public class PercentageBar : ViewCell
    {
        Image myImage = new Image();
        Label barAmount;
        Label barLabel;
        decimal percentage = new decimal();

        public ListView Host { get; set; }

        public static BindableProperty ItemClickCommandProperty =
            BindableProperty.CreateAttached(
            "Command",
            typeof(ICommand),
            typeof(PercentageBar),
            null,
            propertyChanged: OnCommandChanged);

        //BindableProperty.Create<PercentageBar, ICommand>(x => x.ItemClickCommand, null);

        public PercentageBar()
        {
            //var tapGesture = new TapGestureRecognizer();
            //tapGesture.Tapped += TapGesture_Tapped;

            //var panGesture = new PanGestureRecognizer();
            //panGesture.PanUpdated += (s, e) =>
            //{
            //    double x;
            //    if (Math.Abs(e.TotalX) > 4)
            //    {
            //        if (e.TotalX < 0)
            //        {
            //            //Pan Left
            //            x = e.TotalX;
            //        }
            //        else
            //        {
            //            //Pan Right
            //            x = e.TotalX;
            //        }
            //    }
            //};

            Grid myGrid = new Grid
            {
                Padding = new Thickness(0, 0, 5, 0),
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

            myImage.Source = "GreenBar.png";
            myImage.HorizontalOptions = LayoutOptions.Start;
            myImage.Aspect = Aspect.Fill;
            myImage.MinimumHeightRequest = 10;
            //myImage.WidthRequest = App.ScreenWidth * .25;

            barLabel.SetBinding(Label.TextProperty, "BarLabel");
            barAmount.SetBinding(Label.TextProperty, "Percentage");
            //innerGrid.SetBinding(StackLayout.BackgroundColorProperty, "BarColor");
            innerGrid.SetBinding(StackLayout.WidthProperty, "BarWidth");
            myImage.SetBinding(Image.WidthRequestProperty, "BarWidth");
            myImage.SetBinding(Image.SourceProperty, "BarColor");

            innerGrid.Children.Add(barLabel, 0, 0);
            innerGrid.Children.Add(barAmount, 1, 0);
            myGrid.Children.Add(myImage, 0, 1);
            myGrid.Children.Add(innerGrid, 0, 1);

            //myGrid.GestureRecognizers.Add(panGesture);
            //barLabel.GestureRecognizers.Add(tapGesture);

            View = myGrid;
        }

        private void TapGesture_Tapped(object sender, EventArgs e)
        {
            var item = sender as Label;
            
        }

        //protected override void OnTapped()
        //{
        //    base.OnTapped();
        //    Host.SelectedItem.ToString();
        //}

        public ICommand ItemClickCommand
        {
            get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
            set { this.SetValue(ItemClickCommandProperty, value); }
        }

        static void OnCommandChanged(BindableObject view, object oldValue, object newValue)
        {
            var entry = view as ListView;
            if (entry == null)
                return;

            entry.ItemTapped += (sender, e) =>
            {
                var command = (newValue as ICommand);
                if (command == null)
                    return;

                if (command.CanExecute(e.Item))
                {
                    command.Execute(e.Item);
                }

            };
        }

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
