﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSprint.BlockClasses
{
    interface IBlockStates
    {
        void Handle(Blocks brick);
    }
}
