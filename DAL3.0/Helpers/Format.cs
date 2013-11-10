using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    class Format
    {
        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Escape apostrophes and quotes
        /// </summary>
        /////////////////////////////////////////////////////////////
        public static string DBString(string input)
        {
            return input.Replace("'", "''").Replace("\"", "\"\"");
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Remove extra apostrophes and quotes
        /// </summary>
        /////////////////////////////////////////////////////////////
        public static string FromDBString(string input)
        {
            return input.Replace("''", "'").Replace("\"\"", "\"");
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Make sure it's an integer
        /// </summary>
        /////////////////////////////////////////////////////////////
        public static int DBInt(string input)
        {
            int i = 0;
            if (int.TryParse(input, out i))
                return i;
            return i;
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Dates in the correct format
        /// </summary>
        /////////////////////////////////////////////////////////////
        public static string DBDate(string input)
        {
            DateTime date = DateTime.MinValue;

            DateTime.TryParse(input, out date);

            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Dates in the correct format (for oracle)
        /// </summary>
        /////////////////////////////////////////////////////////////
        public static string DBDateOracle(string input)
        {
            DateTime date = DateTime.MinValue;

            DateTime.TryParse(input, out date);
                
            return "to_date('" + date.ToString("yyyy/MM/dd HH:mm:ss") + "', 'yyyy/mm/dd hh24:mi:ss')";
        }
    }
}
