using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Chat
    {

        private static Dictionary<string,int> users = new Dictionary<string,int>(); 
        private static Dictionary<int, string> messages = new Dictionary<int, string>();

        public static string Messenger(string message) 
        {
            string option = message.Split(' ')[1];

            switch (option)
            {
                case "send":
                    return Send(message.Split(' ')[2], message.Split(' ')[3], message.Split(' ')[4]);
                case "get":
                    return Receive(message.Split(' ')[2]);
                case "who":
                    return PrintUsers();
                default:
                    return "Wrong message format!";
                   
            }
        }

        static string PrintUsers() 
        {
            string usersList = "Avaivable users:\n";

            foreach (var user in users.Keys)
                usersList += user + "\n";

            return usersList;
        
        }

        static string Send(string myName, string recipentName,string message) 
        {
            Random r = new Random();
            var names = users.Keys.ToList();
            if(!names.Contains(myName))
            {
                users.Add(myName,r.Next());
            }
            if (!names.Contains(recipentName))
            {
                users.Add(recipentName, r.Next());
            }
            messages.Add(users[recipentName], "Message from: "+myName +" Text: " +message);
            return "You send a message!";
        }

        static string Receive(string myName) 
        {

            if (!messages.Any(x => x.Key == users[myName]))
            {
                return "Not messages for user " + myName;
            }
            var message = messages.First(x => x.Key == users[myName]);
            string toReturn = message.Value;
            messages.Remove(users[myName]);
            return toReturn;
        }
    }
}
