using OfficeOpenXml;
using RacketStringManager.Model;

namespace RacketStringManager.Services.Export;

public class ExcelImportService : IDisposable
{
    private readonly IFilePicker _picker;
    private FileResult _pickResult;
    private ExcelPackage _excelPackage;
    private ExcelWorksheet _worksheet;
    private Dictionary<string, int> _importMapping;

    public ExcelImportService(IFilePicker picker)
    {
        _picker = picker;
    }

    public async Task Import()
    {
        try
        {
            await PickFile();
            if (!HasUserPickedAFile())
                return;

            await OpenFile();
            CreateImportMapping();
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

    private async Task ImportAllJobs()
    {
        await Task.Run(() =>
        {
            try
            {
                var row = 1;
                foreach (var _ in _worksheet.Rows.Skip(2))
                {
                    row++;

                    var job = new Job
                    {
                        Name = GetValue(row, ExportNames.Name)?.ToString() ?? "Kein Name",
                        Racket = GetValue(row, ExportNames.Racket)?.ToString() ?? "Kein Schläger",

                        StringName = GetValue(row, ExportNames.Stringing)?.ToString() ?? "Keine Saite",
                        Comment = GetValue(row, ExportNames.Comment)?.ToString() ?? string.Empty,

                        IsPaid = GetValue(row, ExportNames.Paid).Equals(1),
                        IsCompleted = GetValue(row, ExportNames.Completed).Equals(1),

                        Tension = 0d,
                        StartDate = new DateOnly()
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        });
    }

    private object GetValue(int row, string name)
    {
        var column = _importMapping[name.ToLower()];
        return _worksheet.Cells[row, column].Value;
    }

    public void Dispose()
    {
        _excelPackage?.Dispose();
        _worksheet?.Dispose();
    }
}