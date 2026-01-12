using System;
using UnityEngine;

namespace CodeBase.Questionnaires
{
    [Serializable]
    public class StandardQuestionnaireTemplate
    {
        [SerializeField] private string playerId;
        [SerializeField] private string scientificId;
        [SerializeField] private string scientificName;
        [SerializeField] private string assessmentId;
        [SerializeField] private string questionnaireID;
        [SerializeField] private string questionnaireName;
        [SerializeField] private string questionnaireIntroductionImage;
        [SerializeField] private string questionnaireDescription;
        [SerializeField] private string questionnaireIntroduction;
        [SerializeField] private string questionnaireIcon;
        [SerializeField] private string[] questions;
        [SerializeField] private string[] answers;
        [SerializeField] private int[] reverseScored;
        [SerializeField] private string answerComponentType;
        

        // Public getters / setters
        public string PlayerId { get => playerId; set => playerId = value; }
        public string ScientificId { get => scientificId; set => scientificId = value; }
        public string ScientificName { get => scientificName; set => scientificName = value; }
        public string AssessmentId { get => assessmentId; set => assessmentId = value; }
        public string QuestionnaireID { get => questionnaireID; set => questionnaireID = value; }
        public string QuestionnaireName { get => questionnaireName; set => questionnaireName = value; }
        
        public string QuestionnaireIntroduction { get => questionnaireIntroduction; set => questionnaireIntroduction = value; }
        
        public string QuestionnaireIntroductionImage { get => questionnaireIntroductionImage; set => questionnaireIntroductionImage = value; }
        
        public string QuestionnaireDescription { get => questionnaireDescription; set => questionnaireDescription = value; }
        public string QuestionnaireIcon { get => questionnaireIcon; set => questionnaireIcon = value; }
        public string[] Questions { get => questions; set => questions = value; }
        public string[] Answers { get => answers; set => answers = value; }
        public int[] ReverseScored { get => reverseScored; set => reverseScored = value; }
        public string AnswerComponentType { get => answerComponentType; set => answerComponentType = value; }
      
    }

    [Serializable]
    public class MetaData
    {
        [SerializeField] private string parseType;
        public string ParseType { get => parseType; set => parseType = value; }
    }
}



