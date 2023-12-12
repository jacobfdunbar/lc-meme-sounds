// ReSharper disable InconsistentNaming

using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace LCMemeSounds.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public class PlayerControllerBPatch
    {
        [HarmonyPostfix, HarmonyPatch("DamagePlayer")]
        private static void DamagePlayerPatch(bool fallDamage, CauseOfDeath causeOfDeath, AudioSource ___movementAudio, int ___health)
        {
            if (fallDamage || causeOfDeath != CauseOfDeath.Gravity || ___health <= 0) return;
            PlaySound(StartOfRound.Instance.fallDamageSFX, ___movementAudio);
        }

        [HarmonyPostfix, HarmonyPatch("KillPlayer")]
        private static void KillPlayerPatch(CauseOfDeath causeOfDeath, AudioSource ___movementAudio)
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            
            switch (causeOfDeath)
            {
                case CauseOfDeath.Gravity:
                    PlaySound(stackTrace.ToString().Contains("InteractTrigger")
                        ? MemeSoundsMod.Instance.DeathSoundMarioFall                           // When player falls in pit
                        : MemeSoundsMod.Instance.DeathSoundSpongebobFall, ___movementAudio);   // Normal fall damage death
                    break;
                case CauseOfDeath.Gunshots:
                    PlaySound(MemeSoundsMod.Instance.DeathSoundWasted, ___movementAudio);
                    break;
                case CauseOfDeath.Drowning:
                    PlaySound(MemeSoundsMod.Instance.DeathSoundDrowned, ___movementAudio);
                    break;
            }
        }

        private static void PlaySound(AudioClip clip, AudioSource source)
        {
            HUDManager.Instance.UIAudio.PlayOneShot(clip, 1f);
            WalkieTalkie.TransmitOneShotAudio(source, clip);
        }
    }
}