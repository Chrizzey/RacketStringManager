using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services;
using RacketStringManager.Services.Repository;

namespace RacketStringManager.ViewModel
{
    public partial class CreateJobViewModel : ObservableObject
    {
        private readonly IJobService _jobService;
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

        public bool CanSave => !(string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Racket) || string.IsNullOrWhiteSpace(StringName)) && ParseTension();

        public ObservableCollection<StringingHistoryViewModel> History { get; } = new();

        public CreateJobViewModel(IJobService jobService, IUiService uiService)
        {
            _jobService = jobService;
            _uiService = uiService;
        }

        [ICommand]
        private void PrefillJob(StringingHistoryViewModel historyVm)
        {
            if (string.IsNullOrWhiteSpace(Racket))
                Racket = historyVm.GetRacket();

            Tension = historyVm.Tension.ToString("F1");
            StringName = historyVm.StringName;

            Comment = historyVm.HasComment ? historyVm.Comment : string.Empty;
        }

        [ICommand]
        private void Save()
        {
            var job = new Job
            {
                Name = Name,
                StringName = StringName,
                Racket = Racket,
                Tension = _tensionInKg,
                Comment = Comment,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                IsPaid = false,
                IsCompleted = false
            };

            _jobService.Create(job);

            _uiService.GoBackAsync();
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
                ? _jobService.FindJobsFor(Name)
                : _jobService.FindJobsFor(Name, Racket);

            foreach (var job in jobs)
            {
                History.Add(new StringingHistoryViewModel(job) { Command = PrefillJobCommand });
            }
        }

        [ICommand]
        private async Task ShowListOfStrings()
        {
            try
            {
                var strings = _jobService.GetAllStringNames().ToArray();

                var selected = await Shell.Current.DisplayActionSheet("Auswahl", "Abbrechen", "Ok", strings);

                StringName = selected;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Exception", ex.Message, "OK");
            }
        }
    }
}
