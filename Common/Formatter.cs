using System.Collections.Generic;
using System.IO;
using Azure;
using Microsoft.Rest.Azure;

namespace CSharp_Azure_API.Common
{
    /// <summary>
    /// Formatting Helper
    /// </summary>
    public static class Formatter
    {
        /// <summary>
        /// Returns dictionary of key values from a properties file.
        /// </summary>
        /// <param name="path">Path of the properties file.</param>
        /// <returns>dictionary of key values from a properties file.</returns>
        public static Dictionary<string, string> Properties2Dic(string path)
        {
            var data = new Dictionary<string, string>();

            foreach (string row in File.ReadAllLines(path))
            {
                int idx = row.IndexOf("=");
                if (idx > 0)
                {
                    data.Add(row.Substring(0, idx).Trim(), row.Substring(idx + 1).Trim());
                }
            }
            return data;
        }

        /// <summary>
        /// Returns pages in a pageable object.
        /// </summary>
        /// <param name="pageable">Azure Pageable object.</param>
        /// <returns>pages in a pageable object.</returns>
        public static int CountPageable<T>(Pageable<T> pageable)
        {
            int count = 0;
            foreach (var page in pageable)
            {
                count++;
            }
            return count;
        }

        /// <summary>
        /// Returns pages in a IPage object.
        /// </summary>
        /// <param name="pageable">Azure IPage object.</param>
        /// <returns>pages in a IPage object.</returns>
        public static int CountPageable<T>(IPage<T> pageable)
        {
            int count = 0;
            foreach (var page in pageable)
            {
                count++;
            }
            return count;
        }
    }
}