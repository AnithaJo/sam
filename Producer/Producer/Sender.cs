using System;
using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace Producer
{
    public class Sender
    {
        private ConnectionFactory _connFactory;
        private IModel _channel;
        private IConnection _connection;
        private IBasicProperties _basicProperties;

        public Sender()
        {
            _connFactory = new ConnectionFactory();
            Initialize();
        }

        private void Initialize()
        {
            _connFactory.UserName = "RabbitMQServerAdmin";
            _connFactory.Password = "Password123";
            _connFactory.VirtualHost = "/";
            _connFactory.Protocol = Protocols.DefaultProtocol;
            _connFactory.HostName = "localhost";
            _connFactory.Port = AmqpTcpEndpoint.UseDefaultPort;           
        }

        public void SendEventData()
        {        
            Event evt = GetDummyEvent();
            string jsonified = JsonConvert.SerializeObject(evt);
            byte[] body = Encoding.UTF8.GetBytes(jsonified);

            //if(!_channel.IsOpen)

            using (_connection = _connFactory.CreateConnection())
            {
                using (_channel = _connection.CreateModel())
                {
                    _basicProperties = _channel.CreateBasicProperties();
                    _basicProperties.ContentType = "application/json";
                    _basicProperties.Type = "Event";

                    _channel.QueueDeclare(queue: "PanelEventsQueue",
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                    _channel.BasicPublish(exchange: "",
                                         routingKey: "PanelEventsQueue",
                                         basicProperties: _basicProperties,
                                         body: body);
                }
            }
        }

        private Event GetDummyEvent()
        {
            Event evt = new Event();
            EventType evtType = EventType.Alarm;
            evt.EventTyp = (int)evtType;
            evt.EventDescription = "L01D001 Activated";
            evt.Timestamp = DateTime.Now.ToString();
            Random random = new Random();            
            evt.NodeAddress = "N" + random.Next(1, 100);
            return evt;
        }

        public class Event
        {
            public string NodeAddress { get; set; }
            public int EventTyp { get; set; }
            public string EventDescription { get; set; }
            public string Timestamp { get; set; }            
        }
        
        enum EventType
        {
            Alarm = 1,
            PreAlarm = 2,
            Fire = 3,
            Security = 4
        };
    }

}
