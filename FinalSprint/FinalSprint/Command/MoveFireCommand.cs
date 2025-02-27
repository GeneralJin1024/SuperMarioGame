﻿using FinalSprint.MarioClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSprint
{   // Command Class for the Avatar to fire state
    public class MoveFireCommand : ICommand
    {
        MarioCharacter mario;
        public MoveFireCommand(MarioCharacter mario)
        {
            this.mario = mario;
        }

        public void Execute()
        {   // take the receiver method of power state change
            mario.MoveFire();
        }
    }
}

