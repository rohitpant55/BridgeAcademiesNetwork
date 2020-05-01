namespace TeacherComputerRetrievalLibrary
{
    /// <summary>
    /// Route between cities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Route<T>
    {
        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public City<T> Destination { get; set; }

        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        /// <value>
        /// The distance.
        /// </value>
        public int Distance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Route{T}"/> class.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="distance">The distance.</param>
        public Route(City<T> destination, int distance)
        {
            Destination = destination;
            Distance = distance;
        }
    }
}