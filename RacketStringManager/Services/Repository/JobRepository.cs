using RacketStringManager.Model;
using SQLite;

namespace RacketStringManager.Services.Repository
{
    public abstract class Entity
    {
        public virtual Guid Id { get; set; }
    }

    public class PlayerEntity : Entity
    {
        public string Name { get; set; }

        public string Racket { get; set; }
    }

    public class JobEntity : Entity
    {
        public PlayerEntity Player { get; set; }

        public double Tension { get; set; }

        public string StringName { get; set; }

        public DateOnly StartDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsPaid { get; set; }

        public string Comment { get; set; }

        public Job ToJob() => new()
        {
            Racket = Player.Racket,
            Name = Player.Name,
            Tension = Tension,
            StringName = StringName,
            StartDate = StartDate,
            IsCompleted = IsCompleted,
            IsPaid = IsPaid,
            Comment = Comment
        };
    }

    internal class JobRepository : IJobRepository
    {
        private const string DbName = "RacketStringManager.data.db";
        private readonly SQLiteConnection _database;

        public JobRepository()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<Job>();
        }

        public Task<IEnumerable<Job>> GetAllJobs()
        {
            var jobs = _database.Table<JobEntity>().Select(entity => entity.ToJob()).ToList();

            return Task.FromResult((IEnumerable<Job>)jobs);
        }

        public int Create(JobEntity entity)
        {
            return _database.Insert(entity);
        }

        public int Update(JobEntity entity)
        {
            return _database.Update(entity);
        }

        public int Delete(JobEntity entity)
        {
            return _database.Delete(entity);
        }
    }
}
