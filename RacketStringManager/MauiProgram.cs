using RacketStringManager.Model;
using RacketStringManager.Services;
using RacketStringManager.Services.Repository;
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

        builder.Services.AddRepositories();

        builder.Services.AddSingleton<IJobViewModelFactory, JobViewModelFactory>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();

        builder
            .AddPageWithViewModel<JobDetailsPage, JobDetailsViewModel>()
            .AddPageWithViewModel<CreateJobPage, CreateJobViewModel>()
            .AddPageWithViewModel<EditJobPage, EditJobViewModel>()
            ;
        
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
                    Name = "Kento Momota", Tension = 15.5, Racket = "Yonex Astrox 99", StringName = "Yonex BG65",
                    JobId = Guid.NewGuid(),IsCompleted = false, IsPaid = false,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today)
                },
                new Job
                {
                    Name = "Kento Momota", Tension = 15.5, Racket = "Yonex Astrox 99", StringName = "Yonex BG65",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-18))
                },
                new Job
                {
                    Name = "Kento Momota", Tension = 13.5, Racket = "Yonex Astrox 99", StringName = "Yonex BG80",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-32))
                },
                new Job
                {
                    Name = "Kento Momota", Tension = 15, Racket = "Yonex DUORA Z STRIKE", StringName = "Yonex BG80",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-102))
                },
                new Job
                {
                    Name = "Kento Momota", Tension = 15, Racket = "Yonex DUORA Z STRIKE", StringName = "Yonex BG65",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-132))
                },
                new Job
                {
                    Name = "Viktor Axelsen", Tension = 18.5, Racket = "Yonex Astrox 100 ZZ", StringName = "Yonex BG80",
                    JobId = Guid.NewGuid(),IsCompleted = false, IsPaid = false,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2))
                },
                new Job
                {
                    Name = "Viktor Axelsen", Tension = 15.5, Racket = "Yonex Astrox 100 ZZ", StringName = "Yonex BG80",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-12))
                },
                new Job
                {
                    Name = "Viktor Axelsen", Tension = 13.5, Racket = "Yonex Astrox 100 ZZ", StringName = "Yonex BG80",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-22))
                },
                new Job
                {
                    Name = "Mark Lamsfuß", Tension = 18, Racket = "Yonex Astrox 88S", StringName = "Yonex Aerobite",
                    JobId = Guid.NewGuid(),IsCompleted = false, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-10))
                },
                new Job
                {
                    Name = "Mark Lamsfuß", Tension = 17.5, Racket = "Yonex Astrox 88S", StringName = "Yonex Aerobite",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-24))
                },
                new Job
                {
                    Name = "Mark Lamsfuß", Tension = 18, Racket = "Yonex Astrox 88S", StringName = "Yonex Aerobite",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-34))
                },
                new Job
                {
                    Name = "Mark Lamsfuß", Tension = 17.5, Racket = "Yonex Astrox 88S", StringName = "Yonex Aerobite",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-44))
                },
                new Job
                {
                    Name = "Mark Lamsfuß", Tension = 16, Racket = "Yonex Astrox 88S", StringName = "Yonex Aerobite",
                    JobId = Guid.NewGuid(),IsCompleted = true, IsPaid = true,
                    Comment = "Lorem ipsum dolor sit amet", StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-64))
                },
            };
        }

        public IEnumerable<Job> GetAllJobs()
        {
            return _jobs;
        }

        public IEnumerable<Job> FindJobsFor(string player)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Job> FindJobsFor(string player, string racket)
        {
            throw new NotImplementedException();
        }

        public int Create(Job job)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}

public static class ServiceBuilderExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IJobRepository, JobRepository>();
        services.AddSingleton<IPlayerRepository, PlayerRepository>();
        services.AddSingleton<IRacketRepository, RacketRepository>();
        services.AddSingleton<IStringingRepository, StringingRepository>();

        services.AddSingleton<IAsyncJobRepository, AsyncJobRepository>();

        return services;
    }

    public static MauiAppBuilder AddPageWithViewModel<TPage, TViewModel>(this MauiAppBuilder builder) 
        where TPage : class 
        where TViewModel : class
    {
        builder.Services.AddTransient<TPage>();
        builder.Services.AddTransient<TViewModel>();

        return builder;
    }
}
