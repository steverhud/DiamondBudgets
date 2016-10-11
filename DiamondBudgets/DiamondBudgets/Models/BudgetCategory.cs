using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace DiamondBudgets
{
    public class BudgetCategory
    {

        string id;
        string tenantID;
        string entityType;
        decimal amount;
        string category1;
        string category2;
        int budgetYear;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "TenantID")]
        public string TenantID
        {
            get { return tenantID; }
            set { tenantID = value; }
        }

        [JsonProperty(PropertyName = "EntityType")]
        public string EntityType
        {
            get { return entityType; }
            set { entityType = value; }
        }

        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [JsonProperty(PropertyName = "Category1")]
        public string Category1
        {
            get { return category1; }
            set { category1 = value; }
        }

        [JsonProperty(PropertyName = "Category2")]
        public string Category2
        {
            get { return category2; }
            set { category2 = value; }
        }

        [JsonProperty(PropertyName = "BudgetYear")]
        public int BudgetYear
        {
            get { return budgetYear; }
            set { budgetYear = value; }
        }

        [Version]
        public string Version { get; set; }

    }
}
