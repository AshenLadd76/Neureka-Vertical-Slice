using UnityEngine;

namespace ToolBox.Data.Parsers
{
    public abstract class BaseTextParserSo : ScriptableObject
    {
        public abstract void Parse(TextAsset textAsset);
    }
}

