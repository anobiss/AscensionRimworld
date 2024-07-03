using Verse;

namespace Ascension
{
    public class HediffCompProperties_QiRecovery : HediffCompProperties
    {
        public float offset = 0f;
        public float cultivationSpeedOffset = 0f;
        public bool spirit = false;
        public bool temp = false;
        public HediffCompProperties_QiRecovery()
        {
            compClass = typeof(HediffComp_QiRecovery);
        }
    }
}
