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
        string category;
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

        [JsonProperty(PropertyName = "category")]
        public string Category
        {
            get { return category; }
            set { category = value; }
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
