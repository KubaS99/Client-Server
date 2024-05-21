using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FileTransfer
    {
        public static string FileCommands(string service) 
        {
            switch (service.Split()[1]) 
            {
                case "get":
                    return GetFile(service);
                case "ls":
                    return GetFiles();  
                case "send":
                    return WriteFile(service);                               
                default:
                    return "Invalid option!";
            }
        }
        static string GetFile(string service)
        {
            byte[] fileBytes = File.ReadAllBytes(@"C:\Users\Kuba\source\repos\Client-Server\Tests\Files\" + service.Split(' ')[2]);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in fileBytes)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            File.WriteAllText(@"C:\Users\Kuba\source\repos\Client-Server\ClientServer\Client\test\" + service.Split(' ')[2], sb.ToString());
            return "File downloaded!";
        }
        static string GetFiles()
        {
            DirectoryInfo path = new DirectoryInfo(@"C:\Users\Kuba\source\repos\Client-Server\Tests\Files");
            if (path.Exists) { 
                
                FileInfo[] files = path.GetFiles();
                string list = "";

                foreach (FileInfo i in files)
                    list += i.Name + "\n";

                return list;
            }
            else
                return "Directory error!";     
        }
        
        static string WriteFile(string service)
        {
            byte[] fileBytes = File.ReadAllBytes(@"C:\Users\Kuba\source\repos\Client-Server\ClientServer\Client\test\"+service.Split(' ')[2]);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in fileBytes)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            File.WriteAllText(@"C:\Users\Kuba\source\repos\Client-Server\Tests\Files\"+service.Split(' ')[2], sb.ToString());
            return "File uploaded!";
        }
    }
}
