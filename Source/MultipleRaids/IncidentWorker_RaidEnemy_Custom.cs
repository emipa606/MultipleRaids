using RimWorld;
using Verse;

namespace MultipleRaids;

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