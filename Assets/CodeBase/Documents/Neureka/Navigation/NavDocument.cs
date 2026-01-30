using System;
using System.Collections.Generic;
using CodeBase.Documents.Neureka.Components;
using CodeBase.Helpers;
using CodeBase.Services;
using CodeBase.UiComponents;
using CodeBase.UiComponents.Styles;
using ToolBox.Data.Parsers;
using ToolBox.Extensions;
using ToolBox.Messaging;
using UiFrameWork.Components;
using UiFrameWork.RunTime;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = ToolBox.Utils.Logger;
using Random = UnityEngine.Random;

namespace CodeBase.Documents.Neureka.Navigation
{
    
    //TODO REFACTOR !!!
    public class NavDocument : BaseDocument
    {
        private readonly List<VisualElement> _sectionPages = new();
        
        private string[] _aboutMeIds = (new []{ "MCQ-30", "DASS", "AQ", "HHI", "HRSI", "SHAPS", "SQS" }); 
        
        private Color[] _colors = (new []{ new Color( 0.99f, 0.58f, 0.24f  ), new Color(0.17f, 0.66f, 0.79f ), new Color(0.8f, 0.2f, 0.45f), new Color(0.44f, 0.615f, 0.98f ), new Color(0.52f, 0.50f, 0.67f),  new Color(0.38f, 0.8f, 0.51f ), new Color( 0.36f, 0.45f, 0.76f ) });
        
        private const string NavIconResourcePath = "Navigation/NavIconsSo";
       
        
        private VisualElement _navRoot;
        private VisualElement _content;
        private ScrollView _scrollView;
        
        private List<Image> _imageList = new();
        
        private Color _unSelectedColor = new Color(.56f, .74f, .89f, 1f);  
        private Color _selectedColor = new Color(1f, 1f, 1f, 1f);
        private Color _logoColor = new Color(.2f, .5f, .8f, 1f);

        private IReadOnlyDictionary<string, StandardQuestionnaireSo> _standardQuestionnaireDictionary;
        
        private const string BackgroundGradientPath = "Gradients/fade_2";
        private const string InfoBoxBackgroundGradientPath = "Gradients/blue";
        
        public override bool ShouldCache => true;

        public override void Build(VisualElement root)
        {
            if (root == null)
            {
                Logger.Log("Splash Page Build Failed");
                return;
            }
            
            base.Build(root);
            
            _navRoot = new ContainerBuilder().AddClass(UssClassNames.MainContainer).AttachTo(DocumentRoot).Build();
            
            var header = new ContainerBuilder().AddClass(NavUssClassNames.NavHeader).AttachTo(_navRoot).Build();
            
            LoadLogo(header);
            
            var divider = new ContainerBuilder().AddClass(NavUssClassNames.NavDivider).AttachTo(_navRoot).Build();
            
            //Build content
            _content = new ContainerBuilder().AddClass(NavUssClassNames.NavBodyContainer).AttachTo(_navRoot).Build();
            
            _scrollView = new ScrollViewBuilder().EnableInertia().AddClass(NavUssClassNames.NavScrollViewContainer).HideScrollBars( ScrollerVisibility.Hidden, ScrollerVisibility.Hidden ).AttachTo(_content).Build();

            
            new ContainerBuilder().AddClass(NavUssClassNames.NavScrollSpacer).AttachTo(_scrollView).Build();
           
            MessageBus.Broadcast<Action<IReadOnlyDictionary<string, StandardQuestionnaireSo>>>( QuestionnaireService.OnRequestAllQuestionnaireDataMessage, QuestionnaireDataDictionaryCallBack );
            
            LoadNavSections(_scrollView); 
           
            BuildFooter();
           
            SelectNavPage(_sectionPages[0]);
           
            new FadeHelper(_content, true, true);
            
            Open(root);
        }

        public override void Open(VisualElement root)
        {
            base.Open(root);
            
            _navRoot.style.display = DisplayStyle.Flex;
            _content.style.display = DisplayStyle.Flex;
            _scrollView.style.display = DisplayStyle.Flex;
            
            ShowRootAndActivePage();
        }

        public override void Close()
        {
            base.Close();
            _navRoot.style.display = DisplayStyle.None;
        }

        private void LoadLogo(VisualElement parent)
        {
            var logo = new ContainerBuilder().AddClass(NavUssClassNames.NavHeaderLogo).AttachTo(parent).Build();
            
            var logoTexture = Resources.Load<Texture2D>("Sprites/Neureka/logo_neureka");
            
            Logger.Log(logoTexture != null ? "Loaded successfully!" : "Failed to load!");
            
            var logoImage = new ImageBuilder().SetTexture(logoTexture).AddClass("header-logo").AttachTo(logo).Build();

            logoImage.tintColor = _logoColor;
        }

        private void LoadNavSections(ScrollView scrollView)
        {
            _sectionPages.Clear();
            
            BuildNavSection(scrollView, _aboutMeIds);
            
            BuildAssessment(scrollView);
            
            var emptyContainer = new ContainerBuilder().AddClass("scroll-view-content").AttachTo(scrollView).Build();
            
            _sectionPages.Add(emptyContainer);
            _sectionPages.Add(emptyContainer);
        }
        
        private void BuildNavSection(ScrollView scrollView, string[] ids)
        {
            if (ids.IsNullOrEmpty())
            {
                Logger.Log("Building Nav Section Failed Id array is null or empty");
                return;
            }
            
            var container = new ContainerBuilder().AddClass("scroll-view-content").AttachTo(scrollView).Build();
            
            new InfoBoxBuilder().SetText("This section shows examples of questionnaires from the original app, " +
                                         "dynamically generated using UI Toolkit and a custom UI framework. " +
                                         "They demonstrate how the app creates interactive, responsive questionnaires on the fly.")
                .SetAction(() => { Debug.Log("This works ok"); })
                .AttachTo(container).Build();
            
            int count = 0;

            foreach (var t in ids)
            {   
                BuildMenuCard(container, t, _colors[count]);

                count++;
            }

            _sectionPages.Add(container);
        }

        private void BuildAssessment(ScrollView scrollView)
        {
             var iconSprite = Resources.Load<Sprite>("Sprites/Assessments/risk_factors_icon");

             if (iconSprite == null)
             {
                 Logger.Log("Building Assessment Failed Sprite is null");
             }
            
             var container = new ContainerBuilder().AddClass("scroll-view-content").AttachTo(scrollView).Build();
             
             new InfoBoxBuilder().SetText("This is a sample assessment, dynamically created using a custom UI framework built on top of UI Toolkit. " +
                                          "It represents an assessment from the release version of the app.").AttachTo(container).Build();
            
            new MenuCardBuilder()
                .SetParent(container)
                .SetTitle($"Risk Factors")
                .SetBlurb("The assessment looks at key factors associated with increased likelihood of cognitive, behavioral, or neurological decline")
                .SetIcon(iconSprite)
                .SetProgress(Random.Range(0f, 1f))
                .SetIconBackgroundColor( _colors[2] )
                .SetAction( ()=> { MessageBus.Broadcast(nameof(DocumentServiceMessages.OnRequestOpenDocument), DocumentID.RiskFactors); })
                .Build();
            
            _sectionPages.Add(container); 
        }

        private void BuildMenuCard(VisualElement container,  string id, Color color)
        {
            if (_standardQuestionnaireDictionary.IsNullOrEmpty())
            {
                Logger.Log("Building Menu Card Failed Id array is null or empty");
                return;
            }
            
            var cleanId = id.Trim().ToLower();
            
            var standardQuestionnaireData = _standardQuestionnaireDictionary[cleanId];

            var title = standardQuestionnaireData.Data.QuestionnaireName;
            var blurb  = standardQuestionnaireData.Data.QuestionnaireDescription;
            var icon = standardQuestionnaireData.Icon;

            if (icon == null)
            {
                Logger.LogError("Building Menu Card Failed icon is null or empty");
            }
            
            new MenuCardBuilder()
                .SetParent(container)
                .SetTitle($"{title}")
                .SetBlurb(blurb)
                .SetIcon(icon)
                .SetProgress(Random.Range(0f, 1f))
                .SetIconBackgroundColor( color )
                .SetAction(MenuActions.RequestQuestionnaire(cleanId))
                .Build();
            
            container.style.display = DisplayStyle.Flex;
        }
        
        private void BuildFooter()
        {
            var iconList = LoadNavIcons();

            if (iconList.IsNullOrEmpty()) Logger.LogError("Loading nav icons failed ...icon list is empty");
            
            //Build footer
            var footer = new ContainerBuilder().AddClass(NavUssClassNames.NaVFooter).AttachTo(_navRoot).Build();
            
            Logger.Log($"All pages count : {_sectionPages.Count}");
            
            _imageList.Clear();
            
            for (int i = 0; i < _sectionPages.Count; i++)
            {
                int index = i;
                
                // Only assign click event to the first two buttons
                var builder = new ContainerBuilder().AddClass(NavUssClassNames.NavFooterButton);

                if (i < 2) // first two buttons
                {
                    builder.OnClick(() =>
                    {
                        if (_sectionPages.IsNullOrEmpty()) return;

                        SelectNavPage(_sectionPages[index]);
                        SelectIconImage(index);
                    });
                }

                var footerButton = builder.AttachTo(footer).Build();
                
                if (i < iconList.Count)
                {
                    var iconImage = new ImageBuilder().SetSprite(iconList[i].Selected).AddClass(NavUssClassNames.NavFooterIcon).AttachTo(footerButton).Build();
                    
                    iconImage.tintColor = _unSelectedColor;
                    
                    _imageList.Add(iconImage);
                }
                else
                    Logger.LogWarning($"No nav icon assigned for page index {i}");
            }
            
            SelectIconImage(0);
        }

        private List<NavIcon> LoadNavIcons()
        {
            var navIconsSo = Resources.Load<NavigationIconsSo>(NavIconResourcePath);

            if (navIconsSo != null && !navIconsSo.NavIconList.IsNullOrEmpty()) return navIconsSo.NavIconList;
            
            Debug.LogError("Failed to load NavIcons SO!");
            
            return new List<NavIcon>();
        }


        private VisualElement _lastSelectedPage;
        private void SelectNavPage(VisualElement pageToShow)
        {
            if (pageToShow == null && !_sectionPages.IsNullOrEmpty())
                pageToShow = _sectionPages[0];
            
            
            foreach (var page in _sectionPages)
            {
                if (page == null)
                {
                    Logger.Log("SelectNavPage Failed");
                    continue;
                }

                page.style.display = page == pageToShow ? DisplayStyle.Flex : DisplayStyle.None;
            }
            
            _lastSelectedPage = pageToShow;
        }

        private void SelectIconImage(int index)
        {
            if (!_imageList.IsIndexValid(index)) return;
            
            foreach (var image in _imageList)
                image.tintColor = _unSelectedColor;
            
            _imageList[index].tintColor = _selectedColor;
        }
        
        public virtual void ShowRootAndActivePage()
        {
            if (_navRoot == null) return;

            
            _navRoot.style.display = DisplayStyle.Flex;

            // Make the currently selected page visible, if any
            foreach (var page in _sectionPages)
            {
                if (page == null) continue;

                // Show only the last selected page, or the first page
                page.style.display = page == _lastSelectedPage ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void QuestionnaireDataDictionaryCallBack( IReadOnlyDictionary<string, StandardQuestionnaireSo> dictionary )
        {
            if (dictionary.IsNullOrEmpty())
            {
                Logger.LogError( $"Dictionary is null or empty!" );
                return;
            }

            _standardQuestionnaireDictionary = dictionary;
        }

        
        
        private void SetBackGroundGradientTexture(VisualElement parent)
        {
            var gradientTexture = Resources.Load<Texture2D>(InfoBoxBackgroundGradientPath);

            if (gradientTexture == null)
            {
                Logger.Log("Splash Page Build Failed to load Gradient Texture");
                return;
            }

            new ImageBuilder().SetTexture(gradientTexture).AttachTo(parent).SetScaleMode(ScaleMode.StretchToFill).AddClass(UiStyleClassDefinitions.SplashGradient).Build();
        }
    }


    public static class NavUssClassNames
    {
        public const string NavHeader = "nav-header-container";
        public const string NavBodyContainer = "nav-body-container";
        public const string NavScrollViewContainer = "nav-scroll-no-scroll-bars";
        public const string NavDivider = "nav-divider";
        public const string NavHeaderLogo = "nav-header-logo";
        public const string NaVFooter = "nav-footer-container";
        public const string NavFooterButton = "nav-footer-button";
        public const string NavFooterIcon = "nav-footer-icon";
        public const string NavScrollSpacer = "nav-scroll-view-top-spacer";
    }
}
