// Assembly-CSharp, Version=1.5.8890.16364, Culture=neutral, PublicKeyToken=null
// Verse.CompProperties_HeatPusher
using Verse;
namespace Ascension
{
    public class CompProperties_ConstantHeatPusher : CompProperties
    {
        public float heat;

        public CompProperties_ConstantHeatPusher()
        {
            compClass = typeof(CompConstantHeatPusher);
        }
    }
}


