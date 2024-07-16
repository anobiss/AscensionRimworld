using Verse;

namespace Ascension
{
    public class HediffCompProperties_TribulationOffset : HediffCompProperties
    {
        public float qiOffset = 0f;
        public float strengthOffset = 0f;
        public float speedOffset = 0f;
        public HediffCompProperties_TribulationOffset()
        {
            compClass = typeof(HediffComp_TribulationOffset);
        }
    }
}
