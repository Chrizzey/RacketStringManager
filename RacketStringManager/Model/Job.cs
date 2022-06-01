namespace RacketStringManager.Model
{
    public class Job
    {
        /// <summary>
        /// Gets or sets the ID for the job
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// Gets or sets the name of the player
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the racket
        /// </summary>
        public string Racket { get; set; }

        /// <summary>
        /// Gets or sets the stringing tension in kg
        /// </summary>
        public double Tension { get; set; }

        /// <summary>
        /// Gets or sets the name of the string to use
        /// </summary>
        public string StringName { get; set; }

        /// <summary>
        /// Gets or sets the date on which the job was created
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the stringinig jobs is completed
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the jobs was paid in full
        /// </summary>
        public bool IsPaid { get; set; }
        
        /// <summary>
        /// Gets or sets a free comment containing additional information about the job/racket/player/etc.
        /// </summary>
        public string Comment { get; set; }
    }
}
