﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_AndMySQL
{
    internal class CustomerDAL
    {
        #region Fields
        private static readonly string connectionString = Constants.connectionString;
        #endregion

        #region Methods
        public static void Insert(Customer pCustomer)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand("CustomerInsert", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@p_Name", pCustomer.Name);
                            command.Parameters.AddWithValue("@p_Email", pCustomer.Email);
                            command.Parameters.AddWithValue("@p_User_Name", pCustomer.UserName);
                            command.Parameters.AddWithValue("@p_password", pCustomer.Password);

                            MySqlParameter outputParameter = new MySqlParameter("@Inserted_ID", MySqlDbType.Int32);
                            outputParameter.Direction = System.Data.ParameterDirection.Output;
                            command.Parameters.Add(outputParameter);

                            command.ExecuteNonQuery();

                            pCustomer.ID = Convert.ToInt32(outputParameter.Value);
                        }
                        transaction.Commit();
                    }
                    catch(Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error in Insert: {e.Message}");
                    }
                }
            } 
        }

        public static void Update(Customer pCustomer)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using(MySqlTransaction transaction  = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand("CustomerUpdate", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@p_ID", pCustomer.ID);
                            command.Parameters.AddWithValue("@p_Name", pCustomer.Name);
                            command.Parameters.AddWithValue("@p_Email", pCustomer.Email);
                            command.Parameters.AddWithValue("@p_User_Name", pCustomer.UserName);
                            command.Parameters.AddWithValue("@p_password", pCustomer.Password);
                            command.Parameters.AddWithValue("@p_Is_Active", pCustomer.IsActive);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();

                    }catch(Exception e)
                    {
                        Console.WriteLine($"Error in update: {e.Message}");
                    }
                }
            }
        }

        public static void Delete(Customer pCustomer)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using(MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand("CustomerDelete", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@p_ID", pCustomer.ID);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }catch(Exception e)
                    {
                        Console.WriteLine($"Error in Delete: {e.Message}");
                    }
                }
            }
        }

        public static void GetByID(Customer pCustomer)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using(MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand("CustomerGetByID", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@p_ID", pCustomer.ID);

                            command.Parameters.Add("@p_Name", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                            command.Parameters.Add("@p_Email", MySqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                            command.Parameters.Add("@p_User_Name", MySqlDbType.VarChar, 25).Direction = ParameterDirection.Output;
                            command.Parameters.Add("@p_password", MySqlDbType.VarChar, 25).Direction = ParameterDirection.Output;
                            command.Parameters.Add("@p_Is_Active", MySqlDbType.Bit).Direction = ParameterDirection.Output;

                            command.ExecuteNonQuery();

                            pCustomer.Name = command.Parameters["@p_Name"].Value.ToString();
                            pCustomer.Email = command.Parameters["@p_Email"].Value.ToString();
                            pCustomer.UserName = command.Parameters["@p_User_Name"].Value.ToString();
                            pCustomer.Password = command.Parameters["@p_password"].Value.ToString();
                            pCustomer.IsActive = Convert.ToBoolean(command.Parameters["@p_Is_Active"].Value);
                        }

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error in GetByID: {e.Message}");
                    }
                }

            }
        }

        public static DataSet Browse(Customer pCustomer)
        {
            DataSet dataSet = new DataSet();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using(MySqlTransaction  transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command = new MySqlCommand("CustomerBrowse", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@p_Name", string.IsNullOrEmpty(pCustomer.Name) ? null : pCustomer.Name);
                            command.Parameters.AddWithValue("@p_Email", string.IsNullOrEmpty(pCustomer.Email) ? null : pCustomer.Email);

                            using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command))
                            {
                                dataAdapter.Fill(dataSet);
                            }
                        }

                        transaction.Commit();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Error in Browse: {e.Message}");
                    }
                }      
            }
            return dataSet;
        }

        public static List<Customer> GetCollection(Customer pCustomer)
        {
            List<Customer> customerList = new List<Customer>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("CustomerBrowse", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_Name", string.IsNullOrEmpty(pCustomer.Name) ? null : pCustomer.Name);
                    command.Parameters.AddWithValue("@p_Email", string.IsNullOrEmpty(pCustomer.Email) ? null : pCustomer.Email);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                UserName = reader["User_Name"].ToString(),
                                Password = reader["password"].ToString(),
                                IsActive = Convert.ToBoolean(reader["Is_Active"])
                            };

                            customerList.Add(customer);
                        }
                    }
                }
            }
            return customerList;
        }
        #endregion
    }
}
