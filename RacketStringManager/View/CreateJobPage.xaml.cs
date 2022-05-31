using RacketStringManager.ViewModel;

namespace RacketStringManager.View;

public partial class CreateJobPage : ContentPage
{
	public CreateJobPage(CreateJobViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
	}
}