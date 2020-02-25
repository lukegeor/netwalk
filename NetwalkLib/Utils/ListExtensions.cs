using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetwalkLib.Utils
{
    public static class ListExtensions
    {
        private static Random rng = new Random();

        public static List<T> Shuffle<T>(this IList<T> list)  
        {  
            var newList = new List<T>(list);
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = rng.Next(n + 1);  
                var value = newList[k];  
                newList[k] = newList[n];  
                newList[n] = value;  
            }

            return newList;
        }
    }
}