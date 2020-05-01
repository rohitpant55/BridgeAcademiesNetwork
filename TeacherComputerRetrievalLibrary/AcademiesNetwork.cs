namespace TeacherComputerRetrievalLibrary
{
    using System;
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

        /// <summary>
        /// Gets all paths asynchronous.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public async Task GetAllPathsAsync(T start, T end)
        {
            List<T> visitedList = new List<T>();

            var path = (new List<T>
            {
                start  // Origin city added by default
            }, 0);


            await GetAllRoutesAsync(start, end, visitedList, path);
        }

        /// <summary>
        /// Gets all routes asynchronous.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="visitedList">The visited list.</param>
        /// <param name="path">The path.</param>
        private async Task GetAllRoutesAsync(T start, T destination, List<T> visitedList, (List<T> cities, int total) path)
        {
            if (path.total != 0)
                visitedList.Add(start);

            if (start.Equals(destination) && path.total != 0)
            {
                Console.WriteLine($"{string.Join("->", path.cities)} with cost {path.total}");

                visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));
                return;
            }

            foreach (var i in academies[start].Neighbours)
            {
                path.cities.Add(i.Destination.Name);
                path.total += i.Distance;

                await GetAllRoutesAsync(i.Destination.Name, destination, visitedList,
                                    path);

                path.total -= i.Distance;
                path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(i.Destination.Name)));  // This is to make sure that we only visit a city once.
            }

            // Mark the current node  
            //visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));
        }
    }
}
