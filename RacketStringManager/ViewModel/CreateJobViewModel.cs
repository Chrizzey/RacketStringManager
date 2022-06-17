using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services;
using RacketStringManager.ViewModel.Components;

namespace RacketStringManager.ViewModel
{
    public partial class CreateJobViewModel : ObservableObject
    {
        private readonly IJobService _jobService;
        private readonly IUiService _uiService;

        public JobEditViewModel JobEditViewModel { get; }
        
        public CreateJobViewModel(IJobService jobService, IUiService uiService)
        {
            _jobService = jobService;
            _uiService = uiService;
            JobEditViewModel = new JobEditViewModel(jobService, PrefillJobCommand);
        }

        [ICommand]
        private void PrefillJob(StringingHistoryViewModel historyVm)
        {
            if (string.IsNullOrWhiteSpace(JobEditViewModel.Racket))
                JobEditViewModel.Racket = historyVm.GetRacket();

            JobEditViewModel.Tension = historyVm.Tension.ToString("F1");
            JobEditViewModel.StringName = historyVm.StringName;

            JobEditViewModel.Comment = historyVm.HasComment ? historyVm.Comment : string.Empty;
        }

        [ICommand]
        private void Save()
        {
            var job = JobEditViewModel.GetJob();
            job.StartDate = DateOnly.FromDateTime(DateTime.Today);
            job.IsPaid = false;
            job.IsCompleted = false;

            _jobService.Create(job);

            _uiService.GoBackAsync();
        }

    }
}
