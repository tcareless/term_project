using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner
{
    internal class Test
    {
        static void Main()
        {
            // Set up the journey details
            string startCity = "Windsor";
            string endCity = "London";
            TransportationPlanner.ShipmentType shipmentType = TransportationPlanner.ShipmentType.LTL; // Example: LTL shipment
            double transportRatePerPalletPerKm = 0.5; // Example rate per pallet per km
            int numberOfPallets = 10;
            double reeferCharge = 0.05; 
            bool isReeferRequired = false; // Example: Reefer van is required

            // Calculate the number of days for the journey
            double totalDays = TransportationPlanner.CalculateDays(startCity, endCity, shipmentType);
            Console.WriteLine($"Total days from {startCity} to {endCity}: {totalDays}");

            // Calculate the total distance of the journey
            double totalDistance = TransportationPlanner.CalculateDistance(startCity, endCity);
            Console.WriteLine($"Total distance from {startCity} to {endCity}: {totalDistance} km");

            // Calculate the total cost of the shipment
            double totalCost = TransportationPlanner.CalculateCost(startCity, endCity, shipmentType, transportRatePerPalletPerKm, numberOfPallets, reeferCharge, isReeferRequired);
            Console.WriteLine($"Total cost for the shipment from {startCity} to {endCity}: ${totalCost}");

            Console.ReadLine(); 
        }
    }
}
