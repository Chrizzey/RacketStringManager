using System.Text;
using OfficeOpenXml;
using RacketStringManager.Model;
using RacketStringManager.ViewModel.Components;

namespace RacketStringManager.Services.Export;

public class ExcelImportService : IDisposable, IExcelImportService
{
    private readonly IFilePicker _picker;
    private readonly IJobService _jobService;
    private readonly IUiService _uiService;
    private readonly ITranslationService _translationService;
    private FileResult _pickResult;
    private ExcelPackage _excelPackage;
    private ExcelWorksheet _worksheet;
    private Dictionary<string, int> _importMapping;

    public ExcelImportService(IFilePicker picker, IJobService jobService, IUiService uiService, ITranslationService translationService)
    {
        _picker = picker;
        _jobService = jobService;
        _uiService = uiService;
        _translationService = translationService;
    }

    public async Task Import()
    {
        try
        {
            await PickFile();
            if (!HasUserPickedAFile())
                return;

            var clearDbTask = ClearDatabase();
            await OpenFile();
            CreateImportMapping();

            await clearDbTask;
            await ImportAllJobs();

            var actual = _jobService.GetAllJobs().Count();
            await _uiService.DisplayAlertAsync(
                _translationService.GetTranslatedText("ExcelImport_SuccessPopup_Title"),
                string.Format(_translationService.GetTranslatedText("ExcelImport_SuccessPopup_MessageFormat"), actual, _worksheet.Dimension.Rows - 1),
                _translationService.GetTranslatedText("ExcelImport_Ok"));
        }
        catch (Exception ex)
        {
            await _uiService.DisplayAlertAsync(
                _translationService.GetTranslatedText("ExcelImport_ExceptionPopup_Title"),
                ex.Message,
                _translationService.GetTranslatedText("ExcelImport_Ok"));
        }
    }

    private async Task PickFile() => _pickResult = await _picker.PickAsync();

    private bool HasUserPickedAFile() => _pickResult is not null && Path.GetExtension(_pickResult.FileName).Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase);

    private async Task OpenFile()
    {
        _excelPackage = new ExcelPackage(await _pickResult.OpenReadAsync());
        _worksheet = _excelPackage.Workbook.Worksheets.First();
    }

    private void CreateImportMapping()
    {
        _importMapping = new Dictionary<string, int>
        {
            { ExportNames.Tension.ToLower(), -1},
            { ExportNames.Racket.ToLower(), -1},

            { ExportNames.Date.ToLower(), -1},
            { ExportNames.Comment.ToLower(), -1},

            { ExportNames.Completed.ToLower(), -1},
            { ExportNames.Name.ToLower(), -1},

            { ExportNames.Paid.ToLower(), -1},
            { ExportNames.Stringing.ToLower(), -1},

        };
        var numberOfColumns = _worksheet.Columns.Count();

        for (var i = 1; i <= numberOfColumns; i++)
        {
            var value = _worksheet.Cells[1, i].Value?.ToString()?.ToLower();
            if (string.IsNullOrWhiteSpace(value))
                continue;

            if (_importMapping.ContainsKey(value))
                _importMapping[value] = i;
        }

        var notFound = _importMapping.Where(x => x.Value == -1).Select(x => x.Key).ToArray();
        if (notFound.Any())
        {
            throw new Exception("Entries not found!" + string.Join(", ", notFound));
        }
    }

    private Task ClearDatabase()
    {
        return Task.Run(() => _jobService.Clear());
    }

    private async Task ImportAllJobs()
    {
        await Task.Run(async () =>
        {
            for (var row = 2; row <= _worksheet.Dimension.Rows; row++)
            {
                if (IsRowEmpty(row))
                {
                    continue;
                }

                try
                {
                    var job = ImportJob(row);
                    _jobService.Create(job);
                }
                catch (ImportException ex)
                {
                    await _uiService.DisplayAlertAsync(
                        string.Format(_translationService.GetTranslatedText("ExcelImport_ErrorMessage_ErrorInRow"), row),
                        ex.Message,
                        _translationService.GetTranslatedText("ExcelImport_Ok"));
                }
            }
        });
    }

    private bool IsRowEmpty(int row)
    {
        for (var i = 1; i < _worksheet.Dimension.Columns; i++)
        {
            if (!string.IsNullOrWhiteSpace(_worksheet.Cells[row, i].Value?.ToString()))
                return false;
        }

        return true;
    }

    private Job ImportJob(int row)
    {
        var importErrors = new StringBuilder();

        var tensionString = GetValue(row, ExportNames.Tension)?.ToString();
        if (!JobEditViewModel.ParseTension(tensionString, out var tension))
        {
            tension = 0d;

            if (string.IsNullOrWhiteSpace(tensionString))
                importErrors.AppendLine("Tension is empty");
            else
                importErrors.AppendLine($"Tension '{tensionString}' is not a valid number");
        }

        var job = new Job
        {
            Name = GetValue(row, ExportNames.Name)?.ToString(),
            Racket = GetValue(row, ExportNames.Racket)?.ToString(),

            StringName = GetValue(row, ExportNames.Stringing)?.ToString(),
            Comment = GetValue(row, ExportNames.Comment)?.ToString() ?? string.Empty,

            IsPaid = GetBooleanValue(row, ExportNames.Paid),
            IsCompleted = GetBooleanValue(row, ExportNames.Completed),

            Tension = tension
        };

        var dateString = GetValue(row, ExportNames.Date)?.ToString();

        if (string.IsNullOrWhiteSpace(dateString))
        {
            importErrors.AppendLine("Date is empty");
        }
        else
        {
            try
            {
                job.StartDate = ParseDateOnly(dateString);
            }
            catch (FormatException)
            {
                importErrors.AppendLine($"Date '{dateString}' is not a valid date");
            }
        }

        ImportSanityCheck(job, importErrors);

        return job;
    }

    private DateOnly ParseDateOnly(string date)
    {
        var formats = new[]
        {
            "d.M.yy",
            "d.M.yyyy",
            "M/d/yy",
            "M/d/yyyy",
            "yy-M-d",
            "yyyy-M-d",
            "d.M.yy HH:mm:ss",
            "d.M.yyyy HH:mm:ss",
            "M/d/yy HH:mm:ss",
            "M/d/yyyy HH:mm:ss",
            "yy-M-d HH:mm:ss",
            "yyyy-M-d HH:mm:ss"
        };
        return DateOnly.ParseExact(date, formats);
    }

    private static void ImportSanityCheck(Job job, StringBuilder importErrors)
    {
        if (string.IsNullOrWhiteSpace(job.Name))
            importErrors.AppendLine("Name is empty");
        if (string.IsNullOrWhiteSpace(job.Racket))
            importErrors.AppendLine("Racket is empty");
        if (string.IsNullOrWhiteSpace(job.StringName))
            importErrors.AppendLine("String is empty");

        if (importErrors.Length > 0)
            throw new ImportException(importErrors.ToString());
    }

    private object GetValue(int row, string name)
    {
        var column = _importMapping[name.ToLower()];
        return _worksheet.Cells[row, column].Value;
    }

    private bool GetBooleanValue(int row, string name)
    {
        return GetValue(row, name)?.ToString() == "1";
    }

    public void Dispose()
    {
        _excelPackage?.Dispose();
        _worksheet?.Dispose();
    }
}