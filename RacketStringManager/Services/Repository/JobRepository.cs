using RacketStringManager.Model;
using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository
{   
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
