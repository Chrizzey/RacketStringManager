using System.Collections.ObjectModel;
using AndroidX.Core.Util;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services;
using RacketStringManager.Services.Repository;
using RacketStringManager.View;
using Debug = System.Diagnostics.Debug;

namespace RacketStringManager.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IJobRepository _jobService;
        private readonly IJobViewModelFactory _jobViewModelFactory;

        [ObservableProperty, AlsoNotifyChangeFor(nameof(IsNotBusy))]
        private bool _isBusy;

        public bool IsNotBusy => !_isBusy;

        public ObservableCollection<JobListViewModel> Jobs { get; } = new();

        public MainViewModel(IJobRepository jobService, IJobViewModelFactory jobViewModelFactory)
        {
            _jobService = jobService;
            _jobViewModelFactory = jobViewModelFactory;
        }

        [ICommand]
        private async Task GoToNewJobPage()
        {
            await Shell.Current.GoToAsync(nameof(CreateJobPage), true);
        }

        [ICommand]
        private Task LoadPendingJobs()
        {
            return LoadTask(x => !x.IsCompleted || !x.IsPaid);
        }

        [ICommand]
        private Task LoadAllJobs()
        {
            return LoadTask(x => true);
        }

        private async Task LoadTask(Func<Job, bool> filter)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var jobs = _jobService.GetAllJobs();

                if (Jobs.Count != 0)
                    Jobs.Clear();

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
                await Shell.Current.DisplayAlert("Error!", "Unable to load jobs from cache", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
