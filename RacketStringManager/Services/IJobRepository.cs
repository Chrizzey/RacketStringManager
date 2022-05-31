using RacketStringManager.Model;

namespace RacketStringManager.Services;

public interface IJobRepository
{
    Task<IEnumerable<Job>> GetAllJobs();
}