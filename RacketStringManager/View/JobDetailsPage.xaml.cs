using RacketStringManager.ViewModel;

namespace RacketStringManager.View;

public partial class JobDetailsPage : ContentPage
{
	public JobDetailsPage(JobDetailsViewModel detailsViewModel)
	{
		InitializeComponent();
		BindingContext = detailsViewModel;
	}
}