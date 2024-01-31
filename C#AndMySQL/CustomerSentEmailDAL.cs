using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_AndMySQL
{
    internal class CustomerSentEmailDAL
    {
        #region Fields
        private static readonly string connectionString = Constants.connectionString;
        #endregion

        #region Methods
        public static void Insert(Customer_Sent_Emails pSentEmail)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Customer_Sent_Emails (Customer_ID, From_Address, CC_Address, BCC_Address, Subject, Message, Sent_When) VALUES (@Customer_ID, @From_Address, @CC_Address, @BCC_Address, @Subject, @Message, @Sent_When)";

                using(MySqlTransaction tr = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Customer_ID", pSentEmail.Customer_ID);
                            command.Parameters.AddWithValue("@From_Address", pSentEmail.From_Address);
                            command.Parameters.AddWithValue("@CC_Address", pSentEmail.CC_Address);
                            command.Parameters.AddWithValue("@BCC_Address", pSentEmail.BCC_Address);
                            command.Parameters.AddWithValue("@Subject", pSentEmail.Subject);
                            command.Parameters.AddWithValue("@Message", pSentEmail.Message);
                            command.Parameters.AddWithValue("@Sent_When", pSentEmail.Sent_When);

                            command.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }catch(Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
        }

        public static void Update(Customer_Sent_Emails pSentEmail)
        {
            using(MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Customer_Sent_Emails SET " +
               "From_Address = @From_Address, " +
               "CC_Address = @CC_Address, " +
               "BCC_Address = @BCC_Address, " +
               "Subject = @Subject, " +
               "Message = @Message, " +
               "Sent_When = @Sent_When " +
               "WHERE ID = @ID";

                using (MySqlTransaction tr = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ID", pSentEmail.ID);
                            command.Parameters.AddWithValue("@Customer_ID", pSentEmail.Customer_ID);
                            command.Parameters.AddWithValue("@From_Address", pSentEmail.From_Address);
                            command.Parameters.AddWithValue("@CC_Address", pSentEmail.CC_Address);
                            command.Parameters.AddWithValue("@BCC_Address", pSentEmail.BCC_Address);
                            command.Parameters.AddWithValue("@Subject", pSentEmail.Subject);
                            command.Parameters.AddWithValue("@Message", pSentEmail.Message);
                            command.Parameters.AddWithValue("@Sent_When", pSentEmail.Sent_When);

                            command.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
        }

        public static void Delete(Customer_Sent_Emails pSentEmail)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Customer_Sent_Emails WHERE ID = @ID";

                using (MySqlTransaction tr = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ID", pSentEmail.ID);

                            command.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
        }

        public static void GetByID(Customer_Sent_Emails pSentEmail)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Customer_Sent_Emails WHERE ID = @ID";

                using (MySqlTransaction tr = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ID", pSentEmail.ID);

                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    pSentEmail.Customer_ID = Convert.ToInt32(reader["Customer_ID"]);
                                    pSentEmail.From_Address = reader["From_Address"].ToString();
                                    pSentEmail.CC_Address = reader["CC_Address"].ToString();
                                    pSentEmail.BCC_Address = reader["BCC_Address"].ToString();
                                    pSentEmail.Subject = reader["Subject"].ToString();
                                    pSentEmail.Message = reader["Message"].ToString();
                                    pSentEmail.Sent_When = Convert.ToDateTime(reader["Sent_When"]);
                                }
                            }
                        }
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
        }

        public static DataSet Browse(Customer_Sent_Emails pSentEmail)
        {
            DataSet dataSet = new DataSet();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Customer_ID, From_Address, CC_Address, BCC_Address, Subject, Message, Sent_When " +
                       "FROM Customer_Sent_Emails " +
                       "WHERE (@p_From_Address IS NULL OR From_Address LIKE CONCAT('%', @p_From_Address, '%')) " +
                       "  AND (@p_Subject IS NULL OR Subject LIKE CONCAT('%', @p_Subject, '%'))";

                using (MySqlTransaction tr = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(@query, connection))
                        {
                            command.Parameters.AddWithValue("@p_From_Address", string.IsNullOrEmpty(pSentEmail.From_Address) ? null : pSentEmail.From_Address);
                            command.Parameters.AddWithValue("@p_Subject", string.IsNullOrEmpty(pSentEmail.Subject) ? null : pSentEmail.Subject);

                            using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command))
                            {
                                dataAdapter.Fill(dataSet);
                            }
                        }
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
            return dataSet;
        }

        public static List<Customer_Sent_Emails> GetCollection(Customer_Sent_Emails pSentEmail)
        {
            List<Customer_Sent_Emails> sentEmailsList = new List<Customer_Sent_Emails>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Customer_ID, From_Address, CC_Address, BCC_Address, Subject, Message, Sent_When " +
                       "FROM Customer_Sent_Emails " +
                       "WHERE (@p_From_Address IS NULL OR From_Address LIKE CONCAT('%', @p_From_Address, '%')) " +
                       "  AND (@p_Subject IS NULL OR Subject LIKE CONCAT('%', @p_Subject, '%'))";

                using (MySqlTransaction tr = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@p_From_Address", string.IsNullOrEmpty(pSentEmail.From_Address) ? null : pSentEmail.From_Address);
                            command.Parameters.AddWithValue("@p_Subject", string.IsNullOrEmpty(pSentEmail.Subject) ? null : pSentEmail.Subject);

                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Customer_Sent_Emails sentEmail = new Customer_Sent_Emails
                                    {
                                        ID = Convert.ToInt32(reader["ID"]),
                                        Customer_ID = Convert.ToInt32(reader["Customer_ID"]),
                                        From_Address = reader["From_Address"].ToString(),
                                        CC_Address = reader["CC_Address"].ToString(),
                                        BCC_Address = reader["BCC_Address"].ToString(),
                                        Subject = reader["Subject"].ToString(),
                                        Message = reader["Message"].ToString(),
                                        Sent_When = Convert.ToDateTime(reader["Sent_When"])
                                    };

                                    sentEmailsList.Add(sentEmail);
                                }
                            }
                        }
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
            return sentEmailsList;
        }


        #endregion
    }
}
