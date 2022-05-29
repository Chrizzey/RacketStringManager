using RacketStringManager.Model;
using RacketStringManager.Services;
using RacketStringManager.ViewModel;

namespace RacketStringManager;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IJobService, JobService>();
        builder.Services.AddSingleton<IJobRepository, TestJobRepository>();
        builder.Services.AddSingleton<IJobViewModelFactory, JobViewModelFactory>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }

    private class TestJobRepository : IJobRepository
    {
        public Task<IEnumerable<Job>> GetAllJobs()
        {
            IEnumerable<Job> jobs = new[]
            {
                new Job {Name = "Tim", Racket = "Yonex ArcSaber 11", IsCompleted = false, IsPaid = false},
                new Job {Name = "Rüdiger", Racket = "Yonex ArcSaber 11", IsCompleted = true, IsPaid = true},
                new Job {Name = "Matthias", Racket = "Victor AL6500", IsCompleted = true, IsPaid = false},
                new Job {Name = "Flo", Racket = "Oliver Phantom X9", IsCompleted = false, IsPaid = true}
            };

            return Task.FromResult(jobs);
        }
    }
}

