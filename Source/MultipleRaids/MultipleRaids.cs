using System.IO;
using System.Reflection;
using System.Xml.Linq;
using HarmonyLib;
using Verse;

namespace MultipleRaids;

[StaticConstructorOnStartup]
internal class MultipleRaids
{
    static MultipleRaids()
    {
        var harmony = new Harmony("Mlie.MultipleRaids");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
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
                MultipleRaidsMod.instance.Settings.ForceDesperate = bool.Parse(modSetting.Value);
            }

            if (modSetting.Name == "forceRaidType")
            {
                MultipleRaidsMod.instance.Settings.ForceRaidType = bool.Parse(modSetting.Value);
            }

            if (modSetting.Name == "randomFactions")
            {
                MultipleRaidsMod.instance.Settings.RandomFactions = bool.Parse(modSetting.Value);
            }

            if (modSetting.Name == "spawnThreshold")
            {
                MultipleRaidsMod.instance.Settings.SpawnThreshold = int.Parse(modSetting.Value) / 100f;
            }

            if (modSetting.Name == "extraRaids")
            {
                MultipleRaidsMod.instance.Settings.ExtraRaids = int.Parse(modSetting.Value);
            }

            if (modSetting.Name == "pointsOffset")
            {
                MultipleRaidsMod.instance.Settings.PointsOffset = float.Parse(modSetting.Value);
            }
        }

        xml.Root.Element("MultipleRaids")?.Remove();
        xml.Save(hugsLibConfig);

        Log.Message("[MultipleRaids]: Imported old HugLib-settings");
    }
}