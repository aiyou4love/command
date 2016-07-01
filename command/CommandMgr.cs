using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command
{
    public static class CommandMgr
    {
        public static void runCommand(string[] nValue)
        {
            if ("-t" == nValue[0])
            {
                TableReader tableReader_ = new TableReader();
                tableReader_.runCommand(nValue);
            }
        }
    }
}
