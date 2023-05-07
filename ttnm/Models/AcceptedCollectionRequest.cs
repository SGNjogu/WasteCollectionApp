using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttnm.Models
{
    [INotifyPropertyChanged]
    public partial class AcceptedCollectionRequest
    {
        [ObservableProperty]
        private int item_id;
        [ObservableProperty]
        private string household_remarks;
        [ObservableProperty]
        private string collector_remarks;
        [ObservableProperty]
        private string status;
        [ObservableProperty]
        private string description;
        [ObservableProperty]
        private DateTime? request_date;
        [ObservableProperty]
        private string collected_date;
        [ObservableProperty]
        private string delivered_date;
        [ObservableProperty]
        private string points;
        [ObservableProperty]
        private string total_weight;
        [ObservableProperty]
        private string confirmed_weight;
        [ObservableProperty]
        private string pickup_address;
        [ObservableProperty]
        private string pickup_latitude;
        [ObservableProperty]
        private string pickup_longitude;
        [ObservableProperty]
        private string extra_comments;
        [ObservableProperty]
        private string contact_person;
        [ObservableProperty]
        private string contact_phone;
        [ObservableProperty]
        private string pickup_time;
        [ObservableProperty]
        private string waste_type;
    }
}
