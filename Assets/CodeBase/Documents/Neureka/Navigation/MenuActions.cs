using System;
using CodeBase.Services;
using ToolBox.Messaging;
using ToolBox.Utils;
using UiFrameWork.RunTime;

namespace CodeBase.Documents.Neureka.Navigation
{
    public static class MenuActions
    {
        public static Action RequestQuestionnaire(string questionnaireId)
        {
            return () => 
            {
                Logger.Log( $"{questionnaireId}" );
                MessageBus.Broadcast( QuestionnaireService.OnRequestQuestionnaireMessage, questionnaireId );
            };
        }

        public static Action RequestAssessment(string assessmentId)
        {
            return () => 
            {
                Logger.Log( $"{assessmentId}" );
                MessageBus.Broadcast( QuestionnaireService.OnRequestQuestionnaireMessage, assessmentId );
            };
        }
    }
}