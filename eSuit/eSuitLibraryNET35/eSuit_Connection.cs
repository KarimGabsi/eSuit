using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace eSuitLibraryNET35
{
    internal class eSuit_Connection
    {
        private SerialPort _currentPort = new SerialPort();
        private readonly object _InUse = new object();
        private ManualResetEvent _event = new ManualResetEvent(true);
        private volatile bool _performingAction = false;

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
                AutoResetEvent autoEvent = new AutoResetEvent(false);
                TimerCallback tcb = Sync_eSuit;
                System.Threading.Timer t = new System.Threading.Timer(tcb, autoEvent, 0, 1000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void Sync_eSuit(object state)
        {
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
                                _connected = false;
                                //Port got disconnected manually
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            
            
        }

        public void ExecuteHit(HitPlaces hit, int volts, int duration)
        {
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
            lock (_InUse)
            {
                _performingAction = true;
                _currentPort.Open();
                _currentPort.Write(command);
                _currentPort.Close();
                _performingAction = false;
            }
            
        }

    }
}
