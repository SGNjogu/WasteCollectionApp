using ttnm.ViewModels;

namespace ttnm.Views.Login;

public partial class LoginPage : ContentPage
{
    private readonly AuthViewModel viewModel;
    public LoginPage(AuthViewModel authViewModel)
    {
        InitializeComponent();
        viewModel = authViewModel;
        BindingContext = viewModel;
    }
}