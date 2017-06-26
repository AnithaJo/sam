using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace Sender
{
    class Sender
    {
        private static int i;
        public static void Main()
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            //factory.UserName = "agennt";
            //factory.Password = "agennt";

            //factory.Protocol = Protocols.DefaultProtocol;
            ////factory.HostName = "199.63.213.87";
            //factory.HostName = "10.77.8.19";
            //factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            using (IConnection connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                IDictionary<string, object> args = new Dictionary<string, object>();
                channel.ExchangeDeclare("check", "direct");
                args.Add("x-dead-letter-exchange", "check");
                args.Add("x-max-length", 111);
                var response = channel.QueueDeclare("test", true, false, false, args);
                uint cnt = response.MessageCount;

                if (cnt > 110)
                {
                    channel.QueueDeclare(queue: "queue7",
                                                durable: true,
                                                exclusive: false,
                                                autoDelete: false,
                                                arguments: null
                              );
                    var messageProperties = channel.CreateBasicProperties();
                    messageProperties.Persistent = true;
                    messageProperties.DeliveryMode = 2;
                    channel.ConfirmSelect();
                    channel.BasicAcks += AcknowledgementReceivedHandler;

                    for (i = 0; i < 150; i++)
                    {
                        string message = "Hello World tesing  queue 7 with rabbitmq server stops!" + i;
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "queue7",
                                             basicProperties: messageProperties,
                                             body: body);
                        channel.WaitForConfirmsOrDie();
                        Console.WriteLine(" [x] Sent {0}", message);

                    }
                }
                else
                {
                    var messageProperties = channel.CreateBasicProperties();
                    messageProperties.Persistent = true;
                    messageProperties.DeliveryMode = 2;
                    channel.ConfirmSelect();
                    channel.BasicAcks += AcknowledgementReceivedHandler;
                    for (i = 0; i < 120; i++)
                    {
                        string message = "Hello World testing queue test with rabbitmq server stops " + i;
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "test",
                                             basicProperties: messageProperties,
                                             body: body);
                        channel.WaitForConfirmsOrDie();
                        Console.WriteLine(" [x] Sent {0}", message);

                    }
                }

            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void AcknowledgementReceivedHandler(object sender, RabbitMQ.Client.Events.BasicAckEventArgs e)
        {
            Console.Write("ACK received");
        }
    }
}
