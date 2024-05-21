using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Protocols
{
    class TCP
    {
        public static void StartTCP()
        {
            try
            {
                string service = Client.GetService();
                int loopVal = 1;
                if (service.Split(' ')[0] == "ping")
                {
                    loopVal = Convert.ToInt32(service.Split(' ')[1]);
                }
                for (int i = 0; i < loopVal; i++)
                {
                    string host = "localhost";
                    TcpClient client = new TcpClient(host, 12345);
                    NetworkStream stream = client.GetStream();

                    Stopwatch watch = new Stopwatch();






                    byte[] data = System.Text.Encoding.ASCII.GetBytes(service);


                    watch.Start();
                    stream.Write(data, 0, data.Length);

                    data = new byte[256];

                    string response = string.Empty;

                    int bytes;

                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        watch.Stop();
                        response += System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    }
                    while (stream.DataAvailable);

                    Console.WriteLine("Response: \n" + response);
                    if (loopVal > 1)
                    {
                        Console.WriteLine("Time elapsed: " + watch.Elapsed);
                    }
                    stream.Close();
                    client.Close();
                }      
            }
            catch (Exception e)

            {
                Console.WriteLine(e.Message);
            }


        }

    }
}
