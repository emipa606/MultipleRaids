using HarmonyLib;
using RimWorld;

namespace MultipleRaids;

[HarmonyPatch(typeof(IncidentWorker_RaidEnemy), nameof(IncidentWorker_RaidEnemy.FactionCanBeGroupSource),
    typeof(Faction), typeof(IncidentParms), typeof(bool))]
public static class IncidentWorker_RaidEnemy_FactionCanBeGroupSource
{
    public static void Prefix(ref bool desperate)
    {
        if (MultipleRaidsMod.Instance.Settings.ForceDesperate)
        {
            desperate = true;
        }
    }
}