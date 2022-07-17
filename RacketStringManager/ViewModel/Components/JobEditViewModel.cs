using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.Services;

namespace RacketStringManager.ViewModel.Components
{
    public partial class JobEditViewModel : ObservableObject
    {
        private readonly IJobService _jobService;
        private readonly ICommand _onHistoryClick;
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

        public JobEditViewModel(IJobService jobService, ICommand onHistoryClick)
        {
            _jobService = jobService;
            _onHistoryClick = onHistoryClick;
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

            foreach (var job in jobs.OrderByDescending(x => x.StartDate))
            {
                History.Add(new StringingHistoryViewModel(job) { Command = _onHistoryClick });
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

        public Job GetJob()
        {
            ParseTension();
            return new Job
            {
                Name = Name,
                StringName = StringName,
                Racket = Racket,
                Tension = _tensionInKg,
                Comment = Comment,
            };
        }

        private bool ParseTension()
        {
            return ParseTension(Tension, out _tensionInKg);
        }

        public static bool ParseTension(string value, out double tensionValue)
        {
            var tension = value?.Replace(",", ".");
            return double.TryParse(tension, NumberStyles.Any, CultureInfo.InvariantCulture, out tensionValue);

        }
    }
}
