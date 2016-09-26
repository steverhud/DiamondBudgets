using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DiamondBudgets
{
    public partial class BudgetCatagoryList : ContentPage
    {
        BudgetCategoryManager manager;

        public MasterDetailPage master { get; set; }


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

                foreach(BudgetCategory bc in budgets)
                {
                    BudgetCategory actual = actuals.FirstOrDefault(x => x.Category == bc.Category);
                    if (actual != null)
                    {
                        //TODO: Decide if this should be budget to date percentage or YTD.
                        bc.Amount = (actual.Amount / (bc.Amount/12*9)) * 100; //This line is calcluating the percentageb bases on 9 months of budget
                        if (bc.Amount < 0)
                            bc.Amount = bc.Amount * -1;
                    }
                    else
                        bc.Amount = 0;
                }

                List<PercentageBarValue> PercentageBars = new List<PercentageBarValue>();
                foreach(BudgetCategory budget in budgets)
                {
                    decimal newPercentage = Math.Round((budget.Amount / 100), 2);
                    PercentageBars.Add(new PercentageBarValue
                    {
                        BarLabel = budget.Category,
                        Percentage = newPercentage,
                        Image = MakeImage(newPercentage)
                    });
                }

                DataTemplate dt = new DataTemplate(typeof(PercentageBar));
                //categoryList.ItemsSource = PercentageBars;
                categoryList.ItemsSource = new ObservableCollection<PercentageBarValue>(PercentageBars);
                categoryList.ItemTemplate = dt;
                categoryList.SeparatorVisibility = SeparatorVisibility.None;
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
            var item = e.SelectedItem as PercentageBarValue;
            NavigationPage budgetList = new NavigationPage(new BudgetList() { Category = item.BarLabel, master = master }) { BarBackgroundColor = Constants.DarkPrimaryColor};
            master.Detail = budgetList;
        }

        public ImageSource MakeImage(decimal percentage)
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

            cols = Convert.ToInt32(Math.Round((decimal)width * percentage, 0));

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
    public class PercentageBarValue
    {
        public string BarLabel { get; set; }
        public decimal Percentage { get; set; }
        public ImageSource Image { get; set; }
    }
}
