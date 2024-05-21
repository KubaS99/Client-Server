using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class UDPListner : IListner
    {
        private int port;
        private UdpClient client;


        public UDPListner(int port)
        {
            this.port = port;
        }
        public void Run(CommunicatorD onConnect)
        {
            try
            {
                client = new UdpClient(port);
                onConnect(new UDPCommunicator(client));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Start(CommunicatorD onConnect)
        {
            Task.Run(() => Run(onConnect));
        }


        public void Stop()
        {
            client.Close();
            Console.WriteLine("[UDP] Stopped listener");
        }
    }

    class UDPCommunicator : ICommunicator
    {
        private UdpClient client;
        private CommunicatorD onDisconnect;


        public UDPCommunicator(UdpClient client)
        {
            this.client = client;
        }

        public void Run(CommandD onCommand, CommunicatorD onDisconnect)
        {
            Console.WriteLine("[UDP] Communicator Running");
            try
            {
                while (true)
                {
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

                    byte[] bytes = client.Receive(ref ipEndPoint);
                    string receivedData = Encoding.ASCII.GetString(bytes);

                    Console.WriteLine("[UDP] Received: " + receivedData + " from " + ipEndPoint.Address + " on port " + ipEndPoint.Port);

                    string received = onCommand(receivedData);

                    byte[] sendData = Encoding.ASCII.GetBytes(received);

                    client.Send(sendData, sendData.Length, ipEndPoint);

                    Console.WriteLine("[UDP] Sent: " + received + " to " + ipEndPoint.Address + " port: " + ipEndPoint.Port);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                onDisconnect(this);
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
            Console.WriteLine("[UDP] Stopped communicator");
        }
    }

}
