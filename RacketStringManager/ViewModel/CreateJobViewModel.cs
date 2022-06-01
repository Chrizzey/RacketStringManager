using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services.Repository;

namespace RacketStringManager.ViewModel
{
    public partial class CreateJobViewModel : ObservableObject
    {
        private readonly IJobRepository _jobRepository;

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

        public CreateJobViewModel(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        [ICommand]
        private void Save()
        {
            var job = new Job
            {
                Name = Name,
                Racket = Racket,
                Tension = double.Parse(Tension),
                Comment = Comment,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                IsPaid = false,
                IsCompleted = false
            };

            _jobRepository.Create(job);

            Shell.Current.GoToAsync("..");
        }

        [ICommand]
        private void ReloadHistory()
        {
            if(string.IsNullOrWhiteSpace(Name))
                return;

            if(History.Count != 0)
                History.Clear();

            var jobs = string.IsNullOrWhiteSpace(Racket)
                ? _jobRepository.FindJobsFor(Name)
                : _jobRepository.FindJobsFor(Name, Racket);

            foreach (var job in jobs)
            {
                History.Add(new StringingHistoryViewModel(new StringingHistory(job)));
            }
        }
    }
}
