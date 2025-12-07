using UnityEngine;

namespace ToolBox.Services
{
    
    /// <summary>
    /// Base class for all services that need to subscribe/unsubscribe 
    /// to events, message buses, or other external systems.
    /// Automatically manages subscriptions through Unity's lifecycle.
    /// </summary>
    
    public abstract class BaseService : MonoBehaviour
    {
        private bool _isSubscribed = false;

        protected virtual void OnEnable() => Subscribe();
        protected virtual void OnDisable() => Unsubscribe();

        /// <summary>
        /// Ensures the service subscribes only once.
        /// Calls <see cref="SubscribeToService"/> in derived classes.
        /// </summary>
        protected virtual void Subscribe()
        {
            if (_isSubscribed) return;
            SubscribeToService();
            _isSubscribed = true;
        }

        /// <summary>
        /// Unsubscribes only if currently subscribed.
        /// Calls <see cref="UnsubscribeFromService"/> in derived classes.
        /// </summary>
        protected virtual void Unsubscribe()
        {
            if (!_isSubscribed) return;
            UnsubscribeFromService();
            _isSubscribed = false;
        }

         /// <summary>
         /// Implement to define how the service subscribes to events/messages.
         /// </summary>
        protected abstract void SubscribeToService();
        protected abstract void UnsubscribeFromService();
        
        
        /// <summary>
        /// Implement to define how the service unsubscribes from events/messages.
        /// </summary>

        [ContextMenu("Subscribe to service (Editor)")]
        public void SubscribeEditor()
        {
            if (_isSubscribed) return;
            
            SubscribeToService();
            
            _isSubscribed = true;
        }

        [ContextMenu("Unsubscribe from service (Editor)")]
        public void UnsubscribeEditor()
        {
            if( !_isSubscribed ) return;
            
            UnsubscribeFromService();
            
            _isSubscribed = false;
        }
    }
}