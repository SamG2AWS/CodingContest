using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Scenario;
using Scenario.Configuration;
using Scenario.FileOperations;
using Scenario.Models;
using System.Collections.Generic;
using Moq;

namespace ScenarioOneTests
{
    [TestFixture]
    public class CargoLoaderTest
    {
        private CargoLoader CargoLoader { get; set; }
        private ServiceProvider ServiceProvider { get; set; }

        [SetUp]
        public void Setup()
        {
            ServiceProvider = new ServiceCollection()
                .AddLogging(option =>
                {
                    option.AddConsole();
                })
                .AddTransient<IJsonReader, JsonReader>()
                .BuildServiceProvider();
        }


        /// <summary>
        /// Scenario: Given test case
        /// </summary>
        [Test]
        public void GivenDataTest()
        {
            var appsettingMock = new Mock<IAppSettings>();
            appsettingMock.Setup(f => f.FlightCapacity).Returns(20);

            CargoLoader = new CargoLoader(ServiceProvider.GetService<ILoggerFactory>(), ServiceProvider.GetService<IJsonReader>(), appsettingMock.Object);

            CargoLoader.LoadSchedule(new Flight() { FlightNo = 1, Source = "YUL", Destination = "YYZ", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 2, Source = "YUL", Destination = "YYC", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 3, Source = "YUL", Destination = "YVR", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 4, Source = "YUL", Destination = "YYZ", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 5, Source = "YUL", Destination = "YYC", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 6, Source = "YUL", Destination = "YVR", Day = 2 });

            CargoLoader.ProcessOrders(@"..\..\..\TestData\coding-assigment-orders.json");

            var orderAllocations = CargoLoader.GetOrderAllocations();

            IDictionary<int, int> resultFlightCapacity = new Dictionary<int, int>
            {
                { 1, 20 },
                { 4, 14 },
                { 2, 20 },
                { 5, 2 },
                { 3, 20 },
                { 6, 17 }
            };

            IDictionary<int, string> resultFlightOrders = new Dictionary<int, string>
            {
                { 1, "order-001,order-002,order-003,order-004,order-005,order-006,order-007,order-008,order-009,order-010,order-011,order-012,order-013,order-014,order-015,order-016,order-017,order-018,order-019,order-020" },
                { 4, "order-021,order-022,order-023,order-024,order-025,order-026,order-027,order-028,order-029,order-030,order-031,order-032,order-033,order-034" },
                { 2, "order-035,order-036,order-037,order-038,order-039,order-040,order-041,order-042,order-043,order-044,order-045,order-046,order-047,order-048,order-050,order-052,order-054,order-055,order-056,order-057" },
                { 5, "order-060,order-061" },
                { 3, "order-062,order-063,order-064,order-065,order-066,order-067,order-068,order-069,order-070,order-071,order-072,order-073,order-074,order-075,order-076,order-077,order-078,order-080,order-081,order-082" },
                { 6, "order-083,order-084,order-085,order-086,order-087,order-088,order-089,order-090,order-091,order-092,order-093,order-094,order-095,order-096,order-097,order-098,order-099" }
            };

            Assert.AreEqual(resultFlightCapacity[1], orderAllocations[1].Count);
            Assert.AreEqual(resultFlightCapacity[4], orderAllocations[4].Count);
            Assert.AreEqual(resultFlightCapacity[2], orderAllocations[2].Count);
            Assert.AreEqual(resultFlightCapacity[5], orderAllocations[5].Count);
            Assert.AreEqual(resultFlightCapacity[3], orderAllocations[3].Count);
            Assert.AreEqual(resultFlightCapacity[6], orderAllocations[6].Count);

            Assert.AreEqual(resultFlightOrders[1], string.Join(',',orderAllocations[1]));
            Assert.AreEqual(resultFlightOrders[4], string.Join(',', orderAllocations[4]));
            Assert.AreEqual(resultFlightOrders[2], string.Join(',', orderAllocations[2]));
            Assert.AreEqual(resultFlightOrders[5], string.Join(',', orderAllocations[5]));
            Assert.AreEqual(resultFlightOrders[3], string.Join(',', orderAllocations[3]));
            Assert.AreEqual(resultFlightOrders[6], string.Join(',', orderAllocations[6]));

        }


        /// <summary>
        /// Scenario: Flight have low capacity (just 2)
        /// </summary>
        [Test]
        public void LowFlightCapacityTest()
        {
            var appsettingMock = new Mock<IAppSettings>();
            appsettingMock.Setup(f => f.FlightCapacity).Returns(2);

            CargoLoader = new CargoLoader(ServiceProvider.GetService<ILoggerFactory>(), ServiceProvider.GetService<IJsonReader>(), appsettingMock.Object);

            CargoLoader.LoadSchedule(new Flight() { FlightNo = 1, Source = "YUL", Destination = "YYZ", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 2, Source = "YUL", Destination = "YYC", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 3, Source = "YUL", Destination = "YVR", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 4, Source = "YUL", Destination = "YYZ", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 5, Source = "YUL", Destination = "YYC", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 6, Source = "YUL", Destination = "YVR", Day = 2 });

            CargoLoader.ProcessOrders(@"..\..\..\TestData\coding-assigment-orders.json");

            var orderAllocations = CargoLoader.GetOrderAllocations();

            IDictionary<int, int> resultFlightCapacity = new Dictionary<int, int>
            {
                { 1, 2 },
                { 4, 2 },
                { 2, 2 },
                { 5, 2 },
                { 3, 2 },
                { 6, 2 }
            };

            IDictionary<int, string> resultFlightOrders = new Dictionary<int, string>
            {
                { 1, "order-001,order-002" },
                { 4, "order-003,order-004" },
                { 2, "order-035,order-036" },
                { 5, "order-037,order-038" },
                { 3, "order-062,order-063" },
                { 6, "order-064,order-065" }
            };

            Assert.AreEqual(resultFlightCapacity[1], orderAllocations[1].Count);
            Assert.AreEqual(resultFlightCapacity[4], orderAllocations[4].Count);
            Assert.AreEqual(resultFlightCapacity[2], orderAllocations[2].Count);
            Assert.AreEqual(resultFlightCapacity[5], orderAllocations[5].Count);
            Assert.AreEqual(resultFlightCapacity[3], orderAllocations[3].Count);
            Assert.AreEqual(resultFlightCapacity[6], orderAllocations[6].Count);

            Assert.AreEqual(resultFlightOrders[1], string.Join(',', orderAllocations[1]));
            Assert.AreEqual(resultFlightOrders[4], string.Join(',', orderAllocations[4]));
            Assert.AreEqual(resultFlightOrders[2], string.Join(',', orderAllocations[2]));
            Assert.AreEqual(resultFlightOrders[5], string.Join(',', orderAllocations[5]));
            Assert.AreEqual(resultFlightOrders[3], string.Join(',', orderAllocations[3]));
            Assert.AreEqual(resultFlightOrders[6], string.Join(',', orderAllocations[6]));

        }


        /// <summary>
        /// Scenario: Multiple flights to a given destination on a day
        /// </summary>
        [Test]
        public void MultipleFlightsForDestinationOnDayTest()
        {
            var appsettingMock = new Mock<IAppSettings>();
            appsettingMock.Setup(f => f.FlightCapacity).Returns(20);

            CargoLoader = new CargoLoader(ServiceProvider.GetService<ILoggerFactory>(), ServiceProvider.GetService<IJsonReader>(), appsettingMock.Object);

            CargoLoader.LoadSchedule(new Flight() { FlightNo = 1, Source = "YUL", Destination = "YYZ", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 2, Source = "YUL", Destination = "YYC", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 3, Source = "YUL", Destination = "YVR", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 4, Source = "YUL", Destination = "YYZ", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 5, Source = "YUL", Destination = "YYC", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 6, Source = "YUL", Destination = "YVR", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 7, Source = "YUL", Destination = "YVR", Day = 2 });

            CargoLoader.ProcessOrders(@"..\..\..\TestData\coding-assigment-orders-test-3.json");

            var orderAllocations = CargoLoader.GetOrderAllocations();

            IDictionary<int, int> resultFlightCapacity = new Dictionary<int, int>
            {
                { 1, 20 },
                { 4, 14 },
                { 2, 20 },
                { 5, 2 },
                { 3, 20 },
                { 6, 20 },
                { 7, 3 }
            };

            IDictionary<int, string> resultFlightOrders = new Dictionary<int, string>
            {
                { 1, "order-001,order-002,order-003,order-004,order-005,order-006,order-007,order-008,order-009,order-010,order-011,order-012,order-013,order-014,order-015,order-016,order-017,order-018,order-019,order-020" },
                { 4, "order-021,order-022,order-023,order-024,order-025,order-026,order-027,order-028,order-029,order-030,order-031,order-032,order-033,order-034" },
                { 2, "order-035,order-036,order-037,order-038,order-039,order-040,order-041,order-042,order-043,order-044,order-045,order-046,order-047,order-048,order-050,order-052,order-054,order-055,order-056,order-057" },
                { 5, "order-060,order-061" },
                { 3, "order-062,order-063,order-064,order-065,order-066,order-067,order-068,order-069,order-070,order-071,order-072,order-073,order-074,order-075,order-076,order-077,order-078,order-080,order-081,order-082" },
                { 6, "order-083,order-084,order-085,order-086,order-087,order-088,order-089,order-090,order-091,order-092,order-093,order-094,order-095,order-096,order-097,order-098,order-099,order-0100,order-0101,order-0102" },
                { 7, "order-0103,order-0104,order-0105" },
            };

            Assert.AreEqual(resultFlightCapacity[1], orderAllocations[1].Count);
            Assert.AreEqual(resultFlightCapacity[4], orderAllocations[4].Count);
            Assert.AreEqual(resultFlightCapacity[2], orderAllocations[2].Count);
            Assert.AreEqual(resultFlightCapacity[5], orderAllocations[5].Count);
            Assert.AreEqual(resultFlightCapacity[3], orderAllocations[3].Count);
            Assert.AreEqual(resultFlightCapacity[6], orderAllocations[6].Count);
            Assert.AreEqual(resultFlightCapacity[7], orderAllocations[7].Count);

            Assert.AreEqual(resultFlightOrders[1], string.Join(',', orderAllocations[1]));
            Assert.AreEqual(resultFlightOrders[4], string.Join(',', orderAllocations[4]));
            Assert.AreEqual(resultFlightOrders[2], string.Join(',', orderAllocations[2]));
            Assert.AreEqual(resultFlightOrders[5], string.Join(',', orderAllocations[5]));
            Assert.AreEqual(resultFlightOrders[3], string.Join(',', orderAllocations[3]));
            Assert.AreEqual(resultFlightOrders[6], string.Join(',', orderAllocations[6]));
            Assert.AreEqual(resultFlightOrders[7], string.Join(',', orderAllocations[7]));

        }

        /// <summary>
        /// Scenario: Consider the destination YYE
        /// </summary>
        [Test]
        public void ConsiderDestinationYYETest()
        {
            var appsettingMock = new Mock<IAppSettings>();
            appsettingMock.Setup(f => f.FlightCapacity).Returns(20);

            CargoLoader = new CargoLoader(ServiceProvider.GetService<ILoggerFactory>(), ServiceProvider.GetService<IJsonReader>(), appsettingMock.Object);

            CargoLoader.LoadSchedule(new Flight() { FlightNo = 1, Source = "YUL", Destination = "YYZ", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 2, Source = "YUL", Destination = "YYC", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 3, Source = "YUL", Destination = "YVR", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 7, Source = "YUL", Destination = "YYE", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 4, Source = "YUL", Destination = "YYZ", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 5, Source = "YUL", Destination = "YYC", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 6, Source = "YUL", Destination = "YVR", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 8, Source = "YUL", Destination = "YYE", Day = 2 });

            CargoLoader.ProcessOrders(@"..\..\..\TestData\coding-assigment-orders.json");

            var orderAllocations = CargoLoader.GetOrderAllocations();

            IDictionary<int, int> resultFlightCapacity = new Dictionary<int, int>
            {
                { 1, 20 },
                { 4, 14 },
                { 2, 20 },
                { 5, 2 },
                { 3, 20 },
                { 6, 17 },
                { 7, 3 },
            };

            IDictionary<int, string> resultFlightOrders = new Dictionary<int, string>
            {
                { 1, "order-001,order-002,order-003,order-004,order-005,order-006,order-007,order-008,order-009,order-010,order-011,order-012,order-013,order-014,order-015,order-016,order-017,order-018,order-019,order-020" },
                { 4, "order-021,order-022,order-023,order-024,order-025,order-026,order-027,order-028,order-029,order-030,order-031,order-032,order-033,order-034" },
                { 2, "order-035,order-036,order-037,order-038,order-039,order-040,order-041,order-042,order-043,order-044,order-045,order-046,order-047,order-048,order-050,order-052,order-054,order-055,order-056,order-057" },
                { 5, "order-060,order-061" },
                { 3, "order-062,order-063,order-064,order-065,order-066,order-067,order-068,order-069,order-070,order-071,order-072,order-073,order-074,order-075,order-076,order-077,order-078,order-080,order-081,order-082" },
                { 6, "order-083,order-084,order-085,order-086,order-087,order-088,order-089,order-090,order-091,order-092,order-093,order-094,order-095,order-096,order-097,order-098,order-099" },
                { 7, "order-049,order-051,order-053" },
            };

            Assert.AreEqual(resultFlightCapacity[1], orderAllocations[1].Count);
            Assert.AreEqual(resultFlightCapacity[4], orderAllocations[4].Count);
            Assert.AreEqual(resultFlightCapacity[2], orderAllocations[2].Count);
            Assert.AreEqual(resultFlightCapacity[5], orderAllocations[5].Count);
            Assert.AreEqual(resultFlightCapacity[3], orderAllocations[3].Count);
            Assert.AreEqual(resultFlightCapacity[6], orderAllocations[6].Count);
            Assert.AreEqual(resultFlightCapacity[7], orderAllocations[7].Count);

            Assert.AreEqual(resultFlightOrders[1], string.Join(',', orderAllocations[1]));
            Assert.AreEqual(resultFlightOrders[4], string.Join(',', orderAllocations[4]));
            Assert.AreEqual(resultFlightOrders[2], string.Join(',', orderAllocations[2]));
            Assert.AreEqual(resultFlightOrders[5], string.Join(',', orderAllocations[5]));
            Assert.AreEqual(resultFlightOrders[3], string.Join(',', orderAllocations[3]));
            Assert.AreEqual(resultFlightOrders[6], string.Join(',', orderAllocations[6]));
            Assert.AreEqual(resultFlightOrders[7], string.Join(',', orderAllocations[7]));
        }

        /// <summary>
        /// Scenario: No orders available
        /// </summary>
        [Test]
        public void EmptyOrdersTest()
        {
            var appsettingMock = new Mock<IAppSettings>();
            appsettingMock.Setup(f => f.FlightCapacity).Returns(20);

            CargoLoader = new CargoLoader(ServiceProvider.GetService<ILoggerFactory>(), ServiceProvider.GetService<IJsonReader>(), appsettingMock.Object);

            CargoLoader.LoadSchedule(new Flight() { FlightNo = 1, Source = "YUL", Destination = "YYZ", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 2, Source = "YUL", Destination = "YYC", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 3, Source = "YUL", Destination = "YVR", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 7, Source = "YUL", Destination = "YYE", Day = 1 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 4, Source = "YUL", Destination = "YYZ", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 5, Source = "YUL", Destination = "YYC", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 6, Source = "YUL", Destination = "YVR", Day = 2 });
            CargoLoader.LoadSchedule(new Flight() { FlightNo = 8, Source = "YUL", Destination = "YYE", Day = 2 });

            CargoLoader.ProcessOrders(@"..\..\..\TestData\coding-assigment-orders-empty.json");

            var orderAllocations = CargoLoader.GetOrderAllocations();

            IDictionary<int, int> resultFlightCapacity = new Dictionary<int, int>();

            Assert.AreEqual(resultFlightCapacity.Count, orderAllocations.Keys.Count);
        }
    }
}