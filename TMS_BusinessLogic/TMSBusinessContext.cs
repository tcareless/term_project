using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using MySqlX.XDevAPI.Relational;
using TMS_DataAccess;
namespace TMS_BusinessLogic
{

    public class BusinessLogic
    {
        public OrderTableStorage GetTableMarket(string table)
        {
            GetTable tableRecorder = new GetTable();
            MarketPlaceValues marketValues = new MarketPlaceValues();
            OrderTableStorage tableStorage = new OrderTableStorage();
            tableRecorder.connectMarketPlace(marketValues, tableStorage, table);
            return tableStorage;
        }
        public OrderTableStorage GetTableCustomer(string table)
        {

            GetTable tableRecorder = new GetTable();
            BuyerOrder marketValues = new BuyerOrder();
            OrderTableStorage tableStorage = new OrderTableStorage();
            tableRecorder.connectCustomer(marketValues, tableStorage, table);
            return tableStorage;
        }
        public OrderTableStorage GetCarriers()
        {

            GetTable tableRecorder = new GetTable();
            BuyerOrder marketValues = new BuyerOrder();
            OrderTableStorage tableStorage = new OrderTableStorage();
            tableRecorder.GetCarrier(tableStorage);
            return tableStorage;
        }
        public string CreateInvoice(BuyerOrder order)
        {
            string currentDirectory = Environment.CurrentDirectory;
            string filePath = currentDirectory + $@"\Order{order.OrderID} Invoice.txt";


            try
            {
                // Create and write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"Order: {order.OrderID}\n" +
                                     $"Client Name: {order.ClientName}\n" +
                                     $"Carrier: {order.Carrier}\n" +
                                     $"Job Type: {order.JobType}\n" +
                                     $"Quantity: {order.Quantity}\n" +
                                     $"Origin: {order.Origin}\n" +
                                     $"Destination: {order.Destination}\n" +
                                     $"Van Type: {order.VanType}\n" +
                                     $"Status: {order.Status}\n" +
                                     $"Price: ${order.Price}");

                    // You can write more lines here if needed
                }

                return "File written successfully";
            }
            catch (Exception ex)
            {
                return "An error occurred: " + ex.Message;
            }
        }
        public string InsertSQL(string tableName,MarketPlaceValues objectInsert)
        {
            GetTable table = new GetTable();
            int maxOrderID = table.LatestOrderIDSQL() + 1;//plus one so it will now insert into a unused order id
            if (maxOrderID != -1)
            {
                BuyerOrder newOrder = new BuyerOrder(maxOrderID, objectInsert, "None", "Pending", "unknown");

               table.InsertSQLTableBuyOrder("BuyerOrder", newOrder);
            }
            else
            {
                return "could not get latest Order Id from server";    }
            return "Success";

        }
    }

}