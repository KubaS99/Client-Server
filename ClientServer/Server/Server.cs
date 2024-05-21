using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.ServerCommands;


namespace Server
{
    class Server
    {

        List<ICommunicator> communicators = new List<ICommunicator>();

        public List<IListner> listeners = new List<IListner>();

        public Dictionary<string, IServiceModule> services = new Dictionary<string, IServiceModule>();

        private object lockListener = new object();
        private object lockComunnicator = new object();


        public void AddServiceModule(string name, IServiceModule service) 
        {
            services.Add(name, service);
        }

        public void AddCommunicator(ICommunicator communicator)
        {
            lock (lockComunnicator)
            {
                communicators.Add(communicator);
                communicator.Start(new CommandD(Answer), RemoveCommunicator);
            }
        }

        public void AddListner(IListner listner)
        {
            listeners.Add(listner);
            lock (lockListener)
            {
                Task.Run(()=>listner.Start(new CommunicatorD(AddCommunicator)));
            }
        }

        public void RemoveCommunicator(ICommunicator communicator)
        {
            lock (lockComunnicator)
            {
                communicators.Remove(communicator);
            }
        }

        public void RemoveServiceModule(string name)
        {
            services.Remove(name);
        }
        public string Answer(string service)
        {
            if (service != null)
            {
                if (services.ContainsKey(service.Split(' ')[0]))
                {
                    return services[service.Split(' ')[0]].MainConf(service);
                }
                return "Error";
            }
            return "";
        }
            
        public void Start() 
        {
            foreach (var listener in listeners) 
                listener.Start(new CommunicatorD(AddCommunicator));
        }
        public void Stop()
        {
            foreach (var listener in listeners)
                listener.Stop();

            foreach (var communicator in communicators)
                communicator.Stop();

            listeners.Clear();
            communicators.Clear();
            services.Clear();
        }
        void Delay() 
        {
            while (communicators.Count != 0) 
                Task.Delay(1000);
            while (services.Count != 0)
                Task.Delay(1000);
        }

        static void Main() 
        {
            var server = new Server();

            server.AddServiceModule("ping", new PingService());
            server.AddServiceModule("chat", new ChatService());
            server.AddServiceModule("conf", new ConfService(server));
            server.AddServiceModule("file", new FileTransferService());


            server.Start();

            server.AddListner(new TCPListener("127.0.0.1",12345));
            server.AddListner(new UDPListner(11021));
            server.AddListner(new RS232Listner("COM1", 9600, Parity.None, 8, StopBits.One));
            server.AddListner(new netRemotingListner(7524));
            server.AddListner(new FileListner(@"C:\Users\Kuba\source\repos\Client-Server\Tests\Communicates"));

            server.Delay();
            server.Stop();

        }
    }
}
