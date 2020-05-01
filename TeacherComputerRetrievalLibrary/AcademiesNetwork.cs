namespace TeacherComputerRetrievalLibrary
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Academies Network
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AcademiesNetwork<T>
    {
        /// <summary>
        /// The academies
        /// </summary>
        public readonly Dictionary<T, City<T>> academies = new Dictionary<T, City<T>>();

        /// <summary>
        /// Adds the edge asynchronous.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="cost">The cost.</param>
        public async Task AddEdgeAsync(T source, T destination, int cost)
        {
            var start = await GetAcademyAsync(source);
            start.Neighbours.Add(new Route<T>(await GetAcademyAsync(destination), cost));
        }

        /// <summary>
        /// Gets the academy asynchronous.
        /// </summary>
        /// <param name="cityName">Name of the city.</param>
        /// <returns></returns>
        private async Task<City<T>> GetAcademyAsync(T cityName)
        {
            academies.TryGetValue(cityName, out City<T> city);
            if (city == null)
            {
                city = new City<T>(cityName);
                academies.Add(cityName, city);
            }
            return await Task.FromResult(city);
        }
    }
}
