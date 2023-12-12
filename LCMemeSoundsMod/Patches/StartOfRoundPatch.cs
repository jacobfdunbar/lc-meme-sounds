// ReSharper disable InconsistentNaming

using HarmonyLib;

namespace LCMemeSounds.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class StartOfRoundPatch
    {
        [HarmonyPostfix, HarmonyPatch("Start")]
        private static void StartPatch()
        {
            StartOfRound.Instance.fallDamageSFX = MemeSoundsMod.Instance.OofSound;
            StartOfRound.Instance.HUDSystemAlertSFX = MemeSoundsMod.Instance.DrowningSound;
        }
    }
}