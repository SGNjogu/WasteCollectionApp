using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttnm.Models
{
    [INotifyPropertyChanged]
    public partial class AggCollectionHistory
    {
        [ObservableProperty]
        private string collector_name;

        [ObservableProperty]
        private string created_date;

        [ObservableProperty]
        private double weight;

        [ObservableProperty]
        private string waste_type;

        [ObservableProperty]
        private double order_amount;

    }
}
