using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services.Repository;

namespace RacketStringManager.ViewModel
{
    public partial class CreateJobViewModel : ObservableObject
    {
        private readonly IJobRepository _jobService;

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

        public ObservableCollection<StringingHistoryViewModel> History { get; } = new();

        public CreateJobViewModel(IJobRepository jobService)
        {
            _jobService = jobService;
        }

        [ICommand]
        private async Task Save()
        {
            throw new NotImplementedException();
        }

        [ICommand]
        private void ReloadHistory()
        {
            if(string.IsNullOrWhiteSpace(Name))
                return;

            if(History.Count != 0)
                History.Clear();

            var jobs = string.IsNullOrWhiteSpace(Racket)
                ? _jobService.FindJobsFor(Name)
                : _jobService.FindJobsFor(Name, Racket);

            foreach (var job in jobs)
            {
                History.Add(new StringingHistoryViewModel(new StringingHistory(job)));
            }
        }
    }
}
