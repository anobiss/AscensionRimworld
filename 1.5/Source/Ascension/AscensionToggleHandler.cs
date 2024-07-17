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

        public void GatherQiOnGUI()
        {
            if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent)
            {
                active = !active;
                Event.current.Use();
            }
        }
        public void ElementEmitOnGUI()
        {
            if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent)
            {
                active = !active;
                Event.current.Use();
            }
        }
    }
}