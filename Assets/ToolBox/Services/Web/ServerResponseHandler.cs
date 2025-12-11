using System;
using System.Collections.Generic;
using CodeBase.Services.ToolBox.Services.Web;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Services.Web
{
    public interface IServerResponseHandler<T>
    {

        public void AddHandler(ServerResponseCode responseCode, Action<T> handler);
        public bool RemoveHandler(ServerResponseCode responseCode);

        public bool HandleResponse(ServerResponseCode responseCode, T param);
    }
    
    public class ServerResponseHandler<T> : IServerResponseHandler<T>
    {
        private readonly Dictionary<ServerResponseCode, Action<T>> _handlers = new();
        public void AddHandler(ServerResponseCode responseCode, Action<T> handler)
        {
            if (!_handlers.TryAdd(responseCode, handler))
            {
                Logger.LogError( $"Failed to add handler for {responseCode}" );
            }
        }

        public bool RemoveHandler(ServerResponseCode responseCode)
        {
            if (!_handlers.ContainsKey(responseCode)) return false;
            
            _handlers.Remove(responseCode);
            
            return true;
        }

        public bool HandleResponse(ServerResponseCode responseCode, T param)
        {
            if (!_handlers.TryGetValue(responseCode, out var handler)) return false;
            
            handler.Invoke(param);
            
            return true;
        }
    }
}
