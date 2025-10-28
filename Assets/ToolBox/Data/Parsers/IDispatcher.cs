using UnityEngine;

namespace ToolBox.Data.Parsers
{
    public interface IDispatcher 
    {
        public void Dispatch(string path);
    }
}
