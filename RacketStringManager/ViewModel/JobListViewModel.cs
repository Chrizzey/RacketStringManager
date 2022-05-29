using CommunityToolkit.Mvvm.ComponentModel;
using RacketStringManager.Model;

namespace RacketStringManager.ViewModel;

public partial class JobListViewModel : ObservableObject
{
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
        _name = job.Name;
        _racket = job.Racket;
        _isCompleted = job.IsCompleted;
        _isPaid = job.IsPaid;
    }
}