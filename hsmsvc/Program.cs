using System;

namespace HSMSvc
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int port = 6666;
                HSMService service = new HSMService(port);
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