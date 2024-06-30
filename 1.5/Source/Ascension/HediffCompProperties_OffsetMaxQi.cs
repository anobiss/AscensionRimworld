using Verse;
namespace Ascension
{
    public class HediffCompProperties_OffsetMaxQi : HediffCompProperties
    {
        public float offset = 0f;
        public bool spirit = false;
        public HediffCompProperties_OffsetMaxQi()
        {
            compClass = typeof(HediffComp_OffsetMaxQi);
        }
    }
}
