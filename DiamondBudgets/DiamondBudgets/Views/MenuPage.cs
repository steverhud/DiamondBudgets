﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace DiamondBudgets
{
    public class MenuTableView : TableView
    { }

    public class MenuPage : ContentPage
    {
        MasterDetailPage master;
        UserAppSettings uas;
        TableView tableView;

        public MenuPage(MasterDetailPage m, UserAppSettings userAppSettings)
        {
            master = m;
            uas = userAppSettings;

            Title = "Diamond Budget App";
            Icon = "slideout.png";

            var section = new TableSection()
            {
                new MenuCell {Text = "Budget List", Host = this },
                new MenuCell {Text = "Budget Summary - by Category", Host = this },
                new MenuCell {Text = "Settings", Host = this },
            };

            var root = new TableRoot() { section };

            tableView = new MenuTableView()
            {
                Root = root,
                Intent = TableIntent.Menu
            };

            Content = new StackLayout
            {
                BackgroundColor = Constants.PrimaryColor,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {tableView}
            };
        }

        NavigationPage budgetList, settings, budgetChart;
        public void Selected(string item)
        {
            switch(item)
            {
                case "Budget List":
                    if (budgetList == null)
                        budgetList = new NavigationPage(new BudgetList())
                        {
                            BarBackgroundColor = Constants.DarkPrimaryColor
                        };
                    master.Detail = budgetList;
                    break;

                case "Budget Summary - by Category":
                    if (budgetChart == null)
                        budgetChart = new NavigationPage(new BudgetCategorySummary())
                        {
                            BarBackgroundColor = Constants.DarkPrimaryColor
                        };
                    master.Detail = budgetChart;
                    break;

                case "Settings":
                    if (settings == null)
                        settings = new NavigationPage(new SettingsTabbed())
                        {
                            BarBackgroundColor = Constants.DarkPrimaryColor,
                            BindingContext = uas
                        };
                    master.Detail = settings;
                    break;

            };

            master.IsPresented = false;
        }
    }
}