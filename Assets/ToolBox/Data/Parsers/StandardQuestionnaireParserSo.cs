using System;
using UnityEngine;
using ToolBox.Data.Parsers;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    [CreateAssetMenu(fileName = "StandardQuestionnaireParserSo", menuName = "ToolBox/Parsers/Standard Questionnaire Parser")]
    public class StandardQuestionnaireParserSo : BaseTextParserSo
    {
        // Dictionary to hold questionnaire templates keyed by questionnaire ID
        [SerializeField] private string parserName = "Standard Questionnaire Parser";

        // TODO: Add fields if needed for configuration

        /// <summary>
        /// Parses a JSON file into a Questionnaire SO.
        /// </summary>
        /// <param name="textAsset">the content to parse</param>
        public override void Parse(TextAsset textAsset)
        {
            Logger.Log($"[StandardQuestionnaireParserSo] Parsing JSON at path: {textAsset.text}");

         
            // TODO: Deserialize JSON
            // TODO: Create and populate a Questionnaire SO
            // TODO: Save or register the Questionnaire SO
        }

        // TODO: Optionally add helper methods for deserializing questions, responses, and scoring
    }
}