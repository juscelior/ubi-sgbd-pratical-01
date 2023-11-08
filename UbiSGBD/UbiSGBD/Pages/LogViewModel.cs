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
    public partial class LogViewModel
    {
        [ObservableProperty]
        LogList logList;

        [ObservableProperty]
        string textBackgroundServiceLogTempo;

        [ObservableProperty]
        string textBackgroundServiceLog;


        [ObservableProperty]
        string quantidade;

        IDispatcherTimer timerLogTempo;
        IDispatcherTimer timerLog;


        public LogViewModel()
        {
            LogList = AppData.LogList;
            TextBackgroundServiceLogTempo = "Habilitar timer";
            TextBackgroundServiceLog = "Habilitar timer";

            timerLogTempo = Application.Current.Dispatcher.CreateTimer();
            timerLogTempo.Tick += (s, e) => DoSomethingLogTempo();

            timerLog = Application.Current.Dispatcher.CreateTimer();
            timerLog.Tick += (s, e) => DoSomethingLog();
        }


        [RelayCommand]
        async Task ViewAllLogTempo()
        {
            AppData.ConnectionLog.OpenConnection();

            var logTempoList = await AppData.LoadLogTempoAsync(AppData.ConnectionLog);

            AppData.ConnectionLog.TransactionDispose();
            AppData.ConnectionLog.CloseConnection();


            LogList.ItemsLogTempo = logTempoList;
        }

        [RelayCommand]
        async Task ViewAllLog()
        {
            AppData.ConnectionLog.OpenConnection();

            var logList = await AppData.LoadLogAsync(int.Parse(Quantidade), AppData.ConnectionLog);

            AppData.ConnectionLog.TransactionDispose();
            AppData.ConnectionLog.CloseConnection();

            LogList.ItemsLog = logList;
        }

        [RelayCommand]
        void EnableBackgroundServiceLogTempo()
        {
            if (timerLogTempo.IsRunning)
            {
                TextBackgroundServiceLogTempo = "Habilitar timer";
                timerLogTempo.Stop();
            }
            else
            {
                TextBackgroundServiceLogTempo = "Desabilitar timer";
                timerLogTempo.Interval = TimeSpan.FromMilliseconds(AppData.ConnectionLog.Milissegundos);
                timerLogTempo.Start();
            }
        }

        [RelayCommand]
        void EnableBackgroundServiceLog()
        {
            if (timerLog.IsRunning)
            {
                TextBackgroundServiceLog = "Habilitar timer";
                timerLog.Stop();
            }
            else
            {
                TextBackgroundServiceLog = "Desabilitar timer";
                timerLog.Interval = TimeSpan.FromMilliseconds(AppData.ConnectionLog.Milissegundos);
                timerLog.Start();
            }
        }
        private void DoSomethingLogTempo()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await ViewAllLogTempo();
            });

        }

        private void DoSomethingLog()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await ViewAllLog();
            });

        }
    }


}
