using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository;

public class PlayerRepository : DataRepository, IPlayerRepository
{
    public PlayerRepository()
    {
        Database.CreateTable<PlayerEntity>();
    }

    public IEnumerable<PlayerEntity> GetAll() => Database.Table<PlayerEntity>().ToArray();

    public PlayerEntity Find(string name) => Database.FindWithQuery<PlayerEntity>($"select * from {nameof(PlayerEntity)} where name='{name}'");

    public PlayerEntity Get(Guid id) => Database.Find<PlayerEntity>(id);

    public void Insert(PlayerEntity entity) => Database.Insert(entity);

    public void Clear()
    {
        Database.DropTable<PlayerEntity>();

        Database.CreateTable<StringEntity>();
    }
}