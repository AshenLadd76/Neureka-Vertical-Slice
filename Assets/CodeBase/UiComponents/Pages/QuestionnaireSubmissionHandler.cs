using System;
using System.Collections.Generic;
using CodeBase.Questionnaires;
using CodeBase.Services;
using ToolBox.Helpers;
using ToolBox.Messaging;
using ToolBox.Services.Web;
using ToolBox.Utils;

namespace CodeBase.UiComponents.Pages
{
    public class QuestionnaireSubmissionHandler
    {
        private readonly StandardQuestionnaireTemplate _questionnaireData;
        private readonly Dictionary<int, AnswerData> _answerDataDictionary;
        private readonly ISerializer _jsonSerializer;
        private readonly Action _onFinished;

        public QuestionnaireSubmissionHandler(StandardQuestionnaireTemplate questionnaireData, Dictionary<int, AnswerData> answerDataDictionary, ISerializer jsonSerializer, Action onFinished = null)
        {
            _questionnaireData = questionnaireData ?? throw new ArgumentNullException(nameof(questionnaireData));
            _answerDataDictionary = answerDataDictionary ?? throw new ArgumentNullException(nameof(answerDataDictionary));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _onFinished = onFinished; // optional, can be null
        }

        public void Submit()
        {
            try
            {
                var questionnaireData = new QuestionnaireDataBuilder().SetTemplate(_questionnaireData).SetAnswers(_answerDataDictionary).Build();

                var jsonData = _jsonSerializer.Serialize(questionnaireData);

                
                //Debug
                Logger.Log(jsonData);

                var webData = new WebData
                {
                    Id = _questionnaireData.QuestionnaireID,
                    Data = jsonData
                };

                RequestDataUpload(webData);

                _onFinished?.Invoke();
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to handle submit: {e}");
            }
        }

        private void RequestDataUpload( WebData webData ) =>  MessageBus.Broadcast( WebServiceMessages.OnPostRequestMessage, webData );
    }
}