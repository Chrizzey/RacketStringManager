using RacketStringManager.Model;

namespace RacketStringManager.Services;

public class UiService : IUiService
{
    public async Task GotoPageAsync(string uri)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.GoToAsync(uri, true);
        });
    }

    public async Task GotoPageAsync(string uri, Job job)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.GoToAsync(uri, true, new Dictionary<string, object>
        {
            {"Job", job}
        });
        });
    }

    public async Task GoBackAsync()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.GoToAsync("..", true);
        });
    }

    public async Task<bool> GetUserConfirmation(string title, string message, string accept, string cancel)
    {
        return await MainThread.InvokeOnMainThreadAsync(
            async () => await Shell.Current.DisplayAlert(title, message, accept, cancel)
            );
    }

    public async Task DisplayAlertAsync(string title, string message, string cancel)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.DisplayAlert(title, message, cancel);
        });
    }
}