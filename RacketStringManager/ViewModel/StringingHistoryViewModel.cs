using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;
using RacketStringManager.View;

namespace RacketStringManager.ViewModel
{
    public partial class StringingHistoryViewModel : ObservableObject
    {
        private readonly StringingHistory _model;

        [ObservableProperty]
        private DateOnly _date;

        [ObservableProperty] 
        private string _stringName;

        [ObservableProperty]
        private double _tension;

        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);

        [ObservableProperty, AlsoNotifyChangeFor(nameof(HasComment))]
        private string _comment;

        public StringingHistoryViewModel(StringingHistory model)
        {
            _model = model;
            _date = _model.Date;
            _stringName = _model.StringName;
            _tension = _model.Tension;
            _comment = _model.StringingJob.Comment;
        }

        [ICommand]
        private async Task GotoJob()
        {
            await Shell.Current.GoToAsync(nameof(JobDetailsPage), true, new Dictionary<string, object>
            {
                {"Job", _model.StringingJob}
            });
        }
    }
}
