using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using ttnm.Messages;
using ttnm.Services.DataSync;
using ttnm.Services.Dialogs;
using ttnm.Services.Logging;
using ttnm.Services.Settings;
using ttnm.ViewModels;
using ttnm.Views.Login;
using UserContext = ttnm.Infrastructure.Services.Auth.DTOs.User;

namespace ttnm;

public partial class App : Application
{
    public static UserContext UserContext;

    private readonly ISettingsService _settingsService;
    private readonly IDialogService _dialogService;
    private readonly ICrashlyticsConfig _crashlyticsConfig;
    private readonly IPullDataService _pullDataService;

    private BackgroundWorker BackgroundWorkerClient;

    public App(AuthViewModel viewModel, ISettingsService settingsService, IDialogService dialogService, IPullDataService pullDataService)
    {
        _settingsService = settingsService;
        _dialogService = dialogService;
        _pullDataService = pullDataService;
        InitializeComponent();

        StrongReferenceMessenger.Default.Register<UserContextMessage>(this, (r, m) =>
        {
            if (m.LoadUser)
            {
                _pullDataService.BeginDataSync();
            }
        });

        if (!_settingsService.IsUserLoggedIn())
            MainPage = new NavigationPage(new LoginPage(viewModel));
        else
        {
            GetCurrentUser();
            MainPage = new AppShell();
        }

        GetAppTheme();
        //MainPage = new NavigationPage(new LoginPage(viewModel));
    }

    private async void GetCurrentUser()
    {
        UserContext = await _settingsService.CurrentUser().ConfigureAwait(true);
        StrongReferenceMessenger.Default.Send(new UserContextMessage { LoadUser = true });
    }

    protected override void OnStart()
    {
        _dialogService.ListenForConnectionChanges();
    }

    private void GetAppTheme()
    {
        //CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Colors.Green);
        //CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.DarkContent);
        //_settingsService.GetAppTheme();
        //_settingsService.ListenForThemeChanges();
    }
}
