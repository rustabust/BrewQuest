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
}
