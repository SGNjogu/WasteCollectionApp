using ttnm.Models;
using ttnm.ViewModels;

namespace ttnm.Views.Dashboard;

public partial class NewTransactionPage : ContentPage
{
    private NewTransactionViewModel _dataContext;
    public NewTransactionPage(NewTransactionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _dataContext = viewModel;
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var item = (WasteCollection)button.BindingContext;

        _dataContext.DeleteItem(item);
    }


    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchbar = sender as SearchBar;
        if (searchbar.Text.Length == 0)
        {
            _dataContext.CancelSearch();
        }
    }
}