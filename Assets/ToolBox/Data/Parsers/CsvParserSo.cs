using UnityEditor;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    [CreateAssetMenu(fileName = "CsvParserSo", menuName = "ToolBox/Parsers/CSV Parser")]
    public class CsvParserSo : BaseTextParserSo
    {
        
        private TextAsset _csvContent;
        
        public override void Parse(TextAsset textAsset)
        {
          
            
        }
    }
    
    public class QuestionnaireSo : ScriptableObject
    {
        
    }
}
