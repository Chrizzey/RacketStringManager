using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Services;
using RacketStringManager.View;
using Debug = System.Diagnostics.Debug;

namespace RacketStringManager.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly IJobService _jobService;
        private readonly IJobViewModelFactory _jobViewModelFactory;

        [ObservableProperty, AlsoNotifyChangeFor(nameof(IsNotBusy))]
        private bool _isBusy;
        
        public bool IsNotBusy => !_isBusy;

        public ObservableCollection<JobListViewModel> Jobs { get; } = new();

        public MainViewModel(IJobService jobService, IJobViewModelFactory jobViewModelFactory)
        {
            _jobService = jobService;
            _jobViewModelFactory = jobViewModelFactory;
        }
        
        [ICommand]
        private async Task LoadJobs()
        {
            if(IsBusy)
                return;

            try
            {
                IsBusy = true;
                var jobs = await _jobService.GetAllJobs();

                if(Jobs.Count != 0)
                    Jobs.Clear();

                foreach (var job in jobs)
                {
                    var jobVm = _jobViewModelFactory.CreateJobListViewModel(job);
                    Jobs.Add(jobVm);
                }
            }
            catch(Exception ex)
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
