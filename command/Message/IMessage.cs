using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command
{
    public interface IMessage
    {
        void runError(string nValue);
        void runWarn(string nValue);
        void runInfo(string nValue);
    }
}
