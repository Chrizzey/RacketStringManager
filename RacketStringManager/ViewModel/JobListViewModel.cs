using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.View;

namespace RacketStringManager.ViewModel;

public partial class JobListViewModel : ObservableObject
{
    public Job Job { get; }

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _racket;

    [ObservableProperty]
    private bool _isCompleted;

    [ObservableProperty]
    private bool _isPaid;
    
    public JobListViewModel(Job job)
    {
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

        // Todo: Abstract this UI call
        await Shell.Current.GoToAsync(nameof(JobDetailsPage), true, new Dictionary<string, object>()
        {
            {"Job", viewModel.Job}
        });
    }
}