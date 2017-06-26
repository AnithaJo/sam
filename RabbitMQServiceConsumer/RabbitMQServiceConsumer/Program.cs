using System.ServiceProcess;

namespace RabbitMQServiceConsumer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new RabbitMQConsumerService.RabbitMQConsumerService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
