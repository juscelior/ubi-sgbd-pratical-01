using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UbiSGBD.Models
{
    [INotifyPropertyChanged]
    public partial class LogList
    {
        [ObservableProperty]
        List<LogTempo> itemsLogTempo;

        [ObservableProperty]
        List<Log> itemsLog;
    }
}
