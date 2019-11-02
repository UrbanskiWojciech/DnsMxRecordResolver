namespace DnsMxRecordResolver.Models
{
    public struct MxRecord
    {
        public string Domain { get; set; }

        public string MailExchanger { get; set; }

        public string DnsServerAddress { get; set; }

        public int Preference { get; set; }
    }
}