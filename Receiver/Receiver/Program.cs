using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Http;

namespace Receiver
{
    class Receiver
    {
        //static HttpClient client = new HttpClient();

        public static void Main()
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
           
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                channel.QueueBind(queue: "test",
                                  exchange: "check",
                                  routingKey: "");



                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);


                    Console.WriteLine(" [x] Done");

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: "queue7",
                                    noAck: false,
                                    consumer: consumer);
                channel.BasicConsume(queue: "test",
                                      noAck: false,
                                      consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }
        }
    }
}
