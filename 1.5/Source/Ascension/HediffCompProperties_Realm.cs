using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class HediffCompProperties_Realm : HediffCompProperties
    {



        public Color labelColor;

        [MustTranslate]
        public string letterLabel;

        [MustTranslate]
        public string letterText;

        public HediffCompProperties_Realm()
        {
            compClass = typeof(HediffComp_MessageAfterTicks);
        }
    }
}
