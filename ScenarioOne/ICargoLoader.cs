using Scenario.Models;
using System.Collections.Generic;

namespace Scenario
{
    public interface ICargoLoader
    {
        void LoadSchedule(Flight flight);
        void ProcessOrders(string filename);
        void DisplayOrderAllocation();
        void DisplaySchedule();
        IDictionary<int, List<string>> GetOrderAllocations();
    }
}