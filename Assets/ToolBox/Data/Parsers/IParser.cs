using UnityEngine;

namespace ToolBox.Data.Parsers
{
    
    public interface IParser<T>
    {
        /// <summary>
        /// Parse the data that this parser owns.
        /// </summary>
        void Parse( T data );
    }

}
