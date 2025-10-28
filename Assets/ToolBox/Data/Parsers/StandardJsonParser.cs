using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    [CreateAssetMenu(fileName = "StandardJsonParserSo", menuName = "ToolBox/Parsers/Standard json parser")]
    public class StandardJsonParser : BaseTextParserSo
    {
        public override void Parse(TextAsset textAsset)
        {
            if (textAsset == null)
            {
                Logger.LogError("The text asset is null");
                return;
            }
            
            Logger.Log( $"StandardJsonParser Parse : {textAsset.text}" );
        }
    }
}
