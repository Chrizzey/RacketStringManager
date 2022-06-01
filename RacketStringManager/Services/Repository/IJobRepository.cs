using RacketStringManager.Model;

namespace RacketStringManager.Services.Repository;

public interface IJobRepository : IDisposable
{
    Task<IEnumerable<Job>> GetAllJobs();
}
