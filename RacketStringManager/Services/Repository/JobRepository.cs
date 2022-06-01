using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using RacketStringManager.Model;
using SQLite;

namespace RacketStringManager.Services.Repository
{
    public abstract class Entity
    {
        [PrimaryKey, AutoIncrement]
        public virtual Guid Id { get; set; } = new Guid();
    }

    public class PlayerEntity : Entity
    {
        public string Name { get; set; } = string.Empty;
    }

    public class RacketEntity : Entity
    {
        public string Name { get; set; } = string.Empty;
    }

    public class StringEntity : Entity
    {
        public string Name { get; set; } = string.Empty;
    }

    public class JobEntity : Entity
    {
        [ForeignKey(nameof(PlayerEntity))]
        public Guid PlayerId { get; set; }

        [ForeignKey(nameof(RacketEntity))]
        public Guid RacketId { get; set; }

        [ForeignKey(nameof(StringEntity))]
        public Guid StringId { get; set; }

        public double Tension { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsPaid { get; set; }

        public string Comment { get; set; }

        public JobEntity()
        {
            Comment = string.Empty;
        }

        public JobEntity(Job job)
        {
            PlayerId = Guid.Empty;
            RacketId = Guid.Empty;
            StringId = Guid.Empty;
            Tension = job.Tension;
            StartDate = job.StartDate.ToDateTime(TimeOnly.MinValue);
            IsCompleted = job.IsCompleted;
            IsPaid = job.IsPaid;
            Comment = job.Comment;
        }
    }

    internal class JobRepository : IJobRepository
    {
        private const string DbName = "RacketStringManager.data.db";
        private readonly SQLiteConnection _database;

        public JobRepository()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);
            Debug.WriteLine("Initialized database @ {0}", dbPath, "database");
            _database = new SQLiteConnection(dbPath);
            CreateTables();
        }

        private void CreateTables()
        {
            _database.CreateTable<JobEntity>();
            _database.CreateTable<PlayerEntity>();
            _database.CreateTable<RacketEntity>();
            _database.CreateTable<StringEntity>();
        }

        public IEnumerable<Job> GetAllJobs()
        {
            return _database.Table<JobEntity>().Select(EntityToJob);
        }

        public IEnumerable<Job> FindJobsFor(string player)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Job> FindJobsFor(string player, string racket)
        {
            throw new NotImplementedException();
        }

        private Job EntityToJob(JobEntity jobEntity)
        {
            var playerName = _database.Find<PlayerEntity>(jobEntity.PlayerId)?.Name ?? "name not found";
            var racketName = _database.Find<RacketEntity>(jobEntity.RacketId)?.Name ?? "racket not found";
            var stringName = _database.Find<StringEntity>(jobEntity.StringId)?.Name ?? "string not found";

            return new Job(
                new EntityModel(jobEntity.PlayerId, playerName),
                new EntityModel(jobEntity.RacketId, racketName),
                new EntityModel(jobEntity.StringId, stringName)
            )
            {
                Comment = jobEntity.Comment,
                Tension = jobEntity.Tension,
                StartDate = DateOnly.FromDateTime(jobEntity.StartDate),
                IsCompleted = jobEntity.IsCompleted,
                IsPaid = jobEntity.IsPaid,
                Name = playerName,
                Racket = racketName,
                StringName = stringName,
                JobId = jobEntity.Id
            };
        }

        public int Create(Job job)
        {
            var jobEntity = new JobEntity(job);

            if (job.GetPlayerId == Guid.Empty)
            {
                var player = FindOrInsertPlayer(job);
                jobEntity.PlayerId = player.Id;
            }
            else
            {
                jobEntity.PlayerId = job.GetPlayerId;
            }

            if (job.GetRacketId == Guid.Empty)
            {
                var racket = FindOrInsertRacket(job);
                jobEntity.RacketId = racket.Id;
            }
            else
            {
                jobEntity.RacketId = job.GetRacketId;
            }

            if (job.GetStringId == Guid.Empty)
            {
                var stringEntity = FindOrInsertString(job);
                jobEntity.StringId = stringEntity.Id;
            }
            else
            {
                jobEntity.StringId = job.GetStringId;
            }

            return _database.Insert(jobEntity);
        }

        private StringEntity FindOrInsertString(Job job)
        {
            var stringEntity = _database.FindWithQuery<StringEntity>($"select * from {nameof(StringEntity)} where name='{job.StringName}'");
            if (stringEntity is null)
            {
                stringEntity = new StringEntity { Name = job.StringName };
                _database.Insert(stringEntity);
            }

            return stringEntity;
        }

        private RacketEntity FindOrInsertRacket(Job job)
        {
            var racket = _database.FindWithQuery<RacketEntity>($"select * from {nameof(RacketEntity)} where name = '{job.Racket}'");
            if (racket is null)
            {
                racket = new RacketEntity { Name = job.Racket };
                _database.Insert(racket);
            }

            return racket;
        }

        private PlayerEntity FindOrInsertPlayer(Job job)
        {
            return FindOrInsertPlayer(job.Name);
        }

        private PlayerEntity FindOrInsertPlayer(string playerName)
        {
            var player = _database.FindWithQuery<PlayerEntity>($"select * from {nameof(PlayerEntity)} where name = '{playerName}'");
            if (player is null)
            {
                player = new PlayerEntity { Name = playerName };
                _database.Insert(player);
            }

            return player;
        }

        public Job? Find(Guid id)
        {
            var entity = _database.Find<JobEntity>(id);
            return entity is null ? null : EntityToJob(entity);
        }

        public IEnumerable<string> GetAllPlayerNames()
        {
            return _database.Table<PlayerEntity>().Select(x => x.Name);
        }

        public IEnumerable<string> GetAllStringNames()
        {
            return _database.Table<StringEntity>().Select(x => x.Name);
        }

        public IEnumerable<string> GetAllRacketsForPlayer(string playerName)
        {
            var playerEntity = FindOrInsertPlayer(playerName);

            var query = $"select * from {nameof(RacketEntity)} where id in (" +
                        $"select racketId from {nameof(JobEntity)} where {nameof(JobEntity.PlayerId)} = '{playerEntity.Id}')";

            return _database.Query<RacketEntity>(query).Select(x => x.Name);
        }

        public int Update(JobEntity entity)
        {
            return _database.Update(entity);
        }

        public int Delete(Job job)
        {
            var entity = new JobEntity { Id = job.JobId };
            return _database.Delete(entity);
        }

        public void Clear()
        {
            _database.DropTable<JobEntity>();
            _database.DropTable<PlayerEntity>();
            _database.DropTable<RacketEntity>();
            _database.DropTable<StringEntity>();

            CreateTables();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
