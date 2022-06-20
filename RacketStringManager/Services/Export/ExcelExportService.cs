using OfficeOpenXml;
using RacketStringManager.Services.Repository;


namespace RacketStringManager.Services.Export
{
    public class ExcelExportService
    {
        private readonly IJobService _jobService;
        private readonly IShare _share;

        public ExcelExportService(IJobService jobService, IShare share)
        {
            _jobService = jobService;
            _share = share;
        }

        public async Task Export()
        {
            try
            {
                using var package = new ExcelPackage();

                var worksheet = package.Workbook.Worksheets.Add("App Export");

                var jobs = _jobService.GetAllJobs().ToArray();

                worksheet.Cells[1, 1].Value = ExportNames.Date;
                worksheet.Cells[1, 2].Value = ExportNames.Name;
                worksheet.Cells[1, 3].Value = ExportNames.Racket;
                worksheet.Cells[1, 4].Value = ExportNames.Stringing;
                worksheet.Cells[1, 5].Value = ExportNames.Tension;
                worksheet.Cells[1, 6].Value = ExportNames.Paid;
                worksheet.Cells[1, 7].Value = ExportNames.Completed;
                worksheet.Cells[1, 8].Value = ExportNames.Comment;

                for (var i = 0; i < jobs.Length; i++)
                {
                    var row = i + 2;
                    var job = jobs[i];

                    worksheet.Cells[row, 1].Value = job.StartDate;
                    worksheet.Cells[row, 2].Value = job.Name;
                    worksheet.Cells[row, 3].Value = job.Racket;
                    worksheet.Cells[row, 4].Value = job.StringName;
                    worksheet.Cells[row, 5].Value = job.Tension.ToString("F1");
                    worksheet.Cells[row, 6].Value = job.IsPaid ? 1 : 0;
                    worksheet.Cells[row, 7].Value = job.IsCompleted ? 1 : 0;
                    worksheet.Cells[row, 8].Value = job.Comment;
                }

                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Export.xlsx");

                await package.SaveAsAsync(path);

                var request = new ShareFileRequest("Export.xlsx", new ShareFile(path));

                await _share.RequestAsync(request);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Exception", ex.Message, "OK");
            }
        }
    }
}
