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
        decimal actualAmount;
        string budgetId;
        string category1;
        string category2;
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

        public decimal ActualAmount
        {
            get { return actualAmount; }
            set { actualAmount = value; }
        }

        public bool ActualOverBudget
        {
            get { return Math.Abs(actualAmount) >= Math.Abs(amount); }
        }

        [JsonProperty(PropertyName = "BudgetId")]
        public string BudgetId
        {
            get { return budgetId; }
            set { budgetId = value; }
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
