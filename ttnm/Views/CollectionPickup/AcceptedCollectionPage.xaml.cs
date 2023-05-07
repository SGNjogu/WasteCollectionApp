using ttnm.ViewModels;

namespace ttnm.Views.CollectionPickup;

public partial class AcceptedCollectionPage : ContentPage
{
	private readonly AcceptedCollectionViewModel viewModel;

    public AcceptedCollectionPage()
	{
		InitializeComponent();

		viewModel= new AcceptedCollectionViewModel();
		BindingContext= viewModel;
	}
}