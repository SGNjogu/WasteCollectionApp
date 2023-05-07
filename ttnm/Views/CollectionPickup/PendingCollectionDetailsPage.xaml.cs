using ttnm.Services.DataSync;
using ttnm.Services.Dialogs;
using ttnm.Services.Maps;
using ttnm.ViewModels;

namespace ttnm.Views.CollectionPickup;

public partial class PendingCollectionDetailsPage : ContentPage
{
    private readonly PendingCollectionDetailsViewModel viewModel;
    private readonly IMapService _mapService;
    private readonly IDialogService _dialogService;
    private readonly IPushDataService _pushDataService;
    public PendingCollectionDetailsPage(IMapService mapService, IDialogService dialogService,IPushDataService pushDataService)
    {
        InitializeComponent();
        _mapService = mapService;
        _dialogService = dialogService;
        _pushDataService = pushDataService;
        viewModel = new PendingCollectionDetailsViewModel(_mapService, _dialogService,_pushDataService);
        BindingContext = viewModel;
    }
}