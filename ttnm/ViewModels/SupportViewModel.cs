using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using ttnm.Infrastructure.Services.Support;
using ttnm.Services.Dialogs;
using ttnm.Services.Settings;

namespace ttnm.ViewModels
{
    [INotifyPropertyChanged]
    public partial class SupportViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IDialogService _dialogService;
        private readonly ISupportService _supportService;

        [ObservableProperty]
        private string subject;

        [ObservableProperty]
        private string message;

        public SupportViewModel(ISettingsService settingsService, IDialogService dialogService, ISupportService supportService)
        {
            _settingsService = settingsService;
            _dialogService = dialogService;
            _supportService = supportService;
        }

        [RelayCommand]
        private async Task SubmitQuery()
        {
            try
            {
                if (String.IsNullOrEmpty(Subject) && String.IsNullOrEmpty(Message))
                {
                    _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, "No message provided. Please try again");
                    return;

                }
                if (!String.IsNullOrEmpty(Subject) || !String.IsNullOrEmpty(Message) && App.UserContext != null)
                {
                    _dialogService.ShowActivityIndicator();
                    var response = await _supportService.SubmitQuery(App.UserContext.phone, App.UserContext.email, Subject, Message, App.UserContext.name);
                    if (response.msg == "Message sent successfully!")
                    {
                        _dialogService.HideActivityIndicator();
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.Defined, response.msg);
                        Message = null;
                        Subject = null;
                    }
                    else
                    {
                        _dialogService.HideActivityIndicator();
                        _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _dialogService.HideActivityIndicator();
                _dialogService.ShowSnackBar(DialogService.DialogMessage.UndefinedError);
            }

        }
    }
}
