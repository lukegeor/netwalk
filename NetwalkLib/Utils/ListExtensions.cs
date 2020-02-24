using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetwalkLib.Utils
{
    public static class ListExtensions
    {
        private static Random rng = new Random();

        public static IList<T> Shuffle<T>(this IList<T> list)  
        {  
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = rng.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }

            return list;
        }
    }
}