using RacketStringManager.ViewModel;

namespace RacketStringManager.View;

public partial class DataManagementView : ContentPage
{
	public DataManagementView(DataManagementViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }
}