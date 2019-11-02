using DnsMxRecordResolver.DnsProcessing;
using DnsMxRecordResolver.Utilities;
using System;
using System.Collections.Generic;

namespace DnsMxRecordResolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter absolute path to txt file with list of domains line-by-line (or leave empty): ");
            string pathToFileWithDomains = Console.ReadLine();

            List<string> domains = new List<string>();
            if (string.IsNullOrWhiteSpace(pathToFileWithDomains))
            {
                string domain = string.Empty;

                Console.Write("Enter domain to lookup (until '0'): ");
                domain = Console.ReadLine();

                while (domain != "0")
                {
                    Console.Write("Enter domain to lookup (until '0'): ");
                    domains.Add(domain);
                    domain = Console.ReadLine();
                }
            }
            else
            {
                if (!DomainsLoader.TryLoadDomainsFromFile(pathToFileWithDomains, out domains))
                    return;
            }

            Console.Write("Enter address to DNS server (or leave empty): ");
            string dnsAddress = Console.ReadLine();

            Console.Write("Save to file? ('1' or leave empty): ");
            int saveReportToFile = Console.Read();

            Console.Clear();

            DnsMxResolver dnsMxResolver = new DnsMxResolver(dnsAddress, saveReportToFile);
            dnsMxResolver.ProcessMultipleDnsQueries(domains);
        }
    }
}