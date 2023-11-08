using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UbiSGBD.Models
{
    [INotifyPropertyChanged]
    public partial class Connection
    {
        [ObservableProperty]
        string hostName;

        [ObservableProperty]
        string databaseName;

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        string isolationLevel;

        [ObservableProperty]
        string isolationLevelEncomendas;

        [ObservableProperty]
        int milissegundos;

        [ObservableProperty]
        string referencia;

        private readonly string app;

        public SqlTransaction Transaction { get; private set; }

        public SqlConnection SqlConnection { get; private set; }

        public string ConnectionString
        {
            get
            {
                return $"Server={HostName};Database={DatabaseName};User Id={Username};Password={Password};TrustServerCertificate=True;Application Name=SGBD_{app};Connect Timeout=10;";
            }
        }

        public IsolationLevel IsolationLevelSelected
        {
            get
            {
                IsolationLevel selected;
                if (Enum.TryParse<IsolationLevel>(this.IsolationLevel, out selected))
                {
                    return selected;
                }
                else
                {
                    throw new Exception("IsolationLevel invalid");
                }
            }
        }

        public Connection(string app)
        {
            HostName = "127.0.0.1";
            DatabaseName = "MEI_TRAB";
            Username = "sa";
            Password = "Ubi2023!";
            IsolationLevel = System.Data.IsolationLevel.ReadCommitted.ToString();
            Milissegundos = 10000;
            this.app = app;
        }

        public void OpenConnection()
        {
            if (SqlConnection == null)
            {
                SqlConnection = new SqlConnection(this.ConnectionString);
            }

            if (SqlConnection.State != ConnectionState.Open)
            {
                SqlConnection.Open();
                BeginTransaction();
            }
        }

        public void CloseConnection()
        {
            SqlConnection?.Close();
            SqlConnection?.Dispose();
            SqlConnection = null;
        }

        public void BeginTransaction()
        {
            if (Transaction == null)
            {
                Transaction = SqlConnection.BeginTransaction(this.IsolationLevelSelected);
            }
        }

        public void Commit()
        {
            Transaction?.Commit();
            Transaction?.Dispose();

            Transaction = null;
        }

        public void Rollback()
        {
            Transaction?.Rollback();
            Transaction?.Dispose();

            Transaction = null;
        }


        public void TransactionDispose()
        {
            Transaction?.Dispose();

            Transaction = null;
        }
    }
}
