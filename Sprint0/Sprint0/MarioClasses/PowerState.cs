﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0.MarioClasses
{
    interface PowerState
    {
        void Destroy(Mario mario);

        void PowerUpToFireMario(Mario mario);

        void PowerUpToSuperMario(Mario mario);

        void PowerDownToStandard(Mario mario);
    }
}
