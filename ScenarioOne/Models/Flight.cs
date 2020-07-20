using System;
using System.Collections.Generic;
using System.Text;

namespace Scenario.Models
{
    public class Flight
    {
        public int FlightNo { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public int Day { get; set; }

        public override string ToString()
        {
            return $"FlightNo: {FlightNo}, Departure: {Source}, Arrival: {Destination}, Day: {Day}";
        }
    }
}
