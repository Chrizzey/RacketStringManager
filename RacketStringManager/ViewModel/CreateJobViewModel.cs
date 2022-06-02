using System.Collections.ObjectModel;
using System.Globalization;
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
                StringName = StringName,
                Racket = Racket,
                Tension = ParseTension(),
                Comment = Comment,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                IsPaid = false,
                IsCompleted = false
            };

            _jobRepository.Create(job);

            Shell.Current.GoToAsync("..");
        }

        private double ParseTension()
        {
            var tension = Tension.Replace(",", ".");
            double.TryParse(tension, NumberStyles.Any, CultureInfo.InvariantCulture, out var value);

            return value;
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
