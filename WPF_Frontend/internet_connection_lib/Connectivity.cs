using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace internet_connection_lib
{
    public class Connectivity : IConnectivity
    {
        public async Task<bool> IsConnectedAsync(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host), "Cannot be null or empty!");
            }

            Ping p = new Ping();
            
            try
            {
                PingReply reply = await p.SendPingAsync(host, 3000);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
