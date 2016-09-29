using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace DiamondBudgets
{
    public class BudgetTransaction
    {

        string id;
        string tenantID;
        string account;
        int journalEntry;
        string description;
        string transaction;
        string masterID;
        string transactionDate;
        decimal amount;
        decimal debitAmount;
        decimal creditAmount;

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

        [JsonProperty(PropertyName = "JournalEntry")]
        public int JournalEntry
        {
            get { return journalEntry; }
            set { journalEntry = value; }
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

        [JsonProperty(PropertyName = "Transaction")]
        public string Transaction
        {
            get { return transaction; }
            set { transaction = value; }
        }

        [JsonProperty(PropertyName = "MasterID")]
        public string MasterID
        {
            get { return masterID; }
            set { masterID = value; }
        }

        [JsonProperty(PropertyName = "TransactionDate")]
        public string TransactionDate
        {
            get { return transactionDate; }
            set { transactionDate = value; }
        }

        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [JsonProperty(PropertyName = "DebitAmount")]
        public decimal DebitAmount
        {
            get { return debitAmount; }
            set { debitAmount = value; }
        }

        [JsonProperty(PropertyName = "CreditAmount")]
        public decimal CreditAmount
        {
            get { return creditAmount; }
            set { creditAmount = value; }
        }

        [Version]
        public string Version { get; set; }

    }
}