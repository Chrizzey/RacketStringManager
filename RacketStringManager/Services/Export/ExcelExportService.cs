using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using RacketStringManager.Services.Repository;


namespace RacketStringManager.Services.Export
{
    public class ExcelExportService
    {
        private readonly IJobRepository _jobRepository;
        
        public ExcelExportService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
            
        }

        public void Export()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("App Export");

            var jobs = _jobRepository.GetAllJobs().ToArray();

            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Schläger";

            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Testdaten.xlsx");
            Debug.WriteLine("Saving to " + filePath);
            package.SaveAs(filePath);
        }
    }
}
