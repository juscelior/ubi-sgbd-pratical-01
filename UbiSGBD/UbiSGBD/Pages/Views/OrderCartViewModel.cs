using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UbiSGBD.Models;

namespace UbiSGBD.Pages.Views
{
    [INotifyPropertyChanged]
    public partial class OrderCartViewModel
    {
        [ObservableProperty]
        Order order;

        [ObservableProperty]
        bool isTransactionOpen;

        [ObservableProperty]
        bool isSaveEnable;


        public OrderCartViewModel()
        {
            Order = AppData.Orders.OrderSelected;
            IsTransactionOpen = false;
            IsSaveEnable = true;
        }

        [RelayCommand]
        async Task SaveAsync()
        {
            AppData.Connection.OpenConnection();

            AppData.Connection.BeginTransaction();

            bool success = await AppData.SaveAsync(Order);

            if (success)
            {
                await App.Current.MainPage.DisplayAlert("Sucesso", "Salvo com sucesso", "Okay");
            }

            IsSaveEnable = !success;
            IsTransactionOpen = success;
        }

        [RelayCommand]
        async Task CommitAsync()
        {
            string referencia = await AppData.CommitAsync(Order);

            if (!string.IsNullOrWhiteSpace(referencia))
            {
                await App.Current.MainPage.DisplayAlert("Sucesso", $"Salvo com sucesso! Referência {referencia}", "Okay");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Sucesso", $"Salvo com sucesso! Sem referência.", "Okay");

            }

            await CleanAsync();

            IsSaveEnable = true;
            IsTransactionOpen = false;
        }


        [RelayCommand]
        async Task RollbackAsync()
        {
            AppData.Rollback();

            await App.Current.MainPage.DisplayAlert("Sucesso", $"Rollback com sucesso!", "Okay");

            await CleanAsync();

            IsSaveEnable = true;
            IsTransactionOpen = false;
        }

        [RelayCommand]
        void AddNewItem()
        {
            if (Order.Items == null)
            {
                Order.Items = new List<Item>();
            }

            Order.Items.Add(new Item());

            Order.Items = new List<Item>(Order.Items);
        }

        [RelayCommand]
        async Task CleanAsync()
        {
            await AppData.Orders.OrderSelected.CleanAsync();
        }
    }
}
