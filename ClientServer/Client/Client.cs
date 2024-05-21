using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Client.Protocols;
using Services;

namespace Client
{
    class Client
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\nChoose communication media");
                Console.WriteLine("1. Files");
                Console.WriteLine("2. TCP");
                Console.WriteLine("3. UDP");
                Console.WriteLine("4. RS-232");
                Console.WriteLine("5. .Net Remoting");
                Console.WriteLine("6. Exit");

                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        Files.StartFile();
                        break;

                    case "2":
                        TCP.StartTCP();
                        break;
                    case "3":
                        UDP.StartUDP();
                        break;
                    case "4":
                        RS232.StartRS232();
                        break;
                    case "5":
                        netRemote.StartnetRemote();
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                if (option == "6")
                    break;
            }
            Console.WriteLine("Quitting...");
        }
        public static string GetService()
        {
            Console.WriteLine("Choose service:");
            string service = Console.ReadLine();
            while (true)
            {
                string option = service.Split(' ')[0];
                switch (option)
                {
                    case "ping":
                        return Ping.PingSend(service);                       
                    case "chat":
                        return service;
                    case "conf":
                        return service;
                    case "help":
                        PrintHelp();
                        Console.WriteLine("Choose service");
                        service = Console.ReadLine();
                        break;
                    case "file":
                        return service;
                    default:
                        Console.WriteLine("Incorrect service!\n");
                        Console.WriteLine("Choose servive:");
                        service = Console.ReadLine();
                        break;
                }
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("ping [number of packets] [packet size]");
            Console.WriteLine();
            Console.WriteLine("chat send [myId] [recipentId] [message]");
            Console.WriteLine("chat get [myId]");
            Console.WriteLine("chat who");
            Console.WriteLine();
            Console.WriteLine("file ls");
            Console.WriteLine("file send [filename] [data]");
            Console.WriteLine("file get [filename]");
            Console.WriteLine();
            Console.WriteLine("conf addservice [serviceName]");
            Console.WriteLine("conf removeservice [serviceName]");
            Console.WriteLine("conf addlistener [listenertype] params");
            Console.WriteLine("conf showmedia");
            Console.WriteLine("conf showservices");
        }
    }
}

