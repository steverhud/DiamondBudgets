using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DiamondBudgets
{
    public partial class BudgetList : ContentPage
    {
        BudgetItemManager manager;

        static string category1;
        public string Category1
        {
            get { return category1; }
            set { category1 = value; }
        }

        static string category2;
        public string Category2
        {
            get { return category2; }
            set { category2 = value; }
        }

        public MasterDetailPage master { get; set; }

        public static string PageTitle
        {
            get
            {
                string pageTitle;
                if (category2 != "" && category2 != null)
                    pageTitle = category2;
                else if (category1 != "" && category1 != null)
                    pageTitle = category1;
                else
                    pageTitle = "Budget List";

                return pageTitle;
            }

        }

        public static string Column1 { get { return (App.ScreenWidth * .5).ToString(); }}
        public static string Column2 { get { return (App.ScreenWidth * .25).ToString(); } }
        public static string Column3 { get { return (App.ScreenWidth * .25).ToString(); } }

        public BudgetList()
        {
            InitializeComponent();

            manager = BudgetItemManager.DefaultManager;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Set syncItems to true in order to synchronize the data on startup when running in offline mode
                await RefreshItems(false, syncItems: false);
            }
            catch (FormatException e)
            {
                await DisplayAlert("OnAppearing Error", e.Message + " --- " + e.InnerException, "OK");
            }
            catch (Exception e)
            {
                await DisplayAlert("OnAppearing Error", e.Message + " --- " + e.InnerException, "OK");
            }
        }

        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false, true);
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
                    this.Title = PageTitle;
                    IEnumerable<BudgetItem> budgets = await manager.GetBudgetItemsAsync(syncItems, "Budget", category1, category2);

                    List<BudgetBar> newBudgetList = new List<BudgetBar>();
                    foreach (BudgetItem budget in budgets)
                    {
                        Color textColor;
                        if (Math.Abs(budget.Amount) < Math.Abs(budget.ActualAmount))
                            textColor = Color.Red;
                        else
                            textColor = Color.Black;

                        newBudgetList.Add(new BudgetBar
                        {
                            Account = budget.Account,
                            Description = budget.Description,
                            Amount = budget.Amount.ToString("C"),
                            ActualAmount = budget.ActualAmount.ToString("C"),
                            ActualAmountColor = textColor
                        });
                    }

                    DataTemplate dt = new DataTemplate(typeof(BudgetCell));
                    budgetList.ItemsSource = newBudgetList.OrderBy(x => x.Description);
                    budgetList.ItemTemplate = dt;
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("RefreshItems Error", e.Message + " --- " + e.InnerException, "OK");
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
            var item = e.SelectedItem as BudgetBar;
            NavigationPage transactionList = new NavigationPage(new TransactionsList() { account = item.Account }) { BarBackgroundColor = Constants.DarkPrimaryColor };
            master.Detail = transactionList;

        }
    }

    public class BudgetBar
    {
        public string Account { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string ActualAmount { get; set; }
        public Color ActualAmountColor { get; set; }
    }
}
