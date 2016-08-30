using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace DiamondBudgets
{
    public class UserAppSettings : ViewModelBase
    {
        bool showNotifications;
        bool savePassword;
        string userID;
        string userPassword;

        public bool ShowNotifications
        {
            get { return showNotifications; }
            set { SetProperty(ref showNotifications, value); }
        }

        public bool SavePassword
        {
            get { return savePassword; }
            set { SetProperty(ref savePassword, value); }
        }

        public string UserID
        {
            get { return userID; }
            set { SetProperty(ref userID, value); }
        }

        public string UserPassword
        {
            get { return userPassword; }
            set { SetProperty(ref userPassword, value); }
        }

        public void SaveState(IDictionary<string, object> dictionary)
        {
            dictionary["ShowNotifications"] = ShowNotifications;
            dictionary["SavePassword"] = SavePassword;
            dictionary["UserID"] = UserID;
            dictionary["UserPassword"] = UserPassword;
        }

        public void RestoreState(IDictionary<string, object> dictionary)
        {
            ShowNotifications = GetDictionaryEntry(dictionary, "ShowNotifications", false);
            SavePassword = GetDictionaryEntry(dictionary, "SavePassword", false);
            UserID = GetDictionaryEntry(dictionary, "UserID", "");
            UserPassword = GetDictionaryEntry(dictionary, "UserPassword", "");
        }

        public T GetDictionaryEntry<T>(IDictionary<string, object> dictionary,
                                        string key, T defaultValue)
        {
            if (dictionary.ContainsKey(key))
                return (T)dictionary[key];

            return defaultValue;
        }

    }

    public class AdminAppSettings
    {

    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
