﻿using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MySqlHelper
{
    public class MySqlHelper
    {
        ///private static string connectionString = ConfigurationManager.ConnectionStrings["mysqlconn"].ConnectionString;
        public static string GenLinkString = "Server=localhost;user id=root;password=1q2w3e;Database=mcs;Port=3306;charset=utf8;";
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return ds;
            }
        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string[] arrSql, string SET_ENCODING, string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                try
                {
                    connection.Open();
                    MySqlCommand cmdEncoding = new MySqlCommand(SET_ENCODING, connection);
                    cmdEncoding.ExecuteNonQuery();
                    int rows = 0;
                    foreach (string strN in arrSql)
                    {
                        using (MySqlCommand cmd = new MySqlCommand(strN, connection))
                        {
                            rows += cmd.ExecuteNonQuery();
                        }
                    }
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    connection.Close();
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
