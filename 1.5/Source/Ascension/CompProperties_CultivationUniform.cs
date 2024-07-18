using Verse;

namespace Ascension
{
    public class CompProperties_CultivationUniform : CompProperties
    {
        public float qiRecSpeedOffset = 0f;
        public float cultSpeedOffset = 0f;

        public CompProperties_CultivationUniform()
        {
            compClass = typeof(CompCultivationUniform);
        }
    }
}

