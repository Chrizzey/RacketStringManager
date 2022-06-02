using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Model;

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

        public StringingHistoryViewModel(StringingHistory model)
        {
            _model = model;
            _date = _model.Date;
            _stringName = _model.StringName;
            _tension = _model.Tension;
        }

        [ICommand]
        private async Task GotoJob()
        {

        }
    }
}
