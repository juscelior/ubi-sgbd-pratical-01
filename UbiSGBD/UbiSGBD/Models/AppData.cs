using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UbiSGBD.Models
{
    public static class AppData
    {
        public static Connection Connection = new Connection("Edit");
        public static Connection ConnectionBrowser = new Connection("Browser");
        public static Connection ConnectionLog = new Connection("Log");


        public static OrderList Orders = new OrderList()
        {
            OrderSelected = new(),
            Items = GenerateOrders()
        };

        public static LogList LogList = new LogList()
        {
            ItemsLogTempo = new(),
            ItemsLog = new(),
        };

        public static async Task TestConnection(Connection connection)
        {
            using (var conn = new SqlConnection(connection.ConnectionString))
            {
                try
                {
                    conn.Open();
                    await App.Current.MainPage.DisplayAlert("Sucesso", "Conexão bem-sucedida!", "Okay");
                }
                catch (Exception)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Não foi possível estabelecer a conexão.", "Okay");
                }
            }
        }
        public static async Task<List<Order>> LoadAsync(Connection connection)
        {
            try
            {
                List<Order> orders = new List<Order>();


                using (SqlCommand command = new SqlCommand(
                   "SELECT EncID, ClienteID,Nome,Morada FROM Encomenda ORDER BY EncID DESC;" +
                   "SELECT EncId, ProdutoID, Designacao,Preco, Qtd FROM EncLinha",
                   connection.SqlConnection))
                {
                    command.Transaction = connection.Transaction;


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Order o = new Order
                                {
                                    EncId = reader.GetInt32(0),
                                    ClienteID = reader.GetInt32(1),
                                    Nome = reader.GetString(2),
                                    Morada = reader.GetString(3),
                                    Items = new List<Item>()
                                };

                                orders.Add(o);
                            }


                            reader.NextResult();

                            while (reader.Read())
                            {

                                Item i = new Item
                                {
                                    EncId = reader.GetInt32(0),
                                    ProdutoId = reader.GetInt32(1),
                                    Designacao = reader.GetString(2),
                                    Preco = reader.GetDecimal(3),
                                    Quantidade = reader.GetDecimal(4),
                                };


                                Order o = orders.FirstOrDefault(o => o.EncId == i.EncId);
                                o?.Items.Add(i);
                            }
                        }
                    }
                }

                return orders;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");

                return null;
            }
        }

        public static async Task<List<LogTempo>> LoadLogTempoAsync(Connection connection)
        {
            try
            {
                List<LogTempo> logTempoList = new List<LogTempo>();


                using (SqlCommand command = new SqlCommand(
                   "SELECT LO1.UserId, LO1.Objecto as EncId, DATEDIFF(SS,LO1.Valor, LO2.Valor) as Tempo " +
                   "FROM LogOperations LO1, LogOperations LO2 " +
                   "WHERE LO1.Referencia = LO2.Referencia and LO1.DCriacao < LO2.DCRiacao and " +
                   "LO1.Referencia in (" +
                   "    SELECT Referencia FROM LogOperations WHERE EventType = 'O'" +
                   ") ORDER BY LO1.Objecto DESC;",
                   connection.SqlConnection))
                {
                    command.Transaction = connection.Transaction;


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            LogTempo l = new LogTempo
                            {
                                UserId = reader.GetString(0),
                                EncId = reader.GetString(1),
                                Tempo = reader.GetInt32(2),
                            };

                            logTempoList.Add(l);
                        }
                    }
                }

                return logTempoList;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");

                return null;
            }
        }

        public static async Task<List<Log>> LoadLogAsync(int quantidade, Connection connection)
        {
            try
            {
                List<Log> logList = new List<Log>();


                using (SqlCommand command = new SqlCommand(
                   "SELECT TOP (@Qtd) [NumReg] ,[EventType] ,[Objecto] ,[Valor] ,[Referencia] ,[UserID] ,[TerminalD] ,[TerminalName] ,[DCriacao]  FROM [MEI_TRAB].[dbo].[LogOperations] WHERE EventType in ('I','U','D') ORDER BY [NumReg] DESC",
                   connection.SqlConnection))
                {
                    command.Transaction = connection.Transaction;

                    command.Parameters.Add("@Qtd", System.Data.SqlDbType.Int);
                    command.Parameters["@Qtd"].Value = quantidade;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Log l = new Log
                            {
                                NumReg = reader.GetInt32(0),
                                EventType = reader.GetString(1),
                                Objecto = reader.GetString(2),
                                Valor = reader.GetString(3),

                                Referencia = reader.IsDBNull(4) ? null : reader.GetString(4),
                                UserID = reader.GetString(5),
                                TerminalD = reader.GetString(6),
                                TerminalName = reader.GetString(7),
                                DCriacao = reader.GetDateTime(8),

                            };

                            logList.Add(l);
                        }
                    }
                }

                return logList;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");

                return null;
            }
        }
        public static async Task<Order> LoadAsync(int encID)
        {
            try
            {
                Order order = null;


                using (SqlCommand command = new SqlCommand(
                   "SELECT TOP 1 EncId, ClienteID,Nome,Morada FROM Encomenda WHERE EncId = @EncId;" +
                   "SELECT EncId, ProdutoID, Designacao,Preco, Qtd FROM EncLinha WHERE EncId = @EncId;",
                   AppData.Connection.SqlConnection))
                {
                    command.Transaction = AppData.Connection.Transaction;


                    command.Parameters.Add("@EncId", System.Data.SqlDbType.Int);
                    command.Parameters["@EncId"].Value = encID;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                order = new Order
                                {
                                    EncId = reader.GetInt32(0),
                                    ClienteID = reader.GetInt32(1),
                                    Nome = reader.GetString(2),
                                    Morada = reader.GetString(3),
                                    Items = new List<Item>()
                                };
                            }

                            reader.NextResult();

                            while (reader.Read())
                            {

                                Item i = new Item
                                {
                                    EncId = reader.GetInt32(0),
                                    ProdutoId = reader.GetInt32(1),
                                    Designacao = reader.GetString(2),
                                    Preco = reader.GetDecimal(3),
                                    Quantidade = reader.GetDecimal(4),
                                };


                                order?.Items.Add(i);
                            }
                        }
                    }
                }


                return order;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");

                return null;
            }
        }
        public static async Task<bool> SaveAsync(Order order)
        {
            bool result = false;
            try
            {
                if (order.EncId > 0)
                {
                    result = await UpdateAsync(order);
                }
                else
                {
                    result = await InsertAsync(order);
                }
            }
            catch (Exception ex)
            {
                result = false;
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");
            }


            return result;
        }

        public static async Task<string> CommitAsync(Order order)
        {
            string result = null;

            AppData.Connection.Commit();

            if (!string.IsNullOrWhiteSpace(AppData.Connection.Referencia))
            {
                result = AppData.Connection.Referencia;
                await InsertLogOperationsAsync(order.EncId, AppData.Connection.Referencia);
                AppData.Connection.Referencia = null;
            }

            AppData.Connection.CloseConnection();

            return result;
        }

        public static void Rollback()
        {
            AppData.Connection.Rollback();

            AppData.Connection.CloseConnection();
        }


        private static List<Order> GenerateOrders()
        {
            return new();
        }
        private static async Task<bool> InsertAsync(Order order)
        {
            bool success = true;
            try
            {

                String cmdText = @"DECLARE @EncId int;
                                SELECT @EncId = ISNULL(Max(EncId),0) + 1 FROM Encomenda; 

                                INSERT INTO Encomenda VALUES (@EncId, @ClienteID, @Nome, @Morada);
                                
                                SELECT @EncId;
                                ";

                String cmdEncLinhaText = @"
                                INSERT INTO EncLinha Values (@EncId, @ProdutoID, @Designacao, @Preco, @Qtd);
                                ";

                using (SqlCommand command = new SqlCommand(cmdText, AppData.Connection.SqlConnection))
                {
                    command.Transaction = AppData.Connection.Transaction;

                    command.Parameters.Add("@ClienteID", System.Data.SqlDbType.Int);
                    command.Parameters["@ClienteID"].Value = order.ClienteID;

                    command.Parameters.Add("@Nome", System.Data.SqlDbType.NVarChar);
                    command.Parameters["@Nome"].Value = order.Nome;

                    command.Parameters.Add("@Morada", System.Data.SqlDbType.NVarChar);
                    command.Parameters["@Morada"].Value = order.Morada;

                    order.EncId = Convert.ToInt32(command.ExecuteScalar());

                    //Insert
                    InsertEncLinha(order.EncId, order, cmdEncLinhaText, order.Items);
                }



            }
            catch (Exception ex)
            {
                success = false;
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");
            }

            return success;
        }
        private static async Task<bool> UpdateAsync(Order order)
        {
            bool success = true;
            try
            {

                String cmdText = @"
                                    UPDATE Encomenda SET Morada = @Morada WHERE EncId = @EncId;
                                ";

                String cmdSelectEncLinhaText = @"
                                    SELECT ProdutoID FROM EncLinha WHERE EncId = @EncId;
                                ";

                String cmdEncLinhaText = @"
                                INSERT INTO EncLinha Values (@EncId, @ProdutoID, @Designacao, @Preco, @Qtd);
                                ";

                String cmdUpdateEncLinhaText = @"
                                UPDATE EncLinha SET Qtd = @Qtd WHERE EncId = @EncId AND ProdutoID = @ProdutoID;
                                ";

                List<int> existProdutoIdList = new List<int>();

                using (SqlCommand command = new SqlCommand(cmdSelectEncLinhaText, AppData.Connection.SqlConnection))
                {
                    command.Transaction = AppData.Connection.Transaction;

                    command.Parameters.Add("@EncId", System.Data.SqlDbType.Int);
                    command.Parameters["@EncId"].Value = order.EncId;


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int produtoId = reader.GetInt32(0);
                            existProdutoIdList.Add(produtoId);
                        }
                    }
                }


                using (SqlCommand command = new SqlCommand(cmdText, AppData.Connection.SqlConnection))
                {
                    command.Transaction = AppData.Connection.Transaction;

                    command.Parameters.Add("@EncId", System.Data.SqlDbType.Int);
                    command.Parameters["@EncId"].Value = order.EncId;

                    command.Parameters.Add("@Morada", System.Data.SqlDbType.NVarChar);
                    command.Parameters["@Morada"].Value = order.Morada;

                    int count = Convert.ToInt32(command.ExecuteNonQuery());

                    var toUpdateItems = order.Items.Where(o => existProdutoIdList.Contains(o.ProdutoId));
                    var toInsertItems = order.Items.Where(o => !existProdutoIdList.Contains(o.ProdutoId));

                    //Update
                    UpdateEncLinha(order.EncId, cmdUpdateEncLinhaText, toUpdateItems);

                    //Insert
                    InsertEncLinha(order.EncId, order, cmdEncLinhaText, toInsertItems);

                }

            }
            catch (Exception ex)
            {
                success = false;
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");
            }

            return success;
        }

        private static void InsertEncLinha(int encId, Order order, string query, IEnumerable<Item> toInsertItems)
        {
            using (SqlCommand commandLinha = new SqlCommand(query, AppData.Connection.SqlConnection))
            {
                commandLinha.Transaction = AppData.Connection.Transaction;

                commandLinha.Parameters.Add("@EncId", System.Data.SqlDbType.Int);
                commandLinha.Parameters.Add("@ProdutoID", System.Data.SqlDbType.Int);
                commandLinha.Parameters.Add("@Designacao", System.Data.SqlDbType.NVarChar);
                commandLinha.Parameters.Add("@Preco", System.Data.SqlDbType.Decimal);
                commandLinha.Parameters.Add("@Qtd", System.Data.SqlDbType.Decimal);

                commandLinha.Parameters["@EncId"].Value = encId;

                foreach (var item in toInsertItems)
                {
                    commandLinha.Parameters["@Qtd"].Value = item.Quantidade;
                    commandLinha.Parameters["@Preco"].Value = item.Preco;
                    commandLinha.Parameters["@Designacao"].Value = item.Designacao;
                    commandLinha.Parameters["@ProdutoID"].Value = item.ProdutoId;

                    commandLinha.ExecuteNonQuery();
                }


            }
        }
        private static void UpdateEncLinha(int encId, string query, IEnumerable<Item> toUpdateItems)
        {
            using (SqlCommand commandLinha = new SqlCommand(query, AppData.Connection.SqlConnection))
            {
                commandLinha.Transaction = AppData.Connection.Transaction;

                commandLinha.Parameters.Add("@EncId", System.Data.SqlDbType.Int);
                commandLinha.Parameters.Add("@ProdutoID", System.Data.SqlDbType.Int);
                commandLinha.Parameters.Add("@Qtd", System.Data.SqlDbType.Decimal);

                commandLinha.Parameters["@EncId"].Value = encId;

                foreach (var item in toUpdateItems)
                {
                    commandLinha.Parameters["@Qtd"].Value = item.Quantidade;
                    commandLinha.Parameters["@ProdutoID"].Value = item.ProdutoId;

                    commandLinha.ExecuteNonQuery();
                }
            }
        }
        public static async Task InsertLogOperationsAsync(int encId, string reference)
        {
            try
            {

                String cmdLogOperations = @"
                                Insert Into LogOperations (EventType, Objecto, Valor, Referencia) Values ('O', @EncId, @UserDateTime, @UserReference)
                                ";

                using (SqlCommand commandLinha = new SqlCommand(cmdLogOperations, AppData.Connection.SqlConnection))
                {
                    commandLinha.Transaction = AppData.Connection.Transaction;

                    commandLinha.Parameters.Add("@EncId", System.Data.SqlDbType.VarChar);
                    commandLinha.Parameters.Add("@UserDateTime", System.Data.SqlDbType.DateTime);
                    commandLinha.Parameters.Add("@UserReference", System.Data.SqlDbType.VarChar);

                    commandLinha.Parameters["@EncId"].Value = encId.ToString();
                    commandLinha.Parameters["@UserDateTime"].Value = DateTime.Now;
                    commandLinha.Parameters["@UserReference"].Value = reference;

                    commandLinha.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Okay");
            }
        }

    }
}
