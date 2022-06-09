using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository;

public interface IJobRepository : IDisposable
{
    IEnumerable<JobEntity> GetAllJobs();
    IEnumerable<JobEntity> FindJobsFor(PlayerEntity player);
    IEnumerable<JobEntity> FindJobsFor(PlayerEntity player, RacketEntity racket);
    int Create(JobEntity job);
    int Update(JobEntity job);
    JobEntity Find(Guid id);
    int Delete(JobEntity job);
    IEnumerable<string> GetAllRacketsForPlayer(PlayerEntity playerEntity);
}
