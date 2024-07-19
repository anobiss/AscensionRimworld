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
        public ElementEmitMapComponent.Element elementType;
        public int jobType;
        //display current priority, realm and element type here

        private static string TranslateJobType(int jobType)
        {
            string translatedJob = "AS_Any";

            switch (jobType)
            {
                //0 is Any, 1 is exercise, 2 qi gathering, 3 is qi refining, 4 is body breaktrough, 5 is essence breakthrough, 6 is gc breakthrough, 7 is inner cauldron refinement
                case 0:
                    translatedJob = "AS_Any";
                    break;
                case 1:
                    translatedJob = "AS_Exercise";
                    break;
                case 2:
                    translatedJob = "AS_QiGathering";
                    break;
                case 3:
                    translatedJob = "AS_RefineQi";
                    break;
                case 4:
                    translatedJob = "AS_BBreakthrough";
                    break;
                case 5:
                    translatedJob = "AS_EBreakthrough";
                    break;
                case 6:
                    translatedJob = "AS_GoldenCoreBreakthrough";
                    break;
                case 7:
                    translatedJob = "AS_InnerCJob";
                    break;
            }
            return translatedJob;
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if (!respawningAfterLoad)
            {
                priority = Props.priority;
                realmType = Props.realmType;
                elementType = Props.elementType;//none is any
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
                case ElementEmitMapComponent.Element.Wood:
                    elementText = "AS_Wood";
                    break;
                case ElementEmitMapComponent.Element.Fire:
                    elementText = "AS_Fire";
                    break;
                case ElementEmitMapComponent.Element.Earth:
                    elementText = "AS_Earth";
                    break;
                case ElementEmitMapComponent.Element.Metal:
                    elementText = "AS_Metal";
                    break;
                case ElementEmitMapComponent.Element.Water:
                    elementText = "AS_Water";
                    break;
                case ElementEmitMapComponent.Element.None:
                    elementText = "AS_Any";
                    break;
            }
            return "AS_CultivationSpotInspect".Translate(priority.ToString().Named("PRIORITY"), realmTypeText.Translate().Named("REALM"), elementText.Translate().Named("ELEMENT"), TranslateJobType(jobType).Translate().Named("JOBTRANSLATED"));
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
                case ElementEmitMapComponent.Element.None:
                    elementType = ElementEmitMapComponent.Element.Earth;
                    break;
                case ElementEmitMapComponent.Element.Earth:
                    elementType = ElementEmitMapComponent.Element.Metal;
                    break;
                case ElementEmitMapComponent.Element.Metal:
                    elementType = ElementEmitMapComponent.Element.Water;
                    break;
                case ElementEmitMapComponent.Element.Water:
                    elementType = ElementEmitMapComponent.Element.Wood;
                    break;
                case ElementEmitMapComponent.Element.Wood:
                    elementType = ElementEmitMapComponent.Element.Fire;
                    break;
                case ElementEmitMapComponent.Element.Fire:
                    elementType = ElementEmitMapComponent.Element.None;
                    break;
            }
        }

        private void changeJobType()
        {
            switch (jobType)
            {
                //0 is Any, 1 is exercise, 2 qi gathering, 3 is qi refining, 4 is body breaktrough, 5 is essence breakthrough, 6 is gc breakthrough, 7 is inner cauldron refinement
                case 0:
                    jobType = 1;
                    break;
                case 1:
                    jobType = 2;
                    break;
                case 2:
                    jobType = 3;
                    break;
                case 3:
                    jobType = 4;
                    break;
                case 4:
                    jobType = 5;
                    break;
                case 5:
                    jobType = 6;
                    break;
                case 6:
                    jobType = 7;
                    break;
                case 7:
                    jobType = 0;
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
                icon = AscensionTextures.ChangeElement,
            };
            commandE.action = delegate
            {
                changeElementType();
            };
            yield return commandE;

            Command_Action commandJ = new Command_Action()
            {
                defaultLabel = "AS_ChangeJob".Translate(),
                defaultDesc = "AS_ChangeJobDesc".Translate(),
                Order = 6f,
                icon = AscensionTextures.ChangeRealm,
            };
            commandJ.action = delegate
            {
                changeJobType();
            };
            yield return commandJ;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<int>(ref priority, "priority", Props.priority, false);
            Scribe_Values.Look<string>(ref realmType, "realmType", Props.realmType, false);
            Scribe_Values.Look<ElementEmitMapComponent.Element>(ref elementType, "elementType", Props.elementType, false);
        }
    }
}
