using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services
{
    public class Ping
    {
        public static string PingSend(string service)
        {
            var args = service.Split(' ');
            if(args.Length == 1)
            {
                return "ping 1 10";
            }
            if (args.Length <= 2)
            {
                return "ping " + args[1] +" 10";
            }
            else if (args.Length == 3)
            {
                return "ping " + args[1] + " " + args[2];
            }
            else
            {
                Console.WriteLine("Wrong command format");
                return null;
            }
        }

        public static string Pong(string service)
        {
            string[] tab = service.Split(' ');

            return "pong: "+ PingText(Convert.ToInt32(tab[2]));
        }

        public static string PingText(int n)
        {
            Random r = new Random();
            char[] tab = new char[n];
            for (int i = 0; i < n; i++)
                tab[i] = (char)r.Next(33, 125);
            string toReturn = new string(tab);
            return toReturn;
        }
    }
}
