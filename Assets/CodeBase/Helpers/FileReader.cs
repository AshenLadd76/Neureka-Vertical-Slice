using System.IO;
using UnityEngine;

namespace CodeBase.Helpers
{
    public static class FileReader
    {
        /// <summary>
        /// Reads the entire content of a text file.
        /// </summary>
        /// <param name="filePath">Full path to the text file</param>
        /// <returns>File content as a string, or null if file not found</returns>
        public static string ReadTextFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"File not found: {filePath}");
                return null;
            }

            try
            {
                return File.ReadAllText(filePath);
            }
            catch (IOException e)
            {
                Debug.LogError($"Error reading file: {e.Message}");
                return null;
            }
        }
    }
}

