using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Views;

namespace ttnm.Services.Dialogs
{
    public class DialogService : IDialogService
    {
        public bool IsConnectionAvailable;
        private bool _isFirstLoad = true;

        private readonly string UndefinedError = "Something went wrong, please try again later.";
        private readonly string NetworkError = "Network Error.";
        private readonly string InputError = " is required.";

        private SpinnerPopup _spinnerPopup;

        public enum DialogMessage
        {
            NetworkError,
            Defined,
            UndefinedError,
            InputError
        }

        public async void ShowSnackBar
            (
            DialogMessage error,
            string message = "",
            int duration = 3,
            int fontSize = 14,
            string backgroundColor = "#333333"
            )
        {
            switch (error)
            {
                case DialogMessage.NetworkError:
                    message = NetworkError;
                    break;
                case DialogMessage.UndefinedError:
                    message = UndefinedError;
                    break;
                case DialogMessage.InputError:
                    message = message + InputError;
                    break;
            }

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb(backgroundColor),
                TextColor = Colors.White,
                ActionButtonTextColor = Color.FromArgb("#4caf50"),
                CornerRadius = new CornerRadius(5),
                Font = Font.SystemFontOfSize(fontSize),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            var snackbar = Snackbar.Make(message, actionButtonText: "Dismiss", duration: TimeSpan.FromSeconds(duration), visualOptions: snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }

        private void ListenForConnection()
        {
            Connectivity.ConnectivityChanged += (sender, args) =>
            {
                var access = args.NetworkAccess;

                switch (args.NetworkAccess)
                {
                    case NetworkAccess.Internet:
                        IsConnectionAvailable = true;
                        if (!_isFirstLoad)
                        {
                            ShowSnackBar(DialogMessage.Defined, "Back online", backgroundColor: "#00c853");
                        }
                        break;
                    case NetworkAccess.ConstrainedInternet:
                        IsConnectionAvailable = true;
                        ShowSnackBar(DialogMessage.Defined, "Limited Connection");
                        break;
                    case NetworkAccess.Local:
                    case NetworkAccess.None:
                    case NetworkAccess.Unknown:
                        IsConnectionAvailable = false;
                        if (!_isFirstLoad)
                        {
                            ShowSnackBar(DialogMessage.Defined, "Connection Lost", backgroundColor: "#d50000");
                        }
                        break;
                }
            };
        }

        public bool IsConnected()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet || current == NetworkAccess.ConstrainedInternet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ListenForConnectionChanges()
        {
            if (_isFirstLoad)
            {
                IsConnectionAvailable = IsConnected();
                _isFirstLoad = false;

                if (!IsConnectionAvailable)
                {
                    ShowSnackBar(DialogMessage.Defined, "No Connection", backgroundColor: "#d50000");
                }

                ListenForConnection();
            }
        }

        public void ShowActivityIndicator()
        {
            _spinnerPopup = new SpinnerPopup();
            Application.Current.MainPage.ShowPopup(_spinnerPopup);
        }

        public void HideActivityIndicator() 
        { 
            _spinnerPopup.Close();
        }

        public void OpenPhoneDialer(string phoneNumber)
        {
            if (PhoneDialer.Default.IsSupported)
                PhoneDialer.Default.Open(phoneNumber);
        }

        public async Task<string> OpenTextInput(string title, string currentValue)
        {
           return await App.Current.MainPage.DisplayPromptAsync(title, null,placeholder: currentValue);
        }

        public async Task<string> OpenZoneinput()
        {
            return await App.Current.MainPage.DisplayActionSheet("Select Zone", "Cancel", null, "Kitengela", "Nonkopir", "EPZ" , "New Valley" ,"Kyangombe" , "Milimani", "Yukos" , "Acacia");
        }

        public async Task<bool> OpenAlert(string title, string message , string accept ,string cancel )
        {
            return await App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
