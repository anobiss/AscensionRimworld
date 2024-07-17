using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ascension
{
    public class AscensionToggleHandler
    {
        private bool active;

        public bool Active => active;

        //public static bool GatherQiOnGUI()
        //{
        //    if (AscensionDefOf.ToggleQiDisplay.KeyDownEvent)
        //    {
        //        active = !active;
        //        Event.current.Use();
        //    }
        //    return false;
        //}
        //public static bool ElementEmitOnGUI()
        //{
        //    if (KeyBindingDefOf.ToggleElementDisplay.KeyDownEvent)
        //    {
        //        active = !active;
        //        Event.current.Use();
        //    }
        //}
    }
}