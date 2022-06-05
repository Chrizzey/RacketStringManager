using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services.Repository;
using RacketStringManager.View;

namespace RacketStringManager.ViewModel
{
    [QueryProperty(nameof(Job), "Job")]
    public partial class JobDetailsViewModel : ObservableObject
    {
        private readonly IJobRepository _jobService;

        private Job _job;

        public Job Job
        {
            get => _job;
            set
            {
                SetProperty(ref _job, value);
                UpdateProperties();
            }
        }

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _racket;

        [ObservableProperty]
        private string _stringName;

        [ObservableProperty, AlsoNotifyChangeFor(nameof(HasComment))]
        private string _comment;

        [ObservableProperty]
        private double _tension;

        [ObservableProperty]
        private DateOnly _startDate;

        public bool HasComment => !string.IsNullOrWhiteSpace(_comment);

        public ObservableCollection<StringingHistoryViewModel> History { get; } = new();

        public JobDetailsViewModel(IJobRepository jobService)
        {
            _jobService = jobService;
        }

        [ICommand]
        private async Task GotoEditJobPage()
        {
            await Shell.Current.GoToAsync(nameof(EditJobPage), true, new Dictionary<string, object>
            {
                {"Job", _job}
            });
        }

        private void UpdateProperties()
        {
            Name = Job.Name;
            Racket = Job.Racket;
            StringName = Job.StringName;
            Comment = Job.Comment;
            StartDate = Job.StartDate;
            Tension = Job.Tension;

            var history = _jobService.FindJobsFor(Name, Racket).ToArray();

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
                Shell.Current.DisplayAlert("Error!", "Unable to load jobs from cache", "OK");
            }
        }
    }
}
