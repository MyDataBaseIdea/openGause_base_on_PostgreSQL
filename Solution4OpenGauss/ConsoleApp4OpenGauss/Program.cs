using ClassLibrary4OpenGauss;

using System;

namespace ConsoleApp4OpenGauss
{
    internal class Program
    {
        // Obtain connection string information from the portal
        private static string Host = "";
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
                }

                OpenGaussDbEvent myEvent = new OpenGaussDbEvent();
                switch (MySelect)
                {
                    case 1:
                        {
                            Console.WriteLine("You select 1. Create New openGauss Table...");
                            Console.Out.WriteLine("Opening connection");
                            string FeedBack = "";
                            myEvent.DeleteOrCreateOpenGaussDbTable(Host, User, DBname, Port, Password, TableName, out FeedBack);
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
                            myEvent.CreateOpenGaussDbDataItem(Host, User, DBname, Port, Password, TableName, StrName, IntQuantity, out FeedBack);
                            Console.WriteLine(FeedBack);
                            myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName, out FeedBack);
                            Console.WriteLine(FeedBack);
                            Console.WriteLine("Press RETURN to exit");
                            Console.ReadLine();
                            break;
                        }
                    case 3:
                        {
                            try
                            {
                                string FeedBack = "";
                                Console.WriteLine("You select 3. Read openGauss Table...");
                                Console.Out.WriteLine("Opening connection");
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName, out FeedBack);
                                Console.WriteLine(FeedBack);
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
                                string FeedBack = "";
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName, out FeedBack);
                                Console.WriteLine(FeedBack);
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
                                myEvent.UpdateOpenGaussDbTableItem(Host, User, DBname, Port, Password, TableName, StrName, IntQuantity, out FeedBack);
                                Console.WriteLine(FeedBack);
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName, out FeedBack);
                                Console.WriteLine(FeedBack);
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
                                string FeedBack = "";
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName, out FeedBack);
                                Console.WriteLine(FeedBack);
                                string StrName = "";
                                do
                                {
                                    Console.WriteLine("Please input name of select...");
                                    StrName = Console.ReadLine();
                                } while ((StrName == ""));
                                Console.Out.WriteLine("Opening connection");
                                myEvent.DeleteOpenGaussDbTableItem(Host, User, DBname, Port, Password, TableName, StrName, out FeedBack);
                                myEvent.ReadOpenGaussDbTable(Host, User, DBname, Port, Password, TableName, out FeedBack);
                                Console.WriteLine(FeedBack);
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
}