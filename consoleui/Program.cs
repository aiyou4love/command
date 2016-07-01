using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using command;

namespace consoleui
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandMgr.runCommand(args);
        }
    }
}
