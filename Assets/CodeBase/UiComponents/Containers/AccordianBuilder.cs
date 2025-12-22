using UiFrameWork.Components;
using UnityEngine.UIElements;

namespace CodeBase.UiComponents.Containers
{
    public class AccordianBuilder
    {

        private VisualElement _root;
        
        
        public AccordianBuilder SetParent(VisualElement parent)
        {
            _root = parent;
            return this;
        }

        public VisualElement Build()
        {
            var accordianContainer = new ContainerBuilder().AttachTo(_root).Build();
            
            var header =  new ContainerBuilder().AttachTo(accordianContainer).Build();

            return accordianContainer;
        }
    
    }
}
