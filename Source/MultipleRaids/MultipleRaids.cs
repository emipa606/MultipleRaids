using System.IO;
using System.Reflection;
using System.Xml.Linq;
using HarmonyLib;
using Verse;

namespace MultipleRaids;

[StaticConstructorOnStartup]
internal static class MultipleRaids
{
    static MultipleRaids()
    {
        new Harmony("Mlie.MultipleRaids").PatchAll(Assembly.GetExecutingAssembly());
        var hugsLibConfig = Path.Combine(GenFilePaths.SaveDataFolderPath, Path.Combine("HugsLib", "ModSettings.xml"));
        if (!new FileInfo(hugsLibConfig).Exists)
        {
            return;
        }

        var xml = XDocument.Load(hugsLibConfig);

        var modSettings = xml.Root?.Element("MultipleRaids");
        if (modSettings == null)
        {
            return;
        }

        foreach (var modSetting in modSettings.Elements())
        {
            if (modSetting.Name == "forceDesperate")
            {
                MultipleRaidsMod.Instance.Settings.ForceDesperate = bool.Parse(modSetting.Value);
            }

            if (modSetting.Name == "forceRaidType")
            {
                MultipleRaidsMod.Instance.Settings.ForceRaidType = bool.Parse(modSetting.Value);
            }

            if (modSetting.Name == "randomFactions")
            {
                MultipleRaidsMod.Instance.Settings.RandomFactions = bool.Parse(modSetting.Value);
            }

            if (modSetting.Name == "spawnThreshold")
            {
                MultipleRaidsMod.Instance.Settings.SpawnThreshold = int.Parse(modSetting.Value) / 100f;
            }

            if (modSetting.Name == "extraRaids")
            {
                MultipleRaidsMod.Instance.Settings.ExtraRaids = int.Parse(modSetting.Value);
            }

            if (modSetting.Name == "pointsOffset")
            {
                MultipleRaidsMod.Instance.Settings.PointsOffset = float.Parse(modSetting.Value);
            }
        }

        xml.Root?.Element("MultipleRaids")?.Remove();
        xml.Save(hugsLibConfig);

        Log.Message("[MultipleRaids]: Imported old HugLib-settings");
    }
}