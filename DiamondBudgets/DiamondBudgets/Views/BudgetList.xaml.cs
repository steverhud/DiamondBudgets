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

        static string category;
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public MasterDetailPage master { get; set; }

        public static string PageTitle
        {
            get
            {
                string pageTitle;
                if (category != "" && category != null)
                    pageTitle = "Budget List - " + category;
                else
                    pageTitle = "Budget List";

                return pageTitle;
            }

        }

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
                await RefreshItems(true, syncItems: false);
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
                    budgetList.ItemsSource = await manager.GetBudgetItemsAsync(syncItems, "Budget", category);
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
            var item = e.SelectedItem as BudgetItem;
            NavigationPage transactionList = new NavigationPage(new TransactionsList() { account = item.Account }) { BarBackgroundColor = Constants.DarkPrimaryColor };
            master.Detail = transactionList;

        }
    }
}
