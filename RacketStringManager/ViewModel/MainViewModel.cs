using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services;
using RacketStringManager.Services.Export;
using RacketStringManager.Services.Repository;
using RacketStringManager.View;
using Debug = System.Diagnostics.Debug;

namespace RacketStringManager.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IJobRepository _jobService;
        private readonly IJobViewModelFactory _jobViewModelFactory;
        private readonly IUiService _uiService;
        private readonly INavigationService _navigationService;
        private readonly ExcelExportService _exportService;

        [ObservableProperty]
        private bool _showsOpenJobsOnly;

        [ObservableProperty, AlsoNotifyChangeFor(nameof(IsNotBusy))]
        private bool _isBusy;

        public bool IsNotBusy => !_isBusy;

        public ObservableCollection<JobListViewModel> Jobs { get; } = new();

        public MainViewModel(IJobRepository jobService, IJobViewModelFactory jobViewModelFactory, IUiService uiService, INavigationService navigationService, ExcelExportService exportService)
        {
            _jobService = jobService;
            _jobViewModelFactory = jobViewModelFactory;
            _uiService = uiService;
            _navigationService = navigationService;
            _exportService = exportService;
            _showsOpenJobsOnly = true;
        }

        [ICommand]
        private async Task GoToNewJobPage()
        {
            await _navigationService.GoToCreateJobPage();
        }

        [ICommand]
        private Task LoadPendingJobs()
        {
            _showsOpenJobsOnly = true;
            return LoadJobs();
        }

        [ICommand]
        private Task LoadAllJobs()
        {
            _showsOpenJobsOnly = false;
            return LoadJobs();
        }

        [ICommand]
        private async Task LoadJobs()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var jobs = _jobService.GetAllJobs();

                if (Jobs.Count != 0)
                    Jobs.Clear();

                var filter = _showsOpenJobsOnly ? new Func<Job, bool>(x => !x.IsCompleted || !x.IsPaid) : x => true;

                foreach (var job in jobs.Where(filter))
                {
                    var jobVm = _jobViewModelFactory.CreateJobListViewModel(job);
                    Jobs.Add(jobVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                // Todo: Abstract this UI call
                await _uiService.DisplayAlertAsync("Error!", "Unable to load jobs from cache", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [ICommand]
        private async Task ExportDatabase()
        {
            await _exportService.Export();
        }
    }
}
