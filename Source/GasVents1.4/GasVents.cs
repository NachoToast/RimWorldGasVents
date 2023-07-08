using UnityEngine;
using Verse;

namespace GasVents
{
    public class GasVents : Mod
    {
        private readonly GasVentSettings settings;

        public GasVents(ModContentPack content): base(content)
        {
            settings = GetSettings<GasVentSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.Draw(inRect);
        }

        public override string SettingsCategory()
        {
            return "GasVents".Translate();
        }
    }
}
