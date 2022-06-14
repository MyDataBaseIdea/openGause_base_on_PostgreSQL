using Npgsql;

using System;

namespace ConsoleApp4OpenGauss
{
    internal class Program
    {
        // Obtain connection string information from the portal
        //
        private static string Host = "192.168.1.109";
        private static string User = "gaussdb";
        private static string DBname = "mytest";
        private static string Password = "P@ssw0rd";//"Trq@7251";
        private static string Port = "5432";

        static void Main(string[] args)
        {
            bool isQuit = false;
            do
            {
                int MySelect = -1;
                Console.WriteLine("1. Create New openGauss Table and Data");
                Console.WriteLine("2. Read openGauss Table");
                Console.WriteLine("3. Update openGauss Data");
                Console.WriteLine("4. Delete openGauss Data");
                Console.WriteLine("5. Exit");
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
                            Console.WriteLine("你選擇 1. 新增openGauss資料表及資料...");
                            //OpenGaussDbEvent myEvent = new OpenGaussDbEvent();
                            myEvent.CreateOpenGaussDbTable(Host, User, DBname, Port, Password);
                            break;
                        }
                    case 2:
                        {
                            try
                            {
                                Console.WriteLine("你選擇 2. 讀取openGauss資料表...");
                                //OpenGaussDbEvent myEvent = new OpenGaussDbEvent();
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password);
                                break;
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                    case 3:
                        {
                            try
                            {
                                Console.WriteLine("你選擇 3. 更新openGauss資料...");
                                //OpenGaussDbEvent myEvent = new OpenGaussDbEvent();
                                myEvent.UpdateOpenGaussDbTable(Host, User, DBname, Port, Password);
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
                                Console.WriteLine("你選擇 4. 刪除openGauss資料...");
                                //OpenGaussDbEvent myEvent = new OpenGaussDbEvent();
                                myEvent.DeleteOpenGaussDbTable(Host, User, DBname, Port, Password);
                                break;
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                    case 5:
                        {
                            Console.WriteLine("你選擇 5. 離開...");
                            isQuit = true;
                            Console.WriteLine("請按任意鍵離開...");
                            Console.ReadLine();
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
        public void CreateOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password)
        {
            // Build connection string using parameters from portal
            //
            string connString =
                String.Format(
                    "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);


            using (var conn = new NpgsqlConnection(connString))

            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();

                using (var command = new NpgsqlCommand("DROP TABLE IF EXISTS inventory", conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished dropping table (if existed)");

                }

                using (var command = new NpgsqlCommand("CREATE TABLE inventory(id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER)", conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished creating table");
                }

                using (var command = new NpgsqlCommand("INSERT INTO inventory (name, quantity) VALUES (@n1, @q1), (@n2, @q2), (@n3, @q3)", conn))
                {
                    command.Parameters.AddWithValue("n1", "banana");
                    command.Parameters.AddWithValue("q1", 150);
                    command.Parameters.AddWithValue("n2", "orange");
                    command.Parameters.AddWithValue("q2", 154);
                    command.Parameters.AddWithValue("n3", "apple");
                    command.Parameters.AddWithValue("q3", 100);

                    int nRows = command.ExecuteNonQuery();
                    Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
                }

                conn.Close();
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }
        public void ReadOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password)
        {
            // Build connection string using parameters from portal
            //
            string connString =
                String.Format(
                    "Server={0}; User Id={1}; Database={2}; Port={3}; Password={4};SSLMode=Prefer",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();
                
                using (var command = new NpgsqlCommand("SELECT * FROM inventory", conn))
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
            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }
        public void UpdateOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password)
        {
            // Build connection string using parameters from portal
            //
            string connString =
                String.Format(
                    "Server={0}; User Id={1}; Database={2}; Port={3}; Password={4};SSLMode=Prefer",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();

                using (var command = new NpgsqlCommand("UPDATE inventory SET quantity = @q WHERE name = @n", conn))
                {
                    command.Parameters.AddWithValue("n", "banana");
                    command.Parameters.AddWithValue("q", 200);
                    int nRows = command.ExecuteNonQuery();
                    Console.Out.WriteLine(String.Format("Number of rows updated={0}", nRows));
                }

                conn.Close();
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }
        public void DeleteOpenGaussDbTable(string Host, string User, string DBname, string Port, string Password)
        {
            // Build connection string using parameters from portal
            //
            string connString =
                String.Format(
                    "Server={0}; User Id={1}; Database={2}; Port={3}; Password={4};SSLMode=Prefer",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();

                using (var command = new NpgsqlCommand("DELETE FROM inventory WHERE name = @n", conn))
                {
                    command.Parameters.AddWithValue("n", "orange");

                    int nRows = command.ExecuteNonQuery();
                    Console.Out.WriteLine(String.Format("Number of rows deleted={0}", nRows));
                }

                conn.Close();
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }
    }

}