using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Resources;
using RacketStringManager.Services;
using RacketStringManager.Services.Repository;
using RacketStringManager.View;

namespace RacketStringManager.ViewModel
{
    [QueryProperty(nameof(Job), "Job")]
    public partial class JobDetailsViewModel : ObservableObject
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUiService _uiService;
        private readonly INavigationService _navigationService;

        private Job _job;

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

        [ObservableProperty]
        private bool _isPaid;

        [ObservableProperty] 
        private bool _isCompleted;

        public bool HasComment => !string.IsNullOrWhiteSpace(_comment);

        public ObservableCollection<StringingHistoryViewModel> History { get; } = new();

        public JobDetailsViewModel(IJobRepository jobRepository, IUiService uiService, INavigationService navigationService)
        {
            _jobRepository = jobRepository;
            _uiService = uiService;
            _navigationService = navigationService;
        }

        [ICommand]
        private async Task GotoEditJobPage()
        {
            await _navigationService.GotoEditJobPage(_job);
        }

        [ICommand]
        private async Task DeleteJob()
        {
            var answer = await _uiService.GetUserConfirmation(
                AppRes.JobDetails_DeleteConfirm_Title,
                AppRes.JobDetails_DeleteConfirm_Message,
                AppRes.JobDetails_DeleteConfirm_Accept,
                AppRes.JobDetails_DeleteConfirm_Cancel);
            if (!answer)
                return;

            _jobRepository.Delete(_job);
            await _uiService.GoBackAsync();
        }

        private void UpdateProperties()
        {
            Name = Job.Name;
            Racket = Job.Racket;
            StringName = Job.StringName;
            Comment = Job.Comment;
            StartDate = Job.StartDate;
            Tension = Job.Tension;
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

                    History.Add(new StringingHistoryViewModel(entry));
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
