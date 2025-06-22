namespace GasVents;

public class GasVentSettings : ModSettings
{
    private const int DEFAULT_FUEL_PER_TICK = 10;
    private const int DEFAULT_CELLS_PER_TICK = 5;

    private static int fuelPerTick = DEFAULT_FUEL_PER_TICK;
    private static int cellsPerTick = DEFAULT_CELLS_PER_TICK;

    public static int FuelPerTick => fuelPerTick;
    public static int CellsPerTick => cellsPerTick;

    public static void Draw(Rect inRect)
    {
        Listing_Standard listing = new();

        listing.Begin(inRect);

        string fuelPerTickLabel = fuelPerTick switch
        {
            0 => "GasVents.FuelPerTick0",
            100 => "GasVents.FuelPerTick100",
            _ => "GasVents.FuelPerTickX",
        };

        DrawEntry(
            listing: listing,
            label: fuelPerTickLabel.Translate(fuelPerTick),
            value: ref fuelPerTick,
            defaultValue: DEFAULT_FUEL_PER_TICK,
            min: 0,
            max: 100);

        string cellsPerTickLabel = cellsPerTick switch
        {
            1 => "GasVents.CellsPerTick1",
            _ => "GasVents.CellsPerTickX",
        };

        DrawEntry(
            listing: listing,
            label: cellsPerTickLabel.Translate(cellsPerTick),
            value: ref cellsPerTick,
            defaultValue: DEFAULT_CELLS_PER_TICK,
            min: 1,
            max: 50);

        listing.End();
    }

    public override void ExposeData()
    {
        base.ExposeData();

        Scribe_Values.Look(ref fuelPerTick, "fuelPerTick", DEFAULT_FUEL_PER_TICK);
        Scribe_Values.Look(ref cellsPerTick, "gasPerTick", DEFAULT_CELLS_PER_TICK);
    }

    private static void DrawEntry(
        Listing_Standard listing,
        string label,
        ref int value,
        int defaultValue,
        int min,
        int max)
    {
        listing.Label(label, tooltip: "Default".Translate() + ": " + defaultValue.ToString("n0"));

        int newValue = Mathf.RoundToInt(listing.Slider(value, min, max));

        if (newValue != value)
        {
            value = Mathf.Clamp(newValue, min, max);
        }

        if (value != defaultValue)
        {
            if (listing.ButtonText("Reset".Translate(), widthPct: 0.1f))
            {
                value = defaultValue;
            }
        }
        else
        {
            listing.Gap(Text.LineHeight + 10f);
        }

        listing.Gap();
    }
}
