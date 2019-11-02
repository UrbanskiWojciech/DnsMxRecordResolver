using DnsMxRecordResolver.Models;
using System;
using System.Text;

namespace DnsMxRecordResolver.DnsProcessing
{
    public static class DnsResponseParser
    {
        public static bool TryParseReponse(string domain, string dnsServerAddress, byte[] rawResponse, out MxRecord[] records)
        {
            records = null;

            if (rawResponse.Length < WellKnownDnsResponsePositions.AnswersCountPosition)
                return false;

            int status = rawResponse[WellKnownDnsResponsePositions.StatusPosition];
            int answersCount = rawResponse[WellKnownDnsResponsePositions.AnswersCountPosition];

            if (!CheckIfResponseIsValid(status, answersCount))
            {
                Console.WriteLine("Response is corrupted or empty.");
                return false;
            }

            records = new MxRecord[answersCount];

            // lets skip the question and response general headers
            int position = domain.Length + WellKnownDnsResponsePositions.AnswerHeadersCount;

            for (int recordsIdx = 0; recordsIdx < answersCount; recordsIdx++)
            {
                MxRecord record = new MxRecord()
                {
                    DnsServerAddress = dnsServerAddress,
                    Domain = domain,
                    Preference = rawResponse[position + WellKnownDnsResponsePositions.AnswerPreferencePosition]
                };

                position += WellKnownDnsResponsePositions.AnswerPreferencePosition + 1;
                record.MailExchanger = GetMXRecord(position, rawResponse, out position);

                records[recordsIdx] = record;
            }

            return true;
        }

        private static bool CheckIfResponseIsValid(int status, int answersCount)
        {
            return (status != 128 || answersCount == 0) ? false : true;
        }

        private static string GetMXRecord(int start, byte[] response, out int pos)
        {
            StringBuilder sb = new StringBuilder();

            // length of the following record
            int len = response[start];

            while (len > 0)
            {
                if (sb.Length > 0)
                    sb.Append(".");

                if (len == 192)
                {
                    int newpos = response[start + 1];

                    sb.Append(GetMXRecord(newpos, response, out newpos));
                    start++;
                    break;
                }

                sb.Append(ReadCharactersFromPosition(response, start, len));

                // skip already processed part and get len of next item
                start += len + 1;
                len = response[start];
            }

            // skip already processed part
            pos = start + 1;

            return sb.ToString();
        }

        private static char[] ReadCharactersFromPosition(byte[] response, int position, int count)
        {
            char[] charBuilder = new char[0];

            for (int i = position; i < position + count; i++)
            {
                Array.Resize(ref charBuilder, charBuilder.Length + 1);
                charBuilder[charBuilder.Length - 1] = (char)response[i + 1];
            }

            return charBuilder;
        }
    }
}