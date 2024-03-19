using Mlie;
using UnityEngine;
using Verse;

namespace MultipleRaids;

[StaticConstructorOnStartup]
internal class MultipleRaidsMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static MultipleRaidsMod instance;

    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    private MultipleRaidsSettings settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public MultipleRaidsMod(ModContentPack content) : base(content)
    {
        instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal MultipleRaidsSettings Settings
    {
        get
        {
            if (settings == null)
            {
                settings = GetSettings<MultipleRaidsSettings>();
            }

            return settings;
        }
        set => settings = value;
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Multiple Raids";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("MR.ForceDesperate".Translate(), ref Settings.ForceDesperate,
            "MR.ForceDesperate.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("MR.ForceRaidType".Translate(), ref Settings.ForceRaidType,
            "MR.ForceRaidType.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("MR.RandomFactions".Translate(), ref Settings.RandomFactions,
            "MR.RandomFactions.Tooltip".Translate());

        listing_Standard.Gap();
        listing_Standard.Label("MR.SpawnThreshold".Translate(Settings.SpawnThreshold.ToStringPercent()), -1,
            "MR.SpawnThreshold.Tooltip".Translate());
        Settings.SpawnThreshold = Widgets.HorizontalSlider(listing_Standard.GetRect(20), Settings.SpawnThreshold, 0, 1f,
            false, Settings.SpawnThreshold.ToStringPercent());

        listing_Standard.Gap();
        listing_Standard.Label("MR.PointsOffset".Translate(Settings.PointsOffset.ToStringPercent()), -1,
            "MR.PointsOffset.Tooltip".Translate());
        Settings.PointsOffset = Widgets.HorizontalSlider(listing_Standard.GetRect(20), Settings.PointsOffset, 0, 1f,
            false, Settings.PointsOffset.ToStringPercent());

        listing_Standard.Gap();
        listing_Standard.Label("MR.ExtraRaids".Translate(Settings.ExtraRaids), -1, "MR.ExtraRaids.Tooltip".Translate());
        listing_Standard.IntAdjuster(ref Settings.ExtraRaids, 1, 1);
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("MR.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}