using System;
using System.Reflection;
using HarmonyLib;
using HugsLib;
using RimWorld;
using Verse;

namespace MultipleRaids
{
    [StaticConstructorOnStartup]
    internal class MultipleRaids
    {
        static MultipleRaids()
        {
            var harmony = new Harmony("Mlie.MultipleRaids");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
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
            var settings = HugsLibController.SettingsManager.GetModSettings("MultipleRaids");
            var forceDesperate = false;
            if (settings.ValueExists("forceDesperate"))
            {
                bool.TryParse(settings.PeekValue("forceDesperate"), out forceDesperate);
            }

            if (forceDesperate)
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

        // Maximum number of extra raids
        private int extraRaids;

        // Remove faction environment check
        private bool forceDesperate;

        // Force raid type
        private bool forceRaidType;

        // Static points offset
        private float pointsOffset = 0.35f;

        // Allow spaning raids from random factions
        private bool randomFactions;

        // Threshold for spawning an extra raid
        private int spawnThreshold = 50;

        protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
        {
            return base.FactionCanBeGroupSource(f, map, desperate || forceDesperate);
        }

        //This method is used to execute the incident.
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (HugsLibController.SettingsManager.HasSettingsForMod("MultipleRaids"))
            {
                //Verse.Log.Message("Mod has settings");
                var settings = HugsLibController.SettingsManager.GetModSettings("MultipleRaids");

                if (settings.ValueExists("forceDesperate"))
                {
                    bool.TryParse(settings.PeekValue("forceDesperate"), out forceDesperate);
                }

                if (settings.ValueExists("forceRaidType"))
                {
                    bool.TryParse(settings.PeekValue("forceRaidType"), out forceRaidType);
                }

                if (settings.ValueExists("randomFactions"))
                {
                    bool.TryParse(settings.PeekValue("randomFactions"), out randomFactions);
                }

                if (settings.ValueExists("spawnThreshold"))
                {
                    spawnThreshold = int.Parse(settings.PeekValue("spawnThreshold"));
                }

                if (settings.ValueExists("extraRaids"))
                {
                    extraRaids = int.Parse(settings.PeekValue("extraRaids"));
                }

                if (settings.ValueExists("pointsOffset"))
                {
                    pointsOffset = float.Parse(settings.PeekValue("pointsOffset"));
                }

                if (pointsOffset < 0)
                {
                    pointsOffset = 0.35f;
                }
            }

            //Log.Message("MultipleRaids. forceDesperate: " + forceDesperate + " spawnThreshold: " + spawnThreshold +
            //" numRaids: " + extraRaids);

            var rng = new Random();
            var totalNum = 0;
            for (var i = 0; i < extraRaids; i++)
            {
                var spawnChance = rng.Next(1, 101);
                //Log.Message("spawnChance: " + spawnChance + " spawnThreshold: " + spawnThreshold);
                if (spawnChance <= spawnThreshold)
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
                raidPoints = parms.points * ((1 / (float) totalNum) + pointsOffset);
            }

            if (raidPoints < minPointsToSpawn)
            {
                raidPoints = minPointsToSpawn;
            }

            var retVal = false;
            for (var i = 1; i <= totalNum; i++)
            {
                if (randomFactions)
                {
                    parms.faction = null;
                    TryResolveRaidFaction(parms);
                }

                parms.raidStrategy = null;
                parms.spawnCenter = IntVec3.Invalid;
                parms.points = raidPoints;
                parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;

                if (forceRaidType)
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
}