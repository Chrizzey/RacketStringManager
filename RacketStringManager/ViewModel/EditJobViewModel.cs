using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services;
using RacketStringManager.Services.Repository;

namespace RacketStringManager.ViewModel
{
    [QueryProperty(nameof(Job), "Job")]
    public partial class EditJobViewModel : ObservableObject
    {
        private Job _job;
        private readonly IJobRepository _jobRepository;
        private readonly IUiService _uiService;

        private double _tensionInKg;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CanSave))]
        private string _name;

        [ObservableProperty]
        private string _comment;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CanSave))]
        private string _racket;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CanSave))]
        private string _stringName;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CanSave))]
        private string _tension;

        [ObservableProperty] 
        private bool _isPaid;
        
        [ObservableProperty] 
        private bool _isCompleted;

        public bool CanSave =>
            !(string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Racket) || string.IsNullOrWhiteSpace(StringName)) && ParseTension();

        public ObservableCollection<StringingHistoryViewModel> History { get; } = new();


        public Job Job
        {
            get => _job;
            set
            {
                var job = _jobRepository.Find(value.JobId);
                SetProperty(ref _job, job);
                UpdateProperties();
            }
        }

        [ICommand]
        private async Task Save()
        {
            var job = new Job
            {
                JobId = _job.JobId,
                Name = Name,
                StringName = StringName,
                Racket = Racket,
                Tension = _tensionInKg,
                Comment = Comment,
                StartDate = Job.StartDate,
                IsPaid = _isPaid,
                IsCompleted = IsCompleted
            };

            _jobRepository.Update(job);

            await _uiService.GoBackAsync();
        }

        public EditJobViewModel(IJobRepository repository, IUiService uiService)
        {
            _jobRepository = repository;
            _uiService = uiService;
        }

        private bool ParseTension()
        {
            var tension = Tension?.Replace(",", ".");
            return double.TryParse(tension, NumberStyles.Any, CultureInfo.InvariantCulture, out _tensionInKg);
        }

        [ICommand]
        private void ReloadHistory()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;

            if (History.Count != 0)
                History.Clear();

            var jobs = string.IsNullOrWhiteSpace(Racket)
                ? _jobRepository.FindJobsFor(Name)
                : _jobRepository.FindJobsFor(Name, Racket);

            foreach (var job in jobs)
            {
                History.Add(new StringingHistoryViewModel(new StringingHistory(job)));
            }
        }
        private void UpdateProperties()
        {
            Name = Job.Name;
            Racket = Job.Racket;
            StringName = Job.StringName;
            Comment = Job.Comment;
            Tension = Job.Tension.ToString("F1");
            IsPaid = Job.IsPaid;
            IsCompleted = Job.IsCompleted;

            var history = _jobRepository.FindJobsFor(Name, Racket).ToArray();

            if (History.Count != 0)
                History.Clear();

            try
            {
                foreach (var entry in history)
                {
                    if (entry.JobId == Job.JobId)
                        continue;

                    History.Add(new StringingHistoryViewModel(new StringingHistory(entry)));
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);

                // Todo: Abstract this UI call
                _uiService.DisplayAlertAsync("Error!", "Unable to load jobs from cache", "OK");
            }
        }
    }
}
