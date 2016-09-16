using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace DiamondBudgets
{
    public class BudgetItem
    {

        string id;
        string tenantID;
        string entityType;
        string account;
        string description;
        decimal amount;
        string budgetId;
        string category;
        int budgetYear;
        int budgetPeriod;

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

        [JsonProperty(PropertyName = "Account")]
        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        [JsonProperty(PropertyName = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [JsonProperty(PropertyName = "BudgetId")]
        public string BudgetId
        {
            get { return budgetId; }
            set { budgetId = value; }
        }

        [JsonProperty(PropertyName = "Category")]
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

        [JsonProperty(PropertyName = "BudgetPeriod")]
        public int BudgetPeriod
        {
            get { return budgetPeriod; }
            set { budgetPeriod = value; }
        }

        [Version]
        public string Version { get; set; }

    }
}
