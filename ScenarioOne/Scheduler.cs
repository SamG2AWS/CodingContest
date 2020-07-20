using Scenario.Models;
using System;

namespace Scenario
{
    public class Scheduler: IScheduler
    {
        private readonly ICargoLoader _cargoLoader;
        public Scheduler(ICargoLoader cargoLoader)
        {
            _cargoLoader = cargoLoader;
        }

        public void BeginProcessing()
        {

            _cargoLoader.LoadSchedule(new Flight() { FlightNo = 1, Source = "YUL", Destination = "YYZ", Day = 1 });
            _cargoLoader.LoadSchedule(new Flight() { FlightNo = 2, Source = "YUL", Destination = "YYC", Day = 1 });
            _cargoLoader.LoadSchedule(new Flight() { FlightNo = 3, Source = "YUL", Destination = "YVR", Day = 1 });
            _cargoLoader.LoadSchedule(new Flight() { FlightNo = 4, Source = "YUL", Destination = "YYZ", Day = 2 });
            _cargoLoader.LoadSchedule(new Flight() { FlightNo = 5, Source = "YUL", Destination = "YYC", Day = 2 });
            _cargoLoader.LoadSchedule(new Flight() { FlightNo = 6, Source = "YUL", Destination = "YVR", Day = 2 });

            _cargoLoader.ProcessOrders(@"..\..\..\Orders\coding-assigment-orders.json");

            Console.WriteLine("Use Case 1:");
            _cargoLoader.DisplaySchedule();

            Console.WriteLine("\n\nUse Case 2:");
            _cargoLoader.DisplayOrderAllocation();
        }
    }
}
