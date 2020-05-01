namespace TeacherComputerRetrievalLibrary
{
    using System.Collections.Generic;

    /// <summary>
    /// City class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class City<T>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public T Name { get; set; }

        /// <summary>
        /// Gets or sets the neighbours.
        /// </summary>
        /// <value>
        /// The neighbours.
        /// </value>
        public List<Route<T>> Neighbours { get; set; }

        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        /// <value>
        /// The distance.
        /// </value>
        public int Distance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="City{T}"/> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        public City(T Name)
        {
            this.Name = Name;
            Neighbours = new List<Route<T>>();
            Distance = int.MaxValue;
        }
    }
}