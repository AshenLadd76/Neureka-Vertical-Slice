using System;
using System.IO;
using ToolBox.Utils;

namespace CodeBase.Helpers
{
    public static class TextReader
    {
        public static string ReadTextFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Logger.LogError($"Path is null or empty");
                return string.Empty;
            }
            
            try
            {
                if (File.Exists(path)) return File.ReadAllText(path);
                
                Logger.LogError($"File not found: {path}");
               
                return string.Empty;

            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to read file at {path}. Exception: {e.Message}");
                return string.Empty;
            }
        }
    }
}