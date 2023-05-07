using CommunityToolkit.Mvvm.ComponentModel;

namespace ttnm.Models
{
    [INotifyPropertyChanged]
    public partial class Aggregator
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private int aggregatorId;

        [ObservableProperty]
        private string wasteType;
    }
}
