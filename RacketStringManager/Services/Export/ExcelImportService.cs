using OfficeOpenXml;
using RacketStringManager.Model;
using RacketStringManager.ViewModel.Components;

namespace RacketStringManager.Services.Export;

public class ExcelImportService : IDisposable
{
    private readonly IFilePicker _picker;
    private readonly IJobService _jobService;
    private FileResult _pickResult;
    private ExcelPackage _excelPackage;
    private ExcelWorksheet _worksheet;
    private Dictionary<string, int> _importMapping;

    public ExcelImportService(IFilePicker picker, IJobService jobService)
    {
        _picker = picker;
        _jobService = jobService;
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
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Exception!", ex.Message, "Ok");
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
            if(string.IsNullOrWhiteSpace(value))
                continue;
            
            if(_importMapping.ContainsKey(value))
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
        await Task.Run(() =>
        {
            var row = 1;
                        
            foreach (var _ in _worksheet.Rows.Skip(2))
            {
                row++;
        
                try
                {
                    var job = ImportJob(row);                    
                    _jobService.Create(job);
                }
                catch(ImportException ex)
                {
                    await Shell.DisplayNotification($"Import Error in Row {row}", ex.Message, "Ok");
                }
            }
            
            var actual = _jobService.GetAll().Count();
            await Shell.DisplayNotification($"Successfully imported {actual}/{row} jobs", "Ok)
        });
    }

    private Job ImportJob(int row)
    {
        var importErrors = new StringBuilder();
    
        var tensionString = GetValue(row, ExportNames.Tension)?.ToString();
        if (!JobEditViewModel.ParseTension(tensionString, out var tension))
        {
            tension = 0d;
            
            if(string.isNullOrWihteSpace(tensionString))
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
        
        if(string.IsNullOrWhiteSpace(dateString))
        {
            importErrors.AppendLine("Date is empty");
        }
        else
        {
            try
            {
                job.StartDate = ParseDateOnly(dateString);
            }
            catch(FormatException)
            {
                importErros.AppendLine($"Date '{dateString}' is not a valid date");
            }
        }
        
        ImportSanityCheck(job, importErrors);
        
        return job;
    }
    
    private void ImportSanityCheck(Job job, StringBuilder importErrors)
    {
        if(string.IsNullOrWhiteSpace(job.Name))
            importErrors.AppendLine("Name is empty");
        if(string.IsNullOrWhiteSpace(job.Racket))
            importErrors.AppendLine("Racket is empty");
        if(string.IsNullOrWhiteSpace(job.StringName))
            importErrors.AppendLine("String is empty");
        
        if(importErrors.Length > 0)
            throw new ImportException(importErrors.ToString());
    }

    private object GetValue(int row, string name)
    {
        var column = _importMapping[name.ToLower()];
        return _worksheet.Cells[row, column].Value;
    }
       
    private bool GetBooleanValue(int row, string name)
    {
       return GetValue(row, name).Equals(1);
    }

    public void Dispose()
    {
        _excelPackage?.Dispose();
        _worksheet?.Dispose();
    }
}
