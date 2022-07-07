using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CoreSignalR.Models;

namespace CoreSignalR.Hubs
{
    public class DemoHub : Hub
    {

        public async Task SendValues(string strValueList)
        {

            await Clients.All.SendAsync("ReceiveValues", strValueList);

            Startup.tempValues.Clear();
            Startup.tempValues = JsonConvert.DeserializeObject<List<TagItemModel>>(strValueList);

            foreach (var item in Startup.tempValues)
            {
                var tempValue = Startup.lastValues.FirstOrDefault(x => x.Name == item.Name);
                if (tempValue == null)
                {
                    Startup.lastValues.Add(item);
                }
                else
                {
                    tempValue.Value = item.Value;
                }
            }
        }

        public override async Task OnConnectedAsync()
        {
            //await SendValues(JsonConvert.SerializeObject(Startup.lastValues));
            var connectionId = Context.ConnectionId;
            await Clients.Client(connectionId).SendAsync("GetItemAll", JsonConvert.SerializeObject(Startup.lastValues));
        }
    }
}
