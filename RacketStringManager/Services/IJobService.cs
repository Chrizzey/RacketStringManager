using RacketStringManager.Model;

namespace RacketStringManager.Services;

public interface IJobService
{
    Task<IEnumerable<Job>> GetAllJobs();

    Task<IEnumerable<Job>> FindJobsFor(string name);

    Task<IEnumerable<Job>> FindJobsFor(string name, string racket);
}