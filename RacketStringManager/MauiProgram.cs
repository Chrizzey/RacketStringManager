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
