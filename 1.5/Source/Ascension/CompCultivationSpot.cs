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
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if (!respawningAfterLoad)
            {
                priority = Props.priority;
                realmType = Props.realmType;
                elementType = Props.elementType;
            }
            CultivationMapComponent cultivationMapComp = parent.MapHeld.GetComponent<CultivationMapComponent>();
            cultivationMapComp.CultivationSpots.Add(this);

            base.PostSpawnSetup(respawningAfterLoad);
        }
        public override void PostDeSpawn(Map map)
        {
            CultivationMapComponent cultivationMapComp = map.GetComponent<CultivationMapComponent>();
            cultivationMapComp.CultivationSpots.Remove(this);
            base.PostDeSpawn(map);
        }
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

        private void changePriority()
        {
            priority = (priority + 1) % 8;
        }
        private void changeRealmType()
        {
            switch (realmType)
            {
                case "Any":
                    realmType = "Body";
                    break;
                case "Body":
                    realmType = "Essence";
                    break;
                case "Essence":
                    realmType = "Any";
                    break;
            }
        }
        private void changeElementType()
        {
            switch (elementType)
            {
                case "Any":
                    elementType = "Earth";
                    break;
                case "Earth":
                    elementType = "Metal";
                    break;
                case "Metal":
                    elementType = "Water";
                    break;
                case "Water":
                    elementType = "Wood";
                    break;
                case "Wood":
                    elementType = "Fire";
                    break;
                case "Fire":
                    elementType = "Any";
                    break;
            }
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            Command_Action commandP = new Command_Action()
            {
                defaultLabel = "AS_ChangePriority".Translate(),
                defaultDesc = "AS_ChangePriorityDesc".Translate(),
                Order = 5f,
                icon = AscensionTextures.ChangePriority,
            };
            commandP.action = delegate
            {
                changePriority();
            };
            yield return commandP;

            Command_Action commandR = new Command_Action()
            {
                defaultLabel = "AS_ChangeRealm".Translate(),
                defaultDesc = "AS_ChangeRealmDesc".Translate(),
                Order = 6f,
                icon = AscensionTextures.ChangeRealm,
            };
            commandR.action = delegate
            {
                changeRealmType();
            };
            yield return commandR;

            Command_Action commandE = new Command_Action()
            {
                defaultLabel = "AS_ChangeElement".Translate(),
                defaultDesc = "AS_ChangeElementDesc".Translate(),
                Order = 7f,
                icon = AscensionTextures.ChangeRealm,
            };
            commandE.action = delegate
            {
                changeElementType();
            };
            yield return commandE;
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
