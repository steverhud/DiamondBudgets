using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace DiamondBudgets
{
    public partial class BudgetItemManager
    {
        static BudgetItemManager defaultInstance = new BudgetItemManager();
        MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<BudgetItem> budgetTable;
#else
        IMobileServiceTable<BudgetItem> budgetTable;
#endif

        private BudgetItemManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<BudgetItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.todoTable = client.GetSyncTable<BudgetItem>();
#else
            this.budgetTable = client.GetTable<BudgetItem>();
#endif
        }

        public static BudgetItemManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return budgetTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<BudgetItem>; }
        }

        public async Task<ObservableCollection<BudgetItem>> GetBudgetItemsAsync(bool syncItems = false, string entityType = "Budget",
            string category1 = "", string category2 = "")
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<BudgetItem> items;
                IEnumerable<BudgetItem> actuals;
                List<BudgetItem> returnItems = new List<BudgetItem>();

                if (entityType != "Budget")
                {
                    if (category2 != "" && category2 != null && category1 != "" && category1 != null)
                    {
                        items = await budgetTable
                            .Where(budgetItem => budgetItem.EntityType == entityType && 
                                        budgetItem.Category1 == category1 && budgetItem.Category2 == category2)
                            .OrderBy(bi => bi.Account)
                            .ToEnumerableAsync();
                    }
                    else
                    { 
                        if (category1 != "" && category1 != null)
                        {
                            items = await budgetTable
                                .Where(budgetItem => budgetItem.EntityType == entityType && budgetItem.Category1 == category1)
                                .OrderBy(bi => bi.Category2)
                                .ThenBy(bi => bi.Account)
                                .ToEnumerableAsync();
                        }
                        else
                        {
                            items = await budgetTable
                                .Where(budgetItem => budgetItem.EntityType == entityType)
                                .OrderBy(bi => bi.Category1)
                                .ThenBy(bi => bi.Category2)
                                .ThenBy(bi => bi.Account)
                                .ToEnumerableAsync();
                        }
                    }
                }
                else
                {
                    if (category2 != "" && category2 != null && category1 != "" && category1 != null)
                    {
                        items = await budgetTable
                            .Where(budgetItem => budgetItem.EntityType == "Budget" && 
                                        budgetItem.Category1 == category1 && budgetItem.Category2 == category2)
                            .ToEnumerableAsync();
                        actuals = await budgetTable
                            .Where(budgetItem => budgetItem.EntityType == "Actual" && 
                                        budgetItem.Category1 == category1 && budgetItem.Category2 == category2)
                            .ToEnumerableAsync();
                    }
                    else
                    { 
                        if (category1 != "" && category1 != null)
                        {
                            items = await budgetTable
                                .Where(budgetItem => budgetItem.EntityType == "Budget" && budgetItem.Category1 == category1)
                                .ToEnumerableAsync();
                            actuals = await budgetTable
                                .Where(budgetItem => budgetItem.EntityType == "Actual" && budgetItem.Category1 == category1)
                                .ToEnumerableAsync();
                        }
                        else
                        {
                            items = await budgetTable
                                .Where(budgetItem => budgetItem.EntityType == "Budget")
                                .OrderBy(BudgetItem => BudgetItem.Account)
                                .ToEnumerableAsync();
                            actuals = await budgetTable
                                .Where(budgetItem => budgetItem.EntityType == "Actual")
                                .OrderBy(BudgetItem => BudgetItem.Account)
                                .ToEnumerableAsync();
                        }
                    }
                    foreach (BudgetItem bi in items)
                    {
                        BudgetItem actual = actuals.FirstOrDefault(x => x.Account == bi.Account);
                        if (actual != null)
                        {
                            bi.ActualAmount = actual.Amount;
                        }
                        else
                            bi.ActualAmount = 0;

                        returnItems.Add(bi);
                    }

                }

                return new ObservableCollection<BudgetItem>(returnItems);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task SaveTaskAsync(BudgetItem item)
        {
            if (item.Id == null)
            {
                await budgetTable.InsertAsync(item);
            }
            else
            {
                await budgetTable.UpdateAsync(item);
            }
        }

#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.todoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allTodoItems",
                    this.todoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif
    }
}
