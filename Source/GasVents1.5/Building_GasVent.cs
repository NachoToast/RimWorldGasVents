using PerformanceFish;
using RimWorld;
using Verse;

namespace GasVents
{
    public class Building_GasVent : Building
    {
        private CompPowerTrader powerComp;

        private CompRefuelable refuelableComp;

        private GasDef gasDef;

        private static readonly GasVentSettings settings = LoadedModManager.GetMod<GasVents>().GetSettings<GasVentSettings>();

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            powerComp = GetComp<CompPowerTrader>();
            refuelableComp = GetComp<CompRefuelable>();

            IdentifyingGas identifyingGas = def.GetModExtension<IdentifyingGas>();

            if (identifyingGas == null)
            {
                Verse.Log.Error($"{def.defName} has no mod extension to indicate what type of gas it should emit, defaulting to blind smoke.");

                gasDef = GasDefOf.BlindSmoke;
            }
            else if (identifyingGas.gasDef == null)
            {
                Verse.Log.Error($"{def.defName} has no gasType or gasDef in its mod extension to indicate what type of gas it should emit, defaulting to blind smoke.");
                gasDef = GasDefOf.BlindSmoke;
            }
            else
            {
                gasDef = identifyingGas.gasDef;
            }
        }

        public override void TickRare()
        {
            if (!powerComp.PowerOn)
            {
                return;
            }

            if (!refuelableComp.HasFuel)
            {
                return;
            }

            refuelableComp.ConsumeFuel(settings.fuelConsumptionRate);

            Map.gasGrid.AddGas(Position, gasDef, settings.gasAmount);
        }
    }
}