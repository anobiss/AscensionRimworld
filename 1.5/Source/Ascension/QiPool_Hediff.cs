using System;
using Verse;

namespace Ascension
{
    public class QiPool_Hediff : HediffWithComps
    {
        public float amount = 0; // the only thing saved, everything else is calculated.
        public float maxAmount;
        public float maxAmountOffset = 1f;

        public float qiRecoveryAmountBase = 0f; //used in calc
        public float qiRecoveryAmount = 0f;//amount after calculated
        public float qiRecoveryAmountOffset = 0f; // used in calc

        public float qiRecoverySpeedBase = 1f;//used in calc
        public float qiRecoverySpeed = 0f;//speed after calculated
        public float qiRecoverySpeedOffset = 0f; // used in calc

        public override string SeverityLabel
        {
            get
            {
                string severityText = amount.ToString()+"/"+ maxAmount.ToString();
                severityText += "AS_QPAmountLabel".Translate();
                return severityText;
            }
        }

        private int ticksToQi;
        public int tickRate = 2500;//in game hour

        public override void PostTick()
        {
            base.PostTick();
            this.ticksToQi--;
            if (this.ticksToQi <= 0)
            {
                AscensionUtilities.UpdateQiRecoveryAmount(this);
                tickRate = (int)Math.Floor(2500 / AscensionUtilities.UpdateQiRecoverySpeed(this));
                AscensionUtilities.IncreaseQi(pawn, AscensionUtilities.UpdateQiRecoveryAmount(this), true);
                ticksToQi = tickRate;
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            AscensionUtilities.UpdateQiMax(this);
        }
        public override void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.ticksToQi, "ticksToQi", 0, false);
            Scribe_Values.Look(ref amount, "amount");
            base.ExposeData();
        }
    }
}
