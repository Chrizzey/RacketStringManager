using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository;

public interface IPlayerRepository
{
    IEnumerable<PlayerEntity> GetAll();
    PlayerEntity Find(string name);
    PlayerEntity Get(Guid id);
    void Insert(PlayerEntity entity);
    void Clear();
}