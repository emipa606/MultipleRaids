﻿using HugsLib;
using HugsLib.Settings;

namespace MultipleRaids
{
    public class MultipleRaidsSettings : ModBase
    {
        private SettingHandle<int> extraRaids;

        private SettingHandle<bool> forceDesperate;
        private SettingHandle<bool> forceRaidType;
        private SettingHandle<float> pointsOffset;
        private SettingHandle<bool> randomFactions;
        private SettingHandle<int> spawnThreshold;

        public override string ModIdentifier => "MultipleRaids";

        public override void DefsLoaded()
        {
            forceDesperate = Settings.GetHandle<bool>("forceDesperate", "Force Desperate",
                "Enables spawning of regular enemies on hard environment maps.");
            forceRaidType = Settings.GetHandle<bool>("forceRaidType", "Force Raid Type",
                "Forces every 3d raid to drop pod in center, every 4th to siege. Faction restrictions apply.");
            randomFactions = Settings.GetHandle<bool>("randomFactions", "Random Factions",
                "Allows spawned raids to belong to different factions");
            spawnThreshold = Settings.GetHandle("spawnThreshold", "Spawn Threshold",
                "Determines chance for an extra raid to spawn.", 50);
            extraRaids = Settings.GetHandle("extraRaids", "Extra Raids", "Maximum number of additional raids.", 1);
            pointsOffset = Settings.GetHandle("pointsOffset", "Points offset",
                "If more than one raid spawn, raid points are rescaled with this formula: points = points*(1/totalNum + pointsOffset).",
                0.35f);
        }
    }
}