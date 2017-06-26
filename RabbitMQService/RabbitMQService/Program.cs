using System.ServiceProcess;

namespace RabbitMQProducerService
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
                new RabbitMQProducerService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
