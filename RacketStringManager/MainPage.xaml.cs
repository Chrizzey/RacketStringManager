using RacketStringManager.ViewModel;

namespace RacketStringManager;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as MainViewModel)?.LoadJobsCommand.Execute(null);
    }
}

