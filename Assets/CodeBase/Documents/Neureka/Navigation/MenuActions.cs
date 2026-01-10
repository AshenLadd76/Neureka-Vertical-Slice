using System;
using ToolBox.Messaging;
using ToolBox.Utils;
using UiFrameWork.RunTime;

namespace CodeBase.Documents.Neureka.Navigation
{
    public static class MenuActions
    {
        public static Action RequestDocument(string questionnaireId)
        {
            return () => 
            {
                Logger.Log( $"{questionnaireId}" );
                MessageBus.Broadcast( nameof(DocumentServiceMessages.OnRequestOpenDocument), DocumentID.RiskFactors );
            };
        }
    }
}