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
}
