using Verse;

namespace MultipleRaids;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class MultipleRaidsSettings : ModSettings
{
    public int ExtraRaids = 1;
    public bool ForceDesperate;
    public bool ForceRaidType;
    public float PointsOffset = 0.35f;
    public bool RandomFactions;
    public float SpawnThreshold = 0.5f;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ForceDesperate, "ForceDesperate");
        Scribe_Values.Look(ref ForceRaidType, "ForceRaidType");
        Scribe_Values.Look(ref RandomFactions, "RandomFactions");
        Scribe_Values.Look(ref SpawnThreshold, "SpawnThreshold", 0.5f);
        Scribe_Values.Look(ref ExtraRaids, "ExtraRaids", 1);
        Scribe_Values.Look(ref PointsOffset, "PointsOffset", 0.35f);
    }
}