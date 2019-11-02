using System;
using System.Collections.Generic;
using System.IO;

namespace DnsMxRecordResolver.Utilities
{
    public static class DomainsLoader
    {
        public static bool TryLoadDomainsFromFile(string path, out List<string> domains)
        {
            domains = new List<string>();

            string line;

            try
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    while ((line = streamReader.ReadLine()) != null)
                        domains.Add(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}