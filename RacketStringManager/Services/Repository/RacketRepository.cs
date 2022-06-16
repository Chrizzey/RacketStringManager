using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository;

public class RacketRepository : DataRepository, IRacketRepository
{
    public RacketRepository()
    {
        Database.CreateTable<RacketEntity>();
    }

    public IEnumerable<RacketEntity> GetAll() => Database.Table<RacketEntity>().ToArray();

    public RacketEntity Find(string name) => Database.DeferredQuery<RacketEntity>($"select * from {nameof(RacketEntity)} where name=? COLLATE NoCase", name).FirstOrDefault();

    public RacketEntity Get(Guid id) => Database.Find<RacketEntity>(id);

    public void Insert(RacketEntity entity) => Database.Insert(entity);

    public void Delete(RacketEntity entity) => Database.Delete(entity);

    public void Clear()
    {
        Database.DropTable<PlayerEntity>();

        Database.CreateTable<StringEntity>();
    }
}