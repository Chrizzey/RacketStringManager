using RacketStringManager.Model;

namespace RacketStringManager.Services
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
        void Clear();
    }
}
