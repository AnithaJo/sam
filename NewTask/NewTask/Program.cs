using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace NewTask
{
    class NewTask
    {
        public static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "agennt";
            factory.Password = "agennt";
            //factory.VirtualHost = "/";
            factory.Protocol = Protocols.DefaultProtocol;
            factory.HostName = "10.77.8.19";
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello4",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);



                var message = GetMessage(args);
                var message1 = GetMessagee(args);
             
                var body = Encoding.UTF8.GetBytes(message);
                var body1 = Encoding.UTF8.GetBytes(message1);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello4",
                                     basicProperties: properties,
                                     body: body);
                channel.BasicPublish(exchange: "",
                                  routingKey: "hello6",
                                  basicProperties: properties,
                                  body: body1);
              

                Console.WriteLine(" [x] Sent {0}", message);
                Console.WriteLine(" [x] Sent {0}", message1);
                
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World for worker program consumer 11 !");
        }
        private static string GetMessagee(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World for worker program consumer 12!");
        }
        
    }
}
