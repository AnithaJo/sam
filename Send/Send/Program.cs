using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace Send
{
    class Send
    {

        enum EventName
        {
            Alarm = 1,
            PreAlarm = 2,
            Fire = 3,
            Security = 4
        };
        public static void Main()
        {
            ConnectionFactory factory = new ConnectionFactory();
            //factory.UserName = "agennt";
            //factory.Password = "agennt";
            //factory.VirtualHost = "/";
            factory.Protocol = Protocols.DefaultProtocol;
            factory.HostName = "localhost";
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            var connection = factory.CreateConnection();
            IModel model = connection.CreateModel();
            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.ContentType = "application/json";
            basicProperties.Type = "Event";
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "hele",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            Event e2 = new Event();
            EventName n1 = EventName.Alarm;
            e2.EventNamee = (int)n1;
            e2.EventDescription = "L01D001 Activated";
            

            for (int i = 1; i <= 3; i++)
            {
                e2.Timestamp = "12:30AM";
                e2.NodeAddress = "N" + i;
                string jsonified = JsonConvert.SerializeObject(e2);
                byte[] body = Encoding.UTF8.GetBytes(jsonified);

                channel.BasicPublish(exchange: "",
                 routingKey: "hele",
                 basicProperties: null,
                 body: body);
                Console.WriteLine(" [x] Sent {0}", jsonified);
            }
            
            
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        public class Event
        {
            public int EventNamee { get; set; }
            public string EventDescription { get; set; }
            public string Timestamp { get; set; }
            public string NodeAddress { get; set; }
        }
    }
}
