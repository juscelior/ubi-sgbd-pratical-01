using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UbiSGBD.Models;

namespace UbiSGBD.Pages
{
    [INotifyPropertyChanged]
    public partial class ConnectionViewModel
    {
        [ObservableProperty]
        Connection connection;

        [ObservableProperty]
        Connection connectionBrower;

        [ObservableProperty]
        Connection connectionLog;


        public ConnectionViewModel()
        {
            Connection = AppData.Connection;
            ConnectionBrower = AppData.ConnectionBrowser;
            ConnectionLog = AppData.ConnectionLog;
        }

        [RelayCommand]
        async Task TestConnection()
        {
            await AppData.TestConnection(Connection);
        }

        [RelayCommand]
        async Task TestConnectionBrower()
        {
            await AppData.TestConnection(ConnectionBrower);
        }

        [RelayCommand]
        async Task TestConnectionLog()
        {
            await AppData.TestConnection(ConnectionLog);
        }
    }
}
