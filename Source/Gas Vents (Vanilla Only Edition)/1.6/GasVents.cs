namespace GasVents;

public class GasVents : Mod
{
    public GasVents(ModContentPack content) : base(content)
    {
        GetSettings<GasVentSettings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);

        GasVentSettings.Draw(inRect);
    }

    public override string SettingsCategory()
    {
        return "GasVents".Translate();
    }
}
