using Npgsql;

using System;
using System.Data;

namespace ClassLibrary4OpenGauss
{

    public class OpenGaussDbEvent
    {
        /// <summary>
        /// 資料庫連結字串格式樣板
        /// </summary>
        public string ConnectStringFormat = "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer;No Reset On Close=true";

        /// <summary>
        /// 偵測資料表是否存在(Reference --> https://stackoverflow.com/questions/14746404/check-if-postgresql-table-exists-with-npgsql)
        /// </summary>
        /// <param name="preparedConnectionString">資料庫連結字串</param>
        /// <param name="tableName">資料表名稱</param>
        /// <returns>如果為true,資料表為存在,反之則false.</returns>
        public bool TableExists(string preparedConnectionString, string tableName)
        {
            string sql = "SELECT * FROM information_schema.tables WHERE table_name = '" + tableName + "'";
            using (var con = new NpgsqlConnection(preparedConnectionString))
            {
                using (var cmd = new NpgsqlCommand(sql))
                {
                    if (cmd.Connection == null)
                        cmd.Connection = con;
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    lock (cmd)
                    {
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            try
                            {
                                //if (rdr != null && rdr.HasRows)
                                if (rdr != null)
                                    return true;
                                return false;
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 刪除或新增資料表
        /// </summary>
        /// <param name="Host">伺服器連線位址</param>
        /// <param name="User">系統管理員使用者名稱</param>
        /// <param name="DBname">資料庫名稱</param>
        /// <param name="Port">伺服器連線埠號</param>
        /// <param name="Password">系統管理員使用者密碼</param>
        /// <param name="TableName">資料表名稱</param>
        /// <param name="StrFeedBack">回傳訊息</param>
        public void DeleteOrCreateOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password, string TableName, out string StrFeedBack)
        {
            string connString =
                String.Format(ConnectStringFormat,
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            using (var conn = new NpgsqlConnection(connString))
            {

                conn.Open();
                if (TableExists(connString, TableName))
                {
                    using (var command = new NpgsqlCommand("DROP TABLE IF EXISTS " + TableName, conn))
                    {
                        command.ExecuteNonQuery();
                        StrFeedBack = "Finished droping table";
                    }
                }
                else
                {
                    using (var command = new NpgsqlCommand("CREATE TABLE " + TableName + "(id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER)", conn))
                    {
                        command.ExecuteNonQuery();
                        StrFeedBack = "Finished creating table";
                    }
                }

                conn.Close();
            }
        }

        /// <summary>
        /// 新增資料表
        /// </summary>
        /// <param name="Host">伺服器連線位址</param>
        /// <param name="User">系統管理員使用者名稱</param>
        /// <param name="DBname">資料庫名稱</param>
        /// <param name="Port">伺服器連線埠號</param>
        /// <param name="Password">系統管理員使用者密碼</param>
        /// <param name="TableName">資料表名稱</param>
        /// <param name="StrFeedBack">回傳訊息</param>
        public void CreateOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password, string TableName, out string StrFeedBack)
        {
            string connString =
                String.Format(ConnectStringFormat,
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            using (var conn = new NpgsqlConnection(connString))
            {

                conn.Open();

                using (var command = new NpgsqlCommand("CREATE TABLE " + TableName + "(id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER)", conn))
                {
                    command.ExecuteNonQuery();
                    StrFeedBack = "Finished creating table";
                }

                conn.Close();
            }
        }

        /// <summary>
        /// 刪除資料表
        /// </summary>
        /// <param name="Host">伺服器連線位址</param>
        /// <param name="User">系統管理員使用者名稱</param>
        /// <param name="DBname">資料庫名稱</param>
        /// <param name="Port">伺服器連線埠號</param>
        /// <param name="Password">系統管理員使用者密碼</param>
        /// <param name="TableName">資料表名稱</param>
        /// <param name="StrFeedBack">回傳訊息</param>
        public void DeleteOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password, string TableName, out string StrFeedBack)
        {
            string connString =
                String.Format(ConnectStringFormat,
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            using (var conn = new NpgsqlConnection(connString))
            {

                conn.Open();

                using (var command = new NpgsqlCommand("DROP TABLE IF EXISTS " + TableName, conn))
                {
                    command.ExecuteNonQuery();
                    StrFeedBack = "Finished droping table";
                }

                conn.Close();
            }
        }

        /// <summary>
        /// 新增資料項目
        /// </summary>
        /// <param name="Host">伺服器連線位址</param>
        /// <param name="User">系統管理員使用者名稱</param>
        /// <param name="DBname">資料庫名稱</param>
        /// <param name="Port">伺服器連線埠號</param>
        /// <param name="Password">系統管理員使用者密碼</param>
        /// <param name="TableName">資料表名稱</param>
        /// <param name="strName">輸入內容名稱</param>
        /// <param name="strQuantity">輸入內容數量</param>
        /// <param name="StrFeedBack">回傳訊息</param>
        public void CreateOpenGaussDbDataItem(string Host, string User, string DBname, string Port, string Password, string TableName, string strName, string strQuantity, out string StrFeedBack)
        {
            string connString =
                String.Format(ConnectStringFormat,
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            using (var conn = new NpgsqlConnection(connString))

            {
                conn.Open();
                if (TableExists(connString, TableName))
                {
                    using (var command = new NpgsqlCommand("INSERT INTO " + TableName + " (name, quantity) VALUES (@n1, @q1)", conn))
                    {
                        command.Parameters.AddWithValue("n1", strName);
                        command.Parameters.AddWithValue("q1", strQuantity);

                        int nRows = command.ExecuteNonQuery();
                        StrFeedBack = String.Format("Number of rows inserted={0}", nRows);
                    }
                }
                else
                {
                    StrFeedBack = "Table '" + TableName + "' is not exists, please create new one...";
                }
                conn.Close();
            }
        }

        public void CreateOpenGaussDbDataItem(string Host, string User, string DBname, string Port, string Password, string TableName, string[] FieldArray, string[] DataArray, out string StrFeedBack)
        {
            string connString =
                String.Format(ConnectStringFormat,
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            string stringListOfFieldArray = "";
            for (int i = 0; i < FieldArray.Length; i++)
            {
                stringListOfFieldArray += FieldArray[i];
                if (i < (FieldArray.Length - 1))
                {
                    stringListOfFieldArray += ", ";
                }
            }

            string stringListOfDataArray = "";
            for (int i = 0; i < FieldArray.Length; i++)
            {
                stringListOfDataArray += "@data" + i.ToString();
                if (i < (FieldArray.Length - 1))
                {
                    stringListOfDataArray += ", ";
                }
            }

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                if (TableExists(connString, TableName))
                {
                    using (var command = new NpgsqlCommand("INSERT INTO " + TableName + " (" + stringListOfFieldArray + ") VALUES (" + stringListOfDataArray + ")", conn))
                    {
                        if (FieldArray.Length == DataArray.Length)
                        {
                          int nRows = 0;
                          for (int i = 0; i < FieldArray.Length; i++)
                            {
                                command.Parameters.AddWithValue("data" + i.ToString(), DataArray[i]);
                                nRows = command.ExecuteNonQuery();
                            }
                         StrFeedBack = String.Format("Number of rows inserted={0}", nRows);
                       }
                        else
                        {
                            StrFeedBack = "Field Array and Data Array have difference length.";
                        }
                    }
                }
                else
                {
                    StrFeedBack = "Table '" + TableName + "' is not exists, please create new one...";
                }
                conn.Close();
            }
        }


        /// <summary>
        /// 讀取完整資料
        /// </summary>
        /// <param name="Host">伺服器連線位址</param>
        /// <param name="User">系統管理員使用者名稱</param>
        /// <param name="DBname">資料庫名稱</param>
        /// <param name="Port">伺服器連線埠號</param>
        /// <param name="Password">系統管理員使用者密碼</param>
        /// <param name="TableName">資料表名稱</param>
        /// <param name="StrFeedBack">回傳訊息</param>
        public void ReadOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password, string TableName, out string StrFeedBack)
        {
            string connString =
            String.Format(ConnectStringFormat,
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                StrFeedBack = "";
                if (TableExists(connString, TableName))
                {
                    using (var command = new NpgsqlCommand("SELECT * FROM " + TableName, conn))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            StrFeedBack += string.Format(
                                    "({0}, {1}, {2})\n",
                                    reader.GetInt32(0).ToString(),
                                    reader.GetString(1),
                                    reader.GetInt32(2).ToString()
                                    );
                        }
                        reader.Close();
                    }
                }
                else
                {
                    StrFeedBack = "Table '" + TableName + "' is not exists, please create new one...";
                }
                conn.Close();
            }
        }

        /// <summary>
        /// 更新資料項目
        /// </summary>
        /// <param name="Host">伺服器連線位址</param>
        /// <param name="User">系統管理員使用者名稱</param>
        /// <param name="DBname">資料庫名稱</param>
        /// <param name="Port">伺服器連線埠號</param>
        /// <param name="Password">系統管理員使用者密碼</param>
        /// <param name="TableName">資料表名稱</param>
        /// <param name="strName">輸入內容名稱</param>
        /// <param name="strQuantity">輸入內容數量</param>
        /// <param name="StrFeedBack">回傳訊息</param>
        public void UpdateOpenGaussDbTableItem(string Host, string User, string DBname, string Port, string Password, string TableName, string strName, string strQuantity, out string StrFeedBack)
        {
            string connString =
            String.Format(ConnectStringFormat,
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                if (TableExists(connString, TableName))
                {
                    using (var command = new NpgsqlCommand("UPDATE " + TableName + " SET quantity = @q WHERE name = @n", conn))
                    {
                        command.Parameters.AddWithValue("n", strName);
                        command.Parameters.AddWithValue("q", strQuantity);
                        int nRows = command.ExecuteNonQuery();
                        StrFeedBack = String.Format("Number of rows updated={0}", nRows);
                    }
                }
                else
                {
                    StrFeedBack = "Table '" + TableName + "' is not exists, please create new one...";
                }

                conn.Close();
            }
        }

        /// <summary>
        /// 刪除資料項目
        /// </summary>
        /// <param name="Host">伺服器連線位址</param>
        /// <param name="User">系統管理員使用者名稱</param>
        /// <param name="DBname">資料庫名稱</param>
        /// <param name="Port">伺服器連線埠號</param>
        /// <param name="Password">系統管理員使用者密碼</param>
        /// <param name="TableName">資料表名稱</param>
        /// <param name="strName">輸入內容名稱</param>
        /// <param name="StrFeedBack">回傳訊息</param>
        public void DeleteOpenGaussDbTableItem(string Host, string User, string DBname, string Port, string Password, string TableName, string strName, out string StrFeedBack)
        {
            string connString =
            String.Format(ConnectStringFormat,
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                if (TableExists(connString, TableName))
                {
                    using (var command = new NpgsqlCommand("DELETE FROM " + TableName + " WHERE name = @n", conn))
                    {
                        command.Parameters.AddWithValue("n", strName);

                        int nRows = command.ExecuteNonQuery();
                        StrFeedBack = String.Format("Number of rows deleted={0}", nRows);
                    }
                }
                else
                {
                    StrFeedBack = "Table '" + TableName + "' is not exists, please create new one...";
                }

                conn.Close();
            }
        }
    }
}
