using System;
using System.Drawing;

namespace CarcassoneAPI.Helpers
{
    public static class ColorsHelper
    {
        private static Random randomGen = new Random();
        
        public static string GetRandomKnownColor()
        {
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);

            return randomColor.Name;

        }
    }
}
