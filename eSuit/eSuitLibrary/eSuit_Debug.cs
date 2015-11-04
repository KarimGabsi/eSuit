using System;
using System.Diagnostics;
using System.IO;

namespace eSuitLibrary
{
    public static class eSuit_Debug
    {
        
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
        private static readonly object _InUse = new object();

        public static void Log(string message)
        {
            lock (_InUse)
            {
                if (!File.Exists(path))
                {
                    FileInfo file = new FileInfo(path);
                    file.Directory.Create();
                    //Create log and write exception
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("[" + DateTime.Now.ToString() + "]" + message + "\n\n");
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("[" + DateTime.Now.ToString() + "]" + message + "\n\n");
                    }
                } 
            }
       
        }
        public static void Log(Exception e)
        {
            lock (_InUse)
            {
                if (!File.Exists(path))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(path);
                    file.Directory.Create();
                    // Create a file to write to. 
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("[" + DateTime.Now.ToString() + "]" + e.Message + "\n" + e.InnerException + "\n" + e.Source + "\n" + e.StackTrace.ToString() + "\n\n");
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("[" + DateTime.Now.ToString() + "]" + e.Message + "\n" + e.InnerException + "\n" + e.Source + "\n" + e.StackTrace.ToString() + "\n\n");
                    }
                }
            }
        }

        public static string GetLog()
        {
            lock (_InUse)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        using (StreamReader sr = new StreamReader(path))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception e)
                {
                    Log(e);
                    return null;
                }
            }
        }
    }
}
