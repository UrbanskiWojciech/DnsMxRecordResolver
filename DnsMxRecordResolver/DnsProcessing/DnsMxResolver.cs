using DnsMxRecordResolver.DnsProcessing.Helpers;
using DnsMxRecordResolver.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnsMxRecordResolver.DnsProcessing
{
    public class DnsMxResolver
    {
        public DnsMxResolver(string dnsAddress, int saveReportToFile)
        {
            _dnsAddress = string.IsNullOrWhiteSpace(dnsAddress) ? _defaultDnsServerAddress : dnsAddress;
            _saveReportToFile = saveReportToFile == 49 ? true : false;
        }

        public void ProcessMultipleDnsQueries(List<string> domains)
        {
            if (domains == null || domains.Count == 0)
                return;

            if (_saveReportToFile)
            {
                Console.WriteLine("Report will be saved in {0}", Environment.SpecialFolder.MyDocuments);
                Console.WriteLine("Most preferenced record at the top of each domain");
                Console.WriteLine();
            }

            Parallel.ForEach(domains, item => ProcessSingleDnsQuery(item));
        }

        private void ProcessSingleDnsQuery(string domain)
        {
            byte[] query = DnsQueryProvider.GetDnsQueryAsBytes(domain);

            if (!UdpTransfer.TryResolveQuery(query, out byte[] response, _dnsAddress))
            {
                Console.WriteLine("Error while trying to get DNS response.");
                return;
            }

            if (!DnsResponseParser.TryParseReponse(domain, _dnsAddress, response, out MxRecord[] records))
            {
                Console.WriteLine("Error while trying to parse DNS response.");
                return;
            }

            ReportProvider.ProvideReport(records, _saveReportToFile);
        }

        private readonly string _dnsAddress;
        private readonly bool _saveReportToFile;

        private const string _defaultDnsServerAddress = "8.8.8.8";
    }
}