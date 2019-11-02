using DnsMxRecordResolver.Models;
using System;
using System.IO;

namespace DnsMxRecordResolver.DnsProcessing.Helpers
{
    public static class ReportProvider
    {
        public static void ProvideReport(MxRecord[] records, bool saveReport)
        {
            records = records.SortByPreference();

            for (int i = 0; i < records.Length; i++)
                Console.WriteLine("DNS server: {0, -10} {1, -15} MX preference = {2}, mail exchanger = {3}", records[i].DnsServerAddress, records[i].Domain, records[i].Preference, records[i].MailExchanger);

            if (saveReport)
                SaveReportToFile(records);
        }

        public static void SaveReportToFile(MxRecord[] records)
        {
            records = records.SortByPreference();

            lock (_locker)
            {
                try
                {
                    string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string fileName = string.Concat(_defaultReportName, (_counter++).ToString(), _defaultReportExtension);
                    string fullPath = Path.Combine(directory, fileName);

                    using (StreamWriter writer = new StreamWriter(fullPath))
                    {
                        for (int i = 0; i < records.Length; i++)
                            writer.WriteLine("DNS server: {0, -10} {1, -15} MX preference = {2}, mail exchanger = {3}", records[i].DnsServerAddress, records[i].Domain, records[i].Preference, records[i].MailExchanger);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
        }

        private static int _counter = 0;

        private const string _defaultReportName = "report";
        private const string _defaultReportExtension = ".txt";

        private static readonly object _locker = new object();
    }
}