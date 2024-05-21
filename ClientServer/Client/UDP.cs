using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Protocols
{
    class UDP
    {
        public static void StartUDP()
        {
            string service = Client.GetService();
            int loopVal = 1;
            if (service.Split(' ')[0] == "ping")
            {
                loopVal = Convert.ToInt32(service.Split(' ')[1]);
            }
            for (int i = 0; i < loopVal; i++)
            {
                int clientPort = 11005;
                int serverPort = 11021;
                string server = "localhost";
                UdpClient udpClient = new UdpClient(clientPort);
                Stopwatch watch = new Stopwatch();


                try
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(service);

                    udpClient.Connect(server, serverPort);
                    watch.Start();
                    udpClient.Send(sendBytes, sendBytes.Length);

                    IPEndPoint RemoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receiveBytes = udpClient.Receive(ref RemoteIPEndPoint);
                    watch.Stop();
                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    Console.WriteLine("Response: \n" + returnData);
                    if (service.Split(' ')[0] == "ping")
                    {
                        Console.WriteLine("Time elapsed: " + watch.Elapsed.ToString());
                    }
                    Console.WriteLine();

                    udpClient.Close();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
