using RimWorld;
using Verse;

using System;
using System.Collections.Generic;
using System.Linq;

using HugsLib;
using HugsLib.Settings;

namespace MultipleRaids
{
    public class IncidentWorker_RaidEnemy_Custom : IncidentWorker_RaidEnemy {
        // Threshold for spawning an extra raid
        int spawnThreshold = 50;

        // Maximum number of extra raids
        int extraRaids = 1;

        // Remove faction environment check
        bool forceDesperate = false;
        
        // Force raid type
        bool forceRaidType = false;
        
        // Allow spaning raids from random factions
        bool randomFactions = false;
        
        // Static points offset
        float pointsOffset = 0.35f;
        
        float minPointsToSpawn = 35f;
        
        protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false) {
            return base.FactionCanBeGroupSource(f, map, desperate || forceDesperate);
        }
        
        //This method is used to execute the incident.
        protected override bool TryExecuteWorker(IncidentParms parms) {
            
            if (HugsLibController.SettingsManager.HasSettingsForMod("MultipleRaids")) {
                //Verse.Log.Message("Mod has settings");
                ModSettingsPack settings = HugsLibController.SettingsManager.GetModSettings("MultipleRaids");
                
                if (settings.ValueExists("forceDesperate"))
                    Boolean.TryParse(settings.PeekValue("forceDesperate"), out forceDesperate);
                
                if (settings.ValueExists("forceRaidType"))
                    Boolean.TryParse(settings.PeekValue("forceRaidType"), out forceRaidType);
                    
                if (settings.ValueExists("randomFactions"))
                    Boolean.TryParse(settings.PeekValue("randomFactions"), out randomFactions);
                
                if (settings.ValueExists("spawnThreshold"))
                    spawnThreshold =  Int32.Parse(settings.PeekValue("spawnThreshold"));
                
                if (settings.ValueExists("extraRaids"))
                    extraRaids =  Int32.Parse(settings.PeekValue("extraRaids"));
                
                if (settings.ValueExists("pointsOffset"))
                    pointsOffset =  float.Parse(settings.PeekValue("pointsOffset"));
                
                if (pointsOffset < 0)
                    pointsOffset = 0.35f;
            }

            //Verse.Log.Message("MultipleRaids. forceDesperate: " + forceDesperate + " spawnThreshold: " + spawnThreshold + " numRaids: " + extraRaids);
            
            Random rng = new Random();
            int totalNum = 1;
            for (int i = 0; i < extraRaids; i++) {
                int spawnChance = rng.Next(1,101);
               // Verse.Log.Error("spawnChance: " + spawnChance + " spawnThreshold: " + spawnThreshold);
                if (spawnChance <= spawnThreshold) {
                    totalNum += 1;
                } else
                    break;
            }
            
            parms.faction = null;
            TryResolveRaidFaction(parms);
            
            float raidPoints = parms.points;
            if (totalNum > 1)
                raidPoints = parms.points*((float)(1/totalNum) +(float) pointsOffset);
            
            if (raidPoints < minPointsToSpawn)
                raidPoints = minPointsToSpawn;
            
            bool retVal = false;
            for (int i = 1; i <= totalNum; i++) {
                if (randomFactions) {
                    parms.faction = null;
                    TryResolveRaidFaction(parms);
                }
                
                parms.raidStrategy = null;
                parms.spawnCenter = Verse.IntVec3.Invalid;
                parms.points = raidPoints;
                parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;

                if (forceRaidType) {
                    if (i % 3 == 0) {
                        // Spawn drop pod
                        if (parms.faction.def.techLevel >= TechLevel.Spacer && parms.points >= 240f)
                            parms.raidArrivalMode = PawnsArrivalModeDefOf.CenterDrop;
                    } else {
                        if (i % 4 == 0) {
                            // Spawn siege
                            RaidStrategyDef siegeStrategy = DefDatabase<RaidStrategyDef>.GetNamedSilentFail("Siege");
                            if (siegeStrategy != null && siegeStrategy.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat)) {
                                    parms.raidStrategy = siegeStrategy;
                            }
                        }
                    }
                }
                
                Verse.Log.Message("MultipleRaids. Spawning raid: " + i + " Faction: " + parms.faction.Name + " Points: " + parms.points);
                retVal |= base.TryExecuteWorker(parms);
            }
            
            return retVal;
        }
     }
}