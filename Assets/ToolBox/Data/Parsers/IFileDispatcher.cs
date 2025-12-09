using UnityEngine;

namespace ToolBox.Data.Parsers
{
    public interface IFileDispatcher 
    {
        public void Dispatch(string assetPath, string fullPath);
    }
}
