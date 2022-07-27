using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Services.Export;

namespace RacketStringManager.ViewModel
{
    public partial class DataManagementViewModel : ObservableObject
    {
        private readonly IExcelExportService _exportService;
        private readonly IExcelImportService _importService;

        [ObservableProperty,
         AlsoNotifyChangeFor(nameof(IsIdle))]
        private bool _isBusy;
        public bool IsIdle => !_isBusy;

        public DataManagementViewModel(IExcelExportService exportService, IExcelImportService importService)
        {
            _exportService = exportService;
            _importService = importService;
        }

        [ICommand]
        private async Task ExportDatabase()
        {
            IsBusy = true;

            try
            {
                await _exportService.Export();
            }
            finally
            {
                IsBusy = false;
            }
        }

        [ICommand]
        private async Task ImportDatabase()
        {
            IsBusy = true;
            
            try
            {
                await _importService.Import();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
