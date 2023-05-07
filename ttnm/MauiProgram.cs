using CommunityToolkit.Maui;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using ttnm.CustomControls;
using ttnm.Domain.Data.DataService;
using ttnm.Infrastructure.Services.Aggregator;
using ttnm.Infrastructure.Services.APIService;
using ttnm.Infrastructure.Services.Auth;
using ttnm.Infrastructure.Services.Collector;
using ttnm.Infrastructure.Services.Profile;
using ttnm.Infrastructure.Services.Support;
using ttnm.Services.DataSync;
using ttnm.Services.Dialogs;
using ttnm.Services.Logging;
using ttnm.Services.Maps;
using ttnm.Services.Settings;
using ttnm.ViewModels;
using ttnm.Views.CollectionPickup;
using ttnm.Views.CollectorHistory;
using ttnm.Views.Dashboard;
using ttnm.Views.FAQs;
using ttnm.Views.Login;
using ttnm.Views.NewTransaction;
using ttnm.Views.Settings;

namespace ttnm;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCompatibility()
            .UseMauiCommunityToolkit()
            .RegisterAppServices() // register services before ViewModels
            .RegisterViewModels()
            .RegisterShellPages()
            .RegisterOtherPages()
            .UseMauiMaps()
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddCompatibilityRenderer(typeof(BorderlessEntry), typeof(ttnm.Platforms.Android.CustomControls.BorderlessEntry));
#elif IOS
                handlers.AddCompatibilityRenderer(typeof(BorderlessEntry), typeof(ttnm.Platforms.iOS.CustomControls.BorderlessEntry));
#endif
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-brands.ttf", "FontAwesomeBrands");
                fonts.AddFont("fa-solid.ttf", "FontAwesomeSolid");
                fonts.AddFont("fa-regular.ttf", "FontAwesomeRegular");
                fonts.AddFont("fa-light.ttf", "FontAwesomeLight");
                fonts.AddFont("lato-regular.ttf", "LatoRegular");
                fonts.AddFont("lato-bold.ttf", "LatoBold");
            });

        AppCenter.Start(
                "android=1a841e01-331c-40fb-9266-1ee4acd15a67;" +
                "ios=fa4af378-b0ca-4291-abc3-9dd7ecdb47aa;",
                typeof(Analytics), typeof(Crashes));



        return builder.Build();
    }

    // Register Services first
    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<IDataService, DataService>();
        builder.Services.AddSingleton<IMapService, MapService>();

        builder.Services.AddSingleton<IRestService, RestService>(); // Initialize RestService before all services that use it
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IProfileService, ProfileService>();
        builder.Services.AddSingleton<ISupportService, SupportService>();
        builder.Services.AddSingleton<IAggregatorService, AggregatorService>();

        builder.Services.AddSingleton<ICollectionRequestService, CollectionRequestService>();
        builder.Services.AddSingleton<ICollectorsService, CollectorsService>();
        builder.Services.AddSingleton<ICollectionOrdersListService, CollectionOrdersListService>();
        builder.Services.AddSingleton<IPullDataService, PullDataService>();
        builder.Services.AddSingleton<ICrashlyticsConfig, CrashlyticsConfig>();
        builder.Services.AddSingleton<IPushDataService, PushDataService>();

        return builder;
    }

    // Register ViewModels second
    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        // For pages that appear in Shell TabBar use Singleton
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<FAQsViewModel>();

        // For pages that are navigated to from Shell TabBar pages use Transient
        builder.Services.AddTransient<AuthViewModel>();
        builder.Services.AddTransient<CollectionPickupViewModel>();
        builder.Services.AddTransient<PickupScheduleViewModel>();
        builder.Services.AddTransient<CollectionHistoryViewModel>();
        builder.Services.AddTransient<SupportViewModel>();
        builder.Services.AddTransient<NewTransactionViewModel>();
        builder.Services.AddTransient<RegisterCollectorViewModel>();
        builder.Services.AddTransient<MapViewModel>();

        // For pages that are not directly accessible to Shell
        builder.Services.AddTransient<AcceptedCollectionViewModel>();
        builder.Services.AddTransient<AcceptedCollectionDetailsViewModel>();
        builder.Services.AddTransient<PendingCollectionViewModel>();
        builder.Services.AddTransient<PendingCollectionDetailsViewModel>();
        builder.Services.AddTransient<CollectedCollectionDetailsViewModel>();
        builder.Services.AddTransient<VerifyCollectorViewModel>();
        builder.Services.AddTransient<ConfirmDetailsViewModel>();
        builder.Services.AddTransient<EnterPaymentDetailsViewModel>();
        builder.Services.AddTransient<ConfirmPaymentDetailsViewModel>();
        builder.Services.AddTransient<NewTransactionSuccessViewModel>();
        builder.Services.AddTransient<CollectionDeliveryViewModel>();

        return builder;
    }

    // Register Shell Pages third
    public static MauiAppBuilder RegisterShellPages(this MauiAppBuilder builder) // For Shell pages only. Pages that Navigation is handled with Shell
    {
        builder.Services.AddSingleton<DashboardPage>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<FAQsPage>();
        builder.Services.AddSingleton<CollectionPickupPage>();
        builder.Services.AddSingleton<PickupSchedulePage>();
        builder.Services.AddSingleton<CollectionHistoryPage>();
        builder.Services.AddSingleton<SupportPage>();
        builder.Services.AddSingleton<NewTransactionPage>();
        builder.Services.AddSingleton<RegisterCollectorPage>();
        builder.Services.AddSingleton<MapPage>();

        return builder;
    }

    // Register Other Pages
    public static MauiAppBuilder RegisterOtherPages(this MauiAppBuilder builder) // For pages not directly accessible to Shell
    {
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<ForgotPasswordPage>();
        builder.Services.AddSingleton<PendingCollectionPage>();
        builder.Services.AddSingleton<PendingCollectionDetailsPage>();
        builder.Services.AddSingleton<AcceptedCollectionPage>();
        builder.Services.AddSingleton<AcceptedCollectionDetailsPage>();
        builder.Services.AddSingleton<CollectedCollectionDetailsPage>();
        builder.Services.AddSingleton<CollectionDeliveryPage>();

        // Aggregator pages
        builder.Services.AddSingleton<ConfirmDetailsPage>();
        builder.Services.AddSingleton<EnterPaymentDetailsPage>();
        builder.Services.AddSingleton<ConfirmPaymentPage>();
        builder.Services.AddSingleton<NewTransactionSuccessPage>();
        builder.Services.AddSingleton<VerifyCollectorPage>();

        return builder;
    }
}
