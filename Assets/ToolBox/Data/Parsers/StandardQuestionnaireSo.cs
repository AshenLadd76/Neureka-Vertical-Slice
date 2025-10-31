using CodeBase.Questionnaires;
using UnityEditor;
using UnityEngine;

namespace ToolBox.Data.Parsers
{
    [CreateAssetMenu(menuName = "Questionnaires/Standard Questionnaire")]
    public class StandardQuestionnaireSo : ScriptableObject
    {
        [SerializeField] private StandardQuestionnaireTemplate data;

        public StandardQuestionnaireTemplate Data => data;

        public void SetData(StandardQuestionnaireTemplate template)
        {
            data = template;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this); // Marks the asset dirty so Unity saves changes
#endif
        }
    }
}