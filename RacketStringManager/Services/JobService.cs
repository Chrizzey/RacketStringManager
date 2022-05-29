using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RacketStringManager.Model;

namespace RacketStringManager.Services
{
    public interface IJobService
    {
        Task<IEnumerable<Job>> GetAllJobs();

        Task<IEnumerable<Job>> FindJobsFor(string name);

        Task<IEnumerable<Job>> FindJobsFor(string name, string racket);
    }

    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllJobs();
    }

    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<IEnumerable<Job>> GetAllJobs()
        {
            return (await _jobRepository.GetAllJobs()).OrderByDescending(x => x.StartDate);
        }

        public async Task<IEnumerable<Job>> FindJobsFor(string name)
        {
            return (await GetAllJobs()).Where(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<IEnumerable<Job>> FindJobsFor(string name, string racket)
        {
            var jobs = await FindJobsFor(name);
            return jobs.Where(x => x.Racket.Equals(racket, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
