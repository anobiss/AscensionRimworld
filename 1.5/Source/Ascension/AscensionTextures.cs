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

        public static readonly Texture2D ChangePriority = ContentFinder<Texture2D>.Get("UI/Commands/ChangePriority", true);
        public static readonly Texture2D ChangeRealm = ContentFinder<Texture2D>.Get("UI/Commands/ChangeRealm", true);
        public static readonly Texture2D ChangeElement = ContentFinder<Texture2D>.Get("UI/Commands/ChangeElement", true);

    }
}
