using System.Net;
using System.Net.Sockets;

namespace DnsMxRecordResolver.DnsProcessing.Helpers
{
    public static class UdpTransfer
    {
        public static bool TryResolveQuery(byte[] query, out byte[] response, string dnsServerAddress)
        {
            response = new byte[0];

            int attempts = 0;

            while (attempts < _maxAttempts)
            {
                try
                {
                    using (UdpClient client = new UdpClient())
                    {
                        IPEndPoint server = new IPEndPoint(IPAddress.Parse(dnsServerAddress), _dnsPort);

                        client.Connect(server);
                        client.Send(query, query.Length);

                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

                        response = client.Receive(ref endPoint);
                    }

                    return true;
                }
                catch
                {
                    attempts++;
                }
            }

            return false;
        }

        private const int _dnsPort = 53;

        private const int _maxAttempts = 5;
    }
}