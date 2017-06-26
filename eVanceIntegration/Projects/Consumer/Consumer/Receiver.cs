using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace Consumer
{
    public class Receiver
    {

        private ConnectionFactory _connFactory;
        private IModel _channel;
        private IConnection _connection;

        public Receiver()
        {            
            Initialize();
        }

        private void Initialize()
        {
            _connFactory = new ConnectionFactory();
            _connection = _connFactory.CreateConnection();
            _connFactory.UserName = "RabbitMQServerAdmin";
            _connFactory.Password = "Password123";
            _connFactory.VirtualHost = "/";
            _connFactory.Protocol = Protocols.DefaultProtocol;
            _connFactory.HostName = "localhost";
            _connFactory.Port = AmqpTcpEndpoint.UseDefaultPort;

            //System.Net.Http.HttpClient cons = new System.Net.Http.HttpClient();
            //cons.BaseAddress = new Uri("http://localhost:7967/");
            //cons.DefaultRequestHeaders.Accept.Clear();
            //cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //MyAPIGet(cons).Wait();
        }

        //static async System.Threading.Tasks.Task MyAPIGet(System.Net.Http.HttpClient cons)
        //{
        //    using (cons)
        //    {
        //        System.Net.Http.HttpResponseMessage res = await cons.GetAsync("api/tblTags");
        //        res.EnsureSuccessStatusCode();
        //        if (res.IsSuccessStatusCode)
        //        {
        //            //tblTag tag = await res.Content.ReadAsAsync<tblTag>();
        //            //Console.WriteLine("\n");
        //            //Console.WriteLine("---------------------Calling Get Operation------------------------");
        //            //Console.WriteLine("\n");
        //            //Console.WriteLine("tagId    tagName          tagDescription");
        //            //Console.WriteLine("-----------------------------------------------------------");
        //            //Console.WriteLine("{0}\t{1}\t\t{2}", tag.tagId, tag.tagName, tag.tagDescription);
        //            //Console.ReadLine();
        //        }
        //    }
        //}

        public void ReceiveEventData()
        {            
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "PanelEventsQueue",
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 5, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: "PanelEventsQueue",
                                  noAck: true,
                                  consumer: consumer);

            consumer.Received += Consumer_Received;
        }

        private void Consumer_Received(object model, BasicDeliverEventArgs ea)
        {
            byte[] body = ea.Body;
            string jsonified = Encoding.UTF8.GetString(body);
            Event evt = JsonConvert.DeserializeObject<Event>(jsonified);

            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: true);

            localhost.RabbitMQWebService webService = new localhost.RabbitMQWebService();

            webService.ReceiveEvent(jsonified);
        }
    }

    public class Event
    {
        public string NodeAddress { get; set; }
        public int EventTyp { get; set; }
        public string EventDescription { get; set; }
        public string Timestamp { get; set; }
    }
}




