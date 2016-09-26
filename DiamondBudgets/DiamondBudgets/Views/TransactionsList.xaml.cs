using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DiamondBudgets
{
    public partial class TransactionsList : ContentPage
    {
        BudgetTransactionManager manager;

        public string account { get; set; }

        public TransactionsList()
        {
            InitializeComponent();

            manager = BudgetTransactionManager.DefaultManager;
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
                    transactionList.ItemsSource = await manager.GetBudgetTransactionAsync(syncItems, account);
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
            transactionList.SelectedItem = null;
        }
    }
}
