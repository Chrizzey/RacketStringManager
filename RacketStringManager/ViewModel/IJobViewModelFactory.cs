using RacketStringManager.Model;

namespace RacketStringManager.ViewModel;

public interface IJobViewModelFactory
{
    public JobViewModel CreateViewModel(Job job);

    public JobListViewModel CreateJobListViewModel(Job job);
}