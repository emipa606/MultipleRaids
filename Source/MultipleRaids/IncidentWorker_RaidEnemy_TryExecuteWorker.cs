using HarmonyLib;
using RimWorld;
using Verse;

namespace MultipleRaids;

[HarmonyPatch(typeof(IncidentWorker_RaidEnemy), "TryExecuteWorker", typeof(IncidentParms))]
public static class IncidentWorker_RaidEnemy_TryExecuteWorker
{
    private static int lastIncidentTick;

    public static void Postfix(ref IncidentParms parms,
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