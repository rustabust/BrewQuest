using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewQuest.Models
{
    public static class StringExtensions
    {
        public static string RemoveEverythingBeforeTag(this string input, string tag, bool includeTag)
        {
            int tagIndex = input.IndexOf(tag);
            if (tagIndex >= 0)
            {
                if (includeTag)
                {
                    input = input.Substring(tagIndex, input.Length - tagIndex);
                }
                else
                {
                    input = input.Substring(tagIndex + tag.Length, input.Length - tagIndex - tag.Length);
                }
            }
            return input;
        }
        public static string RemoveEverythingAfterTag(this string input, string tag, bool includeTag)
        {
            int tagIndex = input.IndexOf(tag);
            if (tagIndex > 0)
            {
                var length = tagIndex;
                if (includeTag)
                {
                    length += tag.Length;
                }
                input = input.Substring(0, length);
            }
            return input;
        }
    }

    public static class DateTimeExtensions
    {
        /// <summary>
        /// for nullable datetimes, returns empty string for nulls 
        /// otherwise returns a string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToDateStringSafe(this DateTime? input, string unknownStr = "", string format = "MM/dd/yyyy")
        {
            string result = unknownStr;
            if (input.HasValue)
            {
                result = input.Value.ToString(format);
            }
            return result;
        }
       
    }
}
