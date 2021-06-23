// project=PBDotNet.Core, file=PsrCleaner.cs, create=09:16 Copyright (c) 2021 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using System.Text;

namespace PBDotNet.Core.util
{
    public class PsrCleaner
    {
        public static string Clean(string source)
        {
            StringBuilder cleanSource = new StringBuilder();
            char[] chars = source.ToCharArray();

            for (int i = 0; i < source.Length; i++)
            {
                if (chars[i] != (char)0x00)
                {
                    cleanSource.Append(chars[i]);
                }
            }

            int start = cleanSource.ToString().IndexOf("release");

            cleanSource.Remove(0, start);

            int end = cleanSource.ToString().IndexOf((char)0x02 + "" + (char)0x0E);
            if (end > 0)
            {
                cleanSource.Remove(end, cleanSource.Length - end);
            }

            return cleanSource.ToString();
        }
    }
}