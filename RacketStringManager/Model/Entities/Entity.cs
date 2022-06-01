using SQLite;

namespace RacketStringManager.Model.Entities;

public abstract class Entity
{
    [PrimaryKey, AutoIncrement]
    public virtual Guid Id { get; set; } = new Guid();
}