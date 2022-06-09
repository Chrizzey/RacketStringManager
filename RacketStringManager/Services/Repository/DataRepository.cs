using System.Diagnostics;
using SQLite;

namespace RacketStringManager.Services.Repository;

public abstract class DataRepository : IDisposable
{
    private const string DbName = "RacketStringManager.data.db";
    protected readonly SQLiteConnection Database;

    protected DataRepository()
    {
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);
        Debug.WriteLine("Initialized database @ {0}", dbPath, "Database");
        Database = new SQLiteConnection(dbPath);
    }

    public void Dispose()
    {
        Database.Dispose();
    }
}