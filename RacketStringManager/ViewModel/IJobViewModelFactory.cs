using RacketStringManager.Model;

namespace RacketStringManager.ViewModel;

public interface IJobViewModelFactory
{
    public JobDetailsViewModel CreateViewModel(Job job);

    public JobListViewModel CreateJobListViewModel(Job job);
}