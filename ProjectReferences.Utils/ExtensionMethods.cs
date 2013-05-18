using System.Collections.Generic;

namespace ProjectReferences.Utils
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Adds one list to another for ILists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this IList<T> root, IList<T> items)
        {
            foreach (T item in items)
            {
                root.Add(item);
            }
        }


        public static string RemoveAccent(this string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                return string.Empty;
            }

            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string StripIllegalCharacters(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            str = System.Text.RegularExpressions.Regex.Replace(str, @"%[0-9]{2}", ""); //remove %28 etc
            str = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Remove all non valid chars       
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s", "-"); // //Replace spaces by dashes
            return str;
        }
    }
}
