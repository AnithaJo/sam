using System.ServiceProcess;

namespace RabbitMQConsumerService
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
                new RabbitMQConsumerService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
