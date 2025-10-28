using System;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Data.Parsers
{
    [Serializable]
    public class BaseDispatcherSo : ScriptableObject, IDispatcher
    {
        public virtual void Dispatch(string path)
        {
            Logger.Log( $"Base Dispatcher" );
        }
    }
}