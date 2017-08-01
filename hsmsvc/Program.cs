using Microsoft.Extensions.Configuration.CommandLine;
using System;

namespace HSMSvc
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string ip = "127.0.0.1";
                int port = 6666;

                var cmdLineConfig = new CommandLineConfigurationProvider(args);
                cmdLineConfig.Load();

                string strIP = null;
                string strPort = null;

                cmdLineConfig.TryGet("ip", out strIP);
                cmdLineConfig.TryGet("port", out strPort);

                if (!String.IsNullOrWhiteSpace(strIP))
                    ip = strIP;

                if (!String.IsNullOrWhiteSpace(strPort))
                {
                    if (!int.TryParse(strPort, out port))
                        port = 6666;
                }

                HSMService service = new HSMService(ip, port);
                service.Run();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}