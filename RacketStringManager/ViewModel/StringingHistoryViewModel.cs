using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using RacketStringManager.Model;

namespace RacketStringManager.ViewModel
{
    public partial class StringingHistoryViewModel : ObservableObject
    {
        private readonly Job _job;

        [ObservableProperty]
        private DateOnly _date;

        [ObservableProperty]
        private string _stringName;

        [ObservableProperty] 
        private string _playerName;

        [ObservableProperty] 
        private string _racket;

        [ObservableProperty]
        private double _tension;

        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);

        [ObservableProperty, AlsoNotifyChangeFor(nameof(HasComment))]
        private string _comment;

        [ObservableProperty]
        private ICommand _command;

        public StringingHistoryViewModel(Job job)
        {
            _job = job;
            _date = _job.StartDate;
            _stringName = _job.StringName;
            _playerName = _job.Name;
            _tension = _job.Tension;
            _comment = _job.Comment;
            _racket = _job.Racket;
        }

        public string GetRacket() => _job.Racket;
    }
}
