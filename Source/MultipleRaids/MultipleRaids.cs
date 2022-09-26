using System.IO;
using System.Reflection;
using System.Xml.Linq;
using HarmonyLib;
using RimWorld;
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

[HarmonyPatch(typeof(IncidentWorker_RaidEnemy), "FactionCanBeGroupSource", typeof(Faction), typeof(Map),
    typeof(bool))]
public static class RimWorld_IncidentWorker_RaidEnemy_FactionCanBeGroupSource
{
    [HarmonyPrefix]
    public static void IncidentWorker_RaidEnemy_FactionCanBeGroupSource(ref Faction f, ref Map map,
        ref bool desperate)
    {
        if (MultipleRaidsMod.instance.Settings.ForceDesperate)
        {
            desperate = true;
        }
    }
}

[HarmonyPatch(typeof(IncidentWorker_RaidEnemy), "TryExecuteWorker", typeof(IncidentParms))]
public static class RimWorld_IncidentWorker_RaidEnemy_TryExecuteWorker
{
    private static int lastIncidentTick;

    [HarmonyPostfix]
    public static void IncidentWorker_RaidEnemy_TryExecuteWorker(ref IncidentParms parms,
        IncidentWorker_RaidEnemy __instance)
    {
        if (__instance.def.defName == "MultipleRaids_RaidEnemy")
        {
            return;
        }

        if (GenTicks.TicksAbs == lastIncidentTick)
        {
            return;
        }

        lastIncidentTick = GenTicks.TicksAbs;
        var customRaidDef = DefDatabase<IncidentDef>.GetNamedSilentFail("MultipleRaids_RaidEnemy");
        customRaidDef.Worker.TryExecute(parms);
    }
}

public class IncidentWorker_RaidEnemy_Custom : IncidentWorker_RaidEnemy
{
    private readonly float minPointsToSpawn = 35f;

    protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
    {
        return base.FactionCanBeGroupSource(f, map, desperate || MultipleRaidsMod.instance.Settings.ForceDesperate);
    }

    //This method is used to execute the incident.
    protected override bool TryExecuteWorker(IncidentParms parms)
    {
        //Log.Message("MultipleRaids. forceDesperate: " + forceDesperate + " spawnThreshold: " + spawnThreshold +
        //" numRaids: " + extraRaids);

        var totalNum = 0;
        for (var i = 0; i < MultipleRaidsMod.instance.Settings.ExtraRaids; i++)
        {
            var spawnChance = Rand.Value;
            //Log.Message("spawnChance: " + spawnChance + " spawnThreshold: " + spawnThreshold);
            if (spawnChance <= MultipleRaidsMod.instance.Settings.SpawnThreshold)
            {
                totalNum += 1;
            }
            else
            {
                break;
            }
        }

        if (totalNum == 0)
        {
            return false;
        }

        parms.faction = null;
        TryResolveRaidFaction(parms);

        var raidPoints = parms.points;
        if (totalNum > 1)
        {
            raidPoints = parms.points * ((1 / (float)totalNum) + MultipleRaidsMod.instance.Settings.PointsOffset);
        }

        if (raidPoints < minPointsToSpawn)
        {
            raidPoints = minPointsToSpawn;
        }

        var retVal = false;
        for (var i = 1; i <= totalNum; i++)
        {
            if (MultipleRaidsMod.instance.Settings.RandomFactions)
            {
                parms.faction = null;
                TryResolveRaidFaction(parms);
            }

            parms.raidStrategy = null;
            parms.spawnCenter = IntVec3.Invalid;
            parms.points = raidPoints;
            parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;

            if (MultipleRaidsMod.instance.Settings.ForceRaidType)
            {
                if (i % 3 == 0)
                {
                    // Spawn drop pod
                    if (parms.faction != null && parms.faction.def.techLevel >= TechLevel.Spacer &&
                        parms.points >= 240f)
                    {
                        parms.raidArrivalMode = PawnsArrivalModeDefOf.CenterDrop;
                    }
                }
                else
                {
                    if (i % 4 == 0)
                    {
                        // Spawn siege
                        var siegeStrategy = DefDatabase<RaidStrategyDef>.GetNamedSilentFail("Siege");
                        if (siegeStrategy != null &&
                            siegeStrategy.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat))
                        {
                            parms.raidStrategy = siegeStrategy;
                        }
                    }
                }
            }

            if (parms.faction != null)
            {
                //Log.Message("MultipleRaids. Spawning raid: " + i + " Faction: " + parms.faction.Name + " Points: " +
                //parms.points);
            }

            retVal |= base.TryExecuteWorker(parms);
        }

        return retVal;
    }
}