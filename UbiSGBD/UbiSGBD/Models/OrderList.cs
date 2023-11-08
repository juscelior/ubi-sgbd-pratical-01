using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UbiSGBD.Models
{
    [INotifyPropertyChanged]
    public partial class OrderList
    {
        [ObservableProperty]
        private Order orderSelected;

        [ObservableProperty]
        private List<Order> items;

    }
}
