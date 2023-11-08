using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UbiSGBD.Models
{
    [INotifyPropertyChanged]
    public partial class Order
    {
        [ObservableProperty]
        int encId;

        [ObservableProperty]
        int clienteID;

        [ObservableProperty]
        string nome;

        [ObservableProperty]
        string morada;

        [ObservableProperty]
        List<Item> items;

        [RelayCommand]
        async Task SelectOrder(int encId)
        {
            try
            {
                if (string.IsNullOrEmpty(AppData.Connection.Referencia))
                {
                    AppData.Connection.Referencia = DateTime.Now.ToString("G3-yyyyMMddHHmmssfffffff");
                }

                AppData.Connection.OpenConnection();

                await AppData.InsertLogOperationsAsync(encId, AppData.Connection.Referencia);
                AppData.Connection.BeginTransaction();

                await AppData.Orders.OrderSelected.LoadAsync(await AppData.LoadAsync(encId));

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");
            }
        }

        public async Task CleanAsync()
        {
            try
            {
                EncId = 0;
                ClienteID = 0;
                Nome = null;
                Morada = null;
                Items = null;

                AppData.Connection.TransactionDispose();
                AppData.Connection.CloseConnection();

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");
            }
        }

        public async Task LoadAsync(Order o)
        {
            try
            {
                if (o != null)
                {
                    EncId = o.EncId;
                    ClienteID = o.ClienteID;
                    Nome = o.Nome;
                    Morada = o.Morada;
                    Items = o.Items;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");
            }
        }
    }
}
