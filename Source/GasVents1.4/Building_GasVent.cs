using RimWorld;
using SCGF;
using Verse;

namespace GasVents
{
    public class Building_GasVent : Building
    {

        private CompPowerTrader powerComp;

        private CompRefuelable refuelableComp;

        private GasType gasType;

        private static readonly GasVentSettings settings = LoadedModManager.GetMod<GasVents>().GetSettings<GasVentSettings>();


        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            powerComp = GetComp<CompPowerTrader>();
            refuelableComp = GetComp<CompRefuelable>();

            IdentifyingGas identifyingGas = def.GetModExtension<IdentifyingGas>();
            if (identifyingGas?.gasDef != null)
            {
                gasType = (GasType) (GasLibrary.firstCustomGasIndex + identifyingGas.gasDef.customIndex);
            }
            else if (identifyingGas?.gasType != null)
            {
                gasType = identifyingGas.gasType;
            }
            else if (identifyingGas == null)
            {
                Log.Error(def + " has no mod extension to indicate what type of gas it should emit, defaulting to blind smoke.");
                gasType = GasType.BlindSmoke;
            }
            else
            {
                Log.Error(def + " has no gasType or gasDef in its mod extension to indicate what type of gas it should emit, defaulting to blind smoke.");
                gasType = GasType.BlindSmoke;
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

            Map.gasGrid.AddGas(Position, gasType, settings.gasAmount);
        }
    }
}
