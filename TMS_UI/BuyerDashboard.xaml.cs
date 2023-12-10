using System.Collections.Generic;
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
            OrderTableStorage tableStorage= logic.GetTableCustomer(table);

            foreach (BuyerOrder item in tableStorage.OrdersListCustomer)
            {
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
                        OnManageContractsClick();
                        break;
                    case "TabManageCustomer":
                        table = "BuyerOrder";
                        onManagerCustomerClick(table);
                        foreach (var column in orderGrid.Columns)
                        {
                            if (column.Header != null && column.Header.ToString() == "OrderID")
                            {
                                orderGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "OrderID").DisplayIndex = 0;   //ignore warning there is a check for both if there are no headers and if order id even exists
                            }
                        }
                        break;
                    // Add cases for other tabs as needed
                    case "TabCompletedOrders":
                        table = "CompletedBuyerOrder";
                        onManagerCustomerClick(table);
                        foreach (var column in orderGrid.Columns)
                        {
                            if (column.Header != null && column.Header.ToString() == "OrderID")
                            {
                                orderGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "OrderID").DisplayIndex = 0;   //ignore warning there is a check for both if there are no headers and if order id even exists
                            }
                        }
                        break;
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
                else
                {
                    SelectedGridBuyer = (BuyerOrder)orderGrid.SelectedItem;// bind this to create a new class object of the selected item
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

        // Other helper methods or event handlers as needed

        // Other helper methods or event handlers as needed
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
