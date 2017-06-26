using System.ServiceProcess;
using Consumer;
using System.Threading;
using System.Timers;
using System;

namespace RabbitMQConsumerService
{
    public partial class RabbitMQConsumerService : ServiceBase
    {
        //private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        //private Thread _thread;
       // private bool _isRunning;
        private System.Timers.Timer _timer;

        public RabbitMQConsumerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 2000;
            _timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            _timer.Enabled = true;

            //Receiver receiver = new Receiver();
            //receiver.ReceiveEventData();
            //_isRunning = true;
            //while(_isRunning)
            //{
            //    Thread.Sleep(2000);
            //}
        }

        private void timer_Tick(object sender, ElapsedEventArgs e)
        {
            Receiver receiver = new Receiver();
            receiver.ReceiveEventData();
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;
            //_isRunning = false;
        }
    }
}
