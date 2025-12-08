using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Documents.Neureka.Assessments.RiskFactors
{
    [CreateAssetMenu(fileName = "RiskFactorsSO", menuName = "Neureka/Assessments/RiskFactorsSO")]
    public class RiskFactorsSO: ScriptableObject
    {
        [Header("Intro Page")]
        [SerializeField] private string introTitle = "Risk Factors";
        [SerializeField] private string introButtonText = "Start";
        [SerializeField] private List<BlurbContent> introBlurbContents = new();

        [Header("Continue Page")]
        [SerializeField] private string continueTitle = "Risk Factors";
        [SerializeField] private string continueButtonText = "Continue";
        [SerializeField] private List<BlurbContent> continueBlurbContents = new();

        [Header("Outro Page")]
        [SerializeField] private string outroTitle = "Why This Matters";
        [SerializeField] private string outroButtonText = "Finish";
        [SerializeField] private List<BlurbContent> outroBlurbContents = new();

        // Properties for read-only access
        public string IntroTitle => introTitle;
        public string IntroButtonText => introButtonText;
        public List<BlurbContent> IntroBlurbContents => introBlurbContents;

        public string ContinueTitle => continueTitle;
        public string ContinueButtonText => continueButtonText;
        public List<BlurbContent> ContinueBlurbContents => continueBlurbContents;

        public string OutroTitle => outroTitle;
        public string OutroButtonText => outroButtonText;
        public List<BlurbContent> OutroBlurbContents => outroBlurbContents;
    }

    [System.Serializable]
    public class BlurbContent
    {
        [SerializeField] private string id;
        [SerializeField, TextArea] private string text;
        [SerializeField] private string imagePath;
    
        public string Id => id;
        public string Text => text;
        public string ImagePath => imagePath;
    
        public BlurbContent(string id, string text, string imagePath)
        {
            this.id = id;
            this.text = text;
            this.imagePath = imagePath;
        }
    }
}
