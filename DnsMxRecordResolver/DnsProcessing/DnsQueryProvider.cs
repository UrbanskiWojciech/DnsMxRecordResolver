using System;
using System.IO;
using System.Text;

namespace DnsMxRecordResolver.DnsProcessing
{
    public static class DnsQueryProvider
    {
        public static byte[] GetDnsQueryAsBytes(string domain)
        {
            byte[] domainAsBytes = GetDnsDomainAsBytes(domain);

            byte[] requestBuffer = new byte[RequestConstHead.Length + domainAsBytes.Length + RequestConstTail.Length];

            Buffer.BlockCopy(RequestConstHead, 0, requestBuffer, 0, RequestConstHead.Length);
            Buffer.BlockCopy(domainAsBytes, 0, requestBuffer, RequestConstHead.Length, domainAsBytes.Length);
            Buffer.BlockCopy(RequestConstTail, 0, requestBuffer, RequestConstHead.Length + domainAsBytes.Length, RequestConstTail.Length);

            return requestBuffer;
        }

        private static byte[] GetDnsDomainAsBytes(string domain)
        {
            MemoryStream memoryStream = new MemoryStream();

            string[] domainParts = domain.Split('.');

            for (int i = 0; i < domainParts.Length; i++)
            {
                byte len = (byte)domainParts[i].Length;
                byte[] data = Encoding.ASCII.GetBytes(domainParts[i]);

                memoryStream.WriteByte(len);
                memoryStream.Write(data, 0, data.Length);
            }

            // End of question
            memoryStream.WriteByte(0);

            memoryStream.Close();

            return memoryStream.ToArray();
        }

        // ref: https://tools.ietf.org/html/rfc1035
        private static readonly byte[] RequestConstHead = { 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] RequestConstTail = { 0, 15, 0, 1 };
    }
}