using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Server.ServerCommands
{
    class ConfService : IServiceModule
    {
        private Server server;

        public ConfService(Server server)
        {
            this.server = server;
        }

        public string MainConf(string command)
        {
            Console.WriteLine(command);
            switch (command.Split(' ')[1])
            {
                case "addservice":
                    return AddServiceModule(command);
                case "removeservice":
                    return RemoveServiceModule(command);
                case "addlistener":
                    return AddListener(command);
                case "showmedia":
                    return ShowMedia();
                case "showservices":
                    return ShowServices();
                default:
                    return "Incorrect option";
            }
        }

        public string AddServiceModule(string command)
        {
            string service = command.Split(' ')[2];
            switch (service)
            {
                case "ping":
                    Console.WriteLine("Service" + service + " added");
                    server.AddServiceModule("ping", new PingService());
                    return "Service " + service + " added";
                case "chat":
                    Console.WriteLine("Service" + service + " added");
                    server.AddServiceModule("chat", new ChatService());
                    return "Service " + service + " added";
                case "file":
                    Console.WriteLine("Service" + service + " added");
                    server.AddServiceModule("file", new FileTransferService());
                    return "Service " + service + " added";
                default:
                    Console.WriteLine("Incorrect service");
                    return "Incorrect service";
            }
        }
        public string RemoveServiceModule(string command)
        {
            string service = command.Split(' ')[2];
            switch (service)
            {
                case "ping":
                    Console.WriteLine("Service" + service + " removed");
                    server.RemoveServiceModule("ping");
                    return "Service " + service + " removed";
                case "chat":
                    Console.WriteLine("Service" + service + " removed");
                    server.RemoveServiceModule("chat");
                    return "Service " + service + " removed";
                case "file":
                    Console.WriteLine("Service" + service + " removed");
                    server.RemoveServiceModule("file");
                    return "Service " + service + " removed";
                default:
                    Console.WriteLine("Incorrect service");
                    return "Incorrect service";
            }
        }
        public string AddListener(string command)
        {
            string[] parameters = command.Split(' ');
            if (parameters[2] == "tcplistener")
            {
                server.AddListner(new TCPListener(parameters[3], Convert.ToInt32(parameters[4])));

                return "Added tcplistener";
            }
            else if (parameters[2] == "udplistener")
            {
                server.AddListner(new UDPListner(Convert.ToInt32(parameters[3])));
                return "Added udplistener";
            }
            else if (parameters[2] == "rs232listener")
            {
                server.AddListner(new RS232Listner(parameters[3], Convert.ToInt32(parameters[4]), (Parity)Enum.Parse(typeof(Parity), parameters[5]), Convert.ToInt32(parameters[6]), (StopBits)Enum.Parse(typeof(StopBits), parameters[7])));
                return "Added rs232Listener";
            }
            else if (parameters[2] == "filelistener")
            {
                server.AddListner(new FileListner(parameters[3]));
                return "Added fileListener";
            }
            else if (parameters[2] == "remotelistener")
            {
                server.AddListner(new netRemotingListner(Convert.ToInt32(parameters[3])));
                return "Added remotelistener";
            }
            else
            {
                return "Incorrect listner!";
            }
        }

        public string ShowMedia()
        {
            string info = null;

            if (server.listeners != null)
            {
                info += "\n" + "Number of listeners:" + server.listeners.Count.ToString() + "\n";
                foreach (var listner in server.listeners)
                    info += listner.ToString() + " ";
                return info + "\n";
            }
            return "";
        }

        public string ShowServices()
        {
            string info = null;
            if (server.listeners != null)
            {
                info += "\n" + "Number of services:" + server.services.Count.ToString() + "\n";
                foreach (var service in server.services)
                    info += service.Key + " ";
                return info + "\n";
            }
            return "";
        }
    }
}
