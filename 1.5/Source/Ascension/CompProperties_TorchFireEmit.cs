using Verse;

namespace Ascension
{
    public class CompProperties_TorchFireEmit : CompProperties
    {
        public int amount = 1;
        public int range = 1;

        public CompProperties_TorchFireEmit()
        {
            compClass = typeof(CompTorchFireEmit);
        }
    }
}

