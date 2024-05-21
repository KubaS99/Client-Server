using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class TCPListener : IListner
    {
        int port;
        string ip;
        TcpListener tcpListener;


        public TCPListener(string ip,int port)
        {
            this.port = port;
            this.ip = ip;
        }

        public void Run(CommunicatorD onConnect)
        {

            tcpListener = new TcpListener(IPAddress.Parse(ip), port); 
            TcpClient client;

            tcpListener.Start();

            while (true)
            {

                client = tcpListener.AcceptTcpClient(); 
                onConnect(new TCPCommunicator(client));

                Console.WriteLine("[TCP] Connected with: " + client.Client.RemoteEndPoint); 
            }
        }

        public void Start(CommunicatorD onConnect)
        {
             Task.Run(()=>Run(onConnect));
        }
        public void Stop()
        {
            tcpListener.Stop();
            Console.WriteLine("[TCP] Stopped listener");
        }

    }

    class TCPCommunicator : ICommunicator
    {
        TcpClient client;
        CommunicatorD onDisconnect;
        public TCPCommunicator(TcpClient client)
        {
            this.client = client;
        }

        public void Run(CommandD onCommand)
        {
            Byte[] bytes = new Byte[256];
            string data = string.Empty;
            NetworkStream stream = client.GetStream();
            Console.WriteLine("[TCP] Communicator Running");

            try
            {
                while (client.Connected)
                {
                    if (stream.DataAvailable)
                    {
                        int len = stream.Read(bytes, 0, bytes.Length);
                        data += Encoding.ASCII.GetString(bytes, 0, len);
                    }
                    while (data != string.Empty)
                    {

                        string message = onCommand(data);


                        Byte[] msg = Encoding.ASCII.GetBytes(message);

                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("[TCP] Send: " + message);
                        data = string.Empty;

                    }
                    data = string.Empty;
                }
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onDisconnect = onDisconnect;
            Task.Run(() => Run(onCommand));
        }

        public void Stop()
        {
            client.Close();
            onDisconnect(this);
            Console.WriteLine("[TCP] Stopped communicator");
        }
    }

}
