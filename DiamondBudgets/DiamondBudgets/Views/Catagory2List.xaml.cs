using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DiamondBudgets
{
    public partial class Catagory2List : ContentPage
    {
        BudgetCategory2Manager manager;

        public MasterDetailPage master { get; set; }

        static string category1;
        public string Category1
        {
            get { return category1; }
            set { category1 = value; }
        }

        public Catagory2List()
        {
            InitializeComponent();
            manager = BudgetCategory2Manager.DefaultManager;
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
            try
            {

                using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
                {
                    IEnumerable<BudgetCategory2> budgets = await manager.GetBudgeCategorysAsync(entityType: "Budget", category1: category1);
                    IEnumerable<BudgetCategory2> actuals = await manager.GetBudgeCategorysAsync(entityType: "Actual", category1: category1);

                    if (budgets != null)
                    {
                        foreach (BudgetCategory2 bc in budgets)
                        {
                            BudgetCategory2 actual = actuals.FirstOrDefault(x => x.Category1 == bc.Category1 && x.Category2 == bc.Category2);
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
                        foreach (BudgetCategory2 budget in budgets)
                        {
                            decimal newPercentage = Math.Round((budget.Amount / 100), 2);
                            Color barColor;
                            if (newPercentage >= (decimal)1.0)
                                barColor = Color.Red;
                            else if (newPercentage >= (decimal)0.85)
                                barColor = Color.Yellow;
                            else
                                barColor = Color.Green;

                            PercentageBars.Add(new PercentageBarValue
                            {
                                BarLabel = budget.Category2,
                                Percentage = newPercentage,
                                BarColor = barColor,
                            });
                        }

                        PercentageBars.OrderByDescending(pb => pb.Percentage);

                        DataTemplate dt = new DataTemplate(typeof(PercentageBar));
                        category2List.ItemsSource = new ObservableCollection<PercentageBarValue>(PercentageBars);
                        category2List.ItemTemplate = dt;
                        category2List.SeparatorVisibility = SeparatorVisibility.None;
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
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
            NavigationPage budgetList = new NavigationPage(new BudgetList()
                    { Category1 = this.Category1, Category2 = item.BarLabel, master = master })
                { BarBackgroundColor = Constants.DarkPrimaryColor };
            master.Detail = budgetList;
        }
    }
}
