using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;

namespace TMS_BusinessLogic
{

    public class GetTable
    {

#pragma warning restore CS0219 // Variable is assigned but its value is never used
        public bool UpdateSQLTable(int order_id, string Table,string status)
        {
            string connectionString = "server=localhost;database=termproject;uid=root;pwd=Jackass12!";
            string query = $"UPDATE {Table} SET Status = @NewStatus WHERE Order_ID = @OrderID";// table has to be l
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Add parameters to prevent SQL injection
                //    cmd.Parameters.AddWithValue("@Table", Table);
                    cmd.Parameters.AddWithValue("@NewStatus", status);
                    cmd.Parameters.AddWithValue("@OrderID", order_id);


                  
                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            return true;
                            //worked display
                        }
                        else
                        {
                            return false;
                            //failed display
                        }
                    }
                    catch (MySqlException ex)
                    {
                     //exception will add later
                    }
                }
            }
            return false;
        }
        public string InsertSQLTableBuyOrder(string Table,BuyerOrder order)
        {
            string connectionString = "server=localhost;database=termproject;uid=root;pwd=Jackass12!";
            string query = $"INSERT INTO {Table} (Order_ID, Client_Name, Carrier, Job_Type, Quantity, Origin, Destination, Van_Type, Status, Price) VALUES (@OrderID, @Client, @Carrier, @JobType, @Quantity, @Origin, @Destination, @VanType, @Status, @Price);";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Add parameters to prevent SQL injection
                    //    cmd.Parameters.AddWithValue("@Table", Table);
                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
                    cmd.Parameters.AddWithValue("@Client", order.ClientName);
                    cmd.Parameters.AddWithValue("@Carrier", order.Carrier);
                    cmd.Parameters.AddWithValue("@JobType", order.JobType);
                    cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
                    cmd.Parameters.AddWithValue("@Origin", order.Origin);
                    cmd.Parameters.AddWithValue("@Destination", order.Destination);
                    cmd.Parameters.AddWithValue("@VanType", order.VanType);
                    cmd.Parameters.AddWithValue("@Status", order.Status);
                    cmd.Parameters.AddWithValue("@Price", order.Price);


                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            return "work";
                            //worked display
                        }
                        else
                        {
                            return "did not work";
                            //failed display
                        }
                    }
                    catch (MySqlException ex)
                    {
                        return ex.ToString();
                    }
                }
            }
            return "did not work";
        }


        public void connectMarketPlace(MarketPlaceValues marketStorage, OrderTableStorage OrderTableStorage,string Table)
        {
            string connectionString = "server=159.89.117.198;database=cmp;uid=DevOSHT;pwd=Snodgr4ss!";
            List<string[]> resultData = new List<string[]>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Contract"; // Replace with your table name

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("Client_Name")) && !reader.IsDBNull(reader.GetOrdinal("Job_Type")) && !reader.IsDBNull(reader.GetOrdinal("Quantity")) && !reader.IsDBNull(reader.GetOrdinal("Origin")) && !reader.IsDBNull(reader.GetOrdinal("Destination")))
                                {
                                    
                                    marketStorage.ClientName = reader["Client_Name"].ToString();
                                    marketStorage.JobType = (int)reader["Job_Type"];
                                    marketStorage.Quantity = (int)reader["Quantity"];
                                  
                                    marketStorage.Origin = reader["Origin"].ToString();     // ignore warning because the value is set to no null in the sql
                                    marketStorage.Destination = reader["Destination"].ToString();     // ignore warning because the value is set to no null in the sql
                                    marketStorage.VanType = (int)reader["Van_Type"];     // ignore warning because the value is set to no null in the sql
                                    MarketPlaceValues newStorage = new MarketPlaceValues(marketStorage);
                                    OrderTableStorage.OrdersList.Add(newStorage);
                                }

                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }

 


        }
        public void connectCustomer(BuyerOrder marketStorage, OrderTableStorage OrderTableStorage, string Table)
        {
            string connectionString = "server=localhost;database=termproject;uid=root;pwd=Jackass12!";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM " + Table; // Replace with your table name

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                    if (!reader.IsDBNull(reader.GetOrdinal("Van_Type")) && !reader.IsDBNull(reader.GetOrdinal("Order_ID")) && !reader.IsDBNull(reader.GetOrdinal("Client_Name")) && !reader.IsDBNull(reader.GetOrdinal("Job_Type")) && !reader.IsDBNull(reader.GetOrdinal("Quantity")) && !reader.IsDBNull(reader.GetOrdinal("Origin")) && !reader.IsDBNull(reader.GetOrdinal("Carrier")) && !reader.IsDBNull(reader.GetOrdinal("Destination")) && !reader.IsDBNull(reader.GetOrdinal("Status")))
                                    {
                                        marketStorage.OrderID = (int)reader["Order_ID"];
                                        marketStorage.ClientName = reader["Client_Name"].ToString();
                                        marketStorage.JobType = (int)reader["Job_Type"];
                                        marketStorage.Quantity = (int)reader["Quantity"];
                                        marketStorage.Carrier = reader["Carrier"].ToString();     // ignore warning because the value is set to no null in the sql
                                        marketStorage.Origin = reader["Origin"].ToString();     // ignore warning because the value is set to no null in the sql
                                        marketStorage.Destination = reader["Destination"].ToString();     // ignore warning because the value is set to no null in the sql
                                        marketStorage.VanType = (int)reader["Van_Type"];     // ignore warning because the value is set to no null in the sql
                                        marketStorage.Status = reader["Status"].ToString();     // ignore warning because the value is set to no null in the sql
                                                                                                          // we gotta do this instead of inserting the changes class because we will only insert a reference and not a class object 
                                        BuyerOrder newStorage = new BuyerOrder(marketStorage);
                                        OrderTableStorage.OrdersListCustomer.Add(newStorage);
                                    }



                               
                                // Add more columns as per your table structure


                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }


        }
    }
}