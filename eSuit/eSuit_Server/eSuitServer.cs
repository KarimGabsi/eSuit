using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.Net;
using System.IO;
using System.Xml.Linq;
using eSuitLibrary;

namespace eSuit_Server
{
    public partial class eSuitServer : Form
    {
        static HttpListener listener;
        private Thread listenThread;
        private bool listening = true;

        private System.Windows.Forms.Timer t;

        private eSuit _eSuit = new eSuit();

        public eSuitServer()
        {
            InitializeComponent();

            btnStopServer.Enabled = false;
            _eSuit.Start();

            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8000/");
            listener.Prefixes.Add("http://127.0.0.1:8000/");
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            btnStartServer.Enabled = true;
            btnStopServer.Enabled = false;


            listening = false;
            listener.Stop();

            listenThread.Abort();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            btnStartServer.Enabled = false;
            btnStopServer.Enabled = true;

            listener.Start();

            this.listenThread = new Thread(new ParameterizedThreadStart(Listening));
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void Listening(object o)
        {
            while (listening)
            {
                ////blocks until a client has connected to the server
                ProcessRequest();
            }
            listener.Stop();

        }

        private void ProcessRequest()
        {
            var result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();
        }

        private void ListenerCallback(IAsyncResult result)
        {
            try
            {
                if (listener.IsListening)
                {
                    var context = listener.EndGetContext(result);
                    //Thread.Sleep(1000);
                    var data_text = new StreamReader(context.Request.InputStream,
                    context.Request.ContentEncoding).ReadToEnd();

                    //<eSuitCommands>
                    //<eSuitCommand hitplace="HIT_LEFT_ARM" volts="21" duration="200"/>
                    //</eSuitCommands>
                    if (_eSuit.connected())
                    {
                        var data = XElement.Parse(data_text);
                        List<eSuitCommand> commanddata = (from cmd in data.Elements("eSuitCommand")
                                                          select new eSuitCommand()
                                                          {
                                                              hitplace = (HitPlaces)Enum.Parse(typeof(HitPlaces), cmd.Attribute("hitplace").Value),
                                                              volts = Convert.ToInt32(cmd.Attribute("volts").Value),
                                                              duration = Convert.ToInt32(cmd.Attribute("duration").Value),
                                                          }).ToList();


                        foreach (eSuitCommand esc in commanddata)
                        {
                            _eSuit.ExecuteHit(esc.hitplace, esc.volts, esc.duration);
                        }
                    }


                    context.Response.StatusCode = 200;
                    context.Response.StatusDescription = "OK";

                    context.Response.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void eSuitServer_Load(object sender, EventArgs e)
        {
            //Timer to check connection has changed or not.
            t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += t_Tick;
            t.Start();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            if (_eSuit.connected())
            {
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "eSuit connected on " + _eSuit.currentPort();
            }
            else
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "eSuit disconnected";
            }

            if (listener.IsListening)
            {
                lblServerStatus.ForeColor = Color.Green;
                lblServerStatus.Text = "Server is listening...";
            }
            else
            {
                lblServerStatus.ForeColor = Color.Red;
                lblServerStatus.Text = "Server is offline...";
            }

            txtDebug.Text = eSuit_Debug.GetLog();
            txtDebug.SelectionStart = txtDebug.Text.Length;
            txtDebug.ScrollToCaret();
        }

        private void eSuitServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            listening = false;
            _eSuit.Dispose();
        }

    }
}
