using RacketStringManager.Model;
using RacketStringManager.Services;
using RacketStringManager.View;
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

        builder.Services.AddTransient<JobDetailsPage>();
        builder.Services.AddTransient<JobDetailsViewModel>();

        return builder.Build();
    }

    private class TestJobRepository : IJobRepository
    {
        private IEnumerable<Job> _jobs;

        public TestJobRepository()
        {
            _jobs = new[]
            {
                new Job
                {
                    JobId = Guid.NewGuid(),
                    Name = "Tim", Tension = 11.5, Racket = "Yonex ArcSaber 11", IsCompleted = false, IsPaid = false,
                    Comment = "Lorem ipsum dolor sit amet", StringName = "Yonex BG65",
                    StartDate = DateOnly.FromDateTime(DateTime.Today)
                },
                new Job
                {
                    JobId = Guid.NewGuid(),
                    Name = "Tim", Tension = 11.5, Racket = "Yonex ArcSaber 11", IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StringName = "Yonex BG65",
                    StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-50))
                },
                new Job
                {
                    JobId = Guid.NewGuid(),
                    Name = "Tim", Tension = 10.5, Racket = "Yonex ArcSaber 11", IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StringName = "Yonex BG65",
                    StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-60))
                },
                new Job
                {
                    JobId = Guid.NewGuid(),
                    Name = "Tim", Tension = 10.5, Racket = "Yonex ArcSaber 11", IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StringName = "Yonex BG65",
                    StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-85))
                },


                new Job
                {
                    JobId = Guid.NewGuid(), Name = "Rüdiger", Racket = "Yonex ArcSaber 11", IsCompleted = true,
                    IsPaid = true
                },
                new Job
                {
                    JobId = Guid.NewGuid(), Name = "Matthias", Racket = "Victor AL6500", IsCompleted = true, IsPaid = false
                },
                new Job
                {
                    JobId =Guid.NewGuid(), Name = "Flo", Racket = "Oliver Phantom X9", IsCompleted = false, IsPaid = true
                }
            };
        }

        public Task<IEnumerable<Job>> GetAllJobs()
        {
            return Task.FromResult(_jobs);
        }
    }
}

