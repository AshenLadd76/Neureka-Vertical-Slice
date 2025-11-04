using UnityEngine;
using System.IO;
using Logger =ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    public abstract class BaseTextParserSo : ScriptableObject
    {
        public abstract void Parse(TextAsset textAsset, string path);
        
        protected void DeleteSourceFile(string pathToSourceFile)
        {
            if (string.IsNullOrEmpty(pathToSourceFile))
            {
                Logger.LogError("The source file path is null or empty");
                return;
            }
            
            try
            {
                if( !File.Exists(pathToSourceFile) ) return;
                
                File.Delete(pathToSourceFile);
                Logger.Log($"Deleted source JSON at {pathToSourceFile}");
            }
            catch (System.Exception e)
            {
                Logger.LogError($"Failed to delete JSON file: {e.Message}");
            }
        }
    }
}

