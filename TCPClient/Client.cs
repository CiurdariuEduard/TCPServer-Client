using SimpleTCP;
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
using System.Net.Sockets;

namespace TCPClient
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void bConnect_Click(object sender, EventArgs e)
        {
            IPAddress ipAddress = IPAddress.Parse(txtHost.Text);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, int.Parse(txtPort.Text));
            client.Connect(ipAddress.ToString(),int.Parse(txtPort.Text));
            bConnect.Enabled = false;
        }

        private void Client_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
            txtHost.Text = GetLocalIPAddress();
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;
            });
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            client.WriteLineAndGetReply(txtMsg.Text, TimeSpan.FromSeconds(3));
        }
    }
}
