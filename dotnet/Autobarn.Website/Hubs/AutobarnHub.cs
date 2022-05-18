using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Autobarn.Website.Hubs {
    public class AutobarnHub : Hub {
        public async Task NotifyWebUsers(string user, string message) {
            await Clients.All.SendAsync("ThisIsAMagicString",
                user,
                message
            );
        }
    }
}
