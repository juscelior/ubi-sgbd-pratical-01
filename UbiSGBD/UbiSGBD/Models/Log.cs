using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UbiSGBD.Models
{
    [INotifyPropertyChanged]
    public partial class Log
    {
        [ObservableProperty]
        int numReg;
        [ObservableProperty]
        string eventType;
        [ObservableProperty]
        string objecto;
        [ObservableProperty]
        string valor;
        [ObservableProperty]
        string referencia;
        [ObservableProperty]
        string userID;
        [ObservableProperty]
        string terminalD;
        [ObservableProperty]
        string terminalName;
        [ObservableProperty]
        DateTime dCriacao;
    }
}
