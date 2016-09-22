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
        Image myImage;
        Label barAmount;
        Label barLabel;
        decimal percentage = new decimal();

        public ListView Host { get; set; }

        public PercentageBar()
        {
            Grid myGrid = new Grid
            {
                //Padding = new Thickness(0, 5, 0, 0),
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(10, GridUnitType.Auto) },
                    new RowDefinition {Height = new GridLength(10, GridUnitType.Auto) },
                },

                HorizontalOptions = LayoutOptions.Fill,
            };

            RelativeLayout layout = new RelativeLayout()
            {
//                HorizontalOptions = LayoutOptions.Fill,
            };

            barLabel = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            barAmount = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.Blue,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center
            };

            myImage = new Image()
            {
                Source = MakeImage(percentage),
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };

            barLabel.SetBinding(Label.TextProperty, "BarLabel");
            barAmount.SetBinding(Label.TextProperty, "Percentage");

            layout.Children.Add(myImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            layout.Children.Add(barLabel,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            layout.Children.Add(barAmount,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            myGrid.Children.Add(layout, 0, 1);

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

                MakeBar();
            }
        }

        private void MakeBar()
        {
            myImage.Source = MakeImage(percentage);
        }

        private ImageSource MakeImage(decimal percentage)
        {

            //double width = App.ScreenWidth * 2;
            decimal width = Convert.ToDecimal(App.ScreenWidth);

            if (percentage > 1)
                percentage = 1;

            int rows = 128;
            int cols = 64;
            int red = 0;
            //int green = 255;
            int green = 256;

            cols = Convert.ToInt32(Math.Round((decimal)width * percentage,0));

            BmpMaker bmpMaker = new BmpMaker(cols, rows);

            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    bmpMaker.SetPixel(row, col, red, green, 0);
                }

                red = (Convert.ToInt32(((double)col / (double)width) * 128)) * 2 - 1;
                //green = 256 - red;
                //green = 2 * (128 - Convert.ToInt32((col / width) * 128));
                double greenAlter = Math.Sin((1 - (double)col / (double)width) * Math.PI / 2) * 255;
                green = Convert.ToInt32(greenAlter);
            }
            ImageSource imageSource = bmpMaker.Generate();
            return imageSource;
        }

    }
}
