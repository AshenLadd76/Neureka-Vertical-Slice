#if UNITY_EDITOR

using UiFrameWork.Builders;
using UiFrameWork.Components;
using UiFrameWork.Helpers;
using UiFrameWork.Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiFrameWork.Editor
{
    public class TestEditorWindow : EditorWindow
    {
        // 1️⃣ Menu item to open the window
        [MenuItem("Tools/UI Toolkit/Test Button Builder")]
        public static void ShowWindow()
        {
            var window = GetWindow<TestEditorWindow>();
            window.titleContent = new GUIContent("Test Button Builder");
        }

        private void OnEnable()
        {
            var root = rootVisualElement;
            
           UssLoader.LoadAllUssFromFolder( root, $"Uss/" );
           
           root.AddToClassList( "window-root" );
           
            root.Add(BuildHeader());
           
            // Create a parent container
            var body = BuildBody();
            
           // BuildTextBox(body);
           // BuildSpacer(body);
           // BuildTextBox(body);
           // BuildSpacer(body);
          //  BuildCheckBox(body);
           // BuildLabel(body);
           // BuildProgressBar(body);

          //  BuildSpacer(body);
           // BuildSlider(body);
           
            var panel = BuildPanel(body);
           
            BuildTexture2D(panel);
            
            
            var panel2 = BuildPanel(body);
            
            BuildTexture2D(panel2);
           
            BuildScrollView( root, body );
            
            //root.Add(body);


            var footer = BuildFooter();
            
            root.Add(footer);

            BuildButton(footer);

            // Add the parent to your root
        }

        private VisualElement BuildBody()
        {
            // Create a parent container
            var  body  = new VisualElement();
            body.AddToClassList("body");
            
            // Spacer pushes the button row to the bottom
            var spacer = new VisualElement();
            spacer.AddToClassList("flex-spacer");
            body.Add(spacer);
            
            body.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                body.style.flexDirection = evt.newRect.width > evt.newRect.height ? FlexDirection.Row : FlexDirection.Column;
            });
            
            return body;
        }


        
        private VisualElement BuildHeader()
        {
            var header  = new VisualElement();
            header.AddToClassList("header");
            
            return header;
        }

        private VisualElement BuildFooter()
        {
            // Button row container
            var  footer = new VisualElement();
            footer.AddToClassList("footer");
            
            return footer;
        }

        private void BuildTextBox(VisualElement parent = null)
        {
            new TextFieldBuilder()
                .SetText("Default text...")
                .OnValueChanged((s) => { Debug.Log( s ); })
                .SetMultiline( false )
                .AddClass( "text-field" )
                .AttachTo( parent )
                .Build();
        }

        private VisualElement BuildSpacer(VisualElement parent = null)
        {
            return new SpacerBuilder()
                .SetSize(height: 20)
                .AttachTo(parent)
                .Build();
        }

        private VisualElement BuildCheckBox(VisualElement parent = null)
        {
             return new CheckBoxBuilder()
                .SetLabel( "CheckBox" )
                .SetValue( false )
                .OnValueChanged((b) => { Debug.Log($"Check Box Value : {b}"); })
                .AddClass("checkbox")
                .AttachTo( parent )
                .Build();
            
        }

        private VisualElement BuildScrollView(VisualElement parent = null, VisualElement child = null)
        {
            return new ScrollViewBuilder()
                .SetOrientation(ScrollViewMode.Vertical)
                .HideScrollBars(ScrollerVisibility.Hidden, ScrollerVisibility.Hidden)
                .AddElement(child)
                .AddClass( "scroll-view" )
                .AttachTo( parent )
                .Build();
        }

        private VisualElement BuildButton(VisualElement parent = null)
        {
            return new ButtonBuilder()
                .SetText("Test Button Builder")
                .OnClick(() => { Debug.Log($"this works aok !!!1"); })
                .AddClass("btn-base")
                .AddClass("btn-large")
                .AttachTo( parent )
                .Build();
        }

        private VisualElement BuildLabel(VisualElement parent = null)
        {
            return new LabelBuilder()
                .SetText("An example label")
                .AddClass("header-label")
                .AttachTo(parent)
                .Build();
        }
        
        

        private void BuildProgressBar(VisualElement parent = null)
        {
            // new ProgressBarBuilder()
            //     
            //     .SetFillClass("progress-bar__fill")
            //     .OnMinReached(() => { Debug.Log($"Progress bar is empty"); })
            //     .OnMaxReached(() => { Debug.Log($"Progress bar is full"); })
            //     .AddClass("progress-bar")
            //     .AddClass("progress-bar__label")
            //     .SetMaxFill(300)
            //     .SetFillAmount(150)
            //     .AttachTo(parent)
            //     .Build();
        }

        private void BuildSlider(VisualElement parent = null)
        {
            new SliderBuilder()
                .SetLabelText( "Slider Label" )
                .SetDirection(SliderDirection.Horizontal)
                .SetWidth( 350 )
                .SetHeight( 150 )
                .SetMinValue(0)
                .SetMaxValue(100)
                .SetCurrentValue(10)
                .SetStep(1f)
                .Invert(false)
                .Visible(true)
                .OnValueChanged((v) => { Debug.Log(v); })
                .AttachTo(parent)
                .Build();
        }
        
        

        private void BuildTexture2D(VisualElement parent = null)
        {
            int size = 512;
            int bitSize = 16;
            int ppu = 16;
            
            Color32 white = new Color32(255, 255, 255, 255);
            Color32 gray = Color.gray;
            
            
            var texture2dBuilder = new Texture2DBuilder()
                .SetWidth(size)
                .SetHeight(size)
                .SetFilterMode(FilterMode.Point)
                .SetPixelsPerUnit(ppu)
                .SetPixels(TexturePatterns.GenerateCheckerPattern(size, size, bitSize, white, gray))
                .AttachTo(parent)
                .BuildImage();
        }

        private VisualElement BuildPanel(VisualElement parent = null,  VisualElement child = null)
        {
            return new ContainerBuilder().AddClass("panel")
                .AttachTo(parent)
                .Build();
        }
    }
}

#endif

