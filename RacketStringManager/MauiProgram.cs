﻿using OfficeOpenXml;
using RacketStringManager.Services;
using RacketStringManager.Services.Export;
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

        builder.Services.AddSingleton<IJobViewModelFactory, JobViewModelFactory>()
            .AddSingleton<IUiService, UiService>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<IExcelExportService, ExcelExportService>()
            .AddSingleton<IExcelImportService, ExcelImportService>()
            .AddSingleton<ITranslationService, TranslationService>()
            .AddTransient<MainPage>()
            .AddSingleton<MainViewModel>();
       
        builder
            .AddPageWithViewModel<JobDetailsPage, JobDetailsViewModel>()
            .AddPageWithViewModel<CreateJobPage, CreateJobViewModel>()
            .AddPageWithViewModel<EditJobPage, EditJobViewModel>()
            .AddPageWithViewModel<DataManagementView, DataManagementViewModel>()
            ;

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        builder.Services.AddTransient<ExcelExportService>();
        builder.Services.AddTransient<ExcelImportService>();
        builder.Services.AddSingleton(x => Share.Default);
        builder.Services.AddSingleton(x => FilePicker.Default);

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

        services.AddSingleton<IRepositoryCleaner, RepositoryCleaner>();
        services.AddSingleton<IJobService, JobService>();

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
