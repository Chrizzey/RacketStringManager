using RacketStringManager.Model;
using RacketStringManager.Services;

namespace RacketStringManager.ViewModel;

public class JobViewModelFactory : IJobViewModelFactory
{
    private readonly INavigationService _navigationService;

    public JobViewModelFactory(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public JobDetailsViewModel CreateViewModel(Job job)
    {
        throw new NotImplementedException();
    }

    public JobListViewModel CreateJobListViewModel(Job job)
    {
        return new JobListViewModel(job, _navigationService);
    }
}