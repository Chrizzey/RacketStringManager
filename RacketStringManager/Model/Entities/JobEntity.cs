using System.ComponentModel.DataAnnotations.Schema;
using RacketStringManager.Model;

namespace RacketStringManager.Model.Entities;

public class JobEntity : Entity
{
    [ForeignKey(nameof(PlayerEntity))]
    public Guid PlayerId { get; set; }

    [ForeignKey(nameof(RacketEntity))]
    public Guid RacketId { get; set; }

    [ForeignKey(nameof(StringEntity))]
    public Guid StringId { get; set; }

    public double Tension { get; set; }

    public DateTime StartDate { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsPaid { get; set; }

    public string Comment { get; set; }

    public JobEntity()
    {
        Comment = string.Empty;
    }

    public JobEntity(Job job)
    {
        PlayerId = Guid.Empty;
        RacketId = Guid.Empty;
        StringId = Guid.Empty;
        Tension = job.Tension;
        StartDate = job.StartDate.ToDateTime(TimeOnly.MinValue);
        IsCompleted = job.IsCompleted;
        IsPaid = job.IsPaid;
        Comment = job.Comment;
    }
}