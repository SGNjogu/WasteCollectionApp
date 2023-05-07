using ttnm.Domain.Data.DataService;
using ttnm.Infrastructure.Services.Collector;
using ttnm.Services.Dialogs;
using ttnm.ViewModels;

namespace ttnm.Views.CollectorHistory;

public partial class CollectionDeliveryPage : ContentPage
{
	private readonly CollectionDeliveryViewModel viewModel;
	public CollectionDeliveryPage(IDataService dataService, ICollectionRequestService collectionRequestService, IDialogService dialogService)
	{
		InitializeComponent();
		viewModel = new CollectionDeliveryViewModel(dataService, collectionRequestService, dialogService);
		BindingContext = viewModel;
	}
}