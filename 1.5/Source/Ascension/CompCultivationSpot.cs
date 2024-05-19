using RimWorld;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Noise;

namespace Ascension
{
    public class CompCultivationSpot : ThingComp
    {

        public CompProperties_CultivationSpot Props => (CompProperties_CultivationSpot)props;
        public int priority;
        public string realmType;
        public string elementType;
        //display current priority, realm and element type here



        public override string CompInspectStringExtra()
        {
            string realmTypeText = "AS_Any";//any is "Any"
            switch (realmType)
            {
                case "Body":
                    realmTypeText = "AS_CSBodyRealm";
                    break;
                case "Essence":
                    realmTypeText = "AS_CSEssenceRealm";
                    break;
            }
            string elementText = "AS_Any";
            switch (elementType)
            {
                case "Wood":
                    elementText = "AS_Wood";
                    break;
                case "Fire":
                    elementText = "AS_Fire";
                    break;
                case "Earth":
                    elementText = "AS_Earth";
                    break;
                case "Metal":
                    elementText = "AS_Metal";
                    break;
                case "Water":
                    elementText = "AS_Water";
                    break;
                case "None":
                    elementText = "AS_None";
                    break;
            }
            return "AS_CultivationSpotInspect".Translate(priority.ToString().Named("PRIORITY"), realmTypeText.Translate().Named("REALM"), elementText.Translate().Named("ELEMENT"));
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                priority = Props.priority;
                realmType = Props.realmType;
                elementType = Props.elementType;
            }
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<int>(ref priority, "priority", Props.priority, false);
            Scribe_Values.Look<string>(ref realmType, "realmType", Props.realmType, false);
            Scribe_Values.Look<string>(ref elementType, "elementType", Props.elementType, false);
        }
    }
}
