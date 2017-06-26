using System.ServiceProcess;
using Producer;
using System.Timers;

namespace RabbitMQProducerService
{
    public partial class RabbitMQProducerService : ServiceBase
    {
        private Timer timer = null;

        public RabbitMQProducerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Interval = 2000;
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, ElapsedEventArgs e)
        {
            Sender rabbitMQsender = new Sender();
            rabbitMQsender.SendEventData();
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }
    }
}
