using Verse;

namespace Ascension
{
    public class QiPool_Hediff : HediffWithComps
    {
        public float amount = 0;
        public float maxAmount;
        public float maxAmountOffset = 1f;
        public float realmMaxAmountOffset = 1f; 
        public override string SeverityLabel
        {
            get
            {
                string severityText = amount.ToString()+"/"+ maxAmount.ToString();
                severityText += "AS_QPAmountLabel".Translate();
                return severityText;
            }
        }
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            AscensionUtilities.UpdateQiMax(this);
        }
        public override void ExposeData()
        {
            Scribe_Values.Look(ref amount, "amount");
            Scribe_Values.Look(ref maxAmount, "maxAmount");
            Scribe_Values.Look(ref maxAmountOffset, "maxAmountOffset");
            base.ExposeData();
        }
    }
}
