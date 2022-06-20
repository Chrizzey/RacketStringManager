using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository;

public class StringingRepository : DataRepository, IStringingRepository
{
    public StringingRepository()
    {
        Database.CreateTable<StringEntity>();
    }

    public IEnumerable<StringEntity> GetAll() => Database.Table<StringEntity>().ToArray();

    public StringEntity Find(string name) => Database.FindWithQuery<StringEntity>($"select * from {nameof(StringEntity)} where name='{name}'");

    public StringEntity Get(Guid id) => Database.Find<StringEntity>(id);

    public void Insert(StringEntity entity) => Database.Insert(entity);

    public void Delete(StringEntity entity) => Database.Delete(entity);

    public void Clear()
    {
        Database.DropTable<PlayerEntity>();

        Database.CreateTable<StringEntity>();
    }
}