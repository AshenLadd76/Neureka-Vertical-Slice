using UnityEngine.UIElements;

namespace UiFrameWork.Builders
{
    public class LabelBuilder : BaseBuilder<Label, LabelBuilder>
    {
        public LabelBuilder SetText(string text)
        {
            _visualElement.text = text;
            return this;
        }
        
    }
}
