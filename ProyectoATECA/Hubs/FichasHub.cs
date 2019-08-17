using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ProyectoATECA.Hubs
{
    [HubName("fichasHub")]
    public class FichasHub : Hub
    {
        public static void BroadcastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<FichasHub>();
            context.Clients.All.refreshFichasData();
        }

        public static void BroadcastDataFILA()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<FichasHub>();
            context.Clients.All.refreshFilaData();
        }
        public static void BroadcastDataSonido()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<FichasHub>();
            context.Clients.All.refreshDataSonido();
        }
    }
}
