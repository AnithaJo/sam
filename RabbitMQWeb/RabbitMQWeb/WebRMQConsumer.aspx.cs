using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using RabbitMQ.Client;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Policy;
using RabbitMQ.Client.Events;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RabbitMQWeb
{
    public partial class WebRMQConsumer : System.Web.UI.Page
    {
        //private string[] args;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Enter the credentials to estabilish a connection to Consumer");

        }
        

        protected void Button1_Click1(object sender, EventArgs e)
        { 
       
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = TextBox1.Text;
                factory.UserName = TextBox2.Text;
            factory.Password = TextBox3.Text;
            //factory.VirtualHost = "/";
            factory.Protocol = Protocols.DefaultProtocol;
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

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                        
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    MessageBox.Show("Redirectiong to the RabbitMQ ...");
                    DialogResult dialogResult = MessageBox.Show("Enter the credentials", "Login", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //do something
                        System.Diagnostics.Process.Start("http://localhost:15672/#/");
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        MessageBox.Show(" You can't go to RabbitMQ Page to view the received Message");
                                           }
                   
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue: "hels",
                                     noAck: false,
                                     consumer: consumer);                   
             
            }

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox1.Visible=false;
        }

        protected void TextBox3_TextChanged(object sender, EventArgs e)
        {
            TextBox3.Visible = true;
        }

    }
}