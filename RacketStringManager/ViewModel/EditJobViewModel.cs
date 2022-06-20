using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services;
using RacketStringManager.ViewModel.Components;

namespace RacketStringManager.ViewModel
{
    [QueryProperty(nameof(Job), "Job")]
    public partial class EditJobViewModel : ObservableObject
    {
        private Job _job;
        private readonly IJobService _jobService;
        private readonly IUiService _uiService;
        
        public JobEditViewModel EditViewModel { get; }

        [ObservableProperty] 
        private bool _isPaid;
        
        [ObservableProperty] 
        private bool _isCompleted;
        
        public Job Job
        {
            get => _job;
            set
            {
                var job = _jobService.Find(value.JobId);
                SetProperty(ref _job, job);
                UpdateProperties();
            }
        }

        [ICommand]
        private async Task Save()
        {
            var edited = EditViewModel.GetJob();

            var job = new Job
            {
                JobId = _job.JobId,
                Name = edited.Name,
                StringName = edited.StringName,
                Racket = edited.Racket,
                Tension = edited.Tension,
                Comment = edited.Comment,
                StartDate = Job.StartDate,
                IsPaid = _isPaid,
                IsCompleted = IsCompleted
            };

            _jobService.Update(job);

            await _uiService.GoBackAsync();
        }

        public EditJobViewModel(IJobService jobService, IUiService uiService)
        {
            _jobService = jobService;
            _uiService = uiService;
            EditViewModel = new JobEditViewModel(jobService, new Command(() => { }));
        }
        
        private void UpdateProperties()
        {
            EditViewModel.Name = Job.Name;
            EditViewModel.Racket = Job.Racket;
            EditViewModel.StringName = Job.StringName;
            EditViewModel.Comment = Job.Comment;
            EditViewModel.Tension = Job.Tension.ToString("F1");
            IsPaid = Job.IsPaid;
            IsCompleted = Job.IsCompleted;

            var history = _jobService.FindJobsFor(EditViewModel.Name, EditViewModel.Racket).ToArray();

            if (EditViewModel.History.Count != 0)
                EditViewModel.History.Clear();

            try
            {
                foreach (var entry in history)
                {
                    if (entry.JobId == Job.JobId)
                        continue;

                    EditViewModel.History.Add(new StringingHistoryViewModel(entry));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                
                _uiService.DisplayAlertAsync("Error!", "Unable to load jobs from cache", "OK");
            }
        }
    }
}
