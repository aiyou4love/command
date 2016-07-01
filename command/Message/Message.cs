using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command
{
    public static class Message
    {
        public static void runError(string nValue)
        {

        }

        public static void runWarn(string nValue)
        {

        }

        public static void runInfo(string nValue)
        {

        }

        public static void runInit(IMessage nMessage)
        {
            mMessage = nMessage;
        }

        static IMessage mMessage;
    }
}
