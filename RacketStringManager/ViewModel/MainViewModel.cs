using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.OS;
using CommunityToolkit.Mvvm.ComponentModel;
using RacketStringManager.Model;
using RacketStringManager.Services;
using Debug = System.Diagnostics.Debug;

namespace RacketStringManager.ViewModel
{
    public interface IJobViewModelFactory
    {
        public JobViewModel CreateViewModel(Job job);
    }

    public partial class MainViewModel : BaseViewModel
    {
        private readonly IJobService _jobService;
        private readonly IJobViewModelFactory _jobViewModelFactory;

        [ObservableProperty]
        private bool _isBusy;

        public ObservableCollection<JobViewModel> Jobs { get; } = new();

        public MainViewModel(IJobService jobService, IJobViewModelFactory jobViewModelFactory)
        {
            _jobService = jobService;
            _jobViewModelFactory = jobViewModelFactory;
        }

        private async Task LoadJobsAsync()
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
                    var jobVm = _jobViewModelFactory.CreateViewModel(job);
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
