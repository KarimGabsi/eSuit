using System;
using System.Linq;

using System.IO.Ports;
using System.Threading;

namespace eSuitLibrary
{
    internal class eSuit_Connection
    {
        private SerialPort _currentPort = new SerialPort();

        private HitPlaces _curHitPlace;
        private int _curVolts;
        private int _curDuration;

        private bool syncing = true;
        private bool actionRequest = false;
        private Thread eSuitThread;

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
            eSuitThread = new Thread(() => Sync_eSuit());
            eSuitThread.IsBackground = true;
            eSuitThread.Start();

        }

        // Syncing eSuit Method
        // Check out every port that has devices and performs a manual handshake.
        // Executes hits when needed
        private void Sync_eSuit()
        {
            while (syncing)
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

                                string response = sendCommand("ESUIT_TRY_CONNECTION", false);
                                if (response == "ESUIT_CONNECTION_OK")
                                {
                                    _connected = true;
                                    break;
                                }
                            }
                            catch (Exception)
                            {
                                //eSuit_Debug.Log(ex);
                                //Port got disconnected manually
                                _connected = false;
                            }
                        }

                        //Hit Request?
                        if (actionRequest)
                        {
                            try
                            {
                                switch (_curHitPlace)
                                {
                                    default:
                                        break;
                                    case HitPlaces.FullBody:
                                        sendCommand("HIT_FULLBODY" + "-" + _curVolts.ToString() + "-" + _curDuration.ToString(), true);
                                        break;
                                    case HitPlaces.Left_Arm:
                                        sendCommand("HIT_LEFT_ARM" + "-" + _curVolts.ToString() + "-" + _curDuration.ToString(), true);
                                        break;
                                    case HitPlaces.Right_Arm:
                                        sendCommand("HIT_RIGHT_ARM" + "-" + _curVolts.ToString() + "-" + _curDuration.ToString(), true);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                eSuit_Debug.Log(ex);
                            }
                            finally
                            {
                                actionRequest = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                   //eSuit_Debug.Log(ex);
                }  
            }          
        }

        internal void ExecuteHit(HitPlaces hit, int volts, int duration)
        {
            _curHitPlace = hit;
            _curVolts = volts;
            _curDuration = duration;

            actionRequest = true;
        }

        private string sendCommand(string command, bool log)
        {
            if (log)
            {
                eSuit_Debug.Log("Writing command \"" + command + "\" to eSuit");            
            }

            string response = "";
            using (_currentPort)
            {
                if (!_currentPort.IsOpen)
                {
                    _currentPort.Open();
                }
                _currentPort.Write(command);
                response = _currentPort.ReadLine().ToString().Replace("\r", ""); 
            }


            if (log)
            {
                eSuit_Debug.Log("eSuit Response: " + response);
            }

            return response;
        }
        
        internal void Dispose()
        {
            syncing = false;
            GC.Collect();   
        }
    }
}
