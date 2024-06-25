using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace Ascension
{
    [StaticConstructorOnStartup]
    public static class DaoCardUtility
    {


        private static bool CanControl()
        {
            if (selectedPawn.Downed || selectedPawn.InMentalState)
            {
                return false;
            }
            if (selectedPawn.Faction != Faction.OfPlayer && !selectedPawn.IsPrisonerOfColony)
            {
                return false;
            }
            if (selectedPawn.IsPrisonerOfColony && selectedPawn.Spawned && !selectedPawn.Map.mapPawns.AnyFreeColonistSpawned)
            {
                return false;
            }
            if (selectedPawn.IsPrisonerOfColony && (PrisonBreakUtility.IsPrisonBreaking(selectedPawn) || (selectedPawn.CurJob != null && selectedPawn.CurJob.exitMapOnArrival)))
            {
                return false;
            }
            return true;
        }

        private static float scrollViewHeight = 0f;


        private static bool onTechniqueTab = false;

        public static Pawn selectedPawn;
        public static void DrawPawnDaoCard(Rect outRect, Pawn pawn, Thing thingForMedBills)
        {
            selectedPawn = pawn;
            outRect.y += 20f;
            outRect.height -= 20f;
            outRect = outRect.Rounded();
            Rect rect = new Rect(outRect.x, outRect.y, outRect.width * 1f, outRect.height).Rounded();
            Rect rect2 = new Rect(rect.xMax, outRect.y, outRect.width - rect.width, outRect.height);
            rect.yMin += 11f;
            DrawDaoSummary(rect);
        }

        public static void DrawDaoSummary(Rect rect)
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
                curY = DrawTechniqueTab(rect, curY);
            }
            else
            {
                curY = DrawCultivationTab(rect, curY);
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
                            if (CanControl())
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
                            if (CanControl())
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

        private static float DrawTechniqueTab(Rect rect, float curY)
        {
            //techniq tab shows a list of learned scrolls and thier abilities with the abilities being buttons that toggle them on or off the pawn

            //curY += 2f;
            Widgets.BeginGroup(rect);
            float curTechBarY = 0;// keeps track of current total height for placing new ones.
            float num = rect.x + 17f;
            Rect outRect = new Rect(0f, 0f, rect.width, rect.height - curTechBarY);
            Rect viewRect = new Rect(0f, 0f, rect.width - 16f, scrollViewHeight);

            if (!selectedPawn.health.hediffSet.hediffs.NullOrEmpty())
            {
                foreach (Hediff hediff in selectedPawn.health.hediffSet.hediffs)
                {
                    HediffDef hediffDef = hediff.def;
                    if (AscensionStaticStartUtils.ScrollHediffList.Contains(hediffDef))
                    {
                        Scroll_Hediff scrollHediff = hediff as Scroll_Hediff;
                        if (scrollHediff != null)//do the technique bars here
                        {
                            //first we make the rect and the label for the technique art
                            Rect techLabelRect = new Rect(num, curY + curTechBarY, rect.width / 2f - num, rect.height / 5);//label rect
                            Text.Anchor = TextAnchor.UpperCenter;
                            GUI.color = scrollHediff.LabelColor;
                            Widgets.Label(techLabelRect, scrollHediff.Label);
                            curTechBarY += techLabelRect.height;
                            DrawTechniqueAbilities(rect, scrollHediff, curTechBarY, curY);//does abilities and the buttons
                        }
                    }
                }
            }
            Widgets.EndGroup();
            curY += 10f;
            curY += 6f;
            return curY;//so we know what y the next one is.
        }

        //make draw pawn realms that works like this


        private static void DrawPawnQi(Rect rect)
        {
            float labelWidth = rect.width / 2f - 17f;
            Rect qiLabelRect = new Rect(rect.x + 59f, rect.y, labelWidth, rect.height);
            Rect barRect = new Rect(rect.x + 17f, rect.y + rect.height / 2f - 16f, rect.width - 43f, 32f);
            Rect gatherButtonRect = new Rect(barRect.x, barRect.y + barRect.height + 5f, barRect.width, barRect.height);

            Widgets.Label(qiLabelRect, "AS_QiPool".Translate());

            if (Mouse.IsOver(barRect))
            {
                Widgets.DrawHighlight(barRect);
            }

            float qiRatio = (float)qiPoolHediff.amount / (float)qiPoolHediff.maxAmount;
            GUI.color = Color.white;
            Widgets.FillableBar(barRect.ContractedBy(2f), qiRatio);
            string qiBarText = "AS_QiPoolBar".Translate(qiPoolHediff.amount.ToString("0.#").Named("QI"), qiPoolHediff.maxAmount.ToString("0.#").Named("MAXQI"));
            if (CultivatorHediff != null && eRealmHediff != null)
            {
                if (CultivatorHediff.qiRecoverySpeed != 0)
                {
                    qiBarText += "AS_QiPoolBarRecovery".Translate(CultivatorHediff.qiRecoveryAmount.ToString().Named("QIRECOVERYAMOUNT"));
                    if (CultivatorHediff.qiRecoverySpeed < 1)
                    {
                        qiBarText += "AS_QiPoolBarRecoveryHours".Translate(((2500 / CultivatorHediff.qiRecoverySpeed) / 2500).ToString("0.#").Named("QIRECOVERYSPEED"));
                        //hours logic
                    }
                    else if (2500 / CultivatorHediff.qiRecoverySpeed == 2500)//2500 is hour
                    {
                        qiBarText += "AS_QiPoolBarRecoveryHour".Translate();
                        //hour logic
                    }
                    else if (2500 / CultivatorHediff.qiRecoverySpeed < 2500 && 2500 / CultivatorHediff.qiRecoverySpeed > 41.6)
                    {
                        qiBarText += "AS_QiPoolBarRecoveryMinutes".Translate(((2500 / CultivatorHediff.qiRecoverySpeed) / 41.6).ToString("0.#").Named("QIRECOVERYSPEED"));
                        //minutes
                    }
                    else if (2500 / CultivatorHediff.qiRecoverySpeed == 41.6)
                    {
                        qiBarText += "AS_QiPoolBarRecoveryMinute".Translate();
                        //minute logic
                    }
                    else if (2500 / CultivatorHediff.qiRecoverySpeed < 41.6)//less than a min
                    {
                        qiBarText += "AS_QiPoolBarRecoverySeconds".Translate(((2500 / CultivatorHediff.qiRecoverySpeed) / 0.69f).ToString("0.#").Named("QIRECOVERYSPEED"));
                        //seconds logic
                    }
                }
            }
            Widgets.Label(barRect, qiBarText);

            if (CanControl() && qiPoolHediff.amount < qiPoolHediff.maxAmount)
            {
                if (Widgets.ButtonText(gatherButtonRect, "AS_QiGathering".Translate()))
                {
                    Job job = JobMaker.MakeJob(AscensionDefOf.AS_QiGatheringJob, selectedPawn, CultivationJobUtility.FindCultivationSpot(selectedPawn));
                    SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                    selectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }
                if (Mouse.IsOver(gatherButtonRect))
                {
                    Widgets.DrawHighlight(gatherButtonRect);
                    TooltipHandler.TipRegion(gatherButtonRect, "AS_QiGatheringDesc".Translate());
                }
            }
            Text.Anchor = TextAnchor.UpperLeft;
        }

        #region Realms
        private static Texture2D cultivatorFullTexture = AscensionTextures.UICultivatorIcon;
        private static Texture2D cultivatorEmptyTexture = AscensionTextures.UICultivatorEmptyIcon;
        private static Texture2D goldenCoreTexture = AscensionTextures.UIGoldenCoreIcon;

        private static void DrawPawnRealm(Rect rect)
        {
            Realm_Hediff realm = bRealmHediff;
            if (eRealmHediff != null)
            {
                realm = eRealmHediff;
            }

            if (realm != null)
            {
                Rect realmTextureRect = new Rect(rect.x, rect.y + 35, rect.width, rect.height);
                Rect realmLabelRect = new Rect(realmTextureRect.x, realmTextureRect.yMin - 20f, rect.width, 35f);

                if (Mouse.IsOver(realmLabelRect))
                {
                    Widgets.DrawHighlight(realmLabelRect);
                    TooltipHandler.TipRegion(realmLabelRect, realm.CurStage.extraTooltip);
                }

                float realmRatio = (float)realm.progress / realm.maxProgress;

                // Draw the empty texture
                GUI.DrawTexture(realmTextureRect, cultivatorEmptyTexture);


                // Draw the full texture based on the progress
                if (realmRatio > 0)
                {
                    Rect fullRect = new Rect(realmTextureRect.x, realmTextureRect.y + realmTextureRect.height - (realmTextureRect.height * realmRatio), realmTextureRect.width, realmTextureRect.height * realmRatio);
                    Rect texCoords = new Rect(0f, 0f, 1f, realmRatio);
                    GUI.DrawTextureWithTexCoords(fullRect, cultivatorFullTexture, texCoords);
                }

                Color barColor = realm.def == AscensionDefOf.EssenceRealm ? new Color(0.8f, 0.8f, 1f - realm.Severity / 5f, 1f) : new Color(0.8f, 1f - realm.Severity / 5f, 1f - realm.Severity / 5f, 1f);

                GUI.color = Color.white; // Reset color for the text
                Text.Anchor = TextAnchor.MiddleCenter;
                Text.Font = GameFont.Small;
                string realmProgressText = "AS_RealmProgress".Translate(realm.CurStage.label.Named("REALM"), realm.progress.Named("CURRENT"), realm.maxProgress.Named("MAX"));
                GUI.color = Color.white;

                Widgets.Label(realmLabelRect, realmProgressText);

                if (CultivatorHediff.goldenCoreScore > 0)
                {
                    Rect goldenCoreTextureRect = new Rect(realmTextureRect.x + (realmTextureRect.width - 32f) / 2f, realmTextureRect.y + (realmTextureRect.height - 32f) / 2f, 32f, 32f);
                    GUI.DrawTexture(goldenCoreTextureRect, goldenCoreTexture);

                    Rect goldenCoreLabelRect = new Rect(realmTextureRect.x, goldenCoreTextureRect.y, rect.width, 35f);
                    GUI.color = Color.gray;
                    Widgets.Label(goldenCoreLabelRect, "AS_GoldenCoreLabel".Translate(CultivatorHediff.goldenCoreScore.Named("SCORE")));
                    GUI.color = Color.white;
                    if (Mouse.IsOver(goldenCoreTextureRect))
                    {
                        Widgets.DrawHighlight(goldenCoreTextureRect);
                        TooltipHandler.TipRegion(goldenCoreTextureRect, "AS_GoldenCoreDesc".Translate(CultivatorHediff.goldenCoreScore.Named("SCORE")));
                    }
                }

                GUI.color = Color.white; // Ensure GUI color is reset after drawing
            }

        }

        private static void DrawRealmBreakthrough(Rect rect, Realm_Hediff realm)
        {
            float barWidth = rect.width - 26f;
            Rect barRect = new Rect(rect.x + 17f, rect.y + rect.height / 2f - 16f, barWidth, 32f);
            string tooltipKey = realm.def == AscensionDefOf.EssenceRealm ? "AS_BreakthroughEssenceDesc" : "AS_BreakthroughBodyDesc";
            if (Mouse.IsOver(barRect))
            {
                Widgets.DrawHighlight(barRect);
            }

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(barRect, "AS_QiPool".Translate());
            if (realm.def == AscensionDefOf.EssenceRealm && realm.Severity >= 2 && realm.Severity < 3)// golden core breakthrough
            {
                tooltipKey = "AS_GoldenCoreBreakthroughDesc".Translate();
                if (Widgets.ButtonText(barRect, "AS_GoldenCoreBreakthrough".Translate())) // normal breakthrough
                {
                    Job job = JobMaker.MakeJob(AscensionDefOf.AS_GoldenCoreBreakthrough, selectedPawn, CultivationJobUtility.FindCultivationSpot(selectedPawn));
                    selectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }
            }
            else if (Widgets.ButtonText(barRect, "AS_Breakthrough".Translate())) // normal breakthrough
            {
                Job job = JobMaker.MakeJob(realm.def == AscensionDefOf.EssenceRealm ? AscensionDefOf.AS_BreakthroughEssence : AscensionDefOf.AS_BreakthroughBody, selectedPawn, CultivationJobUtility.FindCultivationSpot(selectedPawn));
                selectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
            }
            TooltipHandler.TipRegionByKey(barRect, tooltipKey);
        }

        private static void DrawMeditationButton(Rect rect, bool isExercise)
        {
            QiPool_Hediff qiPool = selectedPawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.QiPool) as QiPool_Hediff;

            if (isExercise)
            {
                if (Widgets.ButtonText(rect, "AS_Exercise".Translate()))
                {
                    Job job = JobMaker.MakeJob(AscensionDefOf.AS_ExerciseJob, selectedPawn, CultivationJobUtility.FindCultivationSpot(selectedPawn));
                    selectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }
                if (Mouse.IsOver(rect))
                {
                    Widgets.DrawHighlight(rect);
                    TooltipHandler.TipRegion(rect, "AS_ExerciseDesc".Translate());
                }
            }
            else if (qiPool != null && qiPool.amount >= 2 + qiPool.maxAmount / 10)
            {
                if (Widgets.ButtonText(rect, "AS_RefineQi".Translate()))
                {
                    Job job = JobMaker.MakeJob(AscensionDefOf.AS_RefineQiJob, selectedPawn, CultivationJobUtility.FindCultivationSpot(selectedPawn));
                    selectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }
                if (Mouse.IsOver(rect))
                {
                    Widgets.DrawHighlight(rect);
                    TooltipHandler.TipRegion(rect, "AS_RefineQiDesc".Translate());
                }
            }
        }

        private static void DrawRealms(Rect rect, float height, float curY)
        {
            Realm_Hediff realmHediff = null;
            Rect realmRect = new Rect(rect.width / 4, (curY + (rect.width / 5)), rect.width / 2, rect.width / 2);
            Rect meditationRect = new Rect(0f, curY + height + realmRect.height, rect.width, 20f);
            Rect cultivationStatsRect = new Rect(meditationRect.x, meditationRect.y+ meditationRect.height, meditationRect.width, 20f);

            if (eRealmHediff != null)
            {
                realmHediff = eRealmHediff as Realm_Hediff;
            }
            else if (bRealmHediff != null)
            {
                realmHediff = bRealmHediff as Realm_Hediff;
            }

            if (realmHediff != null)
            {
                DrawPawnRealm(realmRect);

                if (CanControl() && realmHediff.progress >= realmHediff.maxProgress && realmHediff.Severity < 7)// this displays breakthrough when it should
                {
                    DrawRealmBreakthrough(meditationRect, realmHediff);
                    // meditation button because if they can breakthrough they shouldnt be able to refine qi
                    cultivationStatsRect.y += meditationRect.height;
                }else if (CanControl() && realmHediff.progress < realmHediff.maxProgress) //this displays the qi refine button instead if they cant breakthrough.
                {
                    DrawMeditationButton(meditationRect, realmHediff != null && realmHediff.def == AscensionDefOf.BodyRealm);
                    cultivationStatsRect.y += meditationRect.height;
                }

                DrawCultivationStatsBasic(cultivationStatsRect);
            }


        }
        #endregion








        #region Cultivation Stats

        private static void DrawCultivationStatsBasic(Rect rect)
        {
            GUI.color = Color.white;

            Rect qiTileRect = new Rect(rect.x,rect.y+ rect.height, rect.width, rect.height);
            Rect innerCRect = new Rect(rect.x, qiTileRect.y + qiTileRect.height + 10f, rect.width, rect.height);

            if (CultivatorHediff == null)
                return;

            DrawCultivationSpeed(rect);
            DrawQiTile(qiTileRect);
            DrawInnerCauldron(innerCRect);

            DrawHighlightsAndTooltips(rect, qiTileRect, innerCRect);
        }


        private static void DrawCultivationStats(Rect rect)
        {
            GUI.color = Color.white;
            Cultivator_Hediff cultivatorHediff = selectedPawn.health.hediffSet.GetFirstHediffOfDef(AscensionDefOf.Cultivator) as Cultivator_Hediff;
            if (cultivatorHediff == null)
                return;

            Rect speedFactorsRect = rect;
            speedFactorsRect.height = 40f;

            Rect maxQiFactorRect = speedFactorsRect;
            maxQiFactorRect.y += speedFactorsRect.height;

            DrawSpeedFactors(speedFactorsRect);
            DrawMaxQiFactors(maxQiFactorRect);

        }

        private static void DrawCultivationSpeed(Rect rect)
        {
            Widgets.Label(rect, "AS_CultivationSpeed".Translate(
                AscensionUtilities.UpdateCultivationSpeed(CultivatorHediff).ToString("0.#").Named("SPEED")));
        }

        private static void DrawSpeedFactors(Rect speedFactorsRect)
        {
            string translatedSpeedFactorText = "AS_CSFactorBaseOffset".Translate(
                CultivatorHediff.cultivationBaseSpeed.ToString("0.#").Named("BASE"),
                CultivatorHediff.cultivationSpeedOffset.ToString("0.#").Named("OFFSET"));

            if (selectedPawn.needs.mood != null)
            {
                translatedSpeedFactorText += "AS_CSFactorMood".Translate(
                    selectedPawn.needs.mood.CurLevelPercentage.ToString("0.#").Named("MOOD"));
            }

            QiGatherMapComponent qiGatherMapComp = CultivatorHediff.pawn.Map.GetComponent<QiGatherMapComponent>();
            if (qiGatherMapComp != null)
            {
                int qiTile = qiGatherMapComp.GetQiGatherAt(selectedPawn.Position.x, selectedPawn.Position.z);
                if (selectedPawn.health.hediffSet.HasHediff(AscensionDefOf.EssenceRealm))
                {
                    translatedSpeedFactorText += "AS_CSFactorQiTile".Translate(
                        (qiTile / 100f).ToString("0.#").Named("QITILE"));
                }
            }

            ElementEmitMapComponent elementEmitMapComp = CultivatorHediff.pawn.Map.GetComponent<ElementEmitMapComponent>();
            string elementText = TranslateElement(CultivatorHediff.element);
            if (elementEmitMapComp != null)
            {
                translatedSpeedFactorText += "AS_CSFactorElement".Translate(
                    elementText.Translate().Named("ELEMENT"),
                    (1 + (elementEmitMapComp.CalculateElementValueAt(
                        new IntVec2(CultivatorHediff.pawn.Position.x, CultivatorHediff.pawn.Position.z), CultivatorHediff.element) / 100f))
                        .ToString("0.#").Named("AMOUNT"));
            }
            speedFactorsRect.height = 35f;
            Widgets.Label(speedFactorsRect, "AS_CSFactorLabel".Translate());
            speedFactorsRect.y += 35f;
            speedFactorsRect.height = 35f;
            Widgets.Label(speedFactorsRect, translatedSpeedFactorText);
            AddHighlightAndTooltip(speedFactorsRect, "AS_CSFactorsDesc", Color.white);
        }

        //prob need to make it so we only grab the hediffs once
        private static void DrawMaxQiFactors(Rect speedFactorsRect)
        {
            speedFactorsRect.height = 50f;
            speedFactorsRect.y += 40f;
            if (qiPoolHediff == null) return;

            StringBuilder translatedSpeedFactorText = new StringBuilder();

            //we always have body size, offset. we have realm if we are in essence realm.

            //body size times 100, then realm and (if above 0) gc scores are added, then offset is applied, then inner cauldron is added
            translatedSpeedFactorText.Append("AS_MQFactorBodySize".Translate(selectedPawn.RaceProps.baseBodySize.ToString("0.#").Named("BS")));

            if (eRealmHediff != null)
            {
                translatedSpeedFactorText.Append("AS_MQFactorRealm".Translate(qiPoolHediff.realmMaxAmountOffset.ToString().Named("REALM")));
            }

            if (CultivatorHediff.goldenCoreScore > 0)
            {
                translatedSpeedFactorText.Append("AS_MQFactorGC".Translate(CultivatorHediff.goldenCoreScore.ToString().Named("GC")));
                translatedSpeedFactorText.Append("AS_MQFactorOffset".Translate(qiPoolHediff.maxAmountOffset.ToString("0.#").Named("OFFSET")));
                translatedSpeedFactorText.Append("AS_MQFactorAC".Translate(CultivatorHediff.innerCauldronQi.ToString().Named("AC")));
            }
            else
            {
                translatedSpeedFactorText.Append("AS_MQFactorOffset".Translate(qiPoolHediff.maxAmountOffset.ToString("0.#").Named("OFFSET")));
                translatedSpeedFactorText.Append("AS_MQFactorIC".Translate(CultivatorHediff.innerCauldronQi.ToString().Named("IC")));
            }
            Widgets.Label(speedFactorsRect, "AS_MQFactorLabel".Translate());
            speedFactorsRect.y += 25f;
            speedFactorsRect.height = 50f;
            Widgets.Label(speedFactorsRect, translatedSpeedFactorText.ToString());
            AddHighlightAndTooltip(speedFactorsRect, "AS_MQFactorsDesc", Color.white);
        }


        private static void DrawQiTile(Rect qiTileRect)
        {
            QiGatherMapComponent qiGatherMapComp = CultivatorHediff.pawn.Map.GetComponent<QiGatherMapComponent>();
            ElementEmitMapComponent elementEmitMapComp = CultivatorHediff.pawn.Map.GetComponent<ElementEmitMapComponent>();

            if (qiGatherMapComp != null && elementEmitMapComp != null)
            {
                string elementText = TranslateElement(CultivatorHediff.element);
                int qiTile = qiGatherMapComp.GetQiGatherAt(selectedPawn.Position.x, selectedPawn.Position.z);
                Widgets.Label(qiTileRect,
                    "AS_CurrentElement".Translate(elementText.Translate().Named("ELEMENT"),
                    elementEmitMapComp.CalculateElementValueAt(
                        new IntVec2(selectedPawn.Position.x, selectedPawn.Position.z), CultivatorHediff.element).Named("AMOUNT")) +
                    "AS_QiTile".Translate(qiTile.Named("QITILE")));
            }
        }

        private static void DrawInnerCauldron(Rect innerCRect)
        {

            string innerCText = "AS_InnerC".Translate(
                CultivatorHediff.innerCauldronQi.Named("ICQI"),
                CultivatorHediff.innerCauldronLimit.Named("ICMAX"));
            string innerCDesc = "AS_InnerCDesc".Translate();
            string innerCJob = "AS_InnerCJob".Translate();
            string innerCJobDesc = "AS_InnerCJobDesc".Translate();

            GUI.color = Color.yellow;
            if (eRealmHediff != null && eRealmHediff.Severity >= 3)
            {
                GUI.color = Color.magenta;
                innerCText = "AS_AnimaC".Translate(CultivatorHediff.innerCauldronQi.Named("ICQI"));
                innerCDesc = "AS_AnimaCDesc".Translate();
                innerCJob = "AS_AnimaCJob".Translate();
                innerCJobDesc = "AS_AnimaCJobDesc".Translate();
            }

            Widgets.Label(innerCRect, innerCText);

            Rect innerCButtonRect = new Rect(innerCRect.x + innerCRect.width / 4, innerCRect.y + innerCRect.height, innerCRect.width / 2f, innerCRect.height);

            if (qiPoolHediff != null)
            {
                long qiCost = 2 + (qiPoolHediff.maxAmount / 50); // 2% + 2
                if (qiPoolHediff.amount >= qiCost)
                {
                    if (Widgets.ButtonText(innerCButtonRect, innerCJob))
                    {
                        Job job = JobMaker.MakeJob(AscensionDefOf.AS_RefineQiCauldronJob, selectedPawn, CultivationJobUtility.FindCultivationSpot(selectedPawn));
                        SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                        selectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }

                    if (Mouse.IsOver(innerCButtonRect))
                    {
                        Widgets.DrawHighlight(innerCButtonRect);
                        TooltipHandler.TipRegion(innerCButtonRect, innerCJobDesc);
                    }
                }
            }
        }

        private static string TranslateElement(ElementEmitMapComponent.Element element)
        {
            return element switch
            {
                ElementEmitMapComponent.Element.Earth => "AS_Earth",
                ElementEmitMapComponent.Element.Metal => "AS_Metal",
                ElementEmitMapComponent.Element.Water => "AS_Water",
                ElementEmitMapComponent.Element.Fire => "AS_Fire",
                ElementEmitMapComponent.Element.Wood => "AS_Wood",
                _ => "AS_None"
            };
        }

        private static void DrawHighlightsAndTooltips(Rect rect, Rect qiTileRect, Rect innerCRect)
        {
            Color highlightColor = new Color(0.8f, 0.8f, 0.8f, 0.5f); // Slightly darker highlight color

            AddHighlightAndTooltip(rect, "AS_CultivationSpeedDesc", highlightColor);

            AddHighlightAndTooltip(qiTileRect, "AS_QiTileDesc", highlightColor);
            AddHighlightAndTooltip(innerCRect, "AS_InnerCDesc", highlightColor);

            GUI.color = Color.white;
        }
        private static void AddHighlightAndTooltip(Rect rect, string tooltipKey, Color highlightColor)
        {
            if (Mouse.IsOver(rect))
            {
                GUI.color = highlightColor;
                Widgets.DrawHighlight(rect);
                TooltipHandler.TipRegion(rect, tooltipKey.Translate());
                GUI.color = Color.white;
            }
        }


        #endregion

        #region Schedule


        private static void DrawSchedule(Rect rect, float height, float curY)
        {
            Cultivator_Hediff cultivatorHediff = CultivatorHediff;
            if (cultivatorHediff == null) return;

            float scheduleBarHieght = 20f;
            float scheduleButtonHieght = 35f;
            // Realm auto cultivation
            Rect autoCultivatorRect = new Rect(rect.x, curY + height + 4f, rect.width, scheduleBarHieght);
            DrawAutoTypeLabel(autoCultivatorRect, "AS_AutoCType");

            Rect autoRealmButton = new Rect(rect.x, autoCultivatorRect.yMax, rect.width - 3f, scheduleButtonHieght);
            DrawAutoCultivationButton(autoRealmButton);

            Rect cultivationStartLabelRect = new Rect(rect.x, autoRealmButton.yMax, rect.width, scheduleBarHieght);
            Rect cultivationStartSlider = new Rect(rect.x, cultivationStartLabelRect.yMax, rect.width, scheduleBarHieght);
            Rect cultivationEndLabelRect = new Rect(rect.x, cultivationStartSlider.yMax, rect.width, scheduleBarHieght);
            Rect cultivationEndSlider = new Rect(rect.x, cultivationEndLabelRect.yMax, rect.width, scheduleBarHieght);

            DrawTimeSlider(cultivatorHediff, cultivationStartLabelRect, cultivationStartSlider, true);
            DrawTimeSlider(cultivatorHediff, cultivationEndLabelRect, cultivationEndSlider, false);

            Rect cultivationStatsRect = new Rect(cultivationEndSlider.x, cultivationEndSlider.yMax, cultivationEndSlider.width, scheduleButtonHieght);
            DrawCultivationStats(cultivationStatsRect);
        }

        private static void DrawAutoTypeLabel(Rect rect, string translationKey)
        {
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect, translationKey.Translate());
            Text.Anchor = TextAnchor.UpperLeft;
        }

        private static void DrawAutoCultivationButton(Rect buttonRect)
        {
            string autoCultivatorTypeText;
            string autoCultivatorTypeDescText;

            switch (CultivatorHediff.autoCultivateType)
            {
                case 1:
                    autoCultivatorTypeText = "AS_RealmAuto".Translate();
                    autoCultivatorTypeDescText = "AS_RealmAutoDesc".Translate();
                    break;
                case 2:
                    autoCultivatorTypeText = "AS_RealmOnlyAuto".Translate();
                    autoCultivatorTypeDescText = "AS_RealmOnlyAutoDesc".Translate();
                    break;
                case 3:
                    autoCultivatorTypeText = "AS_QiGatheringAuto".Translate();
                    autoCultivatorTypeDescText = "AS_QiGatheringAutoDesc".Translate();
                    break;
                default:
                    return;
            }

            if (Widgets.ButtonText(buttonRect, autoCultivatorTypeText))
            {
                CultivatorHediff.autoCultivateType = CultivatorHediff.autoCultivateType < 3 ? CultivatorHediff.autoCultivateType + 1 : 1;
            }

            if (Mouse.IsOver(buttonRect))
            {
                Widgets.DrawHighlight(buttonRect);
                TooltipHandler.TipRegion(buttonRect, autoCultivatorTypeDescText);
            }
        }

        private static void DrawTimeSlider(Cultivator_Hediff cultivatorHediff, Rect labelRect, Rect sliderRect, bool isStartTime)
        {
            float timeValue = isStartTime ? cultivatorHediff.startTime : cultivatorHediff.endTime;
            string labelKey = isStartTime ? "AS_AutoCStart" : "AS_AutoCEnd";

            string formattedTime = FormatTime(timeValue);
            Widgets.Label(labelRect, $"{labelKey.Translate()} {formattedTime}");
            float newTimeValue = Widgets.HorizontalSlider(sliderRect, timeValue, 0f, 24f);

            if (isStartTime)
            {
                cultivatorHediff.startTime = newTimeValue;
            }
            else
            {
                cultivatorHediff.endTime = newTimeValue;
            }
        }

        private static string FormatTime(float timeValue)
        {
            int hours = (int)timeValue;
            int minutes = (int)((timeValue - hours) * 60);
            hours = hours % 12;
            if (hours == 0) hours = 12;

            TimeSpan time = new TimeSpan(hours, minutes, 0);
            string period = (timeValue < 12 || timeValue >= 24) ? "am" : "pm";
            return time.ToString(@"h\:mm") + period;
        }
        #endregion


        public static readonly HediffDef qiPoolHediffDef = AscensionDefOf.QiPool;
        public static readonly HediffDef eRealmHediffDef = AscensionDefOf.EssenceRealm;
        public static readonly HediffDef bRealmHediffDef = AscensionDefOf.BodyRealm;
        public static readonly HediffDef CultivatorHediffDef = AscensionDefOf.Cultivator;

        public static QiPool_Hediff qiPoolHediff;
        public static Realm_Hediff eRealmHediff;
        public static Realm_Hediff bRealmHediff;
        public static Cultivator_Hediff CultivatorHediff;

        private static float DrawCultivationTab(Rect rect, float curY)
        {
            //here we grab the hediffs for use for most of the cultivation tab
            CultivatorHediff = selectedPawn.health.hediffSet.GetFirstHediffOfDef(CultivatorHediffDef) as Cultivator_Hediff;
            qiPoolHediff = selectedPawn.health.hediffSet.GetFirstHediffOfDef(qiPoolHediffDef) as QiPool_Hediff;
            eRealmHediff = selectedPawn.health.hediffSet.GetFirstHediffOfDef(eRealmHediffDef) as Realm_Hediff;
            bRealmHediff = selectedPawn.health.hediffSet.GetFirstHediffOfDef(bRealmHediffDef) as Realm_Hediff;

            Widgets.BeginGroup(rect);
            Rect qiBarRect = new Rect(0f, curY, rect.width, 40f);
            if (qiPoolHediff != null)
            {
                DrawPawnQi(qiBarRect);
            }
            Widgets.DrawLineHorizontal(0f, curY + qiBarRect.height + 35, rect.width, Color.gray);

            float realmsRectWidth = rect.width / 2;
            Rect realmsRect = new Rect(rect.x + rect.width, rect.y, realmsRectWidth, rect.height);
            //Realms rect should include cultivation stats.
            float underQiHeight = qiBarRect.height + 45f;
            DrawRealms(realmsRect, underQiHeight, curY);
            GUI.color = Color.white;
            //the schedule manages the times of day to auto cultivate and the realm to cultivate
            float scheduleRectX = rect.x + realmsRect.width;
            // Create the left side rectangle with the same height as the reference rectangle
            Rect scheduleRect = new Rect(scheduleRectX, rect.y, realmsRectWidth, rect.height);
            DrawSchedule(scheduleRect, underQiHeight, curY);
            //Widgets.DrawLineHorizontal(0f, (rect2.yMax + rect2.y) / 2f, rect.width);
            GUI.color = Color.white;
            Widgets.EndGroup();
            curY += 10f;
            curY += 6f;
            return curY;//so we know what y the next one is.
        }
    }
}

