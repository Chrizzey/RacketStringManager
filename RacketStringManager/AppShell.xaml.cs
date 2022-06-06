using RacketStringManager.View;
using RacketStringManager.ViewModel;

namespace RacketStringManager;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(JobDetailsPage), typeof(JobDetailsPage));
        Routing.RegisterRoute(nameof(CreateJobPage), typeof(CreateJobPage));
        Routing.RegisterRoute(nameof(EditJobPage), typeof(EditJobPage));
    }
}
