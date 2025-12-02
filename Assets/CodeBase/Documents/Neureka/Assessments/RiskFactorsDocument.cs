using CodeBase.Documents.DemoA;
using CodeBase.UiComponents.Styles;
using ToolBox.Services.Haptics;
using UiFrameWork.Components;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Documents.Neureka.Assessments
{
    public class RiskFactorsDocument : BaseDocument
    {
        private const string MainContainerStyle = "fullscreen-container";
        
        private VisualElement _documentRoot;
        
        private AssessmentState _assessmentState;

        private int _progressIndex; 
        
        protected override void Build()
        {
            base.Build();
            Logger.Log( $"Building Risk factors assessment document" );

            _assessmentState = AssessmentState.Continuing;
            
            CheckAssessmentStatus();
        }

        private void CheckAssessmentStatus()
        {
            switch (_assessmentState)
            {
                case AssessmentState.New:
                    LoadIntro();
                    break;
                case AssessmentState.Continuing:
                    LoadNextSection();
                    break;
                case AssessmentState.Completed:
                    LoadEndSection();
                    break;
                default:
                    Logger.LogError("Unknown assessment state");
                    break;
            }

        }

        private void LoadIntro()
        {
            Logger.Log( $"Loading intro pages" );
            
        }

        private void LoadNextSection()
        {
            Logger.Log( $"Loading next section" );
            //Increment progress index;
            
            //Get next questionnaire id
            
            //Request questionnaire from questionnaire builder
            
            //Pass this method as a callback
            
        }

        private void LoadEndSection()
        {
            Logger.Log( $"Loading end section" );
        }
        
        

        private void BuildIntroPage()
        {
            var pageRoot = new ContainerBuilder().AddClass(MainContainerStyle).AttachTo(DocumentRoot).Build();
            
            CreateHeader(pageRoot);
            
            CreateContent(pageRoot);
            
            CreateFooter(pageRoot);
        }
        
        private void CreateHeader(VisualElement parent)
        {
            var headerNav = new ContainerBuilder().AddClass("header-nav").AttachTo(parent).Build();
            
            new ButtonBuilder().SetText("X")
                .OnClick(() => { Logger.Log( "Click X Button" ); })
                .AddClass("demo-header-button")
                .AddClass(UiStyleClassDefinitions.HeaderLabel)
                .AttachTo(headerNav)
                .Build();
            
            var headerTitle =  new ContainerBuilder().AddClass("header-title").AttachTo(parent).Build();

            var label = new LabelBuilder().SetText("Risk Factors").AddClass("header-label").AttachTo(headerTitle).Build();
            
        }

        private void CreateContent(VisualElement parent)
        {
            var content = new ContainerBuilder().AddClass(UssClassNames.BodyContainer).AttachTo(parent).Build();

            //Build the scrollview and add it to the content container
            var scrollview = new ScrollViewBuilder().EnableInertia(true).SetPickingMode(PickingMode.Position)
                .AddClass(UssClassNames.ScrollView).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).Build();
            
            
            content.Add(scrollview);
        }
        
        private Button _continueButton;
        private void CreateFooter(VisualElement parent)
        {
            var footerContainer  = new ContainerBuilder().AddClass("questionnaire-footer").AttachTo(parent).Build();
            
            _continueButton = new ButtonBuilder().SetText("Continue").AddClass("questionnaire-footer-button").OnClick(() =>
            {
                HapticsHelper.RequestHaptics( HapticType.Low );
                HandleContinue();
                
            }).AttachTo(footerContainer).Build();
            
        }

        private void HandleContinue()
        {
            Logger.Log( "Click Continue Button" );
        }
    }

    public enum AssessmentState
    {
        New,
        Continuing,
        Completed
    }
}
