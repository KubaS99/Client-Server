using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IServiceModule
    {
        string MainConf(string command);
    }
    public interface ICommunicator
    {
        void Start(CommandD onCommand, CommunicatorD onDisconnect);
        void Stop();
    }

    public interface IListner
    {
        void Start(CommunicatorD onConnect);
        void Stop();
    }
    public delegate string CommandD(string command);
    public delegate void CommunicatorD(ICommunicator communicator);
}
