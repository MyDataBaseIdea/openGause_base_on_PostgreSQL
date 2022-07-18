using Npgsql;

using System;

namespace ConsoleApp4OpenGauss
{
    internal class Program
    {
        // Obtain connection string information from the portal
        //
        //private static string Host = "tgtgaussdbfast.eastasia.azurecontainer.io";//"192.168.1.109";
        private static string Host = "192.168.1.109";
        private static string User = "gaussdb";
        private static string DBname = "mytest";
        private static string Password = "P@ssw0rd";//"Trq@7251";
        private static string Port = "5432";
        private static string TableName = "inventory1";

        static void Main(string[] args)
        {
            bool isQuit = false;
            do
            {
                int MySelect = -1;
                Console.WriteLine("1. Create New openGauss Table");
                Console.WriteLine("2. Create New openGauss Data");
                Console.WriteLine("3. Read openGauss Table");
                Console.WriteLine("4. Update openGauss Data");
                Console.WriteLine("5. Delete openGauss Data");
                Console.WriteLine("6. Exit");
                Console.WriteLine("Please select a number from 1 to 5 and click ENTER...");
                try
                {
                    MySelect = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    //只要不是輸入正確的值，就重新輸入。
                }
                OpenGaussDbEvent myEvent = new OpenGaussDbEvent();
                /*
                 * 因為，openGaussDb在呼叫過程中，第一次和第二次的呼叫過程中，如果時間間距太短，第一次可以成功，但第二次就失敗。
                 * 所以，我用Switch...Case的方式讓CRUD為四個主要程序，並且每一個主要程序用Try...Catch的方式來包裝，只要是遇到時間過短的連續呼叫，就到Catch的區塊進行break。
                 */
                switch (MySelect)
                {
                    case 1:
                        {
                            Console.WriteLine("You select 1. Create New openGauss Table...");
                            Console.Out.WriteLine("Opening connection");
                            string FeedBack = "";
                            myEvent.CreateOpenGaussDbTable(Host, User, DBname, Port, Password, TableName, out FeedBack);
                            Console.WriteLine(FeedBack);
                            Console.WriteLine("Press RETURN to exit");
                            Console.ReadLine();
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("You select 2. Create New openGauss Data...");
                            string FeedBack = "";
                            string StrName = "";
                            string IntQuantity = "";
                            do
                            {
                                Console.WriteLine("Please input name...");
                                StrName = Console.ReadLine();
                                Console.WriteLine("Please input q...");
                                IntQuantity = Console.ReadLine();
                            } while ((StrName == "") || (IntQuantity == ""));
                            Console.Out.WriteLine("Opening connection");
                            myEvent.CreateOpenGaussDbData(Host, User, DBname, Port, Password, TableName, StrName, IntQuantity, out FeedBack);
                            myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName);
                            Console.WriteLine(FeedBack);
                            Console.WriteLine("Press RETURN to exit");
                            Console.ReadLine();
                            break;
                        }
                    case 3:
                        {
                            try
                            {
                                Console.WriteLine("You select 3. Read openGauss Table...");
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName);
                                Console.WriteLine("Press RETURN to exit");
                                Console.ReadLine();
                                break;
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                    case 4:
                        {
                            try
                            {
                                Console.WriteLine("You select 4. Update openGauss Data...");
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName);
                                string FeedBack = "";
                                string StrName = "";
                                string IntQuantity = "";
                                do
                                {
                                    Console.WriteLine("Please input name of select...");
                                    StrName = Console.ReadLine();
                                    Console.WriteLine("Please input q to change...");
                                    IntQuantity = Console.ReadLine();
                                } while ((StrName == "") || (IntQuantity == ""));
                                Console.Out.WriteLine("Opening connection");
                                myEvent.UpdateOpenGaussDbTable(Host, User, DBname, Port, Password, TableName, StrName, IntQuantity,out FeedBack);
                                Console.WriteLine(FeedBack);
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName);
                                Console.WriteLine("Press RETURN to exit");
                                Console.ReadLine();
                                break;
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                    case 5:
                        {
                            try
                            {
                                Console.WriteLine("You select 5. Delete openGauss Data...");
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName);
                                string FeedBack="";
                                string StrName = "";
                                do
                                {
                                    Console.WriteLine("Please input name of select...");
                                    StrName = Console.ReadLine();
                                } while ((StrName == ""));
                                Console.Out.WriteLine("Opening connection");
                                myEvent.DeleteOpenGaussDbTable(Host, User, DBname, Port, Password, TableName, StrName,out FeedBack);
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName);
                                Console.WriteLine("Please press any key to exit...");
                                Console.ReadLine();
                                break;
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                    case 6:
                        {
                            Console.WriteLine("You select 6. Exit...");
                            isQuit = true;
                            break;
                        }
                    default:
                        break;
                }
            } while (!isQuit);
        }
    }

    public class OpenGaussDbEvent
    {
        private static string ConnectStringFormat = "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer;No Reset On Close=true";
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
                using (var command = new NpgsqlCommand("DROP TABLE IF EXISTS " + TableName, conn))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new NpgsqlCommand("CREATE TABLE " + TableName + "(id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER)", conn))
                {
                    command.ExecuteNonQuery();
                    StrFeedBack = "Finished creating table";
                }

                conn.Close();
            }
        }
        public void CreateOpenGaussDbData(string Host, string User, string DBname, string Port, string Password, string TableName, string strName, string strQuantity, out string StrFeedBack)
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

                using (var command = new NpgsqlCommand("INSERT INTO " + TableName + " (name, quantity) VALUES (@n1, @q1)", conn))
                {
                    command.Parameters.AddWithValue("n1", strName);
                    command.Parameters.AddWithValue("q1", strQuantity);

                    int nRows = command.ExecuteNonQuery();
                    StrFeedBack=String.Format("Number of rows inserted={0}", nRows);
                }

                conn.Close();
            }
        }
        public void ReadOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password, string TableName)
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
                Console.Out.WriteLine("Opening connection");
                conn.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM " + TableName, conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(
                            string.Format(
                                "Reading from table=({0}, {1}, {2})",
                                reader.GetInt32(0).ToString(),
                                reader.GetString(1),
                                reader.GetInt32(2).ToString()
                                )
                            );
                    }
                    reader.Close();
                }
                conn.Close();
            }
        }
        public void UpdateOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password, string TableName, string strName, string strQuantity, out string StrFeedBack)
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

                using (var command = new NpgsqlCommand("UPDATE " + TableName + " SET quantity = @q WHERE name = @n", conn))
                {
                    command.Parameters.AddWithValue("n", strName);
                    command.Parameters.AddWithValue("q", strQuantity);
                    int nRows = command.ExecuteNonQuery();
                    StrFeedBack = String.Format("Number of rows updated={0}", nRows);
                }

                conn.Close();
            }
        }
        public void DeleteOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password, string TableName, string strName, out string StrFeedBack)
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

                using (var command = new NpgsqlCommand("DELETE FROM " + TableName + " WHERE name = @n", conn))
                {
                    command.Parameters.AddWithValue("n", strName);

                    int nRows = command.ExecuteNonQuery();
                    StrFeedBack = String.Format("Number of rows deleted={0}", nRows);
                }

                conn.Close();
            }
        }
    }

}