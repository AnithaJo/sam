using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Sennd
{
    class Sennd
    {
        public static void Main()
        {
            //var client = "10.77.12.89 ";
            //var username = "guest";
            //var password = "guest";
            //var factory = new ConnectionFactory() { HostName = client, UserName= "admin-PC", Password="PasswordVFT" };
            //var factory = new ConnectionFactory() { HostName = client , Port = 5672 , UserName = "admin-PC", Password = "PasswordVFT" };
            //var factory = new ConnectionFactory() { HostName = client , UserName = "agennt", Password = "agennt" };
            //ConnectionFactory cf = new ConnectionFactory();
            //cf.RequestedConnectionTimeout = 180;
            //cf.RequestedHeartbeat = 180;

            //ConnectionFactory factory = new ConnectionFactory();
            //factory.UserName = "admin-PC";
            //factory.Password = "PasswordVFT";
            //factory.VirtualHost = "/";
            //factory.Protocol = Protocols.DefaultProtocol;
            //factory.HostName = "10.77.12.89";
            //factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            //IConnection conn = factory.CreateConnection();
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "agennt";
            factory.Password = "agennt";
            //factory.VirtualHost = "/";
            factory.Protocol = Protocols.DefaultProtocol;
            factory.HostName = "10.77.12.89";
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            using (IConnection connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "Hello World ani!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
