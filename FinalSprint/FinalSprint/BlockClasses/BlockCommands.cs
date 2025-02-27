﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.BlockClasses
{
    class BlockCommands : ICommand
    {
        private Blocks brick;
        
        public BlockCommands(Blocks brick)
        {
            this.brick = brick;
        }

        public virtual void Execute()
        {
            this.brick.currentbState.Handle(this.brick);
        }
    }
}
