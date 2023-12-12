using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LCMemeSounds.Patches;
using UnityEngine;

namespace LCMemeSounds
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class MemeSoundsMod : BaseUnityPlugin
    {
        private const string ModGUID = "com.jacobfdunbar.mods.lc.memesounds";
        private const string ModName = "Meme Sounds";
        private const string ModVersion = "1.0.0";

        private readonly Harmony _harmony = new Harmony(ModGUID);

        public ManualLogSource Log { get; private set; }
        
        public static MemeSoundsMod Instance { get; private set; }
        
        public AudioClip OofSound { get; private set; }
        public AudioClip DeathSoundMarioFall { get; private set; }
        public AudioClip DeathSoundSpongebobFall { get; private set; }
        public AudioClip DeathSoundWasted { get; private set; }
        public AudioClip DrowningSound { get; private set; }
        public AudioClip DeathSoundDrowned { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }
            
            Log = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
            Log.LogInfo($"Initializing {ModName}...");
            
            var modPath = Path.GetDirectoryName(Info.Location);
            if (modPath == null)
            {
                Log.LogError("Failed to get mod path!");
                return;
            }

            var assets = AssetBundle.LoadFromFile(Path.Combine(modPath, $"{ModGUID}.assets"));
            OofSound = assets.LoadAsset<AudioClip>("steve_oof");
            DeathSoundMarioFall = assets.LoadAsset<AudioClip>("mario_death");
            DeathSoundSpongebobFall = assets.LoadAsset<AudioClip>("spongebob_fail_crunch");
            DeathSoundWasted = assets.LoadAsset<AudioClip>("wasted");
            DrowningSound = assets.LoadAsset<AudioClip>("pikmin_drowning");
            DeathSoundDrowned = assets.LoadAsset<AudioClip>("pikmin_death");

            _harmony.PatchAll(typeof(MemeSoundsMod));
            _harmony.PatchAll(typeof(StartOfRoundPatch));
            _harmony.PatchAll(typeof(PlayerControllerBPatch));
        }
    }
}