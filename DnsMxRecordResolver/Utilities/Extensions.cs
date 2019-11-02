using DnsMxRecordResolver.Models;

namespace DnsMxRecordResolver
{
    public static class Extensions
    {
        public static MxRecord[] SortByPreference(this MxRecord[] records)
        {
            MxRecord temp;

            for (int j = 0; j <= records.Length - 2; j++)
            {
                for (int i = 0; i <= records.Length - 2; i++)
                {
                    if (records[i].Preference > records[i + 1].Preference)
                    {
                        temp = records[i + 1];
                        records[i + 1] = records[i];
                        records[i] = temp;
                    }
                }
            }

            return records;
        }
    }
}