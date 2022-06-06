using RacketStringManager.Model;

namespace RacketStringManager.Services;

public class UiService : IUiService
{
    public async Task GotoPageAsync(string uri)
    {
        await Shell.Current.GoToAsync(uri, true);
    }

    public async Task GotoPageAsync(string uri, Job job)
    {
        await Shell.Current.GoToAsync(uri, true, new Dictionary<string, object>
        {
            {"Job", job}
        });
    }

    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    public async Task<bool> GetUserConfirmation(string title, string message, string accept, string cancel)
    {
        return await Shell.Current.DisplayAlert(title, message, accept, cancel);
    }

    public async Task DisplayAlertAsync(string title, string message, string cancel)
    {
        await Shell.Current.DisplayAlert(title, message, cancel);
    }
}