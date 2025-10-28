using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Questionnaires
{
    [Serializable]
    public class LocalizedText
    {
        [SerializeField] private string en;
        [SerializeField] private string fr;

        public string En => en;
        public string Fr => fr;
    }

    [Serializable]
    public class ResponseValue
    {
        [SerializeField] private int value;
        [SerializeField] private LocalizedText text;

        public int Value => value;
        public LocalizedText Text => text;
    }

    [Serializable]
    public class Response
    {
        [SerializeField] private string id;  // responseId
        [SerializeField] private List<ResponseValue> values = new List<ResponseValue>();

        public string Id => id;
        public List<ResponseValue> Values => values;
    }

    [Serializable]
    public class Question
    {
        [SerializeField] private string id;
        [SerializeField] private string responseId;
        [SerializeField] private LocalizedText text;

        public string Id => id;
        public string ResponseId => responseId;
        public LocalizedText Text => text;
    }

    [Serializable]
    public class ScoringMessage
    {
        [SerializeField] private int minScore;
        [SerializeField] private int maxScore;
        [SerializeField] private LocalizedText text;

        public int MinScore => minScore;
        public int MaxScore => maxScore;
        public LocalizedText Text => text;
    }

    [CreateAssetMenu(fileName = "QuestionnaireTemplate", menuName = "Questionnaire/Template")]
    public class QuestionnaireTemplate : ScriptableObject
    {
        [SerializeField] private string questionnaireId;
        [SerializeField] private LocalizedText title;
        [SerializeField] private LocalizedText description;
        [SerializeField] private string type; // single-page, multi-page, etc.

        [SerializeField] private List<Question> questions = new List<Question>();
        [SerializeField] private List<Response> responses = new List<Response>();
        [SerializeField] private List<ScoringMessage> scoringMessages = new List<ScoringMessage>();

        public string QuestionnaireId => questionnaireId;
        public LocalizedText Title => title;
        public LocalizedText Description => description;
        public string Type => type;

        public List<Question> Questions => questions;
        public List<Response> Responses => responses;
        public List<ScoringMessage> ScoringMessages => scoringMessages;
    }
}