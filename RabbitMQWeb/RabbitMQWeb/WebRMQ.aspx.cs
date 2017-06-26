using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RabbitMQ.Client;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Policy;

namespace RabbitMQWeb
{
    public partial class WebRMQ : System.Web.UI.Page
    {
        private string[] args;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
        }
        protected void Button1_Click1(object sender, EventArgs e)
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            TextBox1.Text = localIP;
            ConnectionFactory factory = new ConnectionFactory();
            //factory.UserName = "agennt";
            //factory.Password = "agennt";
            factory.UserName = TextBox2.Text;
            factory.Password = TextBox3.Text;
            //factory.VirtualHost = "/";
            factory.Protocol = Protocols.DefaultProtocol;
            //factory.HostName = "10.77.8.19";
            factory.HostName = TextBox1.Text;
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hels",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: "",
                                     routingKey: "hels",
                                     basicProperties: properties,
                                     body: body);
                // Displays the MessageBox.
                MessageBox.Show("connection estabilished");
                DialogResult dialogResult = MessageBox.Show("Enter the credentials", "Login", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //do something
                    System.Diagnostics.Process.Start("http://localhost:15672/#/");
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                    MessageBox.Show(" You can't go to the RabbitMQ To view the message");
                }
            }
        }
        private static string GetMessage(string[] args)
        {



            byte[] array = System.IO.File.ReadAllBytes("C:\\Users\\h225456\\Desktop\\tesst.txt");
            // int arr = array.Length;
            var str = System.Text.Encoding.UTF8.GetString(array);
            //String s = array.ToString();
            return str;
            //return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
        protected void TextBox3_TextChanged(object sender, EventArgs e)
        {
            TextBox3.Visible = true;
        }
    }
}
