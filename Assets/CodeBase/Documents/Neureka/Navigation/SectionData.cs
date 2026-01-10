using System;
using UnityEngine;

namespace CodeBase.Documents.Neureka.Navigation
{
    public class SectionData
    {
        public string Title { get; set; }
        public int CardCount { get; set; }
        public Color Color { get; set; }
        
        public string DcoumentID { get; set; }

        // Constructor with validation
        public SectionData(string title, int cardCount, Color color, string dcoumentID)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));

            if (cardCount < 0)
                throw new ArgumentOutOfRangeException(nameof(cardCount), "CardCount cannot be negative.");

            // Color is a struct, it can't be null, but you can add checks if needed
            // For example, you might not want fully transparent colors:
            if (color.a < 0f || color.a > 1f)
                throw new ArgumentOutOfRangeException(nameof(color), "Color alpha must be between 0 and 1.");

            Title = title;
            CardCount = cardCount;
            Color = color;
            DcoumentID = dcoumentID;
        }
        
    }
}