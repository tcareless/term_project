using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS_BusinessLogic
{
    public class OrderTableStorage
    {
        public List<MarketPlaceValues> OrdersList = new List<MarketPlaceValues>();

    }
    public class MarketPlaceValues
    {
        public int Order_ID { get; set; }
        public string Cargo { get; set; }
        public string Customer { get; set; }
        public string StartingCity { get; set; }
        public string Destination { get; set; }
        public MarketPlaceValues(int order_ID, string cargo, string customer, string startingCity, string destination)
        {
            Order_ID = order_ID;
            Cargo = cargo;
            Customer = customer;
            StartingCity = startingCity;
            Destination = destination;
        }
    }
    public class Existing_Customer : MarketPlaceValues
    {
        public string Status { get; set; }
        public Existing_Customer(int order_ID, string cargo, string customer, string startingCity, string destination,string status):base(order_ID, cargo, customer, startingCity, destination)
        {
            Status = status;
        }
    }
}
