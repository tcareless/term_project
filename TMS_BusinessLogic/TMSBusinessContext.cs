using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TMS_BusinessLogic
{

    public class GetTable
    {

        public void connectDatabase(MarketPlaceValues marketStorage, OrderTableStorage OrderTableStorage)
        {
            string connectionString = "server=localhost;database=termproject;uid=root;pwd=";
            string column1 = string.Empty;
            string column2 = string.Empty;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Existing_Customers"; // Replace with your table name

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Assuming your table has columns named 'Column1', 'Column2', etc.
                                if (reader["Order_ID"]!= null && reader["Customer"] !=null)
                                {
                                    if (!reader.IsDBNull(reader.GetOrdinal("Order_ID")) && !reader.IsDBNull(reader.GetOrdinal("Customer")))
                                    {
                                        marketStorage.Order_ID = (int)reader["Order_ID"];
                                        marketStorage.Cargo = reader["Cargo"].ToString();
                                        marketStorage.Customer = reader["Customer"].ToString();     // ignore warning because the value is set to no null in the sql
                                        marketStorage.StartingCity = reader["StartingCity"].ToString();     // ignore warning because the value is set to no null in the sql
                                        marketStorage.Destination = reader["Destination"].ToString();     // ignore warning because the value is set to no null in the sql
                                        
                                    }


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
            finally
            {
                OrderTableStorage.OrdersList.Add(marketStorage);
            }

        }
    }
}