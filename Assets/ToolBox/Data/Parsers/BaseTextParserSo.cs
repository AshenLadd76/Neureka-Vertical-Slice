using UnityEngine;
using System.IO;
using Logger =ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    /// <summary>
    /// Base class for ScriptableObject-based text parsers. 
    /// Provides utility methods such as source file deletion.
    /// </summary>
    public abstract class BaseTextParserSo : ScriptableObject
    {
        /// <summary>
        /// Parses the provided text asset and processes its content.
        /// Implementations define the specific parsing logic.
        /// </summary>
        /// <param name="textAsset">The TextAsset containing the content to parse.</param>
        /// <param name="path">The file path of the source asset (useful for saving, logging, or cleanup).</param>
        public abstract void Parse(TextAsset textAsset, string path);
        
        
        /// <summary>
        /// Deletes the source file at the given path.
        /// Logs an error if the path is null, empty, or if the deletion fails.
        /// </summary>
        /// <param name="pathToSourceFile">The full path to the file that should be deleted.</param>
        protected bool DeleteSourceFile(string pathToSourceFile)
        {
            if (string.IsNullOrEmpty(pathToSourceFile))
            {
                Logger.LogError("The source file path is null or empty");
                return false;
            }
            
            try
            {
                if( !File.Exists(pathToSourceFile)) return false;
                
                File.Delete(pathToSourceFile);
                
                Logger.Log($"Deleted source file at {pathToSourceFile}");
            }
            catch (System.Exception e)
            {
                Logger.LogError($"Failed to delete source file: {e.Message}");
                return false;
            }
            
            return true;
        }
    }
}

