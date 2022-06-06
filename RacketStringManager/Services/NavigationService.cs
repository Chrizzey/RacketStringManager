using RacketStringManager.Model;
using RacketStringManager.View;

namespace RacketStringManager.Services;

public class NavigationService : INavigationService
{
    private readonly IUiService _uiService;

    public NavigationService(IUiService uiService)
    {
        _uiService = uiService;
    }

    public async Task GoToJobDetailsPage(Job job)
    {
        await _uiService.GotoPageAsync(nameof(JobDetailsPage), job);
    }

    public async Task GoToCreateJobPage()
    {
        await _uiService.GotoPageAsync(nameof(CreateJobPage));
    }

    public async Task GoBack() => await _uiService.GoBackAsync();
    
    public async Task GotoEditJobPage(Job job)
    {
        await _uiService.GotoPageAsync(nameof(EditJobPage), job);
    }
}