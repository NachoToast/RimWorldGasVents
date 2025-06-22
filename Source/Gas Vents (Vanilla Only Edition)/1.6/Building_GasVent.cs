namespace GasVents;

public class Building_GasVent : Building
{
    private CompPowerTrader powerComp;

    private CompRefuelable fuelComp;

    private GasType gasType;

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);

        powerComp = GetComp<CompPowerTrader>();
        fuelComp = GetComp<CompRefuelable>();

        IdentifyingGas identifyingGas = def.GetModExtension<IdentifyingGas>();

        if (identifyingGas?.gasType != null)
        {
            gasType = identifyingGas.gasType;
        }
        else
        {
            string errorMessage;

            if (identifyingGas == null)
            {
                errorMessage = $"{def} has no mod extension to indicate what type of gas it should emit, defaulting to blind smoke.";
            }
            else
            {
                errorMessage = $"{def} has no gasType in its mod extension to indicate what type of gas it should emit, defaulting to blind smoke.";
            }

            gasType = GasType.BlindSmoke;

            Log.ErrorOnce(errorMessage, 1372 + def.GetHashCode());
        }
    }

    public override void TickRare()
    {
        base.TickRare();

        if (powerComp != null && !powerComp.PowerOn)
        {
            // no power
            return;
        }

        if (fuelComp != null && !fuelComp.HasFuel)
        {
            // no fuel
            return;
        }

        Map.gasGrid.AddGas(Position, gasType, GasVentSettings.CellsPerTick * 255);

        if (GasVentSettings.FuelPerTick > 0)
        {
            fuelComp?.ConsumeFuel(GasVentSettings.FuelPerTick / 100f);
        }
    }
}
