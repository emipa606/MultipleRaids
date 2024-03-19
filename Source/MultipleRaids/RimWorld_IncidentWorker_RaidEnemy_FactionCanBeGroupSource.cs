using HarmonyLib;
using RimWorld;
using Verse;

namespace MultipleRaids;

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