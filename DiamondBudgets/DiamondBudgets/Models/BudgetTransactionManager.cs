using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace DiamondBudgets
{
    public partial class BudgetTransactionManager
    {
        static BudgetTransactionManager defaultInstance = new BudgetTransactionManager();
        MobileServiceClient client;

        IMobileServiceTable<BudgetTransaction> transactionTable;

        private BudgetTransactionManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

            this.transactionTable = client.GetTable<BudgetTransaction>();
        }

        public static BudgetTransactionManager DefaultManager
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
            get { return transactionTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<BudgetTransaction>; }
        }

        public async Task<ObservableCollection<BudgetTransaction>> GetBudgetTransactionAsync(bool syncItems = false, string account = "")
        {
            try
            {
                IEnumerable<BudgetTransaction> items;
                if (account != "" && account != null)
                {
                    items = await transactionTable
                        .Where(budgetTrans => budgetTrans.Account == account)
                        .ToEnumerableAsync();
                }
                else
                {
                    items = await transactionTable
                        .Where(budgetTrans => budgetTrans.Account != "")
                        .ToEnumerableAsync();
                }
                return new ObservableCollection<BudgetTransaction>(items);
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

        public async Task SaveBudgetTransactionItemAsync(BudgetTransaction item)
        {
            if (item.Id == null)
            {
                await transactionTable.InsertAsync(item);
            }
            else
            {
                await transactionTable.UpdateAsync(item);
            }
        }

    }
}
