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

                if (budgets != null)
                {
                    foreach (BudgetCategory bc in budgets)
                    {
                        BudgetCategory actual = actuals.FirstOrDefault(x => x.Category1 == bc.Category1);
                        if (actual != null)
                        {
                            bc.Amount = (actual.Amount / (bc.Amount)) * 100;
                            if (bc.Amount < 0)
                                bc.Amount = bc.Amount * -1;
                        }
                        else
                            bc.Amount = 0;
                    }

                    List<PercentageBarValue> PercentageBars = new List<PercentageBarValue>();
                    foreach (BudgetCategory budget in budgets)
                    {
                        decimal newPercentage = Math.Round((budget.Amount / 100), 2);
                        //Color barColor;
                        //if (newPercentage >= (decimal)1.0)
                        //    barColor = Color.Red;
                        //else if (newPercentage >= (decimal)0.85)
                        //    barColor = Color.Yellow;
                        //else
                        //    barColor = Color.Green;

                        string barColor;
                        if (newPercentage >= (decimal)1.0)
                            barColor = "RedBar.png";
                        else if (newPercentage >= (decimal)0.85)
                            barColor = "YellowBar1.png";
                        else
                            barColor = "GreenBar.png";

                        decimal barWidth = Math.Round(Convert.ToDecimal(App.ScreenWidth) * newPercentage, 0);

                        //barWidth = 1000 * newPercentage;

                        PercentageBars.Add(new PercentageBarValue
                        {
                            BarLabel = budget.Category1,
                            Percentage = newPercentage,
                            BarColor = barColor,
                            BarWidth = barWidth,
                        });
                    }

                    DataTemplate dt = new DataTemplate(typeof(PercentageBar));
                    var sortedList = new ObservableCollection<PercentageBarValue>(PercentageBars).OrderByDescending(x => x.Percentage);
                    categoryList.ItemsSource = sortedList;
                    categoryList.ItemTemplate = dt;
                    categoryList.SeparatorVisibility = SeparatorVisibility.None;
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
            try
            {
                var item = e.SelectedItem as PercentageBarValue;
                NavigationPage budgetList = new NavigationPage(new Catagory2List() { Category1 = item.BarLabel, master = master }) { BarBackgroundColor = Constants.DarkPrimaryColor };
                //Navigation.PushAsync(new Catagory2List() { Category1 = item.BarLabel, master = master }).Wait();
                master.Detail = budgetList;
            }
            catch (Exception ex)
            { await DisplayAlert("Selection Error", "Error occurred during selection: " + ex.Message, "OK"); }
        }
    }
    public class PercentageBarValue
    {
        public string BarLabel { get; set; }
        public decimal Percentage { get; set; }
        //public Color BarColor { get; set;}
        public string BarColor { get; set; }
        public decimal BarWidth { get; set; }
    }
}
