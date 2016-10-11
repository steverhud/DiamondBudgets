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
    public partial class BudgetCategory2Manager
    {
        static BudgetCategory2Manager defaultInstance = new BudgetCategory2Manager();
        MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<BudgetCategory> budgetCategoryTable;
#else
        IMobileServiceTable<BudgetCategory2> budgetCategoryTable;
#endif

        private BudgetCategory2Manager()
        {
            this.client = new MobileServiceClient(
                Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<BudgetItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.todoTable = client.GetSyncTable<BudgetCategory>();
#else
            this.budgetCategoryTable = client.GetTable<BudgetCategory2>();
#endif
        }

        public static BudgetCategory2Manager DefaultManager
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
            get { return budgetCategoryTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<BudgetCategory2>; }
        }

        public async Task<ObservableCollection<BudgetCategory2>> GetBudgeCategorysAsync(bool syncItems = false, 
            string entityType = "Budget", string category1 = "")
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<BudgetCategory2> items;

                if (category1 != null && category1 != "")
                {
                    items = await budgetCategoryTable
                        .Where(budgetItem => budgetItem.EntityType == entityType && budgetItem.Category1 == category1)
                        .ToEnumerableAsync();
                }
                else
                {
                    items = await budgetCategoryTable
                        .Where(budgetItem => budgetItem.EntityType == entityType)
                        .ToEnumerableAsync();
                }

                return new ObservableCollection<BudgetCategory2>(items);
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

        public async Task SaveTaskAsync(BudgetCategory2 item)
        {
            if (item.Id == null)
            {
                await budgetCategoryTable.InsertAsync(item);
            }
            else
            {
                await budgetCategoryTable.UpdateAsync(item);
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
