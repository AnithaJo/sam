using System.ServiceProcess;
using Consumer;

namespace RabbitMQConsumerService
{
    public partial class RabbitMQConsumerService : ServiceBase
    {
        public RabbitMQConsumerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Receiver receiver = new Receiver();
            receiver.ReceiveEventData();
        }        

        protected override void OnStop()
        {
            
        }
    }
}
