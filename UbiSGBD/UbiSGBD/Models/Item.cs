using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UbiSGBD.Models
{
    [INotifyPropertyChanged]
    public partial class Item
    {
        [ObservableProperty]
        int encId;

        [ObservableProperty]
        int produtoId;

        [ObservableProperty]
        string designacao;

        [ObservableProperty]
        decimal quantidade;

        [ObservableProperty]
        decimal preco;
    }
}
