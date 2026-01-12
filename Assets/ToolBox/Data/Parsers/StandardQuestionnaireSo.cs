using CodeBase.Questionnaires;
using UnityEditor;
using UnityEngine;

namespace ToolBox.Data.Parsers
{
    [CreateAssetMenu(menuName = "Questionnaires/Standard Questionnaire")]
    public class StandardQuestionnaireSo : ScriptableObject
    {
        [SerializeField] private StandardQuestionnaireTemplate data;
        [SerializeField] private Sprite icon;

        public StandardQuestionnaireTemplate Data => data;

        public Sprite Icon { get; set; }

        public void SetData(StandardQuestionnaireTemplate template)
        {
            data = template;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this); // Marks the asset dirty so Unity saves changes
#endif
        }

        public void SetIcon(Sprite sprite)
        {
            icon = sprite;
        }
        
    }
    
}