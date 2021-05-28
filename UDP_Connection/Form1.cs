using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace UDP_Connection
{
    public partial class Form1 : Form
    {
        UdpClient u;
        Thread th;

        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false; //忽略跨執行緒的錯誤
            th = new Thread(Listen);
            th.Start();
            button1.Enabled = false;
        }

        private void Form1_load(object sender, EventArgs e)
        {
            this.Text += " 本主機IP位置為: " + MyIP();
        }
        /// <summary>
        /// 取得本機IP並顯示於螢幕上
        /// </summary>
        /// <returns></returns>
        private string MyIP()
        {
            string hn = Dns.GetHostName(); //取得主機的名稱
            IPAddress[] ip = Dns.GetHostEntry(hn).AddressList;
            foreach(var item in ip)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork) //如果是IPv4格式
                {
                    return item.ToString();
                }
            }
            return " ";
        }
        /// <summary>
        /// 監聽UDPㄉPort有沒有EP傳東西進來
        /// </summary>
        private void Listen()
        {
            int Port = int.Parse(textBox1.Text);
            u = new UdpClient(Port); //監聽UDP監聽器實體
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port); //建立本機EndPoint資訊
            while (true)
            {
                byte[] b = u.Receive(ref EP);
                textBox2.Text = Encoding.Default.GetString(b);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string IP = textBox3.Text;
            int Port = int.Parse(textBox4.Text);
            byte[] B = Encoding.Default.GetBytes(textBox5.Text);
            UdpClient s = new UdpClient();
            s.Send(B, B.Length, IP, Port);
            s.Close();
        }

        private void Form_closing(object sender, FormClosingEventArgs e)
        {
            try 
            {
                th.Abort(); //關閉監聽執行緒
                u.Close(); //關閉Socket
            }
            catch
            {
                //忽略錯誤程式繼續執行;
            }
        }
    }
}
