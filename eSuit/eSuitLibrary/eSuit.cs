using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace eSuitLibrary
{
    public class eSuit : IDisposable
    {
        /// <summary>
        ///     A DeviceManager to watch for devices
        /// </summary>
        private DeviceManager _DeviceManager;

        /// <summary>
        ///     A SerialPort containing the connected device
        /// </summary>
        private SerialPort _currentPort = null;

        /// <summary>
        ///    eSuit current port
        /// </summary>
        public string currentPort
        {
            get {
                if (_currentPort != null)
                { return _currentPort.PortName; }
                else { return string.Empty; }
                }
        }

        private bool _connected = false;
        /// <summary>
        ///    eSuit connection status
        /// </summary>
        public bool connected
        {
            get { return _connected; }
        }

        /// <summary>
        ///    eSuit constructor
        ///    Starts an instance of Device Manager and event for PropertyChanged
        /// </summary>
        public eSuit()
        {
            _DeviceManager = new DeviceManager();
            _DeviceManager.PropertyChanged += new PropertyChangedEventHandler(_DeviceManager_PropertyChanged);

            //Try Connection in case eSuit is already connected.
            if (_DeviceManager.SerialPorts.Count != 0)
            {
                Connect_eSuit(_DeviceManager.SerialPorts);
            }
        }

        /// <summary>
        ///     Triggers everytime a device connects/deconnects from the PC.
        ///     Checks whether or not that device is eSuit
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void _DeviceManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((sender as DeviceManager).SerialPorts.Count != 0)
            {
                Connect_eSuit((sender as DeviceManager).SerialPorts);
            }
            else
            {
                _connected = false;
            }
        }

        /// <summary>
        ///     Attempts to connect to eSuit
        /// </summary>
        /// <param name="ports">Connected Ports</param>
        private void Connect_eSuit(Dictionary<string, SerialPort> ports)
        {
            foreach (KeyValuePair<string, SerialPort> port in ports)
            {
                try
                {
                    _currentPort = port.Value;
                    _currentPort.DtrEnable = true;
                    _currentPort.RtsEnable = true;

                    string response = sendCommand("ESUIT_TRY_CONNECTION", false);
                    if (response == "ESUIT_CONNECTION_OK")
                    {
                        _connected = true;
                        break;
                    }
                    else
                    {
                        _currentPort = null;
                    }

                }
                catch (Exception ex)
                {
                    //Port got disconnected manually
                    eSuit_Debug.Log(ex);
                    _currentPort = null;
                }
            }
            if (_currentPort == null)
            {
                _connected = false;
            }

        }
        
        /// <summary>
        ///     Executes a hit!
        /// </summary>
        /// <param name="hit">The place to hit</param>
        /// <param name="volts">Voltage: min. 1v - max. 35v</param>
        /// <param name="duration">Duration: min. 10 - max. 3000 milliseconds</param>
        public bool ExecuteHit(HitPlaces hit, int volts, int duration)
        {
            if (_connected)
            {
                if (volts > 35 || volts < 1)
                {
                    eSuit_Debug.Log("Attempt Execute Hit: volts must have a value between 1 and 60");
                    return false;
                }
                else if (duration < 10 || duration > 3000)
                {
                    eSuit_Debug.Log("Attempt Execute Hit: duration must have a value between 10 and 3000");
                    return false;
                }
                else
                {
                    switch (hit)
                    {
                        default:
                            break;
                        case HitPlaces.FULLBODY:
                            sendCommand("HIT_FULLBODY" + "-" + volts.ToString() + "-" + duration.ToString(), true);
                            break;
                        case HitPlaces.LEFT_ARM:
                            sendCommand("HIT_LEFT_ARM" + "-" + volts.ToString() + "-" + duration.ToString(), true);
                            break;
                        case HitPlaces.RIGHT_ARM:
                            sendCommand("HIT_RIGHT_ARM" + "-" + volts.ToString() + "-" + duration.ToString(), true);
                            break;
                    }

                    return true;
                }
            }
            else 
            {
                eSuit_Debug.Log("Attempt Execute Hit while eSuit is not connected");
                return false;
            }
     
        }


        /// <summary>
        ///     Sends a command to the eSuit & returns a response
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <param name="log">Log this event</param>
        private string sendCommand(string command, bool log)
        {
            if (log)
            {
                eSuit_Debug.Log("Writing command \"" + command + "\" to eSuit");
            }

            _currentPort.Open();
            _currentPort.Write(command);
            string response = _currentPort.ReadLine().ToString().Replace("\r", "");
            _currentPort.Close();

            if (log)
            {
                eSuit_Debug.Log("eSuit Response: " + response);
            }

            return response;
        }

        /// <summary>
        ///     Dispose this and the devicemanager
        /// </summary>
        public void Dispose()
        {
            eSuit_Debug.Log("eSuit Disposed");
            _DeviceManager.Dispose();
        }
    }
}
