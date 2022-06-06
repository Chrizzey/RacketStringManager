using RacketStringManager.Model;

namespace RacketStringManager.Services;

public interface INavigationService
{
    Task GoToJobDetailsPage(Job job);

    Task GoToCreateJobPage();

    Task GoBack();

    Task GotoEditJobPage(Job job);
}