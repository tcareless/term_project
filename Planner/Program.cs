using System;
using System.Collections.Generic;


namespace Project
{
    class TransportationPlanner
    {
        // Enum to represent the type of shipment
        public enum ShipmentType { LTL, FTL }

        private static readonly Dictionary<string, int> cityIndices = new Dictionary<string, int>
    {
        {"Windsor", 0},
        {"London", 1},
        {"Hamilton", 2},
        {"Toronto", 3},
        {"Oshawa", 4},
        {"Belleville", 5},
        {"Kingston", 6},
        {"Ottawa", 7}
    };

        private static readonly double[] travelTimes = { 2.5, 1.75, 1.25, 1.3, 1.65, 1.2, 2.5 };
        private static readonly double[] distances = { 191, 128, 68, 60, 134, 82, 196 }; // Distances between cities

        private const double MaxWorkingHours = 12.0;
        private const double MaxDrivingHours = 8.0;
        private const double LoadingTime = 2.0;
        private const double UnloadingTime = 2.0;
        private const double MandatoryBreak = 2.0;

        public static double CalculateDays(string startCity, string endCity, ShipmentType shipmentType)
        {
            double totalDays = 1.0;
            double currentDayDrivingTime = 0.0;
            double currentDayWorkingTime = LoadingTime; // Start with the loading time

            int startIndex = cityIndices[startCity];
            int endIndex = cityIndices[endCity];

            bool isReverseDirection = startIndex > endIndex;

            int step = isReverseDirection ? -1 : 1;
            int travelTimeIndex = isReverseDirection ? startIndex - 1 : startIndex;

            while ((isReverseDirection && travelTimeIndex >= endIndex) || (!isReverseDirection && travelTimeIndex < endIndex))
            {
                double travelTime = travelTimes[travelTimeIndex];

                if (currentDayDrivingTime + travelTime > MaxDrivingHours || currentDayWorkingTime + travelTime + ((isReverseDirection && travelTimeIndex > endIndex) || (!isReverseDirection && travelTimeIndex < endIndex - 1) ? MandatoryBreak : UnloadingTime) > MaxWorkingHours)
                {
                    totalDays++;
                    currentDayDrivingTime = 0.0;
                    currentDayWorkingTime = 0.0;
                }

                currentDayDrivingTime += travelTime;
                currentDayWorkingTime += travelTime;

                if (shipmentType == ShipmentType.LTL && ((isReverseDirection && travelTimeIndex > endIndex) || (!isReverseDirection && travelTimeIndex < endIndex - 1)))
                {
                    currentDayWorkingTime += MandatoryBreak; // 2-hour break for LTL shipments at intermediate cities
                }

                travelTimeIndex += step;
            }

            currentDayWorkingTime += UnloadingTime;

            if (currentDayWorkingTime > MaxWorkingHours)
            {
                totalDays++;
            }

            return totalDays;
        }


        public static double CalculateDistance(string startCity, string endCity)
        {
            int startIndex = cityIndices[startCity];
            int endIndex = cityIndices[endCity];

            bool isReverseDirection = startIndex > endIndex;

            double totalDistance = 0.0;

            if (isReverseDirection)
            {
                for (int i = startIndex - 1; i >= endIndex; i--)
                {
                    totalDistance += distances[i];
                }
            }
            else
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    totalDistance += distances[i];
                }
            }

            return totalDistance;
        }


        public static double CalculateCost(string startCity, string endCity, ShipmentType shipmentType, double transportRatePerPalletPerKm, int numberOfPallets, double reeferCharge, bool isReeferRequired)
        {
            double totalDistance = CalculateDistance(startCity, endCity);
            double totalDays = CalculateDays(startCity, endCity, shipmentType);

            // Calculate base transportation cost
            double transportationCost = 0.0;
            if (shipmentType == ShipmentType.LTL)
            {
                transportationCost = totalDistance * transportRatePerPalletPerKm * numberOfPallets;
            }
            else // FTL
            {
                transportationCost = totalDistance * transportRatePerPalletPerKm;
            }

            // Apply markup based on shipment type
            double markupRate = shipmentType == ShipmentType.FTL ? 0.08 : 0.05; // 8% for FTL, 5% for LTL
            double markedUpTransportationCost = transportationCost * (1 + markupRate);

            // Apply reefer charge if required
            if (isReeferRequired)
            {
                markedUpTransportationCost *= (1 + reeferCharge);
            }

            // Add additional cost per day
            double totalCost = markedUpTransportationCost;
            if (totalDays > 1)
            {
                totalCost += (totalDays - 1) * 150; // Assuming first day is included in the base cost
            }

            return totalCost;
        }

    }

}

