using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Threading;
using System.Diagnostics.Contracts;
using System.Management;

namespace eSuitPlugin
{
    internal class eSuit_Connection
    {
        private SerialPort _currentPort = new SerialPort();
        public SerialPort currentPort
        {
            get { return _currentPort; }
        }

        private bool _connected;
        public bool connected
        {
            get { return _connected; }
        }

        private bool perfomingAction = false;

        public eSuit_Connection()
        {
            try
            {
                AutoResetEvent autoEvent = new AutoResetEvent(false);
                TimerCallback tcb = Synchronisation;
                System.Threading.Timer t = new System.Threading.Timer(tcb, autoEvent, 1000, 4000);
            }
            catch (Exception)
            {
                
            }
        }
        private void Sync_eSuit()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                if (searcher.Get().Count == 0)
                {
                    _connected = false;
                }
                foreach (ManagementObject item in searcher.Get())
                {
                    string deviceId = item["DeviceID"].ToString();

                    _currentPort = new SerialPort(deviceId, 9600, Parity.None, 8, StopBits.One);
                    _currentPort.DtrEnable = true;
                    _currentPort.RtsEnable = true;

                    if (Detect_eSuit())
                    {
                        _connected = true;
                        break;
                    }
                }
                

            }
            catch (Exception ex)
            {
                _connected = false;
                throw ex;
            }
        }
        private bool Detect_eSuit()
        {
            try
            {
                _currentPort.Open();
                _currentPort.Write("ESUIT_TRY_CONNECTION");
                string response = _currentPort.ReadLine().ToString().Replace("\r", "");
                _currentPort.Close();
                if (response == "ESUIT_CONNECTION_OK")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }


        }
        public void Synchronisation(object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            if (!perfomingAction && !_currentPort.IsOpen)
            {
                perfomingAction = true;
                Sync_eSuit();
                perfomingAction = false;
            }
            autoEvent.Set();

        }
        public void Dispose()
        {
            if (_currentPort != null)
            {
                _currentPort.Dispose();
                _currentPort = null;
            }
        }

        public void ExecuteHit(HitPlaces hit, int volts, int duration)
        {
            Contract.Requires(volts > 0, "Volt must be bigger than zero");
            Contract.Requires(volts <= 60, "Volt must be smaller or equil to 60");

            Contract.Requires(duration >= 500, "duration must be bigger or equal than 500 milliseconds");
            Contract.Requires(duration <= 3000, "duration must be smaller or equal than 3000 milliseconds");

            if (_connected)
            {
                perfomingAction = true;
                SendCommand(hit, volts, duration);
                perfomingAction = false;
            }
        }

        private void SendCommand(HitPlaces hit, int volts, int duration)
        {
            try
            {
                _currentPort.Open();
                _currentPort.Write("VOLTS_" + volts.ToString());
                _currentPort.Close();

                _currentPort.Open();
                _currentPort.Write("DURATION_" + duration.ToString());
                _currentPort.Close();

                switch (hit)
                {
                    default:
                        break;
                    case HitPlaces.FullBody:
                        _currentPort.Open();
                        _currentPort.Write("HIT_FULLBODY");
                        _currentPort.Close();
                        break;
                    case HitPlaces.Left_Arm:
                        _currentPort.Open();
                        _currentPort.Write("HIT_LEFT_ARM");
                        _currentPort.Close();
                        break;
                    case HitPlaces.Right_Arm:
                        _currentPort.Open();
                        _currentPort.Write("HIT_RIGHT_ARM");
                        _currentPort.Close();
                        break;
                }

            }
            catch (Exception)
            {
            }
        }
    }
}
