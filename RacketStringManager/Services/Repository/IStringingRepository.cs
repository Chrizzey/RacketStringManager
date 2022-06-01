using RacketStringManager.Model.Entities;

namespace RacketStringManager.Services.Repository;

public interface IStringingRepository
{
    IEnumerable<StringEntity> GetAll();
    StringEntity Find(string name);
    StringEntity Get(Guid id);
    void Insert(StringEntity entity);
    void Clear();
}