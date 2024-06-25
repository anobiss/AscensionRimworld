using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class QiPool_Hediff : HediffWithComps
    {
        public long amount = 0;
        public long maxAmount;
        public float maxAmountOffset = 1f;
        public int realmMaxAmountOffset = 0; 
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
            maxAmount = (long)Math.Floor((pawn.RaceProps.baseBodySize * 100f) * maxAmountOffset);
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
