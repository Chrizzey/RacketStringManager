using RacketStringManager.Model;

namespace RacketStringManager.ViewModel;

public class JobViewModelFactory : IJobViewModelFactory
{
    public JobViewModel CreateViewModel(Job job)
    {
        throw new NotImplementedException();
    }

    public JobListViewModel CreateJobListViewModel(Job job)
    {
        return new JobListViewModel(job);
    }
}