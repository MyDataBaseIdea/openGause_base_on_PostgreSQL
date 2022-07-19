using ClassLibrary4OpenGauss;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm4GaussdbCRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Obtain connection string information from the portal
        private static string Host = "192.168.1.109";
        private static string User = "gaussdb";
        private static string DBname = "mytest";
        private static string Password = "P@ssw0rd";//"Trq@7251";
        private static string Port = "5432";
        //private static string TableName = "inventory1";

        OpenGaussDbEvent theGsEvent = new OpenGaussDbEvent();

        public string MyConnectString = "";

        private void button1_Click(object sender, EventArgs e)
        {
            //ConnectStringFormat -> "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer;No Reset On Close=true"
            //Reference -> https://www.c-sharpcorner.com/UploadFile/736bf5/messagebox-show/
            string StrFeedBack = "";
            MyConnectString = string.Format(theGsEvent.ConnectStringFormat,Host,User,DBname,Port,Password);
            if (theGsEvent.TableExists(MyConnectString,textBox1.Text) == true)
            {
                DialogResult d;
                d = MessageBox.Show("是否要清空資料表?","資料表已經存在",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (d==DialogResult.Yes)
                {
                    theGsEvent.DeleteOpenGaussDbTable(Host, User, DBname, Port, Password, textBox1.Text, out StrFeedBack);
                    theGsEvent.CreateOpenGaussDbTable(Host, User, DBname, Port, Password, textBox1.Text, out StrFeedBack);
                    MessageBox.Show("資料表已清空。", "資料表處理結果",MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("資料表已保留。", "資料表處理結果", MessageBoxButtons.OK);
                }
            }
            else
            {
                theGsEvent.CreateOpenGaussDbTable(Host, User, DBname, Port, Password, textBox1.Text, out StrFeedBack);
                MessageBox.Show("資料表已新增。", "資料表處理結果", MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value >0)
            {
                for (int i = 0; i < numericUpDown1.Value; i++)
                {
                    TextBox fieldTxtBox = new TextBox();
                    fieldTxtBox.Size = new Size(100, 200);
                    fieldTxtBox.Location = new Point(i * 120, 0);
                    fieldTxtBox.Name = "fieldTxt" + i.ToString();
                    panel2.Controls.Add(fieldTxtBox);
                    TextBox dataTxtBox = new TextBox();
                    dataTxtBox.Size = new Size(100, 200);
                    dataTxtBox.Location = new Point(i * 120, 50);
                    dataTxtBox.Name = "dataTxt" + i.ToString();
                    panel2.Controls.Add(dataTxtBox);
                }
            }
        }
    }
}
