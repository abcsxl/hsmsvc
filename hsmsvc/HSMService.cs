using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace hsmsvc
{
    public class HSMService
    {
        private static int BUFF_MAX_SIZE = 256;

        private IPAddress ipAddress;
        private int port;
        public HSMService(int port)
        {
            this.port = port;
            //string hostName = Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
            //this.ipAddress = null;
            //for (int i = 0; i < ipHostInfo.AddressList.Length; ++i)
            //{
            //    if (ipHostInfo.AddressList[i].AddressFamily ==
            //    AddressFamily.InterNetwork)
            //    {
            //        this.ipAddress = ipHostInfo.AddressList[i];
            //        break;
            //    }
            //}
            this.ipAddress = IPAddress.Parse("127.0.0.1");

            if (this.ipAddress == null)
                throw new Exception("No IPv4 address for server");
        }
        public async void Run()
        {
            TcpListener listener = new TcpListener(this.ipAddress, this.port);
            listener.Start();
            Console.Write("Array Min and Avg service is now running");

            Console.WriteLine($" on port {this.port}");
            Console.WriteLine("Hit <enter> to stop service\n");
            while (true)
            {
                try
                {
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    Task t = Process(tcpClient);
                    //await t;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task Process(TcpClient tcpClient)
        {
 //           string clientEndPoint =
 //tcpClient.Client.RemoteEndPoint.ToString();
 //           Console.WriteLine("Received connection request from "
 //           + clientEndPoint);
 //           try
 //           {
 //               NetworkStream networkStream = tcpClient.GetStream();
 //               StreamReader reader = new StreamReader(networkStream);
 //               StreamWriter writer = new StreamWriter(networkStream);
 //               writer.AutoFlush = true;
 //               while (true)
 //               {
 //                   string request = await reader.ReadLineAsync();
 //                   if (request != null)
 //                   {
 //                       Console.WriteLine("Received service request: " + request);
 //                       string response = Response(request);
 //                       Console.WriteLine("Computed response is: " + response + "\n");
 //                       await writer.WriteLineAsync(response);
 //                   }
 //                   else
 //                       break; // Client closed connection
 //               }
 //               tcpClient.Close();
 //           }
 //           catch (Exception ex)
 //           {
 //               Console.WriteLine(ex.Message);
 //               if (tcpClient.Connected)
 //                   tcpClient.Close();
 //           }


            Console.WriteLine($"Received connection request from {tcpClient.Client.RemoteEndPoint.ToString()}");

            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                while (true)
                {
                    await networkStream.FlushAsync();

                    byte[] buff = new byte[BUFF_MAX_SIZE];
                    int requestlen = await networkStream.ReadAsync(buff, 0, buff.Length);
                    if (requestlen > 0)
                    {   
                        Console.WriteLine($"Received service request: {buff.ByteArrayToHex(requestlen)}");

                        byte[] response = Process(buff.SubBytes(0,requestlen));
                        Console.WriteLine($"Computed response is: {response.ByteArrayToHex()}");

                        await networkStream.WriteAsync(response, 0, response.Length);
                    }
                    else
                        break; // Client closed connection
                }
                tcpClient.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (tcpClient.Connected)
                    tcpClient.Dispose();
            }
        }
        private static byte[] Process(byte[] request)
        {
            //string[] pairs = request.Split('&');
            //string methodName = pairs[0].Split('=')[1];
            //string valueString = pairs[1].Split('=')[1];
            //string[] values = valueString.Split(' ');
            //double[] vals = new double[values.Length];
            //for (int i = 0; i < values.Length; ++i)
            //    vals[i] = double.Parse(values[i]);
            //string response = "";
            //if (methodName == "average") response += Average(vals);
            //else if (methodName == "minimum") response += Minimum(vals);
            //else response += "BAD methodName: " + methodName;
            //int delay = ((int)vals[0]) * 1000; // Dummy delay
            //System.Threading.Thread.Sleep(delay);

            int responselen = request.Length;
            byte[] response = new byte[responselen];

            Array.Copy(request, response, responselen);

            //System.Threading.Thread.Sleep(200);
            
            return response;
        }
    }
}