using System;
using HugsLib;
using HugsLib.Settings;
using Verse;

namespace MultipleRaids
{
    public class MultipleRaidsSettings : ModBase {
        
        public override string ModIdentifier {
            get { return "MultipleRaids"; }
        }
        
        private SettingHandle<bool> forceDesperate;
        private SettingHandle<bool> forceRaidType;
        private SettingHandle<bool> randomFactions;
        private SettingHandle<int> spawnThreshold;
        private SettingHandle<int> extraRaids;
        private SettingHandle<float> pointsOffset;
        
        public override void DefsLoaded() {
            forceDesperate = Settings.GetHandle<bool>("forceDesperate", "Force Desperate", "Enables spawning of regular enemies on hard environment maps.", false);
            forceRaidType = Settings.GetHandle<bool>("forceRaidType", "Force Raid Type", "Forces every 3d raid to drop pod in center, every 4th to siege. Faction restrictions apply.", false);
            randomFactions = Settings.GetHandle<bool>("randomFactions", "Random Factions", "Allows spawned raids to belong to different factions", false);
            spawnThreshold = Settings.GetHandle<int>("spawnThreshold", "Spawn Threshold", "Determines chance for an extra raid to spawn.", 50);
            extraRaids = Settings.GetHandle<int>("extraRaids", "Extra Raids", "Maximum number of additional raids.", 1);
            pointsOffset = Settings.GetHandle<float>("pointsOffset", "Points offset", "If more than one raid spawn, raid points are rescaled with this formula: points = points*(1/totalNum + pointsOffset).", 0.35f);
        }
        
    }
}
