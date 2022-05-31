namespace RacketStringManager.Model;

public class StringingHistory
{
    public DateOnly Date { get; }

    public string StringName { get; }

    public double Tension { get; }

    public StringingHistory(Job job)
    {
        Date = job.StartDate;
        StringName = job.StringName;
        Tension = job.Tension;
    }
}