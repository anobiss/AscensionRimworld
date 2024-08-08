using Verse;

namespace Ascension
{
    public class HediffCompProperties_QiRecovery : HediffCompProperties
    {
        public float amountOffset = 0f;
        public float amountBaseBonus = 0f;
        public float speedOffset = 0f;
        public float speedBaseBonus = 0f;
        public float cultivationSpeedOffset = 0f;
        public float cultivationSpeedBaseBonus = 0f;
        public float breakthroughChanceBaseBonus = 0f;
        public float breakthroughChanceOffset = 0f;
        public float foundationTrainingOffset = 0f;
        public bool spirit = false;
        public bool temp = false;
        public HediffCompProperties_QiRecovery()
        {
            compClass = typeof(HediffComp_QiRecovery);
        }
    }
}
