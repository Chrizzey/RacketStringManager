using RacketStringManager.Model;
using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository;

public interface IAsyncJobRepository : IAsyncDisposable
{
    Task<IEnumerable<Job>> GetAllJobs();

    Task<IEnumerable<Job>> FindJobsFor(string player);

    Task<IEnumerable<Job>> FindJobsFor(string player, string racket);
}

public interface IJobRepository : IDisposable
{
    IEnumerable<Job> GetAllJobs();

    IEnumerable<Job> FindJobsFor(string player);

    IEnumerable<Job> FindJobsFor(string player, string racket);
    int Create(Job job);
    int Update(Job job);
}

public class AsyncJobRepository : IAsyncJobRepository
{
    private readonly IJobRepository _jobRepository;

    public AsyncJobRepository(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public ValueTask DisposeAsync()
    {
        _jobRepository.Dispose();
        return ValueTask.CompletedTask;
    }

    public Task<IEnumerable<Job>> GetAllJobs()
    {
        return Task.Run(() => _jobRepository.GetAllJobs());
    }

    public Task<IEnumerable<Job>> FindJobsFor(string player)
    {
        return Task.Run(() => _jobRepository.FindJobsFor(player));
    }

    public Task<IEnumerable<Job>> FindJobsFor(string player, string racket)
    {
        return Task.Run(() => _jobRepository.FindJobsFor(player, racket));
    }

    
}