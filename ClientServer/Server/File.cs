using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FileListner : IListner
    {
        FileSystemWatcher watcher;
        public string path;
        CommunicatorD onConnect;

        public FileListner(string path)
        {
            this.path = path;
            watcher = new FileSystemWatcher(path);
        }

        public void Start(CommunicatorD onConnect)
        {
            this.onConnect = onConnect;
            watcher.Changed += ServerWatcherChanged;
            watcher.Filter = "*.in";
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;
        }

        private void ServerWatcherChanged(object sender, FileSystemEventArgs e)
        {

            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                onConnect(new FileCommunicator(e.FullPath));
            }
        }

        public void Stop()
        {
            Console.WriteLine("[File] Stopped listener");
        }
    }
    class FileCommunicator : ICommunicator
    {
        readonly string path;
        CommunicatorD onDisconnect;
        public FileCommunicator(string path)
        {
            this.path = path;
        }

        public void Listener(CommandD onCommand) 
        {
            try
            {
                string data;
                using (StreamReader sr = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                 

                        string writeFilePath = path.Replace(".in", ".out");                
                        using (StreamWriter sw = new StreamWriter(writeFilePath))
                        {
                            data = sr.ReadLine();
                            string message = onCommand(data);
                            sw.WriteLine(message);
                        }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onDisconnect = onDisconnect;
            Task.Run(() => Listener(onCommand));
        }

        public void Stop()
        {
            onDisconnect(this);
            Console.WriteLine("[File] Stopped communicator");
        }
    }
}
