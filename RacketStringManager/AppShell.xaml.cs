using RacketStringManager.View;

namespace RacketStringManager;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(JobDetailsPage), typeof(JobDetailsPage));
    }
}
