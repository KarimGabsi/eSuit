using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using eSuitLibrary;

namespace eSuit_Server
{
    public class eSuitCommand
    {
        public HitPlaces hitplace { get; set; }
        public int volts { get; set; }
        public int duration { get; set; }
    }
}
