using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
//using term_project.DataAccess;   // Assuming you have a DataAccess namespace
using TMS_DataAccess;
using TMS_BusinessLogic;
namespace term_project
{
    public partial class BuyerDashboard : Window
    {
        public ObservableCollection<BuyerOrder> CustomerList { get; set; } = new ObservableCollection<BuyerOrder>();  // make sure to set it up right because if there ar eno get set then iut wont owrk
        public ObservableCollection<MarketPlaceValues> OrdersList { get; set; } = new ObservableCollection<MarketPlaceValues>();  // make sure to set it up right because if there ar eno get set then iut wont owrk
        public MarketPlaceValues SelectedGrid { get; set; } = new MarketPlaceValues();  // make sure to set it up right because if there ar eno get set then iut wont owrk
        // it may be better to store the sqltables as class values so i dont have to get the table everytime, but if the sqltable were to be update without me knowing (eg someone elses computer change the table)
        // there could be conflicts so we will not
        public BuyerDashboard()
        {
            InitializeComponent();
            
        }

        private void onManagerCustomerClick()
        {
            BusinessLogic logic = new BusinessLogic();
            OrdersList.Clear();
            CustomerList.Clear();
            orderGrid.ItemsSource = CustomerList;
            int firstColumnIndex = orderGrid.Columns[0].DisplayIndex;
            orderGrid.Columns[0].DisplayIndex = orderGrid.Columns[1].DisplayIndex;
            orderGrid.Columns[1].DisplayIndex = firstColumnIndex;
            string table = "BuyerOrder";
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

        private void OnGenerateInvoiceClick(object sender, RoutedEventArgs e)
        {

        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            var selectedTab = tabControl.SelectedItem as TabItem;

            if (selectedTab != null)
            {
                switch (selectedTab.Name)
                {
                    case "TabManageMarketplace":
                        OnManageContractsClick();
                        break;
                    case "TabManageCustomer":
                        
                        onManagerCustomerClick();
                        foreach (var column in orderGrid.Columns)
                        {
                            if (column.Header != null && column.Header.ToString() == "OrderID")
                            {
                                orderGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "OrderID").DisplayIndex = 0;   //ignore warning there is a check for both if there are no headers and if order id even exists
                            }
                        }
                        
                        break;
                        // Add cases for other tabs as needed
                }
            }
        }

        private void orderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (orderGrid.SelectedItem != null)
            {
                SelectedGrid = (MarketPlaceValues)orderGrid.SelectedItem;// bind this to create a new class object of the selected item
            }
        }

        private void Init_Order(object sender, RoutedEventArgs e)
        {
            OrderTableStorage tableStorage = new OrderTableStorage();
            GetTable table= new GetTable();
            int maxOrderID = table.LatestOrderIDSQL()+1;//plus one so it will now insert into a unused order id
            if (maxOrderID != -1 && orderGrid.SelectedItem != null)
            {
                BuyerOrder newOrder = new BuyerOrder(maxOrderID, SelectedGrid, "None", "Pending", "unknown");

                MessageBox.Show(table.InsertSQLTableBuyOrder("BuyerOrder", newOrder));
            }
            else if (orderGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a row before Initating Order");
            }
            else
            {
                MessageBox.Show("could not get latest Order Id from server");
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
