using OfficeOpenXml;
using RacketStringManager.Services.Repository;


namespace RacketStringManager.Services.Export
{
    public class ExcelExportService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IShare _share;

        public ExcelExportService(IJobRepository jobRepository, IShare share)
        {
            _jobRepository = jobRepository;
            _share = share;
        }

        public async Task Export()
        {
            try
            {
                using var package = new ExcelPackage();

                var worksheet = package.Workbook.Worksheets.Add("App Export");

                var jobs = _jobRepository.GetAllJobs().ToArray();

                worksheet.Cells[1, 1].Value = "Datum";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Schläger";
                worksheet.Cells[1, 4].Value = "Saite";
                worksheet.Cells[1, 5].Value = "Bespannung";
                worksheet.Cells[1, 6].Value = "Bezahlt";
                worksheet.Cells[1, 7].Value = "Fertig";
                worksheet.Cells[1, 8].Value = "Kommentar";

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
                    worksheet.Cells[row, 6].Value = job.Comment;
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
