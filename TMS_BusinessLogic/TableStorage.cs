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
        public List<BuyerOrder> OrdersListCustomer = new List<BuyerOrder>();
    }
    public class MarketPlaceValues
    {
        public string ClientName { get; set; }
        public int JobType { get; set; }
        public int Quantity { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int VanType { get; set; }
        public MarketPlaceValues()
        {
            ClientName ="unknown";
            JobType = -1;
            Quantity = -1;
            Origin = "unknown";
            Destination = "unknown";
            VanType = -1;
        }
        public MarketPlaceValues(MarketPlaceValues value)
        {
            ClientName= value.ClientName;
            JobType = value.JobType;
            Quantity = value.Quantity;
            Origin = value.Origin;
            Destination = value.Destination;
            VanType = value.VanType;
        }
        public MarketPlaceValues(string clientName, int jobType, int quantity, string origin, string destination, int vanType)
        {
            ClientName = clientName;
            JobType = jobType;
            Quantity = quantity;
            Origin = origin;
            Destination = destination;
            VanType = vanType;
        }
    }
    public class BuyerOrder : MarketPlaceValues
    {
        public int OrderID { get; set; }
        public string Carrier { get; set; }
        public string Status { get; set; }
        public int Price { get; set; }
        public BuyerOrder() : base() 
        {
            OrderID = -1;
            Carrier= "unknown";
            Status= "unknown";
            Price = 0;
        }
        
        public BuyerOrder(int orderID, MarketPlaceValues value, string carrier,string status,int price): base(value)
        {
            OrderID = orderID;
            Carrier = carrier;
            Status = status;
            Price = price;
        }
        public BuyerOrder(BuyerOrder value) : base(value)
        {
            OrderID = value.OrderID;
            Carrier = value.Carrier;
            Status = value.Status;
            Price = value.Price;
        }
        public BuyerOrder(int orderID,string clientName, string carrier, int jobType, int quantity, string origin, string destination, int vanType,string status,int price) : base(clientName, jobType, quantity, origin, destination,vanType)
        {
            OrderID = orderID;
            Carrier = carrier;
            Status= status;
            Price = price;

        }
    }
}
