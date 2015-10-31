using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace eSuitLibraryNET35
{
    public class eSuit
    {
        private eSuit_Connection eSuitCon;
        public eSuit()
        {
        }
        public void Start()
        {
            eSuitCon = new eSuit_Connection();
        }
        public void ExecuteHit(HitPlaces hit, int volts, int duration)
        {
            Thread hitThread = new Thread(() => eSuitCon.ExecuteHit(hit, volts, duration));
            hitThread.IsBackground = true;
            hitThread.Start();            
        }
        public bool connected()
        {
            return eSuitCon.connected;
        }

        public string currentPort()
        {
            return eSuitCon.currentPort.PortName;
        }
        public void Dispose()
        {
            eSuitCon.Dispose();
            GC.Collect();
        }

        
    }
}
