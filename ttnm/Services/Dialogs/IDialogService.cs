namespace ttnm.Services.Dialogs
{
    public interface IDialogService
    {
        bool IsConnected();
        void ListenForConnectionChanges();
        void ShowSnackBar(DialogService.DialogMessage error, string message = "", int duration = 2, int fontSize = 14, string backgroundColor = "#333333");
        void ShowActivityIndicator();
        void HideActivityIndicator();
        void OpenPhoneDialer(string phoneNumber);
        Task<string> OpenTextInput(string title, string currentValue);

        Task<string> OpenZoneinput();
        Task<bool> OpenAlert(string title, string message, string accept, string cancel);
    }
}