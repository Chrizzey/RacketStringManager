using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Services;

namespace RacketStringManager.ViewModel
{
    public partial class CreateJobViewModel : ObservableObject
    {
        private readonly IJobService _jobService;

        [ObservableProperty]
        private string _name;
        
        [ObservableProperty]
        private string _comment;
        
        [ObservableProperty]
        private string _racket;
        
        [ObservableProperty]
        private string _stringName;

        [ObservableProperty] 
        private string _tension;

        public ObservableCollection<StringingHistory> History { get; } = new();

        public CreateJobViewModel(IJobService jobService)
        {
            _jobService = jobService;
        }

        [ICommand]
        private async Task Save()
        {
            throw new NotImplementedException();
        }

        [ICommand]
        private async Task ReloadHistory()
        {
            if(string.IsNullOrWhiteSpace(Name))
                return;

            if(History.Count != 0)
                History.Clear();

            var jobs = string.IsNullOrWhiteSpace(Racket)
                ? _jobService.FindJobsFor(Name)
                : _jobService.FindJobsFor(Name, Racket);

            foreach (var job in await jobs)
            {
                History.Add(new StringingHistory(job));
            }
        }
    }
}
