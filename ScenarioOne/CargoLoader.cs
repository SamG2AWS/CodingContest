using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Scenario.Configuration;
using Scenario.FileOperations;
using Scenario.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario
{
    public class CargoLoader : ICargoLoader
    {
        private IDictionary<int, Flight> FlightSchedule { get; set; }
        private IDictionary<string, int> OrderAllocation { get; set; }

        //Maintains a list of flights available for a given destination, along with the capacity for each flight
        private IDictionary<string, Dictionary<int, int>> DestinationTotalCapacityByFlight { get; set; }

        private readonly int _flightCapacity;
        private readonly ILogger<CargoLoader> _logger;
        private readonly IJsonReader _jsonReader;

        public CargoLoader(ILoggerFactory loggerFactory, IJsonReader jsonReader, IAppSettings appSettings)
        {
            FlightSchedule = new Dictionary<int, Flight>();
            OrderAllocation = new Dictionary<string, int>();
            DestinationTotalCapacityByFlight = new Dictionary<string, Dictionary<int, int>>();

            _logger = loggerFactory.CreateLogger<CargoLoader>();
            _jsonReader = jsonReader;
            _flightCapacity = appSettings.FlightCapacity;
        }

        /// <summary>
        /// Load the given schedule in the dictionary
        /// </summary>
        /// <param name="day"></param>
        /// <param name="flight"></param>
        public void LoadSchedule(Flight flight)
        {
            if (!FlightSchedule.ContainsKey(flight.FlightNo))
                FlightSchedule.Add(flight.FlightNo, flight);

            if (DestinationTotalCapacityByFlight.ContainsKey(flight.Destination))
            {
                if (!DestinationTotalCapacityByFlight[flight.Destination].ContainsKey(flight.FlightNo))
                {
                    (DestinationTotalCapacityByFlight[flight.Destination]).Add(flight.FlightNo, _flightCapacity);
                }
            }
            else
            {
                DestinationTotalCapacityByFlight.Add(flight.Destination, new Dictionary<int, int>());
                (DestinationTotalCapacityByFlight[flight.Destination]).Add(flight.FlightNo, _flightCapacity);
            }
        }

        /// <summary>
        /// Display schedule on the UI
        /// </summary>
        public void DisplaySchedule()
        {
            foreach (var kp in FlightSchedule)
            {
                Console.WriteLine(kp.Value.ToString());
            }
        }

        /// <summary>
        /// Read orders from the json file and process the data
        /// </summary>
        /// <param name="filename"></param>
        public void ProcessOrders(string filename)
        {
            JObject jObject = _jsonReader.ReadJsonFile(filename);
            //JObject jObject = JObject.Parse(File.ReadAllText(filename));
            foreach (var element in jObject.Children<JProperty>())
            {
                var order = element.Name;
                var destination = (element.Value)["destination"].ToString();

                AllocateOrder(order, destination);
            }
        }

        /// <summary>
        /// Allocate the order box to the next available flight with capacity
        /// </summary>
        /// <param name="order"></param>
        /// <param name="destination"></param>
        private void AllocateOrder(string order, string destination)
        {
            if (DestinationTotalCapacityByFlight.ContainsKey(destination) && (DestinationTotalCapacityByFlight[destination]).Keys.Count > 0)
            {
                //Get the first available flight, to the destination, with capacity
                var flight_no = (DestinationTotalCapacityByFlight[destination]).Keys.FirstOrDefault();

                //Allocate the order to the selected flight
                if (!OrderAllocation.ContainsKey(order))
                {
                    OrderAllocation.Add(order, flight_no);
                }

                //Reduce the flight capacity by 1
                (DestinationTotalCapacityByFlight[destination])[flight_no] -= 1;

                //If the flight has no capacity left, remove it from the list of available flights
                if ((DestinationTotalCapacityByFlight[destination])[flight_no] <= 0)
                    DestinationTotalCapacityByFlight[destination].Remove(flight_no);
            }
            //No alloctions made or no capacity left
            else
            {
                OrderAllocation.Add(order, -1);
            }
        }

        /// <summary>
        /// Display allocation of orders to the flight
        /// </summary>
        public void DisplayOrderAllocation()
        {
            foreach (var orderAllocationKP in OrderAllocation)
            {
                if (orderAllocationKP.Value > 0)
                {
                    var flight = FlightSchedule[orderAllocationKP.Value];
                    Console.WriteLine($"Order: {orderAllocationKP.Key}, {flight}");
                }
                else
                    Console.WriteLine($"Order: {orderAllocationKP.Key}, FlightNo: not scheduled");
            }
        }

        /// <summary>
        /// Get order box allocation
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, List<string>> GetOrderAllocations()
        {
            IDictionary<int, List<string>> flightAllocations = new Dictionary<int, List<string>>();

            foreach (var ord in OrderAllocation)
            {
                if (flightAllocations.ContainsKey(ord.Value))
                {
                    flightAllocations[ord.Value].Add(ord.Key);
                }
                else
                {
                    flightAllocations.Add(ord.Value, new List<string>());
                    flightAllocations[ord.Value].Add(ord.Key);
                }
            }

            return flightAllocations;
        }

    }
}
