using RacketStringManager.Model;

namespace RacketStringManager.Services.Repository;

public interface IJobRepository
{
    Task<IEnumerable<Job>> GetAllJobs();
}