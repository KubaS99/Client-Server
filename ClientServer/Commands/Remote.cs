using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Remote : MarshalByRefObject
    {
        public  delegate  string CommandD(string command);
        public  CommandD onCommand;
        public Remote( CommandD  onCommand) => this.onCommand = onCommand;

        public string Answer(string command)
        {
            string answer = onCommand(command);
            return answer;
        }
    }   
}
