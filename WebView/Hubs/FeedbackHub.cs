using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebView.Hubs
{
    //This class is created automatically and retrieved using: var context = GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>();
    //[HubName("feedbackHub")]
    public class FeedbackHub : Hub // in javascript the first character will be lowercased
    {
        public void SendRow(string id, string modelstr) // to be called by javascript, the first character on the method will be lowercased when used in javascript
        {
            // Call the addNewRecord function on javascript/client side
            Clients.All.addNewRecord(id, modelstr);
        }
    }
}