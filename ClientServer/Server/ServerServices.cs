using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ChatService : IServiceModule
    {
        public string MainConf(string command) => Chat.Messenger(command);
    }
    class FileTransferService : IServiceModule
    {
        public string MainConf(string command) => FileTransfer.FileCommands(command);
    }
    class PingService : IServiceModule
    {
        public string MainConf(string command) => Ping.Pong(command);
    }
}
