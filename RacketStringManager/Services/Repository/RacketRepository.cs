using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository;

public class RacketRepository : DataRepository, IRacketRepository
{
    public RacketRepository()
    {
        Database.CreateTable<RacketEntity>();
    }

    public IEnumerable<RacketEntity> GetAll() => Database.Table<RacketEntity>().ToArray();

    public RacketEntity Find(string name) => Database.FindWithQuery<RacketEntity>($"select * from {nameof(RacketEntity)} where name='{name}'");

    public RacketEntity Get(Guid id) => Database.Find<RacketEntity>(id);

    public void Insert(RacketEntity entity) => Database.Insert(entity);

    public void Clear()
    {
        Database.DropTable<PlayerEntity>();

        Database.CreateTable<StringEntity>();
    }
}