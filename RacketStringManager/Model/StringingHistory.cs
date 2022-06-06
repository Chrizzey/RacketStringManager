namespace RacketStringManager.Model;

public class StringingHistory
{
    public DateOnly Date { get; }

    public string StringName { get; }

    public double Tension { get; }

    public string Comment { get; }

    public Job StringingJob {get;}
    
    public StringingHistory(Job job)
    {
        StringingJob = job;
        Date = job.StartDate;
        StringName = job.StringName;
        Tension = job.Tension;
        Comment = job.Comment;
    }
}
