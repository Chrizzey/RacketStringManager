using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RacketStringManager.Services.Export;

namespace RacketStringManager.ViewModel
{
    public partial class DataManagementViewModel : ObservableObject
    {
        private readonly IExcelExportService _exportService;
        private readonly IExcelImportService _importService;

        public DataManagementViewModel(IExcelExportService exportService, IExcelImportService importService)
        {
            _exportService = exportService;
            _importService = importService;
        }

        [ICommand]
        private async Task ExportDatabase()
        {
            await _exportService.Export();
        }

        [ICommand]
        private async Task ImportDatabase()
        {
            await _importService.Import();
        }
    }
}
