using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using RacketStringManager.Model;
using RacketStringManager.Services;

namespace RacketStringManager.ViewModel
{
    public class StringingHistory
    {
        public DateOnly Date { get; }

        public string StringName { get; }

        public double Tension { get; }

        public StringingHistory(Job job)
        {
            Date = job.StartDate;
            StringName = job.StringName;
            Tension = job.Tension;
        }
    }

    [QueryProperty(nameof(Job), "Job")]
    public partial class JobDetailsViewModel : BaseViewModel
    {
        private readonly IJobService _jobService;

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

        [ObservableProperty]
        private string _comment;

        [ObservableProperty]
        private double _tension;

        [ObservableProperty]
        private DateOnly _startDate;

        public ObservableCollection<StringingHistory> History { get; } = new();

        public JobDetailsViewModel(IJobService jobService)
        {
            _jobService = jobService;
        }

        private void UpdateProperties()
        {
            Name = Job.Name;
            Racket = Job.Racket;
            StringName = Job.StringName;
            Comment = Job.Comment;
            StartDate = Job.StartDate;
            Tension = Job.Tension;

            var history =  _jobService.FindJobsFor(Name, Racket).GetAwaiter().GetResult().ToArray();

            if(History.Count != 0)
                History.Clear();

            try
            {
                foreach (var entry in history)
                {
                    if (entry.JobId == Job.JobId)
                        continue;

                    History.Add(new StringingHistory(entry));
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
