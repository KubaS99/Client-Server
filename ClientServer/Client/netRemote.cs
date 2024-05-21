using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using Services;

namespace Client.Protocols
{
    class netRemote
    {
        public static void StartnetRemote() 
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
                    Stopwatch watch = new Stopwatch();

                    ChannelServices.RegisterChannel(new TcpChannel(), false);

                    watch.Start();
                    var obj = (Remote)Activator.GetObject(typeof(Remote), "tcp://localhost:7524/myRemote");

                    Console.WriteLine(obj.Answer(service));
                    if (service.Split(' ')[0] == "ping")
                        Console.WriteLine("Time elapsed: " + watch.Elapsed.ToString());
                    var channels = ChannelServices.RegisteredChannels;
                    foreach(var channel in channels)
                    {
                        ChannelServices.UnregisterChannel(channel);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
