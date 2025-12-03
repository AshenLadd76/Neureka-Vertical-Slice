using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolBox.Extensions
{
   
    
    public static class CollectionExtensions
    {
        
        private static Random rng = new Random();
        
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();
        
        public static bool IsIndexValid<T>(this IList<T> list, int index) => list != null && index >= 0 && index < list.Count;
        
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
    }
}
