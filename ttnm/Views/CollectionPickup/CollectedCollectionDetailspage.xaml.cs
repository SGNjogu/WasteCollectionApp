using ttnm.Domain.Data.DataService;
using ttnm.Infrastructure.Services.Collector;
using ttnm.Services.Dialogs;
using ttnm.Services.Maps;
using ttnm.ViewModels;

namespace ttnm.Views.CollectionPickup;

public partial class CollectedCollectionDetailsPage : ContentPage
{
	private readonly CollectedCollectionDetailsViewModel viewModel;
	private readonly IMapService _mapService;
	private readonly IDialogService _dialogService;
	private readonly ICollectionRequestService _collectionRequestService;

	public CollectedCollectionDetailsPage(IMapService mapService, IDialogService dialogService, IDataService dataService, ICollectionRequestService collectionRequestService)
	{
		InitializeComponent();

		_mapService = mapService;
		_dialogService = dialogService;
		_collectionRequestService = collectionRequestService;

		viewModel = new CollectedCollectionDetailsViewModel(_mapService, _collectionRequestService, _dialogService, dataService);
		BindingContext = viewModel;
	}
}