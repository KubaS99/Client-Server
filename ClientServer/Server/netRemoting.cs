using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels;
using Services;

namespace Server
{
    class netRemotingListner : IListner
    {

        private int port;
        TcpChannel channel;

        public netRemotingListner(int port) 
        {
            this.port = port;
        }


        public void Start(CommunicatorD onConnect)
        {
            channel = new TcpChannel(port);
            onConnect(new netRemotingCommunicator(channel));
        }

        public void Stop()
        {
            Console.WriteLine("[.net remote] Stopped listener");
        }
    }

    class netRemotingCommunicator : ICommunicator
    {

        private TcpChannel channel;
        CommunicatorD onDisconnect;
        public netRemotingCommunicator(TcpChannel channel)
        {
            this.channel = channel;
        }

        public void Run(CommandD onCommand, CommunicatorD onDisconnect)
        {
            try
            {
                ChannelServices.RegisterChannel(channel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(Remote), "myRemote", WellKnownObjectMode.Singleton);
                Remote remoteObject = new Remote(new Remote.CommandD(onCommand));
                RemotingServices.Marshal(remoteObject, "myRemote");

            }
            catch (Exception e)
            {
                onDisconnect(this);
                Console.WriteLine(e.Message);
            }
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onDisconnect = onDisconnect;
            Task.Run(() => Run(onCommand, onDisconnect));
        }

        public void Stop()
        {
            onDisconnect(this);
            ChannelServices.UnregisterChannel(channel);
            Console.WriteLine("[.net remote] Stopped communicator");
        }
    }

}
