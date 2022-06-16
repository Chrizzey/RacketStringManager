using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository;

public interface IStringingRepository : IDisposable
{
    IEnumerable<StringEntity> GetAll();
    StringEntity Find(string name);
    StringEntity Get(Guid id);
    void Insert(StringEntity entity);
    void Clear();
    void Delete(StringEntity entity);
}