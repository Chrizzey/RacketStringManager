using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services;
using RacketStringManager.View;

namespace RacketStringManager.ViewModel;

public partial class JobListViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    public Job Job { get; }

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _racket;

    [ObservableProperty]
    private bool _isCompleted;

    [ObservableProperty]
    private bool _isPaid;
    
    public JobListViewModel(Job job, INavigationService navigationService)
    {
        _navigationService = navigationService;
        Job = job;

        _name = job.Name;
        _racket = job.Racket;
        _isCompleted = job.IsCompleted;
        _isPaid = job.IsPaid;
    }
    
    [ICommand]
    private async Task GotoJobDetails(JobListViewModel viewModel)
    {
        if (viewModel is null)
            return;

        await _navigationService.GoToJobDetailsPage(viewModel.Job);
    }
}