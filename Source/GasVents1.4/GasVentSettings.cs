using SCGF;
using UnityEngine;
using Verse;


namespace GasVents
{
    public class GasVentSettings : ModSettings
    {
        public static readonly float defaultFuelConsumptionRate = 0.2f;
        public static readonly int defaultGasAmount = 255;

        public float fuelConsumptionRate = defaultFuelConsumptionRate;
        public int gasAmount = defaultGasAmount;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref fuelConsumptionRate, "fuelConsumptionRate", defaultFuelConsumptionRate);
            Scribe_Values.Look(ref gasAmount, "gasAmount", defaultGasAmount);
        }

        public void Draw(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);


            DrawFuelConsumptionRate(listingStandard);

            listingStandard.Gap();

            DrawGasAmount(listingStandard);

            listingStandard.End();
        }


        private static float HoursBetweenShellConsume(float fuelConsumptionRate, out string calculations)
        {
            float rareTicksPerShell = 10 / fuelConsumptionRate;
            calculations = $"Rare ticks per shell:\n<color=#808080>= units per shell / units per rare tick\n= 10 / {fuelConsumptionRate:0.00}</color>\n= {rareTicksPerShell:0.00}";

            float ticksPerShell = GenTicks.TickRareInterval * rareTicksPerShell;
            calculations += $"\n\nTicks per shell:\n<color=#808080>= rare tick interval * rare ticks per shell\n= {GenTicks.TickRareInterval} * {rareTicksPerShell:0.00}</color>\n= {ticksPerShell:0.00}";

            float hoursPerShell = ticksPerShell / 2500;
            calculations += $"\n\nHours per shell:\n<color=#808080>= ticks per shell / ticks per hour\n= {ticksPerShell:0.00} / 2500</color>\n= {hoursPerShell:0.00}";

            return 0.05f * Mathf.Round(hoursPerShell / 0.05f);
        }

        private void DrawGasAmount(Listing_Standard listingStandard)
        {
            string gasAmountLabel = "GasVents.Settings.GasAmount.Label".Translate(gasAmount);
            string gasAmountTooltip = "GasVents.Settings.GasAmount.Tooltip".Translate(GasVentSettings.defaultGasAmount);

            float percentageIncrease = 100 * gasAmount / 255f;

            string gasAmountDescription;

            if (gasAmount == 0)
            {
                gasAmountDescription = "GasVents.Settings.GasAmount.Description.Zero".Translate();
            }
            else if (gasAmount < 25)
            {
                gasAmountDescription = "GasVents.Settings.GasAmount.Description.Normal".Translate(percentageIncrease.ToString("F2"));
            }
            else if (gasAmount <= 255)
            {
                gasAmountDescription = "GasVents.Settings.GasAmount.Description.Normal".Translate(percentageIncrease.ToString("F0"));
            }
            else
            {
                gasAmountDescription = "GasVents.Settings.GasAmount.Description.Overflow".Translate(percentageIncrease.ToString("F0"));
            }

            if (gasAmount == defaultGasAmount)
            {
                gasAmountDescription += "GasVents.Settings.DefaultSuffix".Translate();
            }

            gasAmount = (int)listingStandard.SliderLabeled(gasAmountLabel, gasAmount, 0, 512, 0.25f, gasAmountTooltip);

            listingStandard.Label(gasAmountDescription);

            if (listingStandard.ButtonText("GasVents.Settings.ResetToDefault".Translate(), null, 0.2f))
            {
                gasAmount = GasVentSettings.defaultGasAmount;
            }

        }

        private void DrawFuelConsumptionRate(Listing_Standard listingStandard)
        {
            string fuelConsumptionRateLabel = "GasVents.Settings.FuelConsumptionRate.Label".Translate(fuelConsumptionRate.ToString("F2"));
            string fuelConsumptionRateTooltip = "GasVents.Settings.FuelConsumptionRate.Tooltip".Translate(GasVentSettings.defaultFuelConsumptionRate.ToString("F2"));

            string fuelConsumptionRateDescription;
            string fuelConsumptionRateDescriptionTooltip = null;

            if (fuelConsumptionRate == 0)
            {
                fuelConsumptionRateDescription = "GasVents.Settings.FuelConsumptionRate.Description.Infinite".Translate();
            }
            else
            {
                float hoursBetweenShellConsume = HoursBetweenShellConsume(fuelConsumptionRate, out fuelConsumptionRateDescriptionTooltip);
                if (hoursBetweenShellConsume == 1)
                {
                    fuelConsumptionRateDescription = "GasVents.Settings.FuelConsumptionRate.Description.Single".Translate();
                }
                else
                {
                    fuelConsumptionRateDescription = "GasVents.Settings.FuelConsumptionRate.Description.Multi".Translate(hoursBetweenShellConsume.ToString());
                }
            }

            if (Mathf.Abs(fuelConsumptionRate - defaultFuelConsumptionRate) < 0.0001f)
            {
                fuelConsumptionRateDescription += "GasVents.Settings.DefaultSuffix".Translate();
            }

            fuelConsumptionRate = 0.01f * Mathf.Round(listingStandard.SliderLabeled(fuelConsumptionRateLabel, fuelConsumptionRate, 0, 1f, 0.25f, fuelConsumptionRateTooltip) / 0.01f);

            listingStandard.Label(fuelConsumptionRateDescription, -1, fuelConsumptionRateDescriptionTooltip);

            if (listingStandard.ButtonText("GasVents.Settings.ResetToDefault".Translate(), null, 0.2f))
            {
                fuelConsumptionRate = GasVentSettings.defaultFuelConsumptionRate;
            }
        }
    }
}
