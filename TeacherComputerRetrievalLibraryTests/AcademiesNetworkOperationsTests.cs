namespace TeacherComputerRetrievalLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TeacherComputerRetrievalLibrary;

    /// <summary>
    /// Academies Network Operations Test
    /// </summary>
    [TestClass]
    public class AcademiesNetworkOperationsTest
    {
        /// <summary>
        /// The route operations
        /// </summary>
        private IAcademiesNetworkOperations<char> routeOperations = null;

        /// <summary>
        /// The network
        /// </summary>
        private AcademiesNetwork<char> network = null;

        /// <summary>
        /// Initializes the network.
        /// </summary>
        [TestInitialize]
        public async Task InitializeNetwork()
        {
            routeOperations = new AcademiesNetworkOperations<char>();
            network = await routeOperations.CreateAcademiesNetworkAsync(new List<(char start, char end, int distance)> {
                ('A', 'B', 5),
                ('A', 'E', 7),
                ('A', 'D', 5),
                ('B', 'C', 4),
                ('C', 'D', 8),
                ('D', 'C', 8),
                ('D', 'E', 6),
                ('C', 'E', 2),
                ('E', 'B', 3)
            });
        }

        /// <summary>
        /// Shortests the route between academies asynchronous test.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestMethod]
        [DataRow(new char[] { 'A', 'B', 'C' }, "9")]
        public async Task RouteDistanceCheckTest(char[] route, string expectedResult)
        {
            var result = await routeOperations.TotalDistanceAlongRouteAsync(route.ToList(), network);
            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        /// Noes the route check test asynchronous.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestMethod]
        [DataRow(new char[] { 'A', 'E', 'D' }, "NO SUCH ROUTE")]
        public async Task NoRouteCheckTestAsync(char[] route, string expectedResult)
        {
            var result = await routeOperations.TotalDistanceAlongRouteAsync(route.ToList(), network);
            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        /// Routes the distance check test v2 asynchronous.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestMethod]
        [DataRow(new char[] { 'A', 'E', 'B', 'C', 'D' }, "22")]
        public async Task RouteDistanceCheckTestV2Async(char[] route, string expectedResult)
        {
            var result = await routeOperations.TotalDistanceAlongRouteAsync(route.ToList(), network);
            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        /// Totals the trips from origin to origin with maximum three stops check.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="stops">The stops.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestMethod]
        [DataRow('C','C', 3, 2)]
        public async Task TotalTripsFromOriginToOriginWithMaximumThreeStopsCheckAsync(char start, char end, int stops, int expectedResult)
        {
            var withStopsLessThanEqual = await routeOperations.TotalRoutesBetweenAcademiesWithStopsAsync(start, end, stops, network);
            Assert.AreEqual(withStopsLessThanEqual.Count, expectedResult);
        }

        /// <summary>
        /// Totals the trips from origin to origin with exactly four stops check asynchronous.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="exactStops">if set to <c>true</c> [exact stops].</param>
        /// <param name="stops">The stops.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestMethod]
        [DataRow('A', 'C', true, 4, 3)]
        public async Task TotalTripsFromOriginToOriginWithExactlyFourStopsCheckAsync(char start, char end, bool exactStops, int stops, int expectedResult)
        {
            var withStopsLessThanEqual = await routeOperations.TotalRoutesBetweenAcademiesWithStopsAsync(start, end, stops, network, exactStops);
            Assert.AreEqual(withStopsLessThanEqual.Count, expectedResult);
        }

        /// <summary>
        /// Totals the trips under a distance range asynchronous.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="distanceRange">The distance range.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestMethod]
        [DataRow('C', 'C', 30, 7)]
        public async Task TotalTripsUnderADistanceRangeAsync(char start, char end, int distanceRange, int expectedResult)
        {
            var result = await routeOperations.TotalRoutesBetweenAcademiesWithDistanceRangeAsync(start, end, distanceRange, network);
            Assert.AreEqual(result.Count, expectedResult);
        }

        /// <summary>
        /// Invalids the cities shortest path test asynchronous.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestMethod]
        [DataRow('B', 'M', int.MaxValue)]
        public async Task InvalidCitiesShortestPathTestAsync(char start, char end, int expectedResult)
        {
            var (_, total) = await routeOperations.ShortestRouteBetweenAcademiesAsync(start, end, network);
            Assert.AreEqual(total, expectedResult);
        }

        /// <summary>
        /// Totals the trips under invalid range asynchronous.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="distanceRange">The distance range.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestMethod]
        [DataRow('C', 'C', 0, null)]
        public async Task TotalTripsUnderInvalidRangeAsync(char start, char end, int distanceRange, int? expectedResult)
        {
            var result = await routeOperations.TotalRoutesBetweenAcademiesWithDistanceRangeAsync(start, end, distanceRange, network);
            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        /// Routes the check to self asynchronous.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestMethod]
        [DataRow(new char[] { 'B', 'B' }, "NO SUCH ROUTE")]
        public async Task RouteCheckToSelfAsync(char[] route, string expectedResult)
        {
            var result = await routeOperations.TotalDistanceAlongRouteAsync(route.ToList(), network);
            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        /// Routes the check to unknown city asynchronous.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="expectedResult">The expected result.</param>
        [TestMethod]
        [DataRow(new char[] { 'B', 'Z' }, "NO SUCH ROUTE")]
        public async Task RouteCheckToUnknownCityAsync(char[] route, string expectedResult)
        {
            var result = await routeOperations.TotalDistanceAlongRouteAsync(route.ToList(), network);
            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        /// Shortests the path between two cities asynchronous.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="expectedResult">The expected result.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetShortestPathData), DynamicDataSourceType.Method)]
        public async Task ShortestPathBetweenTwoCitiesAsync(char start, char end, int expectedResult)
        {
            var (_, total) = await routeOperations.ShortestRouteBetweenAcademiesAsync(start, end, network);
            Assert.AreEqual(total, expectedResult);
        }

        /// <summary>
        /// Gets the shortest path data.
        /// </summary>
        /// <returns>all test case data with expected result</returns>
        public static IEnumerable<object[]> GetShortestPathData()
        {
            yield return new object[] { 'A', 'C', 9 };
            yield return new object[] { 'B', 'B', 9 };
        }

        /// <summary>
        /// Routes the distance check for all cases test.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="expectedResult">The expected result.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetTotalDistanceCheckData), DynamicDataSourceType.Method)]
        public async Task RouteDistanceCheckForAllCasesTest(char[] route, string expectedResult)
        {
            var result = await routeOperations.TotalDistanceAlongRouteAsync(route.ToList(), network);
            Assert.AreEqual(result, expectedResult);
        }

        /// <summary>
        /// Gets the total distance check data.
        /// </summary>
        /// <returns>all test case data with expected result</returns>
        public static IEnumerable<object[]> GetTotalDistanceCheckData()
        {
            yield return new object[] { new char[] { 'A', 'E', 'B', 'C', 'D' }, "22" };
            yield return new object[] { new char[] { 'A', 'B', 'C' }, "9" };
            yield return new object[] { new char[] { 'A', 'E', 'D' }, "NO SUCH ROUTE" };     
        }
    }
}