using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eSuitPlugin
{
    public class eSuit
    {
        private eSuit_Connection eSuitCon;

        public eSuit()
        {
            eSuitCon = new eSuit_Connection();
        }

        public void ExecuteHit(HitPlaces hit, int volts, int duration)
        {
            Contract.Requires(volts > 0, "Volt must be bigger than zero");
            Contract.Requires(volts <= 60, "Volt must be smaller or equil to 60");

            Contract.Requires(duration >= 500, "duration must be bigger or equal than 500 milliseconds");
            Contract.Requires(duration <= 3000, "duration must be smaller or equal than 3000 milliseconds");

            eSuitCon.ExecuteHit(hit, volts, duration);
        }

        public bool connected()
        {
            return eSuitCon.connected;
        }

        public string currentPort()
        {
            return eSuitCon.currentPort.PortName;
        }

        
    }
}
