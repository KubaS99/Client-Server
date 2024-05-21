using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


namespace Server
{
    class RS232Listner : IListner
    {
        private string portName;
        private int baudRate;
        private int dataBits;
        private Parity parityBits;
        private StopBits stopBits;

        public RS232Listner(string portName, int baudRate, Parity parityBits, int dataBits, StopBits stopBits)
        {
            this.portName = portName;
            this.baudRate = baudRate;
            this.parityBits = parityBits;
            this.dataBits = dataBits;
            this.stopBits = stopBits;
        }

        public void Start(CommunicatorD onConnect)
        {
            SerialPort client = new SerialPort(portName, baudRate, parityBits, dataBits, stopBits);
            onConnect(new RS232Communicator(client));
        }

        public void Stop()
        {
            Console.WriteLine("[RS-232] Stopped listener");
        }
    }

    class RS232Communicator : ICommunicator
    {
        private SerialPort port;
        private CommunicatorD onDisconnect;
        public RS232Communicator(SerialPort port)
        {
            this.port = port;
        }


        public void Run(CommandD onCommand)
        {
            Console.WriteLine("[RS232] Communicator Running");
            try
            {
                port.Open();
                while (true)
                {
                    string message = port.ReadLine();
                    Console.WriteLine("[RS-232] Download: " + message);
                    string command = onCommand(message);
                    port.NewLine = "\r\n";
                    port.WriteLine(command);

                    Console.WriteLine("[RS-232] Send: " + command);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Stop();
            }

        }
        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onDisconnect = onDisconnect;
            Task.Run(() => Run(onCommand));
        }

        public void Stop()
        {

            port.Close();
            onDisconnect(this);
            Console.WriteLine("[RS-232] Stopped communicator");
        }
    }
}
