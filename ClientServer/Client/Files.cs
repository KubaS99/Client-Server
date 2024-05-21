using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Protocols
{
    class Files
    {

        private static bool isPing;
        private static string path;
        private static Stopwatch watch;
        private static bool executing;


        public static void StartFile()
        {
            string service = Client.GetService();
            int loopVal = 1;
            isPing = false;
            if (service.Split(' ')[0] == "ping")
            {
                loopVal = Convert.ToInt32(service.Split(' ')[1]);
                isPing = true;
            }
            for (int i = 0; i < loopVal; i++)
            {
                Thread.Sleep(1000);
                watch = new Stopwatch();
                path = @"C:\Users\Kuba\source\repos\Client-Server\Tests\Communicates";
                FileSystemWatcher watcher = new FileSystemWatcher(path);

                watcher.Changed += ClientWatcherChanged;
                watcher.Filter = "*.out";
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                watcher.EnableRaisingEvents = true;
                watcher.IncludeSubdirectories = true;

                try
                {
                    watch.Start();
                    executing = true;
                    using (StreamWriter sw = new StreamWriter(path + "\\service.in"))
                    {
                        sw.WriteLine(service);
                    }
                    while (executing)
                    {

                    }
                    
                    
                    System.IO.File.Move(path+"\\service.in", path+"\\Archive\\"+DateTime.Now.ToString().Replace('.', ' ').Replace(':', ' ') + ".in");
                    System.IO.File.Move(path + "\\service.out", path + "\\Archive\\" + DateTime.Now.ToString().Replace('.', ' ').Replace(':', ' ') + ".out");

                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.Message);
                }
            }
        }

        private static void ClientWatcherChanged(object sender, FileSystemEventArgs e)
        {
            watch.Stop();
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(path + "\\service.out"))
                    {
                        Console.Write("Response: ");
                        string line;
                        while ((line = sr.ReadLine()) != null)
                            Console.WriteLine(line);

                        if (isPing)
                        {
                            Console.WriteLine("Time elapsed: " + watch.Elapsed.ToString());
                        }
                        executing = false;
                        sr.Close();
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    executing = false;
                }
            }
        }
    }
}
