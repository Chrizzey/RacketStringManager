using RacketStringManager.ViewModel;

namespace RacketStringManager.View;

public partial class EditJobPage : ContentPage
{
	public EditJobPage(EditJobViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}