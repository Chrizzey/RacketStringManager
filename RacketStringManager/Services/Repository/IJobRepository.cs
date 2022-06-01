using RacketStringManager.Model;

namespace RacketStringManager.Services.Repository;

public interface IJobRepository : IDisposable
{
    IEnumerable<Job> GetAllJobs();

    IEnumerable<Job> FindJobsFor(string player);

    IEnumerable<Job> FindJobsFor(string player, string racket);
}
