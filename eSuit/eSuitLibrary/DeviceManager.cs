using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;

namespace eSuitLibrary
{
    /// <summary>
    ///     Provides automated detection and initiation of COM devices. This class cannot be inherited.
    /// </summary>
    public sealed class DeviceManager : IDisposable, INotifyPropertyChanged
    {
        /// <summary>
        ///     A System Watcher to hook events from the WMI tree.
        /// </summary>
        private readonly ManagementEventWatcher _deviceWatcher = new ManagementEventWatcher(new WqlEventQuery(
            "SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 OR EventType = 3"));

        /// <summary>
        ///     A list of all dynamically found SerialPorts.
        /// </summary>
        private Dictionary<string, SerialPort> _serialPorts = new Dictionary<string, SerialPort>();

        /// <summary>
        ///     Initialises a new instance of the <see cref="DeviceManager"/> class.
        /// </summary>
        public DeviceManager()
        {
            // Attach an event listener to the device watcher.
            _deviceWatcher.EventArrived += _deviceWatcher_EventArrived;

            // Start monitoring the WMI tree for changes in SerialPort devices.
            _deviceWatcher.Start();

            // Initially populate the devices list.
            DiscoverDevices();
        }

        /// <summary>
        ///     Gets a list of all dynamically found SerialPorts.
        /// </summary>
        /// <value>A list of all dynamically found SerialPorts.</value>
        public Dictionary<string, SerialPort> SerialPorts
        {
            get { return _serialPorts; }
            private set
            {
                _serialPorts = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Stop the WMI monitors when this instance is disposed.
            _deviceWatcher.Stop();
        }

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Handles the EventArrived event of the _deviceWatcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArrivedEventArgs"/> instance containing the event data.</param>
        private void _deviceWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            DiscoverDevices();
        }

        /// <summary>
        ///     Dynamically populates the SerialPorts property with relevant devices discovered from the WMI Win32_SerialPorts class.
        /// </summary>
        private void DiscoverDevices()
        {
            // Create a temporary dictionary to superimpose onto the SerialPorts property.
            var dict = new Dictionary<string, SerialPort>();

            try
            {
                // Scan through each SerialPort registered in the WMI.
                foreach (ManagementObject device in
                    new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort").Get())
                {
                    // Ignore all devices that do not have a relevant VendorID.
                    if (!device["PNPDeviceID"].ToString().Contains("VID_2341") && // Arduino
                        !device["PNPDeviceID"].ToString().Contains("VID_04d0")) return; // Digi International (X-Bee)

                    // Create a SerialPort to add to the collection.
                    var port = new SerialPort();

                    // Gather related configuration details for the Arduino Device.
                    var config = device.GetRelated("Win32_SerialPortConfiguration")
                                       .Cast<ManagementObject>().ToList().FirstOrDefault();

                    // Set the SerialPort's PortName property.
                    port.PortName = device["DeviceID"].ToString();

                    // Set the SerialPort's BaudRate property. Use the devices maximum BaudRate as a fallback.
                    port.BaudRate = (config != null)
                                        ? int.Parse(config["BaudRate"].ToString())
                                        : int.Parse(device["MaxBaudRate"].ToString());

                    // Add the SerialPort to the dictionary. Key = Arduino device description.
                    dict.Add(device["Description"].ToString(), port);
                }

                // Return the dictionary.
                SerialPorts = dict;
            }
            catch (ManagementException mex)
            {
                // Send a message to debug.
                eSuit_Debug.Log(mex);
            }
        }

        /// <summary>
        ///     Called when a property is set.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}