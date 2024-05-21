using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace Client.Protocols
{
    class RS232
    {
        public static void StartRS232()
        {
            string service = Client.GetService();
            int loopVal = 1;
            if (service.Split(' ')[0] == "ping")
            {
                loopVal = Convert.ToInt32(service.Split(' ')[1]);
            }
            for (int i = 0; i < loopVal; i++)
            {
                Stopwatch watch = new Stopwatch();
                SerialPort port = new SerialPort("COM2", 9600, Parity.None, 8, StopBits.One);

                try
                {
                    watch.Start();
                    port.Open();

                    port.WriteLine(service);
                    watch.Stop();
                    port.NewLine = "\r\n";
                    string returnMessage = port.ReadLine();

                    Console.WriteLine("Response: \n" + returnMessage);
                    if (service.Split(' ')[0] == "ping")
                    {
                        Console.WriteLine("Time elapsed: " + watch.Elapsed.ToString());
                    }
                    Console.WriteLine();

                    port.Close();
                    port.Dispose();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
