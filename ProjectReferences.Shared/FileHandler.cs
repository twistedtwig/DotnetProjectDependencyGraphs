using System;
using System.IO;

namespace ProjectReferences.Shared
{
    public class FileHandler
    {
        /// <summary>
        /// Will ensure folder exists and no file is there, (will delete one if it exists)
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <param name="allText"></param>
        public static void WriteToOutputFile(string fullFilePath, string allText)
        {
            EnsureFolderExistsForFullyPathedLink(fullFilePath);

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }

            File.WriteAllText(fullFilePath, allText);
        }

        /// <summary>
        /// Will ensure that a folder exists for the path given, will create if it doesn't
        /// </summary>
        /// <param name="fullFilePath"></param>
        public static void EnsureFolderExistsForFullyPathedLink(string fullFilePath)
        {
            var dir = Path.GetDirectoryName(fullFilePath);
            if (string.IsNullOrWhiteSpace(dir))
            {
                throw new ArgumentOutOfRangeException(fullFilePath);
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}