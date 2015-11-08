using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using eSuitLibrary;

namespace eSuit_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class eSuitService : IeSuitService, IDisposable
    {
        private static eSuit _eSuit = new eSuit();

        public bool ExecuteHit(string hitplace, int volts, int duration)
        {
            //WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return _eSuit.ExecuteHit((HitPlaces)Enum.Parse(typeof(HitPlaces), hitplace), volts, duration);
        }

        public bool Connected()
        {
            //WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return _eSuit.connected;
        }
        public string CurrentPort()
        {
            //WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return _eSuit.currentPort;
        }

        public void Dispose()
        {
            _eSuit.Dispose();
        }
    }
}
