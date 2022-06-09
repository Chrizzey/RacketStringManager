using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository;

public interface IRacketRepository : IDisposable
{
    IEnumerable<RacketEntity> GetAll();
    RacketEntity Find(string name);
    RacketEntity Get(Guid id);
    void Insert(RacketEntity entity);
    void Clear();
}