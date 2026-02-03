using System;
using System.Collections.Generic;
using ToolBox.Extensions;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

 namespace FluentUI.Builders
 {
     public partial class BaseBuilder<TElement, TBuilder> : IUIBuilder<TElement, TBuilder>
         where TElement : VisualElement, new()
         where TBuilder : BaseBuilder<TElement, TBuilder>
     {
         protected TElement VisualElement = new TElement();
        
         public TElement Build() => VisualElement;
         public TBuilder AddClass(string className) { VisualElement.AddToClassList( className ); return (TBuilder)this; }

         public TBuilder AddClasses(string[] classes)
         {
             if (classes.IsNullOrEmpty())
             {
                 Logger.Log( $"No classes to add to UI builder : {typeof(TElement).Name}" );
                 return (TBuilder)this;
             }

             for (int i = 0; i < classes.Length; i++)
             {
                 if (string.IsNullOrEmpty(classes[i]))
                 {
                     Logger.Log( $"No class to add to UI builder : {typeof(TElement).Name}" );
                     continue;
                 }

                 VisualElement.AddToClassList( classes[i]);
             }

             return (TBuilder)this;
         }

         public TBuilder RemoveClass(string className) { VisualElement.RemoveFromClassList( className ); return (TBuilder)this; }
        
         public TBuilder AttachTo(VisualElement parent)
         {
             if (parent == null) return (TBuilder)this; 
             parent.Add(VisualElement);
             return (TBuilder)this;
         }
        
        
         // 🆕 Add a single child element
         public TBuilder AddChild(VisualElement child)
         {
             VisualElement.Add(child);
             return (TBuilder)this;
         }

         // 🆕 Add a child builder (auto-builds its element)
         public TBuilder AddChild<TChildElement, TChildBuilder>(IUIBuilder<TChildElement, TChildBuilder> childBuilder) where TChildElement : VisualElement where TChildBuilder : IUIBuilder<TChildElement, TChildBuilder>
         {
             VisualElement.Add( childBuilder.Build() );
             return (TBuilder)this;
         }

         // Optional helper for multiple children at once
         public TBuilder AddChildren<TChildElement, TChildBuilder>(IEnumerable<IUIBuilder<TChildElement, TChildBuilder>> childBuilders) where TChildElement : VisualElement where TChildBuilder : IUIBuilder<TChildElement, TChildBuilder>
         {
             foreach( var child in childBuilders )
             {
                 VisualElement.Add( child.Build() );
             }
             return (TBuilder)this;
         }
        
         //Ends
         
         
         //Events
         
         private EventCallback<ClickEvent> _clickCallback;
         
         public virtual TBuilder OnClick(Action onClick)
         {
             ClearOnClick();

             _clickCallback = evt =>
             {
                 onClick.Invoke();
                 evt.StopPropagation();
             };



             VisualElement.RegisterCallback(_clickCallback);

             // Optionally make sure pointer events are enabled
             VisualElement.pickingMode = PickingMode.Position;

             return (TBuilder)this;
         }
         
         private void ClearOnClick()
         {
             if (_clickCallback != null)
             {
                 VisualElement.UnregisterCallback(_clickCallback);
                 _clickCallback = null;
             }
         }
         
         //Events end
        
     }
 }