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
        class Program
        {
            static void Main()
            {
                // Define the input parameters and constraints for the shipment
                string startCity = "Windsor";
                string endCity = "Kingston";
                TransportationPlanner.ShipmentType shipmentType = TransportationPlanner.ShipmentType.LTL; // Example: LTL shipment
                double transportRatePerPalletPerKm = 0.5; // Example rate per pallet per km
                int numberOfPallets = 5; // Example: 5 pallets for LTL
                double reeferCharge = 0.05; // Example: 5% reefer charge
                bool isReeferRequired = true; // Example: Reefer van is required

                // Print the input parameters and constraints
                Console.WriteLine("Journey Details:");
                Console.WriteLine($"Start City: {startCity}");
                Console.WriteLine($"End City: {endCity}");
                Console.WriteLine($"Shipment Type: {shipmentType}");
                Console.WriteLine($"Transport Rate Per Pallet Per Km: ${transportRatePerPalletPerKm}");
                Console.WriteLine($"Number of Pallets: {numberOfPallets}");
                Console.WriteLine($"Reefer Charge: {(reeferCharge * 100)}%");
                Console.WriteLine($"Is Reefer Required: {isReeferRequired}");
                Console.WriteLine();

                // Calculate and print the number of days for the journey
                double totalDays = TransportationPlanner.CalculateDays(startCity, endCity, shipmentType);
                Console.WriteLine($"Total days from {startCity} to {endCity}: {totalDays}");

                // Calculate and print the total distance of the journey
                double totalDistance = TransportationPlanner.CalculateDistance(startCity, endCity);
                Console.WriteLine($"Total distance from {startCity} to {endCity}: {totalDistance} km");

                // Calculate and print the total cost of the shipment
                double totalCost = TransportationPlanner.CalculateCost(startCity, endCity, shipmentType, transportRatePerPalletPerKm, numberOfPallets, reeferCharge, isReeferRequired);
                Console.WriteLine($"Total cost for the shipment from {startCity} to {endCity}: ${totalCost}");

                Console.ReadLine(); // Keep the console window open
            }
        }

    }
}
