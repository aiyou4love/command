﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command
{
    interface ICommand
    {
        void runCommand(string[] nValue);
    }
}
