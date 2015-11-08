using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace eSuit_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IeSuitService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        //[WebInvoke] POST
        bool ExecuteHit(string hitplace, int volts, int duration);

        [OperationContract]
        [WebGet(ResponseFormat=WebMessageFormat.Json)]
        bool Connected();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)] 
        string CurrentPort();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetLog();
    }
}
