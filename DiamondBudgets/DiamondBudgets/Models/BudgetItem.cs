using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace DiamondBudgets
{
    public class BudgetItem
    {
        string id;
        string account;
        string description;
        decimal amount;
        string budgetId;
        string category;
        int year;
        int period;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "account")]
        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        [JsonProperty(PropertyName = "description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [JsonProperty(PropertyName = "budgetId")]
        public string BudgetId
        {
            get { return budgetId; }
            set { budgetId = value; }
        }

        [JsonProperty(PropertyName = "category")]
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        [JsonProperty(PropertyName = "year")]
        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        [JsonProperty(PropertyName = "period")]
        public int Period
        {
            get { return period; }
            set { period = value; }
        }

        [Version]
        public string Version { get; set; }

    }
}
