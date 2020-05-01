namespace TeacherComputerRetrievalLibrary
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for Academies Network Operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAcademiesNetworkOperations<T>
    {
        /// <summary>
        /// Creates the academies network asynchronous.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <returns>
        /// cities network
        /// </returns>
        Task<AcademiesNetwork<T>> CreateAcademiesNetworkAsync(List<(T start, T end, int distance)> locations);

        /// <summary>
        /// Calculates the total distance along route.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <param name="network">The network.</param>
        /// <returns>
        /// a distance value in number or 'No SUCH ROUTE' as message.
        /// </returns>
        Task<string> TotalDistanceAlongRouteAsync(List<T> locations, AcademiesNetwork<T> network);

        /// <summary>
        /// Total routes between academies.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="range">The range.</param>
        /// <param name="network">The network.</param>
        /// <returns>
        /// total routes between 2 academies
        /// </returns>
        Task<List<(List<T> path, int total)>> TotalRoutesBetweenAcademiesWithDistanceRangeAsync(T start, T end, int range, AcademiesNetwork<T> network);

        /// <summary>
        /// Totals the routes between academies with stops asynchronous.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="stops">The stops.</param>
        /// <param name="network">The network.</param>
        /// <param name="matchExact">if set to <c>true</c> [match exact].</param>
        /// <returns>
        /// list of routes and their total distance with cities less than mentioned stops
        /// </returns>
        Task<List<(List<T> path, int total)>> TotalRoutesBetweenAcademiesWithStopsAsync(T start, T end, int stops, AcademiesNetwork<T> network, bool matchExact = false);

        /// <summary>
        /// Shortests route between academies.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="network">The network.</param>
        /// <returns>
        /// shortest path
        /// </returns>
        Task<(List<T> path, int total)> ShortestRouteBetweenAcademiesAsync(T start, T end, AcademiesNetwork<T> network);
    }
}
