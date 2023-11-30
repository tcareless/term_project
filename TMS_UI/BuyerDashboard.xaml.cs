﻿using System.Collections.Generic;
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
        public ObservableCollection<MarketPlaceValues> OrdersList { get; set; } = new ObservableCollection<MarketPlaceValues>();  // make sure to set it up right because if there ar eno get set then iut wont owrk

        public BuyerDashboard()
        {
            InitializeComponent();

        }
        private void GetTable(string table)
        {
            GetTable tableRecorder = new GetTable();
            MarketPlaceValues marketValues = new MarketPlaceValues();
            OrderTableStorage tableStorage = new OrderTableStorage();
            tableRecorder.connectDatabase(marketValues, tableStorage, table);
            foreach (MarketPlaceValues item in tableStorage.OrdersList)
            {
                OrdersList.Add(item);// populate the object grid with the class object datagrid will auto make it for you
            }
            this.DataContext = this;    //update the grid
        }
        private void onManagerCustomerClick(object sender, RoutedEventArgs e)
        {
            OrdersList.Clear();
            string table = "Existing_Customers";
            GetTable(table);

        }
        private void OnManageContractsClick(object sender, RoutedEventArgs e)
        {
            OrdersList.Clear();
            string table = "Contract_Marketplace";
            GetTable(table);
           
        }

        private void OnInitiateOrderClick(object sender, RoutedEventArgs e)
        {
         
            GetTable tableRecorder = new GetTable();
            MarketPlaceValues marketValues = new MarketPlaceValues();
            OrderTableStorage tableStorage = new OrderTableStorage();
            tableRecorder.connectDatabase(marketValues, tableStorage, "Contract_Marketplace");
            List<MarketPlaceValues> ContractList=  tableStorage.OrdersList;     //reference to this
            foreach (var item in orderGrid.SelectedItems)
            {
                var selectedItem = item as MarketPlaceValues;// break it down even further to be usable
                if (selectedItem != null)
                {
                    if (selectedItem.Status == "Available")
                    {
                        // add the funny question mark so the compiler can stop being annoying about null values
                        MarketPlaceValues? market = ContractList.FirstOrDefault(x => x.Order_ID == selectedItem.Order_ID); //this value will never be a null because it is a primary key and set to not null
                        if (market != null)
                        {
                            market.Status = "Taken";
                        }
                       // MessageBox.Show(market.Order_ID.ToString() + market.Status);
                        //table is updated but we need to now update the sql tableDD
                    }
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

        private void OnViewOrderHistoryClick(object sender, RoutedEventArgs e)
        {
            // TODO: Display a loading indicator while fetching data (e.g., a progress bar or spinner)

            // TODO: Fetch order history asynchronously from the Business Logic layer
            // This might involve a call like: var orderHistory = await OrderManager.GetBuyerOrderHistoryAsync();

            // TODO: Check if the order history list is empty
            // If empty, display a message indicating "No orders found in your history"

            // TODO: If order history is available, proceed to display them
            // Consider using a DataGrid or ListView for listing the orders
            // Define columns for Order ID, Date, Product Details, Quantity, Total Amount, Status, etc.

            // TODO: Format the data for presentation
            // Ensure dates are displayed in a user-friendly format
            // Summarize product details if they are too lengthy for a grid display

            // TODO: Implement sorting and filtering functionality for the order history
            // Allow users to sort by date, status, amount, etc.
            // Provide filters to view specific types of orders (e.g., delivered, pending, cancelled)

            // TODO: Implement pagination or a lazy loading mechanism if the list is extensive
            // This helps in managing the display and performance for a large number of orders

            // TODO: Add interaction capabilities to the order list
            // Allow users to click on an order to view detailed information or to perform actions (e.g., repeat order, view invoice)

            // TODO: Handle any exceptions or errors during the fetching and displaying process
            // Display error messages in case of failure in data retrieval or processing

            // TODO: Hide the loading indicator once the data is fully loaded and displayed

            // TODO: Optionally, implement a refresh button to reload the order history

            // TODO: Provide a 'Back' or 'Cancel' button for returning to the previous view or dashboard

            // TODO: Add a search functionality to quickly find specific orders in the history
            // This could involve searching by order ID, date, product name, etc.
        }

        private void OnTrackOrderStatusClick(object sender, RoutedEventArgs e)
        {
            // TODO: Display a loading indicator while preparing the tracking interface (e.g., a progress bar or spinner)

            // TODO: Fetch a list of current orders asynchronously from the Business Logic layer
            // This might involve a call like: var currentOrders = await OrderManager.GetCurrentOrdersAsync();

            // TODO: Check if the current orders list is empty
            // If empty, display a message indicating "No current orders to track"

            // TODO: If current orders are available, display them in a selectable format
            // Consider using a ListView or DataGrid to list orders with basic details like Order ID, Date, and Brief Description

            // TODO: Allow the user to select an order from the list to track its status
            // Provide a radio button or a checkbox next to each order for selection

            // TODO: Add a 'Track Status' button
            // Upon clicking, validate if an order has been selected
            // If no order is selected, display a prompt asking the user to select an order

            // TODO: Once an order is selected and 'Track Status' is clicked:
            // Fetch the current status of the selected order from the Business Logic layer
            // This might involve a method like: var orderStatus = OrderManager.GetOrderStatus(selectedOrderId);

            // TODO: Display the status details of the selected order in a readable format
            // Show information such as Current Status (e.g., Processing, Dispatched, Delivered), Estimated Delivery Date, etc.

            // TODO: Implement real-time or near-real-time tracking, if possible
            // This could involve periodically updating the status or providing a 'Refresh' button

            // TODO: Handle any exceptions or errors during the status fetching process
            // Display an error message if there's a failure in retrieving the status

            // TODO: Hide the loading indicator once the status is fetched and displayed

            // TODO: Optionally, provide additional details or actions related to the order status
            // For example, a map view for dispatched orders, contact support for issues, etc.

            // TODO: Implement a 'Back' or 'Cancel' button to return to the previous view

            // TODO: Optionally, add a search or filter functionality to find specific orders quickly for status tracking
        }

        private void OnUpdatePersonalInfoClick(object sender, RoutedEventArgs e)
        {
            // TODO: Retrieve the current user's personal information
            // This might involve a call like: var userInfo = UserManager.GetCurrentUserInfo();

            // TODO: Display the current information in a form
            // The form should include fields such as Name, Email, Phone Number, Address, etc.
            // Populate the form fields with the retrieved information

            // TODO: Allow the user to edit their personal information in the form
            // Ensure that the form fields are editable and properly formatted (e.g., text box for name, email field for email)

            // TODO: Add input validation for form fields
            // Check for required fields to ensure they are not left empty
            // Validate the format of the data (e.g., proper email format, phone number format)
            // Implement custom validations if necessary (e.g., valid postal addresses)

            // TODO: Add a 'Save Changes' button for submitting the updated information
            // Upon clicking, perform client-side validation of all form fields

            // TODO: If validation fails, display appropriate error messages next to the respective fields

            // TODO: If validation passes, send the updated information to the Business Logic layer
            // This might involve a method like: UserManager.UpdateUserInfo(updatedUserInfo);

            // TODO: Handle any exceptions or errors during the information update process
            // Display an error message if the update process fails

            // TODO: On successful update, provide feedback to the user
            // This could be a confirmation message indicating "Your information has been successfully updated"

            // TODO: Optionally, implement a 'cancel' button to discard changes and revert to the original information

            // TODO: Refresh the form or the relevant UI components to display the updated information

            // TODO: Implement security best practices for handling personal information
            // Ensure sensitive data is handled securely both in transit and at rest
        }

        private void OnChangePasswordClick(object sender, RoutedEventArgs e)
        {
            // TODO: Display a form for password change
            // The form should include fields for the Current Password, New Password, and Confirm New Password

            // TODO: Implement input validation for the form fields
            // Validate that all fields are filled out
            // Ensure the New Password and Confirm New Password fields match
            // Validate the strength of the new password (e.g., minimum length, inclusion of numbers, symbols, etc.)

            // TODO: Add a 'Submit' button for changing the password
            // Upon clicking submit, first perform client-side validation of all form fields

            // TODO: If validation fails, display appropriate error messages next to the respective fields
            // Examples: "Current password is required", "New passwords do not match", "New password does not meet criteria"

            // TODO: If validation passes, send the current and new passwords to the Business Logic layer
            // This might involve a method like: UserManager.ChangePassword(currentPassword, newPassword);
            // Ensure the transmission of passwords is secure (e.g., over HTTPS, hashed passwords)

            // TODO: In the Business Logic layer, validate the current password against stored credentials
            // If the current password is incorrect, return an error to the UI layer

            // TODO: If the current password is correct, update the password with the new one
            // Ensure the new password is securely stored (e.g., hashed and salted)

            // TODO: Handle any exceptions or errors during the password change process
            // Display an error message if the process fails for reasons like incorrect current password, database issues, etc.

            // TODO: On successful password change, provide feedback to the user
            // Display a confirmation message like "Your password has been successfully changed"

            // TODO: Implement security best practices throughout the process
            // This includes secure storage and handling of passwords, as well as protection against common security threats like SQL injection, cross-site scripting, etc.

            // TODO: Optionally, log the password change event for security monitoring

            // TODO: Implement a 'cancel' button to allow users to exit the password change process

            // TODO: After a successful password change, recommend or force the user to log out and log in again with the new password
        }

        private void orderGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }



        // Other helper methods or event handlers as needed

        // Other helper methods or event handlers as needed
    }
}
