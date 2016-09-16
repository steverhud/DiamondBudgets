using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DiamondBudgets
{
    public partial class BudgetCatagoryList : ContentPage
    {
        BudgetCategoryManager manager;

        public BudgetCatagoryList()
        {
            InitializeComponent();

            manager = BudgetCategoryManager.DefaultManager;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            await RefreshItems(true, syncItems: false);
        }

        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false, false);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }
        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                IEnumerable<BudgetCategory> budgets = await manager.GetBudgeCategorysAsync(entityType: "Budget");
                IEnumerable<BudgetCategory> actuals = await manager.GetBudgeCategorysAsync(entityType: "Actual");
                //List<BudgetCategory> budgetList = new List<BudgetCategory>(actuals);

                foreach(BudgetCategory bc in budgets)
                {
                    BudgetCategory actual = actuals.FirstOrDefault(x => x.Category == bc.Category);
                    if (actual != null)
                    {
                        bc.Amount = (actual.Amount / bc.Amount) * 100;
                        if (bc.Amount < 0)
                            bc.Amount = bc.Amount * -1;
                    }
                    else
                        bc.Amount = 0;
                }

                //categoryList.ItemsSource = budgets.OrderByDescending(s => s.Amount);

                //DepartmentAL.Children.Add(new Label { Text = "Hello" });
                var i = 0;
                //foreach(BudgetCategory budget in budgets.OrderByDescending(s => s.Amount))
                foreach(BudgetCategory budget in budgets)
                {
                    AddBar(Math.Round((budget.Amount/100),2), i, budget.Category);
                    i++;
                    //if (i == 10)
                    //    break;
                }
            }
        }
        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var todo = e.SelectedItem as TodoItem;
            if (Device.OS != TargetPlatform.iOS && todo != null)
            {
                // Not iOS - the swipe-to-delete is discoverable there
                if (Device.OS == TargetPlatform.Android)
                {
                    await DisplayAlert(todo.Name, "Press-and-hold to complete task " + todo.Name, "Got it!");
                }
                else
                {
                    // Windows, not all platforms support the Context Actions yet
                    if (await DisplayAlert("Mark completed?", "Do you wish to complete " + todo.Name + "?", "Complete", "Cancel"))
                    {
                        //await CompleteItem(todo);
                    }
                }
            }

            // prevents background getting highlighted
            //categoryList.SelectedItem = null;
        }

        private void AddBar(decimal percentage, Int32 position, string barLabel)
        {
            Image image;
            StackLayout stackLayout;
            Label label;
            ContentView contentView;
            double layoutPostition = position * .095;

            image = new Image { HorizontalOptions = LayoutOptions.Start };
            image.Source = MakeImage(percentage);
            stackLayout = new StackLayout();
            stackLayout.Children.Add(image);
            DepartmentAL.Children.Add(stackLayout, new Rectangle(0, layoutPostition, 1, 0.08), AbsoluteLayoutFlags.All);
            stackLayout = null;
            image = null;

            label = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                Text = barLabel,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Constants.DarkTextColor,
            };
            contentView = new ContentView { Content = label };
            DepartmentAL.Children.Add(contentView, new Rectangle(0, layoutPostition + 0.025, 1, 0.08), AbsoluteLayoutFlags.All);
            contentView = null;
            label = null;

            string strPercent = string.Format("{0:#,#.##}", (percentage * 100)) + "%";
            label = new Label
            {
                HorizontalOptions = LayoutOptions.End,
                Text = strPercent,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Constants.DarkTextColor,
            };
            contentView = new ContentView { Content = label };
            DepartmentAL.Children.Add(contentView, new Rectangle(0, layoutPostition + 0.025, 1, 0.08), AbsoluteLayoutFlags.All);
            contentView = null;
            label = null;

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
            int green = 255;

            cols = Convert.ToInt32(width * percentage);
            int greenThreshold = Convert.ToInt32(Math.Round(cols * .85, 0));

            BmpMaker bmpMaker = new BmpMaker(cols, rows);

            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    bmpMaker.SetPixel(row, col, red, green, 0);
                }

                red = Convert.ToInt32((col / width) * 255);
                //if (col > greenThreshold)
                    green = 255 - red;
            }
            ImageSource imageSource = bmpMaker.Generate();
            return imageSource;
        }

    }
}
