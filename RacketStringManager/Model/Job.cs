namespace RacketStringManager.Model
{
    public class EntityModel
    {
        public Guid Id { get; }

        public string Name { get; set; }

        public EntityModel(Guid id, string name)
        {
            Name = name;
            Id = id;
        }
    }

    public class Job
    {
        private readonly EntityModel _player;
        private readonly EntityModel _racket;
        private readonly EntityModel _string;

        /// <summary>
        /// Gets or sets the ID for the job
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// Gets or sets the name of the player
        /// </summary>
        public string Name
        {
            get => _player.Name;
            set => _player.Name = value;
        }

        /// <summary>
        /// Gets or sets the name of the racket
        /// </summary>
        public string Racket
        {
            get => _racket.Name;
            set => _racket.Name = value;
        }

        /// <summary>
        /// Gets or sets the name of the string to use
        /// </summary>
        public string StringName
        {
            get => _string.Name;
            set => _string.Name = value;
        }

        /// <summary>
        /// Gets or sets the stringing tension in kg
        /// </summary>
        public double Tension { get; set; }

        /// <summary>
        /// Gets or sets the date on which the job was created
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the stringing jobs is completed
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

        public Job()
        {
            Comment = string.Empty;
            _player = new EntityModel(Guid.Empty, string.Empty);
            _racket = new EntityModel(Guid.Empty, string.Empty);
            _string = new EntityModel(Guid.Empty, string.Empty);
        }

        public Job(EntityModel player, EntityModel racket, EntityModel stringing)
        : this()
        {
            _player = player;
            _racket = racket;
            _string = stringing;
        }

        public Guid GetPlayerId => _player.Id;

        public Guid GetRacketId => _racket.Id;

        public Guid GetStringId => _string.Id;
    }
}
