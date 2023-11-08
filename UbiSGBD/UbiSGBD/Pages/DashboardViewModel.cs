using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UbiSGBD.Models;

namespace UbiSGBD.Pages
{
    [INotifyPropertyChanged]
    public partial class DashboardViewModel
    {
        [ObservableProperty]
        OrderList orders;

        [ObservableProperty]
        string textBackgroundService;

        IDispatcherTimer timer;

        public DashboardViewModel()
        {
            Orders = AppData.Orders;
            TextBackgroundService = "Habilitar timer";
            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Tick += (s, e) => DoSomething();
        }


        [RelayCommand]
        async Task ViewAll()
        {
            AppData.ConnectionBrowser.OpenConnection();

            var orders = await AppData.LoadAsync(AppData.ConnectionBrowser);

            AppData.ConnectionBrowser.TransactionDispose();
            AppData.ConnectionBrowser.CloseConnection();


            Orders.Items = orders;

        }

        [RelayCommand]
        void EnableBackgroundService()
        {
            if (timer.IsRunning)
            {
                TextBackgroundService = "Habilitar timer";
                timer.Stop();
            }
            else
            {
                TextBackgroundService = "Desabilitar timer";
                timer.Interval = TimeSpan.FromMilliseconds(AppData.Connection.Milissegundos);
                timer.Start();
            }
        }
        private void DoSomething()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await ViewAll();
            });

        }
    }

}
