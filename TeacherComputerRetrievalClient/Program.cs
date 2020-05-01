namespace TeacherComputerRetrievalClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TeacherComputerRetrievalLibrary;

    class Program
    {
        static async Task Main(string[] args)
        {
            //Test Input
            //AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7
            IAcademiesNetworkOperations<string> routeOperations = new AcademiesNetworkOperations<string>();
            var network = await routeOperations.CreateAcademiesNetworkAsync(new List<(string start, string end, int distance)> {
                ("Kiserian", "Rongai", 5), // AB5
                ("Kiserian", "Kajiado", 7),  // AE7
                ("Kiserian", "Karen", 5), // AD5
                ("Rongai", "Masai", 4),  // BC4
                ("Masai", "Karen", 8),   // CD8
                ("Karen", "Masai", 8),   // DC8
                ("Karen", "Kajiado", 6),  // DE6
                ("Masai", "Kajiado", 2),  // CE2
                ("Kajiado", "Rongai", 3)  // EB3
            });

            Console.WriteLine();
            Console.WriteLine($"Distance from Kiserian -> Rongai -> Masai : {await routeOperations.TotalDistanceAlongRouteAsync(new List<string> { "Kiserian", "Rongai", "Masai" }, network)}");
            Console.WriteLine($"Distance from Kiserian -> Kajiado -> Rongai -> Masai -> Karen : {await routeOperations.TotalDistanceAlongRouteAsync(new List<string> { "Kiserian", "Kajiado", "Rongai", "Masai", "Karen" }, network)}");
            Console.WriteLine($"Distance from Kiserian -> Kajiado -> Karen : {await routeOperations.TotalDistanceAlongRouteAsync(new List<string> { "Kiserian", "Kajiado", "Karen" }, network)}");

            Console.WriteLine();
            Console.WriteLine("Find all paths between Masai -> Masai with Maximum of 3 stops");
            var withStopsLessThanEqual  = await routeOperations.TotalRoutesBetweenAcademiesWithStopsAsync("Masai", "Masai", 3, network);
            Console.WriteLine($"Total number of routes - {withStopsLessThanEqual.Count}");
            foreach (var (path, total) in withStopsLessThanEqual)
            {
                Console.WriteLine($"{string.Join("->", path)} with total distance : {total}");
            }

            Console.WriteLine();
            Console.WriteLine("Find all paths between Kiserian -> Masai with 4 stops exactly");
            var withExactStops = await routeOperations.TotalRoutesBetweenAcademiesWithStopsAsync("Kiserian", "Masai", 4, network, true);
            Console.WriteLine($"Total number of routes - {withExactStops.Count}");

            foreach (var (path, total) in withExactStops)
            {
                Console.WriteLine($"{string.Join("->", path)} with total distance : {total}");
            }

            Console.WriteLine();
            Console.WriteLine("Find shortest path between Kiserian -> Masai");
            var shortestRoute = await routeOperations.ShortestRouteBetweenAcademiesAsync("Kiserian", "Masai", network);
            Console.WriteLine($"{string.Join("->", shortestRoute.path)} with total distance : {shortestRoute.total}");
            Console.WriteLine();
            Console.WriteLine("Find shortest path between Rongai -> Rongai");
            shortestRoute = await routeOperations.ShortestRouteBetweenAcademiesAsync("Rongai", "Rongai", network);
            Console.WriteLine($"{string.Join("->", shortestRoute.path)} with total distance : {shortestRoute.total}");

            Console.WriteLine();
            Console.WriteLine("Total paths between Masai -> Masai with distance less than 30");
            var routeWithRange = await routeOperations.TotalRoutesBetweenAcademiesWithDistanceRangeAsync("Masai", "Masai", 30, network);
            Console.WriteLine($"Total number of routes - {routeWithRange.Count}");
            foreach (var (path, total) in routeWithRange)
            {
                Console.WriteLine($"{string.Join("->", path)} with total distance : {total}");
            }
            Console.ReadLine();
        }
    }
}
