﻿using FinalSprint.MarioClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSprint
{   // Command Class for the Avatar to Super state
    public class MoveSuperCommand : ICommand
    {
        MarioCharacter mario;
        public MoveSuperCommand(MarioCharacter mario)
        {
            this.mario = mario;
        }

        public void Execute()
        {   // take the receiver method of power state change
            mario.MoveSuper();
        }
    }
}
