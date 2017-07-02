using HSMSvc.Common;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HSMSvc
{
    public class HSMService
    {
        /// <summary>
        /// 报文最大字节数
        /// </summary>
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
 //           string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
 //           Console.WriteLine("Received connection request from " + clientEndPoint);
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
                    int requestlen = await networkStream.ReadAsync(buff, 0, 2);
                    if (requestlen > 0)
                    {
                        int buflen = buff[0] * 0x1000 + buff[1];
                        requestlen = await networkStream.ReadAsync(buff, 2, buflen);
                        if (requestlen > 0)
                        {
                            Console.WriteLine($"Received service request: {buff.ByteArrayToHex(requestlen+2)}");

                        byte[] response = Process(buff.SubBytes(0,requestlen+2));
                        Console.WriteLine($"Computed response is: {response.ByteArrayToHex()}");

                        await networkStream.WriteAsync(response, 0, response.Length);
                        }
                        else
                            break; // Client closed connection
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
            int responselen = request.Length;
            byte[] response = new byte[responselen];

            Array.Copy(request, response, responselen);

            //System.Threading.Thread.Sleep(200);
            
            return response;
        }
    }
}