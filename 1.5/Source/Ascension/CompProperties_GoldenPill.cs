

// Assembly-CSharp, Version=1.5.8890.16364, Culture=neutral, PublicKeyToken=null
// Verse.CompProperties_HeatPusher
using Verse;
namespace Ascension
{
    public class CompProperties_GoldenPill : CompProperties
    {
        public float amount;
        public bool noQuality;

        public CompProperties_GoldenPill()
        {
            compClass = typeof(CompGoldenPill);
        }
    }
}


