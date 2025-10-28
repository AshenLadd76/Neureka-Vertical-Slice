using UnityEngine;


namespace ToolBox.Data.Parsers
{
    public abstract class BaseTextParserSo : ScriptableObject, IParser<TextAsset>
    {
        public virtual void Parse(TextAsset textAsset)
        {
            
        }
    }
}
