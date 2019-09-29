﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.MarioClasses
{
    interface IActionState
    {
        MarioState.ActionType Type { get; set; }
        void Up(Mario mario);
        void Down(Mario mario);
        void Left(Mario mario);
        void Right(Mario mario);
    }
}