namespace TeacherComputerRetrievalLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for network operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="TeacherComputerRetrievalLibrary.IAcademiesNetworkOperations{T}" />
    public class AcademiesNetworkOperations<T> : IAcademiesNetworkOperations<T>
    {
        /// <summary>
        /// Creates the academies network asynchronous.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <returns>
        /// cities network
        /// </returns>
        public async Task<AcademiesNetwork<T>> CreateAcademiesNetworkAsync(List<(T start, T end, int distance)> locations)
        {
            var result = new AcademiesNetwork<T>();
            foreach (var (start, end, distance) in locations)
            {
                await result.AddEdgeAsync(start, end, distance);
            }

            return result;

        }

        /// <summary>
        /// Shortests route between academies.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="network">The network.</param>
        /// <returns>
        /// shortest path
        /// </returns>
        public async Task<(List<T> path, int total)?> ShortestRouteBetweenAcademiesAsync(T start, T end, AcademiesNetwork<T> network)
        {
            if (network == null || network.academies.Count < 1)
            {
                return null;
            }

            var path = (new List<T>
            {
                start
            }, total: 0);

            var shortestPath = (new List<T>
            {
                start
            }, total: int.MaxValue);

            GetShortestPath(start, end, ref path, network.academies, ref shortestPath);
            return await Task.FromResult(shortestPath);
        }

        /// <summary>
        /// Calculates the total distance along route.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <param name="network">The network.</param>
        /// <returns>
        /// a distance value in number or 'No SUCH ROUTE' as message.
        /// </returns>
        public async Task<string> TotalDistanceAlongRouteAsync(List<T> locations, AcademiesNetwork<T> network)
        {
            if (network == null || network.academies.Count < 1)
            {
                return null;
            }

            return await GetRouteDistanceAsync(locations, network.academies);
        }

        /// <summary>
        /// Total routes between academies.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="network">The network.</param>
        /// <param name="range"></param>
        /// <returns>
        /// total routes between 2 academies
        /// </returns>
        public async Task<List<(List<T> path, int total)>> TotalRoutesBetweenAcademiesWithDistanceRangeAsync(T start, T end, int range, AcademiesNetwork<T> network)
        {
            if (network == null || network.academies.Count < 1 || range <= 0)
            {
                return null;
            }

            var paths = new List<(List<T> path, int total)>();

            GetAllRoutesUnderDistanceRange(start, end, (new List<T>
            {
                start
            }, 0), network.academies, ref paths, range);
            return await Task.FromResult(paths);
        }

        /// <summary>
        /// Totals the routes between academies with stops asynchronous.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="stops">The stops.</param>
        /// <param name="network">The network.</param>
        /// <param name="matchExact">if set to <c>true</c> [match exact].</param>
        /// <returns>list of routes and their total distance with cities less than mentioned stops</returns>
        public async Task<List<(List<T> path, int total)>> TotalRoutesBetweenAcademiesWithStopsAsync(T start, T end, int stops, AcademiesNetwork<T> network, bool matchExact = false)
        {
            if (network == null || network.academies.Count < 1 || stops <= 0)
            {
                return null;
            }

            var paths = new List<(List<T> path, int total)>();

            GetAllRoutesByStops(start, end, (new List<T>
            {
                start
            }, 0), network.academies, ref paths, stops, matchExact);
            return await Task.FromResult(paths);
        }

        /// <summary>
        /// Gets all routes.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="path">The path.</param>
        /// <param name="academies">The academies.</param>
        /// <param name="paths">The paths.</param>
        /// <param name="range">The range.</param>
        private void GetAllRoutesUnderDistanceRange(T start, T destination,
                    (List<T> cities, int total) path, Dictionary<T, City<T>> academies, ref List<(List<T> path, int total)> paths, int range)
        {
            if (start.Equals(destination) && path.total != 0 && path.total < range)
            {
                paths.Add((path: new List<T>(path.cities), path.total));
            }

            foreach (var i in academies[start].Neighbours)
            {
                if (range > path.total + i.Distance)
                {
                    path.cities.Add(i.Destination.Name);
                    path.total += i.Distance;

                    GetAllRoutesUnderDistanceRange(i.Destination.Name, destination, path, academies, ref paths, range);
                    path.total -= i.Distance;
                }
            }

            path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(start)));
        }

        /// <summary>
        /// Gets all routes by stops.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="path">The path.</param>
        /// <param name="academies">The academies.</param>
        /// <param name="paths">The paths.</param>
        /// <param name="stops">The stops.</param>
        /// <param name="matchExact">if set to <c>true</c> [match exact].</param>
        private void GetAllRoutesByStops(T start, T destination,
            (List<T> cities, int total) path, Dictionary<T, City<T>> academies, ref List<(List<T> path, int total)> paths,
                int stops, bool matchExact)
        {
            if (start.Equals(destination) && path.total != 0)
            {
                if (matchExact ? path.cities.Count - 1 == stops : path.cities.Count - 1 <= stops)
                {
                    paths.Add((path: new List<T>(path.cities), path.total));
                }
            }

            foreach (var i in academies[start].Neighbours)
            {
                if (path.cities.Count <= stops)
                {
                    path.cities.Add(i.Destination.Name);
                    path.total += i.Distance;

                    GetAllRoutesByStops(i.Destination.Name, destination,
                                        path, academies, ref paths, stops, matchExact);
                    path.total -= i.Distance;
                }
            }

            path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(start)));
        }


        /// <summary>
        /// Gets the shortest path.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="path">The path.</param>
        /// <param name="academies">The academies.</param>
        /// <param name="shortestPath">The shortest path.</param>
        private void GetShortestPath(T start, T destination, ref (List<T> cities, int total) path, Dictionary<T, City<T>> academies, ref (List<T> cities, int total) shortestPath)
        {
            if (start.Equals(destination) && path.total != 0)
            {
                shortestPath = (cities: new List<T>(path.cities), path.total);
            }

            foreach (var i in academies[start].Neighbours)
            {
                if (!path.cities.Contains(i.Destination.Name) || i.Destination.Name.Equals(destination))
                {
                    if (shortestPath.total > (path.total + i.Distance))
                    {
                        path.cities.Add(i.Destination.Name);
                        path.total += i.Distance;

                        GetShortestPath(i.Destination.Name, destination,
                                            ref path, academies, ref shortestPath);

                        path.total -= i.Distance;
                    }
                }
            }

            path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(start)));
        }
        /// <summary>
        /// Gets the path distance asynchronous.
        /// </summary>
        /// <param name="cities">The cities.</param>
        /// <param name="academies">The academies.</param>
        /// <returns>total distance of specified route OR "NO SUCH ROUTE"</returns>
        private async Task<string> GetRouteDistanceAsync(List<T> cities, Dictionary<T, City<T>> academies)
        {
            var totalDistance = 0;
            if (cities.Count == 1)
            {
                return "NO SUCH ROUTE";
            }

            for (int i = 1; i < cities.Count; i++)
            {
                var s = academies[cities[i - 1]]?.Neighbours.FirstOrDefault(t => t.Destination.Name.Equals(cities[i]))?.Distance;
                if (s.HasValue)
                    totalDistance += s.Value;
                else
                    return "NO SUCH ROUTE";
            }

            return await Task.FromResult(totalDistance.ToString());
        }
    }
}