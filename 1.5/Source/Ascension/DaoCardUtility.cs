using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using static System.Collections.Specialized.BitVector32;
using Verse.Sound;
using static HarmonyLib.Code;
using Unity.Jobs;
using Verse.Noise;
using System.Reflection.Emit;
using RimWorld.Planet;

namespace Ascension
{
    [StaticConstructorOnStartup]
    public static class DaoCardUtility
    {


        private static bool CanControl(Pawn pawn)
        {
            Pawn selPawnForGear = pawn;
            if (selPawnForGear.Downed || selPawnForGear.InMentalState)
            {
                return false;
            }
            if (selPawnForGear.Faction != Faction.OfPlayer && !selPawnForGear.IsPrisonerOfColony)
            {
                return false;
            }
            if (selPawnForGear.IsPrisonerOfColony && selPawnForGear.Spawned && !selPawnForGear.Map.mapPawns.AnyFreeColonistSpawned)
            {
                return false;
            }
            if (selPawnForGear.IsPrisonerOfColony && (PrisonBreakUtility.IsPrisonBreaking(selPawnForGear) || (selPawnForGear.CurJob != null && selPawnForGear.CurJob.exitMapOnArrival)))
            {
                return false;
            }
            return true;
        }

        private static bool highlight = true;

        private static float scrollViewHeight = 0f;


        private static bool onTechniqueTab = false;

        private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

        private static readonly Color StaticHighlightColor = new Color(0.75f, 0.75f, 0.85f, 1f);

        public static void DrawPawnDaoCard(Rect outRect, Pawn pawn, Thing thingForMedBills)
        {
            
            outRect.y += 20f;
            outRect.height -= 20f;
            outRect = outRect.Rounded();
            Rect rect = new Rect(outRect.x, outRect.y, outRect.width * 1f, outRect.height).Rounded();
            Rect rect2 = new Rect(rect.xMax, outRect.y, outRect.width - rect.width, outRect.height);
            rect.yMin += 11f;
            DrawDaoSummary(rect, pawn, thingForMedBills);
        }

        public static void DrawDaoSummary(Rect rect, Pawn pawn, Thing thingForMedBills)
        {
            GUI.color = Color.white;
            Widgets.DrawMenuSection(rect);
            List<TabRecord> list = new List<TabRecord>
        {
            new TabRecord("AS_Cultivation".Translate(), delegate
            {
                onTechniqueTab = false;
            }, !onTechniqueTab)};
            list.Add(new TabRecord("AS_Technique".Translate(), delegate
            {
                onTechniqueTab = true;
            }, onTechniqueTab));
            TabDrawer.DrawTabs(rect, list);
            rect = rect.ContractedBy(2f);
            Widgets.BeginGroup(rect);
            float curY = 0f;
            Text.Font = GameFont.Medium;
            GUI.color = Color.white;
            Text.Anchor = TextAnchor.UpperCenter;
            if (onTechniqueTab)
            {
                PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.MedicalOperations, KnowledgeAmount.FrameDisplayed);
                curY = DrawTechniqueTab(rect, pawn, curY);
            }
            else
            {
                curY = DrawCultivationTab(rect, pawn, curY);
            }
            Text.Font = GameFont.Small;
            GUI.color = Color.white;
            Text.Anchor = TextAnchor.UpperLeft;
            
            Widgets.EndGroup();
        }

        //techniquetab methods here
        private static void DrawTechniqueAbilities(Rect rect, Scroll_Hediff scrollHediff, float curTechBarY, float curY)
        {
            float curAbilityX = rect.x;
            float curAbilityY = curTechBarY;//add itself to y and reset x every time we add a new line
            HediffComp_AddScrollAbility scrollAbilityComp = scrollHediff.TryGetComp<HediffComp_AddScrollAbility>();
            if (scrollAbilityComp != null)
            {
                foreach (AbilityDef abilityDef in scrollAbilityComp.Props.scrollCompAbilityDefList)
                {
                    Rect abilityRect = new Rect(curAbilityX, curAbilityY - 40f, 50f, 50f);
                    GUI.color = Color.white;
                    Ability ability = scrollHediff.pawn.abilities.GetAbility(abilityDef, true);
                    if (ability == null)
                    {
                        if (Widgets.ButtonImage(abilityRect, ContentFinder<Texture2D>.Get(abilityDef.iconPath), true, abilityDef.description))
                        {
                            if (CanControl(scrollHediff.pawn))
                            {
                                scrollHediff.pawn.abilities.GainAbility(abilityDef);
                            }
                        }
                    }
                    else
                    {
                        GUI.color = Color.grey;
                        if (Widgets.ButtonImage(abilityRect, ContentFinder<Texture2D>.Get(abilityDef.iconPath), true, abilityDef.description))
                        {
                            if (CanControl(scrollHediff.pawn))
                            {
                                scrollHediff.pawn.abilities.RemoveAbility(abilityDef);
                            }
                        }
                    }
                    curAbilityX += abilityRect.width + 5f;

                }
            }

        }
        private static Vector2 scrollPosition = Vector2.zero;

        private static float DrawTechniqueTab(Rect rect, Pawn pawn, float curY)
        {
            //techniq tab shows a list of learned scrolls and thier abilities with the abilities being buttons that toggle them on or off the pawn

            //curY += 2f;
            Widgets.BeginGroup(rect);
            float curTechBarY = 0;// keeps track of current total height for placing new ones.
            float num = rect.x + 17f;
            Rect outRect = new Rect(0f, 0f, rect.width, rect.height - curTechBarY);
            Rect viewRect = new Rect(0f, 0f, rect.width - 16f, scrollViewHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

            if (!pawn.health.hediffSet.hediffs.NullOrEmpty())
            {
                foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                {
                    HediffDef hediffDef = hediff.def;
                    if (AscensionStaticStartUtils.ScrollHediffList.Contains(hediffDef))
                    {
                        Scroll_Hediff scrollHediff = hediff as Scroll_Hediff;
                        if (scrollHediff != null)//do the technique bars here
                        {
                            //first we make the rect and the label for the technique art
                            Rect techLabelRect = new Rect(num, curY+curTechBarY, rect.width / 2f - num, rect.height/5);//label rect
                            Text.Anchor = TextAnchor.UpperCenter;
                            GUI.color = scrollHediff.LabelColor;
                            Widgets.Label(techLabelRect, scrollHediff.Label);
                            curTechBarY += techLabelRect.height;
                            DrawTechniqueAbilities(rect, scrollHediff, curTechBarY, curY);//does abilities and the buttons
                        }
                    }
                }
            }
            Widgets.EndScrollView();
            Widgets.EndGroup();
            curY += 10f;
            curY += 6f;
            return curY;//so we know what y the next one is.
        }

        private static void DrawPawnRealm(Rect rect, Realm_Hediff realm)
        {
            float num = rect.x + 17f;
            Rect rect2 = new Rect(num, rect.y + rect.height / 2f - 16f, 32f, 32f);
            num += 42f;
            num += (rect.width / 2f) + 10f;
            Rect rect4 = new Rect(rect2.x, rect.y + rect.height / 2f - 16f, 0f, 32f);
            Rect rect5 = new Rect(num, rect.y + rect.height / 2f - 16f, rect.width - num - 26f, 32f);
            rect4.xMax = rect5.xMax;
            if (Mouse.IsOver(rect4))
            {
                Widgets.DrawHighlight(rect4);
                TooltipHandler.TipRegion(rect4, realm.CurStage.extraTooltip);
            }
            float realmRatio = (float)realm.progress / (float)realm.maxProgress;
            float realmColorMod = realm.Severity / 5f;
            if (realm.def == AscensionDefOf.EssenceRealm)
            {
                GUI.color = new Color(0.8f, 0.8f, 1f - realmColorMod, 1f);//sets bar color based on severity
                Widgets.FillableBar(rect4.ContractedBy(4f), realmRatio);
                GUI.color = Color.white;
                Text.Anchor = TextAnchor.MiddleCenter;
                GUI.color = new Color(1f, 1f, 1f - realmColorMod, 1f);//sets text color based on severity
                Text.Font = GameFont.Small;
                Widgets.Label(rect4.ContractedBy(0.5f), "AS_EssenceRealm".Translate() + realm.Label + " - " + realm.progress + "/" + realm.maxProgress);
            }else if (realm.def == AscensionDefOf.BodyRealm)
            {
                GUI.color = new Color(0.8f, 1f - realmColorMod, 1f - realmColorMod, 1f);//sets bar color based on severity
                Widgets.FillableBar(rect4.ContractedBy(4f), realmRatio);
                GUI.color = Color.white;
                Text.Anchor = TextAnchor.MiddleCenter;
                GUI.color = new Color(1f, 1f - realmColorMod, 1f - realmColorMod, 1f);//sets bar color based on severity
                Text.Font = GameFont.Small;
                Widgets.Label(rect4.ContractedBy(0.5f), "AS_BodyRealm".Translate() + realm.Label + " - " + realm.progress + "/" + realm.maxProgress);
            }else
            {
                Log.Message("Ascension error: the realm attempting to be drawn is not body or essence");
            }
        }

        //make draw pawn realms that works like this


        private static void DrawPawnQi(Pawn pawn, Rect rect, QiPool_Hediff qiPool)
        {
            float num = rect.x + 17f;
            Rect rect2 = new Rect(num, rect.y + rect.height / 2f - 16f, 32f, 32f);
            num += 42f;
            Text.Anchor = TextAnchor.MiddleLeft;
            Rect qiLabelRect = new Rect(num, rect.y, rect.width / 2f - num, rect.height);//label rect
            Widgets.Label(qiLabelRect, "AS_QiPool".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            num += qiLabelRect.width + 10f;
            Rect rect4 = new Rect(rect2.x, rect.y + rect.height / 2f - 16f, 0f, 32f);
            Rect rect5 = new Rect(num, rect.y + rect.height / 2f - 16f, rect.width - num - 26f, 32f);
            rect4.xMax = rect5.xMax;
            if (Mouse.IsOver(rect4))
            {
                Widgets.DrawHighlight(rect4);
            }
            float qiRatio = (float)qiPool.amount / (float)qiPool.maxAmount;
            GUI.color = Color.white;
            Widgets.FillableBar(rect4.ContractedBy(2f), qiRatio);
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect4, qiPool.amount+"/"+qiPool.maxAmount+" "+ "AS_Qi".Translate());
            Rect rectGathering = new Rect(rect4.x, rect4.y+rect4.height+5f, rect4.width, rect4.height);
            if (CanControl(pawn))
            {
                if (qiPool.amount < qiPool.maxAmount)
                {
                    if (Widgets.ButtonText(rectGathering, "AS_QiGathering".Translate()))
                    {
                        Job job = JobMaker.MakeJob(AscensionDefOf.AS_QiGatheringJob, pawn, CultivationJobUtility.FindCultivationSpot(pawn));
                        SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }
                    if (Mouse.IsOver(rectGathering))
                    {
                        Widgets.DrawHighlight(rectGathering);
                        TooltipHandler.TipRegion(rectGathering, "AS_QiGatheringDesc".Translate());
                    }
                }

            }
            Text.Anchor = TextAnchor.UpperLeft;
        }

        private static void DrawRealmBreakthrough(Pawn pawn, Rect rect, Realm_Hediff realm)
        {

            float num = rect.x + 17f;
            Rect rect2 = new Rect(num, rect.y + rect.height / 2f - 16f, 32f, 32f);
            num += 42f;
            Text.Anchor = TextAnchor.MiddleLeft;
            Rect qiLabelRect = new Rect(num, rect.y, rect.width / 2f - num, rect.height);//label rect
            Widgets.Label(qiLabelRect, "AS_QiPool".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            num += qiLabelRect.width + 10f;
            Rect rect4 = new Rect(rect2.x, rect.y + rect.height / 2f - 16f, 0f, 32f);
            Rect rect5 = new Rect(num, rect.y + rect.height / 2f - 16f, rect.width - num - 26f, 32f);
            rect4.xMax = rect5.xMax;
            if (Mouse.IsOver(rect4))
            {
                Widgets.DrawHighlight(rect4);
            }
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect4, "AS_QiPool".Translate());
            if (Widgets.ButtonText(rect4, "AS_Breakthrough".Translate()))
            {
                Job job;
                SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                if (realm.def == AscensionDefOf.EssenceRealm)
                {
                    job = JobMaker.MakeJob(AscensionDefOf.AS_BreakthroughEssence, pawn, CultivationJobUtility.FindCultivationSpot(pawn));
                    pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }
                else if (realm.def == AscensionDefOf.BodyRealm)
                {
                    job = JobMaker.MakeJob(AscensionDefOf.AS_BreakthroughBody, pawn, CultivationJobUtility.FindCultivationSpot(pawn));
                    pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }
            }
            if (realm.def == AscensionDefOf.EssenceRealm)
            {
                TooltipHandler.TipRegionByKey(rect4, "AS_BreakthroughEssenceDesc");
            }
            else if (realm.def == AscensionDefOf.BodyRealm)
            {
                TooltipHandler.TipRegionByKey(rect4, "AS_BreakthroughBodyDesc");
            }else
            {
                Log.Message("Ascension error: tooltip undecided realm not body or essence.");
            }
        }


        private static void DrawMeditationButton(Pawn pawn, Rect rect, bool isExercise)
        {
            if (isExercise)
            {
                if (Widgets.ButtonText(rect, "AS_Exercise".Translate()))
                {
                    Job job = JobMaker.MakeJob(AscensionDefOf.AS_ExerciseJob, pawn, CultivationJobUtility.FindCultivationSpot(pawn));
                    SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                    pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }
                if (Mouse.IsOver(rect))
                {
                    Widgets.DrawHighlight(rect);
                    TooltipHandler.TipRegion(rect, "AS_ExerciseDesc".Translate());
                }
            }
            else
            {
                QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
                if (qiPool != null)
                {
                    int qiCost = 2 + (qiPool.maxAmount / 10);
                    if (qiPool.amount >= qiCost)
                    {
                        if (Widgets.ButtonText(rect, "AS_RefineQi".Translate()))
                        {
                            Job job = JobMaker.MakeJob(AscensionDefOf.AS_RefineQiJob, pawn, CultivationJobUtility.FindCultivationSpot(pawn));
                            SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                            pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        }
                        if (Mouse.IsOver(rect))
                        {
                            Widgets.DrawHighlight(rect);
                            TooltipHandler.TipRegion(rect, "AS_RefineQiDesc".Translate());
                        }
                    }
                }

            }
        }

        private static void DrawRealms(Pawn pawn, Rect rect, float height, float curY)
        {
            Realm_Hediff realmHediff;
            Rect realmRect = new Rect(0f, curY + height + 4f, rect.width, 40f);
            Rect breakthroughRect = new Rect(0f, curY + height + realmRect.height - 2f, rect.width, 40f);
            Rect meditationRect = new Rect(breakthroughRect.x + (breakthroughRect.width / 4), breakthroughRect.y, breakthroughRect.width / 2, breakthroughRect.height);
            if (pawn.health.hediffSet.HasHediff(AscensionDefOf.EssenceRealm))
            {
                realmHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm) as Realm_Hediff;
                DrawPawnRealm(realmRect, realmHediff);
                if (CanControl(pawn))
                {
                    if (realmHediff.progress >= realmHediff.maxProgress && realmHediff.Severity < 7)// draw body breakthrough
                    {
                        DrawRealmBreakthrough(pawn, breakthroughRect, realmHediff); //draw the breakthrough button when at 100%
                        meditationRect.y += meditationRect.height;
                    }
                    DrawMeditationButton(pawn, meditationRect, false);
                }
            }
            else if (pawn.health.hediffSet.HasHediff(AscensionDefOf.BodyRealm))
            {
                realmHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.BodyRealm) as Realm_Hediff;
                DrawPawnRealm(realmRect, realmHediff);
                if (CanControl(pawn))
                {
                    if (realmHediff.progress >= realmHediff.maxProgress && realmHediff.Severity < 7)// draw body breakthrough
                    {
                        DrawRealmBreakthrough(pawn, breakthroughRect, realmHediff); //draw the breakthrough button when at 100%
                        meditationRect.y += meditationRect.height;
                    }
                    DrawMeditationButton(pawn, meditationRect, true);
                }
            }
            Rect cultivationStatsRect = new Rect(realmRect.x, meditationRect.y + meditationRect.height, realmRect.width, realmRect.height-15f);
            DrawCultivationStats(pawn, cultivationStatsRect);




        }
        private static void DrawCultivationStats(Pawn pawn, Rect rect)
        {

            GUI.color = Color.white;
            Rect SpeedFactorsRect = new Rect(rect.x, rect.y + rect.height, rect.width, rect.height+10f);
            Rect QiTileRect = SpeedFactorsRect;
            QiTileRect.height -= 10f;
            QiTileRect.y += SpeedFactorsRect.height;
            Rect innerCRect = QiTileRect;
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                Widgets.Label(rect, "AS_CultivationSpeed".Translate(AscensionUtilities.UpdateCultivationSpeed(cultivatorHediff).ToString("0.#").Named("SPEED")));

                string translatedSpeedFactorText = "AS_CSFactorBaseOffset".Translate(cultivatorHediff.cultivationBaseSpeed.ToString("0.#").Named("BASE"), cultivatorHediff.cultivationSpeedOffset.ToString("0.#").Named("OFFSET"));
                if (pawn.needs.mood != null)
                {
                    translatedSpeedFactorText += "AS_CSFactorMood".Translate(pawn.needs.mood.CurLevelPercentage.ToString("0.#").Named("MOOD"));
                }
                QiGatherMapComponent qiGatherMapComp = cultivatorHediff.pawn.Map.GetComponent<QiGatherMapComponent>();
                int qiTile = 0;
                if (qiGatherMapComp != null)
                {
                    qiTile = qiGatherMapComp.GetQiGatherAt(pawn.Position.x, pawn.Position.y);
                    if (pawn.health.hediffSet.HasHediff(AscensionDefOf.EssenceRealm))
                    {
                        translatedSpeedFactorText += "AS_CSFactorQiTile".Translate((qiTile / 100f).ToString("0.#").Named("QITILE"));
                    }
                }

                Widgets.Label(SpeedFactorsRect, translatedSpeedFactorText);

                if (qiGatherMapComp != null)
                {
                    Widgets.Label(QiTileRect, "AS_QiTile".Translate(qiTile.Named("QITILE")));
                    innerCRect.y += QiTileRect.height;
                }

                Realm_Hediff essenceHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.EssenceRealm, false) as Realm_Hediff;
                string innerCText = "AS_InnerC".Translate(cultivatorHediff.innerCauldronQi.Named("ICQI"), cultivatorHediff.innerCauldronLimit.Named("ICMAX"));
                string innerCDesc = "AS_InnerCDesc".Translate();
                string innerCJob = "AS_InnerCJob".Translate();
                string innerCJobDesc = "AS_InnerCJobDesc".Translate();
                GUI.color = Color.yellow;
                if (essenceHediff != null)
                {
                    if (essenceHediff.Severity >= 3)
                    {
                        GUI.color = Color.magenta;
                        innerCText = "AS_AnimaC".Translate(cultivatorHediff.innerCauldronQi.Named("ICQI"));
                        innerCDesc = "AS_AnimaCDesc".Translate();
                        innerCJob = "AS_AnimaCJob".Translate();
                        innerCJobDesc = "AS_AnimaCJobDesc".Translate();
                    }
                }

                Widgets.Label(innerCRect, innerCText);
                Rect innerCButtonRect = innerCRect;
                innerCButtonRect.y += innerCRect.height;
                innerCButtonRect.x += innerCButtonRect.width / 4;
                innerCButtonRect.width /= 2f;

                QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;

                if (qiPool != null)
                {
                    int qiCost = 2 + (qiPool.maxAmount / 50);// 2% + 2
                    if (qiPool.amount >= qiCost)
                    {
                        if (Widgets.ButtonText(innerCButtonRect, innerCJob))
                        {
                            Job job = JobMaker.MakeJob(AscensionDefOf.AS_RefineQiCauldronJob, pawn, CultivationJobUtility.FindCultivationSpot(pawn));
                            SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                            pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        }

                        if (Mouse.IsOver(innerCButtonRect))
                        {
                            Widgets.DrawHighlight(innerCButtonRect);
                            TooltipHandler.TipRegion(innerCButtonRect, innerCJobDesc);
                        }
                    }
                }

                GUI.color = Color.white;


                if (Mouse.IsOver(rect))
                {
                    Widgets.DrawHighlight(rect);
                    TooltipHandler.TipRegion(rect, "AS_CultivationSpeedDesc".Translate());
                }

                if (Mouse.IsOver(SpeedFactorsRect))
                {
                    Widgets.DrawHighlight(SpeedFactorsRect);
                    TooltipHandler.TipRegion(SpeedFactorsRect, "AS_CSFactorsDesc".Translate());
                }

                if (Mouse.IsOver(QiTileRect))
                {
                    Widgets.DrawHighlight(QiTileRect);
                    TooltipHandler.TipRegion(QiTileRect, "AS_QiTileDesc".Translate());
                }

                if (Mouse.IsOver(innerCRect))
                {
                    Widgets.DrawHighlight(innerCRect);
                    TooltipHandler.TipRegion(innerCRect, innerCDesc);
                }
            }
        }

        private static void DrawSchedule(Pawn pawn, Rect rect, float height, float curY)
        {
            Cultivator_Hediff cultivatorHediff = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff != null)
            {
                //realm auto cultivation
                Rect autoCultivatorRect = new Rect(rect.x, curY + height + 4f, rect.width, rect.height / 12);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(autoCultivatorRect, "AS_AutoCType".Translate());
                Text.Anchor = TextAnchor.MiddleRight;
                Text.Anchor = TextAnchor.UpperLeft;
                Rect autoRealmButton = new Rect(rect.x, curY + height + 4f + autoCultivatorRect.height, rect.width - 3f, rect.height / 12);
                TaggedString autoCultivatorTypeText;
                TaggedString autoCultivatorTypeDescText;
                if (cultivatorHediff.autoCultivateType == 1)
                {
                    autoCultivatorTypeText = "AS_RealmAuto".Translate();
                    autoCultivatorTypeDescText = "AS_RealmAutoDesc".Translate();
                }
                else if (cultivatorHediff.autoCultivateType == 2)
                {
                    autoCultivatorTypeText = "AS_RealmOnlyAuto".Translate();
                    autoCultivatorTypeDescText = "AS_RealmOnlyAutoDesc".Translate();
                }
                else if (cultivatorHediff.autoCultivateType == 3)
                {
                    autoCultivatorTypeText = "AS_QiGatheringAuto".Translate();
                    autoCultivatorTypeDescText = "AS_QiGatheringAutoDesc".Translate();
                }
                if (Widgets.ButtonText(autoRealmButton, autoCultivatorTypeText))
                {
                    if (cultivatorHediff.autoCultivateType <3)
                    {
                        cultivatorHediff.autoCultivateType += 1;
                    }
                    else if (cultivatorHediff.autoCultivateType >= 3)
                    {
                        cultivatorHediff.autoCultivateType = 1;
                    }
                }
                if (Mouse.IsOver(autoRealmButton))
                {
                    Widgets.DrawHighlight(autoRealmButton);
                    TooltipHandler.TipRegion(autoRealmButton, autoCultivatorTypeDescText);
                }

                float floatValueStart = cultivatorHediff.startTime;
                // Convert the float value to hours and minutes
                int hours = (int)floatValueStart;
                int minutes = (int)((floatValueStart - hours) * 60);

                // Handle the case where the hour part exceeds 12
                hours = hours % 12;
                if (hours == 0)
                {
                    hours = 12; // Set it to 12 if it's 0
                }

                // Create a TimeSpan object
                TimeSpan time = new TimeSpan(hours, minutes, 0);

                // Determine whether it's AM or PM
                string period = (floatValueStart >= 24 || floatValueStart <= 0|| floatValueStart < 12) ? "am" : "pm";

                // Format the TimeSpan to the desired string format
                string formattedTime = time.ToString(@"h\:mm") + period;



                //auto cultivation times
                Rect cultivationStartLabelRect = new Rect(rect.x, curY + height + 4f + autoRealmButton.height*2, rect.width, rect.height / 12);
                Rect cultivationStartSlider = new Rect(rect.x, cultivationStartLabelRect.y+cultivationStartLabelRect.height, rect.width, rect.height / 12);
                Rect cultivationEndLabelRect = new Rect(rect.x, cultivationStartSlider.y + cultivationStartSlider.height, rect.width, rect.height / 12);
                Rect cultivationEndSlider = new Rect(rect.x, cultivationEndLabelRect.y + cultivationEndLabelRect.height, rect.width, rect.height / 12);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(cultivationStartLabelRect, "AS_AutoCStart".Translate()+ formattedTime);
                cultivatorHediff.startTime = Widgets.HorizontalSlider(cultivationStartSlider, cultivatorHediff.startTime, 0f, 24f);


                float floatValueEnd = cultivatorHediff.endTime;
                // Convert the float value to hours and minutes
                hours = (int)floatValueEnd;
                minutes = (int)((floatValueEnd - hours) * 60);

                // Handle the case where the hour part exceeds 12
                hours = hours % 12;
                if (hours == 0)
                {
                    hours = 12; // Set it to 12 if it's 0
                }

                // Create a TimeSpan object
                time = new TimeSpan(hours, minutes, 0);

                // Determine whether it's AM or PM
                period = (floatValueEnd < 12 && floatValueEnd >= 1) ? "am" : "pm";

                // Format the TimeSpan to the desired string format
                formattedTime = time.ToString(@"h\:mm") + period;

                Widgets.Label(cultivationEndLabelRect, "AS_AutoCEnd".Translate() + formattedTime);
                cultivatorHediff.endTime = Widgets.HorizontalSlider(cultivationEndSlider, cultivatorHediff.endTime, 0f, 24f);

            }

        }
        private static float DrawCultivationTab(Rect rect, Pawn pawn, float curY)
        {
            Widgets.BeginGroup(rect);
            Rect qiBarRect = new Rect(0f, curY, rect.width, 40f);
            QiPool_Hediff qiPool = pawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;
            if (qiPool != null)
            {
                DrawPawnQi(pawn, qiBarRect, qiPool);
            }
            Widgets.DrawLineHorizontal(0f, curY + qiBarRect.height + 35, rect.width, Color.gray);
            Rect realmsRect = new Rect(rect.x+rect.width, rect.y, (rect.width/3)*2, rect.height);

            //Realms rect should include cultivation stats.



            float underQiHeight = qiBarRect.height + 45f;
            DrawRealms(pawn, realmsRect, underQiHeight, curY);
            GUI.color = Color.white;
            //the schedule manages the times of day to auto cultivate and the realm to cultivate
            float scheduleRectWidth = rect.width / 3;
            float scheduleRectX = rect.x + realmsRect.width;
            // Create the left side rectangle with the same height as the reference rectangle
            Rect scheduleRect = new Rect(scheduleRectX, rect.y, scheduleRectWidth, rect.height);
            DrawSchedule(pawn, scheduleRect, underQiHeight, curY);

            //Widgets.DrawLineHorizontal(0f, (rect2.yMax + rect2.y) / 2f, rect.width);

            GUI.color = Color.white;
            Widgets.EndGroup();
            curY += 10f;
            curY += 6f;
            return curY;//so we know what y the next one is.
        }
    }
}

