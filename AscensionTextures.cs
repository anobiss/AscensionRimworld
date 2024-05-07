using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Ascension
{
    [StaticConstructorOnStartup]
    public static class AscensionTextures
    {
        public static readonly Texture2D UIBreakthrough = ContentFinder<Texture2D>.Get("UI/Abilities/Breakthrough", true);
        public static readonly Texture2D UIQiIcon = ContentFinder<Texture2D>.Get("UI/Icons/QiIcon", true);
    }
}
