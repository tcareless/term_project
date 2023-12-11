using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS_DataAccess
{

        public class OrderTableStorage
        {
            public List<MarketPlaceValues> OrdersList = new List<MarketPlaceValues>();
            public List<BuyerOrder> OrdersListCustomer = new List<BuyerOrder>();
            public List<CarrierTable> CarrierList = new List<CarrierTable>();
       
         }
        public class CarrierTable
        {
            public string cName { get; set; }
            public string dCity { get; set; }
        public CarrierTable()
        {
            cName= "unknown";
            dCity = "unknown";
        }
            public CarrierTable(CarrierTable table)
            {
                cName= table.cName;
                dCity= table.dCity;
            }
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
                ClientName = "unknown";
                JobType = -1;
                Quantity = -1;
                Origin = "unknown";
                Destination = "unknown";
                VanType = -1;
            }
            public MarketPlaceValues(MarketPlaceValues value)
            {
                ClientName = value.ClientName;
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
            public string Price { get; set; }
            public  ObservableCollection<string> AvailableCarriers
            {
                get; set;
            }
            public BuyerOrder() : base()
            {
                OrderID = -1;
                Carrier = "unknown";
                Status = "unknown";
                Price = "unknown";
            AvailableCarriers = new ObservableCollection<string>();
            
            }

            public BuyerOrder(int orderID, MarketPlaceValues value, string carrier, string status, string price) : base(value)
            {
                OrderID = orderID;
                Carrier = carrier;
                Status = status;
                Price = price;
            AvailableCarriers = new ObservableCollection<string>();

            }

            public BuyerOrder(BuyerOrder value) : base(value)
                {
                    OrderID = value.OrderID;
                    Carrier = value.Carrier;
                    Status = value.Status;
                    Price = value.Price;
            AvailableCarriers = new ObservableCollection<string>();
 
            }
                public BuyerOrder(int orderID, string clientName, string carrier, int jobType, int quantity, string origin, string destination, int vanType, string status, string price) : base(clientName, jobType, quantity, origin, destination, vanType)
                {
                    OrderID = orderID;
                    Carrier = carrier;
                    Status = status;
                    Price = price;
            AvailableCarriers = new ObservableCollection<string>();

                }
        }
    
}
