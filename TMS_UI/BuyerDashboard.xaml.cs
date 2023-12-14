using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
/*
    File Name: BuyerDashboard.xaml.cs (ui)
    Authors: Richest Tran (Student No: 7602477) 
    Class: software quality
    Date: 2023-11-28
    Description: 
    UI for the buyer dashbaord displays contract,buyerorder,and completed orders and calls for changes in erahc table
*/
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
        public ObservableCollection<BuyerOrder> CustomerList { get; set; }  // make sure to set it up right because if there ar eno get set then iut wont owrk
        public ObservableCollection<MarketPlaceValues> OrdersList { get; set; } 
        public MarketPlaceValues SelectedGridMarket { get; set; } 
        public BuyerOrder SelectedGridBuyer { get; set; }
        public string CurrentTab { get; set; }
        public ObservableCollection<BuyerOrder> Items { get; set; }
        // it may be better to store the sqltables as class values so i dont have to get the table everytime, but if the sqltable were to be update without me knowing (eg someone elses computer change the table)
        // there could be conflicts so we will not
        public BuyerDashboard()
        {
            InitializeComponent();

            Items = new ObservableCollection<BuyerOrder>();
            OrdersList= new ObservableCollection<MarketPlaceValues>();
            CustomerList = new ObservableCollection<BuyerOrder> { new BuyerOrder() };
            CurrentTab= string.Empty;
            SelectedGridBuyer = new BuyerOrder();
            SelectedGridMarket= new MarketPlaceValues();
        }
        //method: onManagerCustomerClick
        //purpose:gets the sql table for the buyerorder class and matches the carrier for each destination city
        private void BuyOrderClick(string table)
        {
            BusinessLogic logic = new BusinessLogic();
            // clear so it doesnt display the older ordergrid
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
        //method: onManagerCustomerClick
        //purpose:gets the sql table for the contract marketplace class
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

        //method: TabControl_SelectionChanged
        //purpose:controller for the main menu of the buyer dashboard
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            if (tabControl != null)
            {
                var selectedTab = tabControl.SelectedItem as TabItem;


                string table;
                if (selectedTab != null)
                {
                    CurrentTab = selectedTab.Name;
                    switch (selectedTab.Name)
                    {
                        case "TabManageMarketplace":

                            OnManageContractsClick();
                            // make sure toggle combo is always at the end because it reads what the CURRENT datagrid is and everything before is updating the data grid
                            ToggleCombo(true);
                            break;
                        case "TabManageCustomer":

                            table = "BuyerOrder";
                            BuyOrderClick(table);
                            OrgOrderID();
                            ToggleCombo(false);
                            break;
                        // Add cases for other tabs as needed
                        case "TabCompletedOrders":
                        
                            table = "CompletedBuyerOrder";
                            BuyOrderClick(table);
                            OrgOrderID();
                            ToggleCombo(true);          
                            break;
                    }
                }
            }
        }
        //method: OrgOrderID
        //purpose:organizes the order id to be first in the datagrid
        private void OrgOrderID()
        {
            foreach (var column in orderGrid.Columns)
            {
                if (column.Header != null && column.Header.ToString() == "OrderID")
                {
                    var orderIDColumn = orderGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "OrderID");
                    if (orderIDColumn != null)
                    {
                        orderIDColumn.DisplayIndex = 0;
                    }
                }
            }
        }
        //method: orderGrid_SelectionChanged
        //purpose: WHenever the ordergrid is selected check which one it is and update the selected class value for the market and the buyer
        private void orderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
 
            if (orderGrid.SelectedItem != null)
            {

                if (CurrentTab == "TabManageMarketplace" && orderGrid.SelectedItem != null)
                {
                    // update the selected grid market
                    SelectedGridMarket = (MarketPlaceValues)orderGrid.SelectedItem;// bind this to create a new class object of the selected item
                    
                }
                else if(orderGrid.SelectedItem != null && CurrentTab== "TabCompletedOrders") 
                {
                    // update the selected completed buyerorder for the invoice
                    SelectedGridBuyer = (BuyerOrder)orderGrid.SelectedItem;
                    Items.Clear();// clear the view box
                    Items.Add(SelectedGridBuyer);// update the viewbox with the selected value
                }
            }
        }
        //method: Init_Order
        //purpose: create a new order from the contract marketplace
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
        //method: Create_invoice_Click
        //purpose: create invoice from the completed orders
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
        //method: ToggleCombo
        //purpose: This controls the datagrid columns if it is suppose to display combo box and also does the same thing for the carrier column
        private void ToggleCombo(bool HideCombo)
        {
            foreach (var column in orderGrid.Columns)
            {
                if (column.Header != null && column.Header.ToString() == "AvailableCarriers")
                {
                    if (HideCombo) column.Visibility = Visibility.Collapsed;
                    else column.Visibility = Visibility.Visible;

                }
                if (column.Header != null && column.Header.ToString() == "Carrier")
                {
                    if (HideCombo)column.Visibility = Visibility.Visible;
                   
                    else column.Visibility = Visibility.Collapsed;
                    
                
                }
            }
 
        }
        //method: orderGrid_AutoGeneratingColumn
        //purpose: This controls the datagrid columns if it is suppose to display combo box 
        private void orderGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "AvailableCarriers")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }


        }
        //method: Carrier_Changed
        //purpose: this decides if the carrier should be displayed in the combo box in each row of the data grid
        private void Carrier_Changed(object sender, SelectionChangedEventArgs e)
        {
            GetTable tableRecorder = new GetTable();
            BuyerOrder marketValues = new BuyerOrder();
            OrderTableStorage tableStorage = new OrderTableStorage();
            tableRecorder.connectCustomer(marketValues, tableStorage, "BuyerOrder");// set up the current buyer order table

            List<BuyerOrder> ContractList = tableStorage.OrdersListCustomer;  // get the list for the buy order

            foreach (var item in orderGrid.SelectedItems)
            {
                var selectedItem = item as BuyerOrder;// break it down even further to be usable
                if (selectedItem != null)
                {

                    // add the funny question mark so the compiler can stop being annoying about null values
                    BuyerOrder? Buyer = ContractList.FirstOrDefault(x => x.OrderID == selectedItem.OrderID); 
                    if (Buyer != null)
                    {
                        tableRecorder.UpdateSQLTable(selectedItem.OrderID, "BuyerOrder", selectedItem.Carrier);
                    }
                    
                    //table is updated but we need to now update the sql tableDD
                    
                }
            }

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
