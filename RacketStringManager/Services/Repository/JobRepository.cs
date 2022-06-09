using RacketStringManager.Model;
using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository
{
    public interface IJobService
    {
        IEnumerable<Job> GetAllJobs();
        IEnumerable<Job> FindJobsFor(string player);
        IEnumerable<Job> FindJobsFor(string player, string racket);
        void Create(Job job);
        Job Find(Guid id);
        IEnumerable<string> GetAllPlayerNames();
        IEnumerable<string> GetAllStringNames();
        IEnumerable<string> GetAllRacketsForPlayer(string playerName);
        void Update(Job job);
        void Delete(Job job);
    }

    public class JobService : IJobService, IDisposable
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IRacketRepository _racketRepository;
        private readonly IStringingRepository _stringRepository;
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository, IPlayerRepository playerRepository, IRacketRepository racketRepository, IStringingRepository stringRepository)
        {
            _jobRepository = jobRepository;
            _playerRepository = playerRepository;
            _racketRepository = racketRepository;
            _stringRepository = stringRepository;
        }

        public IEnumerable<Job> GetAllJobs()
        {
            return _jobRepository.GetAllJobs().Select(EntityToJob);
        }

        public IEnumerable<Job> FindJobsFor(string player)
        {
            var playerEntity = _playerRepository.Find(player);
            if(playerEntity is null)
                yield break;

            foreach(var jobEntity in _jobRepository.FindJobsFor(playerEntity))
            {
                yield return EntityToJob(jobEntity);
            }
        }

        public IEnumerable<Job> FindJobsFor(string player, string racket)
        {
            var playerEntity = _playerRepository.Find(player);
            if (playerEntity is null)
                yield break;

            var racketEntity = _racketRepository.Find(racket);
            if (racketEntity is null)
                yield break;

            foreach (var job in _jobRepository.FindJobsFor(playerEntity, racketEntity))
            {
                yield return EntityToJob(job);
            }
        }

        public void Create(Job job)
        {            
            var jobEntity = JobToJobEntity(job);
            _jobRepository.Create(jobEntity);
        }

        public Job Find(Guid id)
        {
            var entity = _jobRepository.Find(id);
            return entity is null ? null : EntityToJob(entity);
        }
        
        public IEnumerable<string> GetAllPlayerNames()
        {
            return _playerRepository.GetAll().Select(x => x.Name);
        }

        public IEnumerable<string> GetAllStringNames()
        {
            return _stringRepository.GetAll().Select(x => x.Name);
        }

        public IEnumerable<string> GetAllRacketsForPlayer(string playerName)
        {
            var playerEntity =_playerRepository.Find(playerName);
            if(playerEntity is null)
                return Array.Empty<string>();

            return _jobRepository.GetAllRacketsForPlayer(playerEntity);
        }

        public void Update(Job job)
        {
            var entity = JobToJobEntity(job);
            _jobRepository.Update(entity);
        }

        public void Delete(Job job)
        {            
            var entity = new JobEntity { Id = job.JobId };
            _jobRepository.Delete(entity);
        }

        private Job EntityToJob(JobEntity jobEntity)
        {
            var playerName = _playerRepository.Get(jobEntity.PlayerId)?.Name ?? "name not found";
            var racketName = _racketRepository.Get(jobEntity.RacketId)?.Name ?? "racket not found";
            var stringName = _stringRepository.Get(jobEntity.StringId)?.Name ?? "string not found";

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

        private JobEntity JobToJobEntity(Job job)
        {
            var jobEntity = new JobEntity(job);

            if (job.JobId != Guid.Empty)
            {
                jobEntity.Id = job.JobId;
            }

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

            return jobEntity;
        }
        
        private PlayerEntity FindOrInsertPlayer(Job job)
        {
            var playerEntity = _playerRepository.Find(job.Name);
            if (playerEntity is null)
            {
                playerEntity = new PlayerEntity {Name = job.Name};
                _playerRepository.Insert(playerEntity);
            }

            return playerEntity;
        }
        
        private RacketEntity FindOrInsertRacket(Job job)
        {
            var racketEntity = _racketRepository.Find(job.Racket);
            if (racketEntity is null)
            {
                racketEntity = new RacketEntity {Name = job.Racket};
                _racketRepository.Insert(racketEntity);
            }

            return racketEntity;
        }

        private StringEntity FindOrInsertString(Job job)
        {
            var stringEntity = _stringRepository.Find(job.StringName);
            if (stringEntity is null)
            {
                stringEntity = new StringEntity {Name = job.StringName };
                _stringRepository.Insert(stringEntity);
            }

            return stringEntity;
        }

        public void Dispose()
        {            
            _jobRepository.Dispose();
            _playerRepository.Dispose();
            _racketRepository.Dispose();
            _stringRepository.Dispose();
        }
    }

    internal class JobRepository : DataRepository, IJobRepository
    {
        public JobRepository()
        {
            CreateTables();
        }

        private void CreateTables()
        {
            Database.CreateTable<JobEntity>();
        }

        public IEnumerable<JobEntity> GetAllJobs()
        {
            return Database.Table<JobEntity>().OrderByDescending(x => x.StartDate).ToArray();
        }

        public IEnumerable<JobEntity> FindJobsFor(PlayerEntity playerEntity)
        {
            var query = $"select * from {nameof(JobEntity)} where {nameof(JobEntity.PlayerId)} == '{playerEntity.Id}'";
            return Database.Query<JobEntity>(query).ToArray();
        }

        public IEnumerable<JobEntity> FindJobsFor(PlayerEntity playerEntity, RacketEntity racketEntity)
        {           
            var query = $"select * from {nameof(JobEntity)} where {nameof(JobEntity.PlayerId)} == '{playerEntity.Id}' and {nameof(JobEntity.RacketId)} == '{racketEntity.Id}'";
            return Database.Query<JobEntity>(query).ToArray();            
        }

        public int Create(JobEntity jobEntity)
        {
            return Database.Insert(jobEntity);
        }

        public JobEntity Find(Guid id)
        {
            return Database.Find<JobEntity>(id);            
        }

        public IEnumerable<string> GetAllRacketsForPlayer(PlayerEntity playerEntity)
        {            
            var query = $"select * from {nameof(RacketEntity)} where id in (" +
                        $"select racketId from {nameof(JobEntity)} where {nameof(JobEntity.PlayerId)} = '{playerEntity.Id}')";

            return Database.Query<RacketEntity>(query).Select(x => x.Name);
        }

        public int Update(JobEntity entity)
        {
            return Database.Update(entity);
        }

        public int Delete(JobEntity entity)
        {
            return Database.Delete(entity);
        }

        public void Clear()
        {
            Database.DropTable<JobEntity>();
            
            CreateTables();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
