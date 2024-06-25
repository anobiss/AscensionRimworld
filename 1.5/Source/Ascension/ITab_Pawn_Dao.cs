using RimWorld;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class ITab_Pawn_Dao : ITab
    {

        public const float Width = 630f;

        private Pawn PawnForDao
        {
            get
            {
                if (SelPawn != null)
                {
                    return SelPawn;
                }
                if (base.SelThing is Corpse corpse)
                {
                    return corpse.InnerPawn;
                }
                return null;
            }
        }
        public ITab_Pawn_Dao()
        {
            size = new Vector2(750f, 480f);
            labelKey = "TabDao";
            tutorTag = "Dao";
        }

        protected override void FillTab()
        {
            Pawn pawnForDao = PawnForDao;
            if (pawnForDao == null)
            {
                Log.Error("Dao tab found no selected pawn to display.");
            }
            else
            {
                DaoCardUtility.DrawPawnDaoCard(new Rect(Vector2.zero, size), pawnForDao, base.SelThing);
            }
        }


        //cultivator hediff stores info for this menu and its visibility to reduce clutter
        public override bool IsVisible
        {
            get
            {
                if (SelPawn.health.hediffSet.HasHediff(AscensionDefOf.Cultivator))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

}
