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
        public static readonly Texture2D ToggleQi = ContentFinder<Texture2D>.Get("UI/Icons/ToggleQi", true);
        public static readonly Texture2D ToggleElement = ContentFinder<Texture2D>.Get("UI/Icons/ToggleElement", true);

        public static readonly Texture2D UIBreakthrough = ContentFinder<Texture2D>.Get("UI/Abilities/Breakthrough", true);

        public static readonly Texture2D UIQiIcon = ContentFinder<Texture2D>.Get("UI/Icons/QiIcon", true);

        public static readonly Texture2D UICultivatorIcon = ContentFinder<Texture2D>.Get("UI/Icons/Cultivator", true);
        public static readonly Texture2D UICultivatorEmptyIcon = ContentFinder<Texture2D>.Get("UI/Icons/CultivatorEmpty", true);
        public static readonly Texture2D UIGoldenCoreIcon = ContentFinder<Texture2D>.Get("UI/Icons/GoldenCoreIcon", true);

        public static readonly Texture2D ChangePriority = ContentFinder<Texture2D>.Get("UI/Commands/ChangePriority", true);
        public static readonly Texture2D ChangeRealm = ContentFinder<Texture2D>.Get("UI/Commands/ChangeRealm", true);
        public static readonly Texture2D ChangeElement = ContentFinder<Texture2D>.Get("UI/Commands/ChangeElement", true);

        public static readonly Texture2D ChangeJob = ContentFinder<Texture2D>.Get("UI/Commands/ChangeJob", true);
        public static readonly Texture2D TogglePublic = ContentFinder<Texture2D>.Get("UI/Commands/TogglePublic", true);

        public static readonly Texture2D ElementFactorsIcon = ContentFinder<Texture2D>.Get("UI/Icons/ElementFactorsIcon", true);

        public static readonly Texture2D AbilityBackground = ContentFinder<Texture2D>.Get("UI/Icons/AbilityBackground", true);
        public static readonly Texture2D AbilityBackgroundEnabled = ContentFinder<Texture2D>.Get("UI/Icons/AbilityBackgroundEnabled", true);
    }
}
