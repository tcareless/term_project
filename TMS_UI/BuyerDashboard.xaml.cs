﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
//using term_project.DataAccess;   // Assuming you have a DataAccess namespace
using TMS_DataAccess;
using TMS_BusinessLogic;
using MySqlX.XDevAPI.Relational;
using System;

namespace term_project
{
    public partial class BuyerDashboard : Window
    {
        public ObservableCollection<BuyerOrder> CustomerList { get; set; } = new ObservableCollection<BuyerOrder>();  // make sure to set it up right because if there ar eno get set then iut wont owrk
        public ObservableCollection<MarketPlaceValues> OrdersList { get; set; } = new ObservableCollection<MarketPlaceValues>();  // make sure to set it up right because if there ar eno get set then iut wont owrk
        public MarketPlaceValues SelectedGridMarket { get; set; } = new MarketPlaceValues();  // make sure to set it up right because if there ar eno get set then iut wont owrk
        public BuyerOrder SelectedGridBuyer { get; set; } = new BuyerOrder();
        // it may be better to store the sqltables as class values so i dont have to get the table everytime, but if the sqltable were to be update without me knowing (eg someone elses computer change the table)
        // there could be conflicts so we will not
        public BuyerDashboard()
        {
            InitializeComponent();
            
        }

        private void onManagerCustomerClick(string table)
        {
            BusinessLogic logic = new BusinessLogic();
            OrdersList.Clear();
            CustomerList.Clear();
            orderGrid.ItemsSource = CustomerList;
            OrderTableStorage tableStorage= logic.GetTableCustomer(table);// populate the tablestorage
            List<string> orders = new List<string>();
            OrderTableStorage carrierStorage = logic.GetCarriers();
            foreach (BuyerOrder item in tableStorage.OrdersListCustomer)
            {
                orders = new List<string>();
                foreach(CarrierTable item2 in carrierStorage.CarrierList)
                {
                    if(item2.dCity== item.Destination && item.Status=="Pending")
                    {
                        orders.Add(item2.cName);
                 
                    }
                   
                }
                item.AvailableCarriers = new ObservableCollection<string>(orders);  //fill the combo box


                CustomerList.Add(item);// populate the object grid with the class object datagrid will auto make it for you
            }
            this.DataContext = this;    //update the grid

        }
        private void OnManageContractsClick()
        {
            BusinessLogic logic = new BusinessLogic();
            CustomerList.Clear();
            OrdersList.Clear();
            orderGrid.ItemsSource = OrdersList;
            string table = "Contract_Marketplace";
            OrderTableStorage tableStorage = logic.GetTableMarket(table);
            foreach (MarketPlaceValues item in tableStorage.OrdersList)
            {
                
                OrdersList.Add(item);// populate the object grid with the class object datagrid will auto make it for you
            }
            this.DataContext = this;    //update the grid
        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            var selectedTab = tabControl.SelectedItem as TabItem;
            string table;
            if (selectedTab != null)
            {
                switch (selectedTab.Name)
                {
                    case "TabManageMarketplace":

                        ToggleCombo(true);
                        OnManageContractsClick();
                        break;
                    case "TabManageCustomer":
                        ToggleCombo(false);
                        table = "BuyerOrder";
                        onManagerCustomerClick(table);
                        OrgOrderID();
                        break;
                    // Add cases for other tabs as needed
                    case "TabCompletedOrders":
                        ToggleCombo(true);
                        table = "CompletedBuyerOrder";
                        onManagerCustomerClick(table);
                        OrgOrderID();
                        break;
                }
            }
        }
        private void OrgOrderID()
        {
            foreach (var column in orderGrid.Columns)
            {
                if (column.Header != null && column.Header.ToString() == "OrderID")
                {
                    orderGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "OrderID").DisplayIndex = 0;   //ignore warning there is a check for both if there are no headers and if order id even exists
                }
            }
        }
        private void orderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool marketplace = true;
            if (orderGrid.SelectedItem != null)
            {
                for (int i=0; i<orderGrid.Columns.Count;i++) // use this to check if this is the market place or the buyer order class
                {
                    var columnName = orderGrid.Columns[i].Header.ToString();
                    if(columnName=="Price")
                    {
                        marketplace = false;
                        break;
                    }

                }
                if (marketplace)
                {
                    SelectedGridMarket = (MarketPlaceValues)orderGrid.SelectedItem;// bind this to create a new class object of the selected item
                    
                }

            }
        }

        private void Init_Order(object sender, RoutedEventArgs e)
        {

            OrderTableStorage tableStorage = new OrderTableStorage();
            BusinessLogic logic = new BusinessLogic();
            if (orderGrid.SelectedItem != null)
            {
                logic.InsertSQL("BuyerOrder", SelectedGridMarket);

            }
            else if (orderGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a row before Initating Order");
            }

        }

        private void Create_invoice_Click(object sender, RoutedEventArgs e)
        {
            OrderTableStorage tableStorage = new OrderTableStorage();
            BusinessLogic logic = new BusinessLogic();
            if ( orderGrid.SelectedItem != null)
            {
                MessageBox.Show(logic.CreateInvoice(SelectedGridBuyer));
                
            }
            else 
            {
                MessageBox.Show("Please select a row before Creating an invoice");
            }

        }
        private void ToggleCombo(bool HideCombo)
        {
            foreach (var column in orderGrid.Columns)
            {
                if (column.Header != null && column.Header.ToString() == "AvailableCarriersColumn")
                {
                    if (HideCombo)
                    {
                        column.Visibility = Visibility.Collapsed;
                    }
                    else if (!HideCombo)
                    {
                        column.Visibility = Visibility.Visible;
                    }
                }
            }
 
        }
        private void orderGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "AvailableCarriers")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }

        }

        private void Carrier_Changed(object sender, SelectionChangedEventArgs e)
        {
            GetTable tableRecorder = new GetTable();
            BuyerOrder marketValues = new BuyerOrder();
            OrderTableStorage tableStorage = new OrderTableStorage();
            tableRecorder.connectCustomer(marketValues, tableStorage, "BuyerOrder");

            List<BuyerOrder> ContractList = tableStorage.OrdersListCustomer;     //reference to this

            foreach (var item in orderGrid.SelectedItems)
            {
                var selectedItem = item as BuyerOrder;// break it down even further to be usable
                if (selectedItem != null)
                {

                    // add the funny question mark so the compiler can stop being annoying about null values
                    BuyerOrder? Buyer = ContractList.FirstOrDefault(x => x.OrderID == selectedItem.OrderID); //this value will never be a null because it is a primary key and set to not null
                    if (Buyer != null)
                    {
                           
                        MessageBox.Show(tableRecorder.UpdateSQLTable(selectedItem.OrderID, "BuyerOrder", selectedItem.Carrier));
                    }
                    // MessageBox.Show(market.Order_ID.ToString() + market.Status);
                    //table is updated but we need to now update the sql tableDD
                    
                }
            }

        }


    }
}

//private void OnInitiateOrderClick()
//{

//    GetTable tableRecorder = new GetTable();
//    MarketPlaceValues marketValues = new MarketPlaceValues();
//    OrderTableStorage tableStorage = new OrderTableStorage();
//    tableRecorder.connectMarketPlace(marketValues, tableStorage, "Contract_Marketplace");
//    List<MarketPlaceValues> ContractList=  tableStorage.OrdersList;     //reference to this
//    foreach (var item in orderGrid.SelectedItems)
//    {
//        var selectedItem = item as MarketPlaceValues;// break it down even further to be usable
//        if (selectedItem != null)
//        {
//            //if (selectedItem.Status == "Available")
//            //{
//            //    // add the funny question mark so the compiler can stop being annoying about null values
//            //    MarketPlaceValues? market = ContractList.FirstOrDefault(x => x.Order_ID == selectedItem.Order_ID); //this value will never be a null because it is a primary key and set to not null
//            //    if (market != null)
//            //    {
//            //        market.Status = "Taken";
//            //        if(!tableRecorder.UpdateSQLTable(market.Order_ID, "Contract_Marketplace", market.Status))MessageBox.Show(market.Status+ " " + market.Order_ID);
//            //    }
//            //   // MessageBox.Show(market.Order_ID.ToString() + market.Status);
//            //    //table is updated but we need to now update the sql tableDD
//            //}
//        }
//    }

//    OrdersList.Clear();// clear the existing grid and update it again it would probably be better to delete/update just the selected value but thats for another time
//    foreach (MarketPlaceValues item in tableStorage.OrdersList)
//    {
//        OrdersList.Add(item);// populate the object grid with the class object datagrid will auto make it for you
//    }
//}
