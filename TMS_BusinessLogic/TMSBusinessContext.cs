using System;
using System.Collections;
using System.Collections.Generic;
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


    }
}