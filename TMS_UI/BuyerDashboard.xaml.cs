using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
//using term_project.DataAccess;   // Assuming you have a DataAccess namespace
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
        private void GetTableMarket(string table)
        {
            GetTable tableRecorder = new GetTable();
            MarketPlaceValues marketValues = new MarketPlaceValues();
            OrderTableStorage tableStorage = new OrderTableStorage();
            tableRecorder.connectMarketPlace(marketValues, tableStorage, table);
            foreach (MarketPlaceValues item in tableStorage.OrdersList)
            {
                OrdersList.Add(item);// populate the object grid with the class object datagrid will auto make it for you
            }
            this.DataContext = this;    //update the grid
        }
        private void GetTableCustomer(string table)
        {
            
            GetTable tableRecorder = new GetTable();
            BuyerOrder marketValues = new BuyerOrder();
            OrderTableStorage tableStorage = new OrderTableStorage();
            tableRecorder.connectCustomer(marketValues, tableStorage, table);

            foreach (BuyerOrder item in tableStorage.OrdersListCustomer)
            {
                CustomerList.Add(item);// populate the object grid with the class object datagrid will auto make it for you
            }
            this.DataContext = this;    //update the grid
        }
        private void onManagerCustomerClick()
        {
            OrdersList.Clear();
            CustomerList.Clear();
            orderGrid.ItemsSource = CustomerList;
            int firstColumnIndex = orderGrid.Columns[0].DisplayIndex;
            orderGrid.Columns[0].DisplayIndex = orderGrid.Columns[1].DisplayIndex;
            orderGrid.Columns[1].DisplayIndex = firstColumnIndex;
            string table = "BuyerOrder";
            GetTableCustomer(table);

        }
        private void OnManageContractsClick()
        {
            CustomerList.Clear();
            OrdersList.Clear();
            orderGrid.ItemsSource = OrdersList;
            string table = "Contract_Marketplace";
            GetTableMarket(table);
            
        }

        private void OnInitiateOrderClick()
        {
         
            GetTable tableRecorder = new GetTable();
            MarketPlaceValues marketValues = new MarketPlaceValues();
            OrderTableStorage tableStorage = new OrderTableStorage();
            tableRecorder.connectMarketPlace(marketValues, tableStorage, "Contract_Marketplace");
            List<MarketPlaceValues> ContractList=  tableStorage.OrdersList;     //reference to this
            foreach (var item in orderGrid.SelectedItems)
            {
                var selectedItem = item as MarketPlaceValues;// break it down even further to be usable
                if (selectedItem != null)
                {
                    //if (selectedItem.Status == "Available")
                    //{
                    //    // add the funny question mark so the compiler can stop being annoying about null values
                    //    MarketPlaceValues? market = ContractList.FirstOrDefault(x => x.Order_ID == selectedItem.Order_ID); //this value will never be a null because it is a primary key and set to not null
                    //    if (market != null)
                    //    {
                    //        market.Status = "Taken";
                    //        if(!tableRecorder.UpdateSQLTable(market.Order_ID, "Contract_Marketplace", market.Status))MessageBox.Show(market.Status+ " " + market.Order_ID);
                    //    }
                    //   // MessageBox.Show(market.Order_ID.ToString() + market.Status);
                    //    //table is updated but we need to now update the sql tableDD
                    //}
                }
            }
            
            OrdersList.Clear();// clear the existing grid and update it again it would probably be better to delete/update just the selected value but thats for another time
            foreach (MarketPlaceValues item in tableStorage.OrdersList)
            {
                OrdersList.Add(item);// populate the object grid with the class object datagrid will auto make it for you
            }
        }

        private void OnGenerateInvoiceClick(object sender, RoutedEventArgs e)
        {
            // TODO: Display a loading indicator while fetching data (e.g., a progress bar or spinner)

            // TODO: Fetch completed orders asynchronously from the Business Logic layer
            // This might involve a call like: var completedOrders = await OrderManager.GetCompletedOrdersAsync();

            // TODO: Check if the completed orders list is empty
            // If empty, display a message indicating "No completed orders available"

            // TODO: If completed orders are available, display them in a selectable format
            // Consider using a ListView or DataGrid to list orders with relevant details like Order ID, Date, Total Amount, etc.

            // TODO: Allow the user to select an order from the list for which to generate an invoice
            // Provide a checkbox or a selection button next to each order

            // TODO: Add a 'Generate Invoice' button
            // Upon clicking, validate if an order has been selected
            // If no order is selected, display a prompt asking the user to select an order

            // TODO: Once an order is selected and 'Generate Invoice' is clicked:
            // Call the Business Logic layer to generate the invoice for the selected order
            // This might involve a method like: var invoice = OrderManager.GenerateInvoice(selectedOrderId);

            // TODO: Handle any exceptions or errors during invoice generation
            // Display an error message if there's a failure in generating the invoice

            // TODO: Display the generated invoice in a readable format
            // This could be in a new window, a panel, or a modal dialog
            // Show details like Invoice ID, Order Details, Amount, Date, Billing Information, etc.

            // TODO: Provide an option to download or print the invoice
            // Implement functionality to export the invoice as a PDF or print it directly

            // TODO: Optionally, include features like emailing the invoice directly to the customer

            // TODO: Hide the loading indicator once the invoice is generated and displayed

            // TODO: Implement a 'Back' or 'Cancel' button to return to the previous view

            // TODO: Optionally, add a search or filter functionality to find specific completed orders quickly
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
                        break;
                        // Add cases for other tabs as needed
                }
            }
        }

        private void orderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (orderGrid.SelectedItem != null)
            {
                SelectedGrid = (MarketPlaceValues)orderGrid.SelectedItem;// bind this to create a new class object

                // If your DataGrid is bound to a collection of a specific type, you can cast it
                // For example, if it's bound to a collection of 'Person' objects:
                // var selectedPerson = (Person)dataGrid.SelectedItem;

                // Use 'selectedRow' or 'selectedPerson' as needed
            }
        }

        private void Init_Order(object sender, RoutedEventArgs e)
        {
            OrderTableStorage tableStorage = new OrderTableStorage();
            GetTable table= new GetTable();
            BuyerOrder newOrder = new BuyerOrder(6,SelectedGrid,"None", "Pending",0);

            MessageBox.Show(table.InsertSQLTableBuyOrder("BuyerOrder", newOrder));

        }

        // Other helper methods or event handlers as needed

        // Other helper methods or event handlers as needed
    }
}
