using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension
{
    public static class AscensionStaticUtils
    {
        public static string PercentageString(float chance, bool isLabel = true)
        {
            if (isLabel == true)
            {
                return (chance * 100f).ToString("0.##");
            }
            else
            {
                return (chance).ToString("0.###");
            }
        }
    }
}
