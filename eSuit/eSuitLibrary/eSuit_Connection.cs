using System;
using System.Linq;

using System.IO.Ports;
using System.Threading;

namespace eSuitLibrary
{
    internal class eSuit_Connection
    {
        private SerialPort _currentPort = new SerialPort();
        private readonly object _InUse = new object();
        private ManualResetEvent _event = new ManualResetEvent(true);
        private volatile bool _performingAction = false;
        private System.Threading.Timer syncTimer;

        public SerialPort currentPort
        {
            get { return _currentPort; }
        }

        private volatile bool _connected;

        public bool connected
        {
            get { return _connected; }
        }

        public eSuit_Connection()
        {
            try
            {
                // Create a timer for syncing the eSuit
                // Timer checks every second whether or not the eSuit is connected
                AutoResetEvent autoEvent = new AutoResetEvent(false);
                TimerCallback tcb = Sync_eSuit;
                syncTimer = new System.Threading.Timer(tcb, autoEvent, 0, 1000);               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Syncing eSuit Method
        // Check out every port that has devices and performs a manual handshake.
        private void Sync_eSuit(object state)
        {
            //when executing a hit, tell this thread to suspend.
            _event.WaitOne();
            if (!_performingAction)
            {
                try
                {
                    string[] ports = SerialPort.GetPortNames();
                    if (ports.Count() == 0)
                    {
                        _connected = false;
                    }
                    else
                    {
                        //Loop through connected ports
                        foreach (string port in ports)
                        {
                            try
                            {
                                _currentPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
                                _currentPort.DtrEnable = true;
                                _currentPort.RtsEnable = true;

                                if (!_currentPort.IsOpen)
                                {
                                    _currentPort.Open();
                                }

                                _currentPort.Write("ESUIT_TRY_CONNECTION");
                                string response = _currentPort.ReadLine().ToString().Replace("\r", "");

                                if (_currentPort.IsOpen)
                                {
                                    _currentPort.Close();
                                }

                                if (response == "ESUIT_CONNECTION_OK")
                                {
                                    _connected = true;
                                    break;
                                }
                            }
                            catch (Exception)
                            {
                                //Port got disconnected manually
                                _connected = false;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            
            
        }

        internal void ExecuteHit(HitPlaces hit, int volts, int duration)
        {
            //Tells the syncing thread to stop.
            _event.Reset();
            if (_connected)
            {
                try
                {
                    sendCommand("VOLTS_" + volts.ToString());
                    sendCommand("DURATION_" + duration.ToString());
                    switch (hit)
                    {
                        default:
                            break;
                        case HitPlaces.FullBody:
                            sendCommand("HIT_FULLBODY");
                            break;
                        case HitPlaces.Left_Arm:
                            sendCommand("HIT_LEFT_ARM");
                            break;
                        case HitPlaces.Right_Arm:
                            sendCommand("HIT_RIGHT_ARM");
                            break;
                    }
                }
                catch (Exception)
                {
                    //throw ex;
                }
            }
            _event.Set();
        }

        private void sendCommand(string command)
        {
            //Lock incase multiple hits got received
            lock (_InUse)
            {
                _performingAction = true;
                _currentPort.Open();
                _currentPort.Write(command);
                _currentPort.Close();
                _performingAction = false;
            }
            
        }

        internal void Dispose()
        {
            syncTimer.Dispose();
            GC.Collect();       
        }
    }
}
