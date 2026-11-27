using Quintessential;
using System;

namespace HalvingMetallurgy;

public class HalvingMetallurgy : QuintessentialMod, IDataMod
{
    public static HalvingMetallurgy Instance { get; }

    public override string ModId => "halving_metallurgy";

    public override Type SettingsType => typeof(HalvingMetallurgySettings);

    public const string LogPrefix = "Halving Metallurgy: ";

    // permissions
    public const string HalvesPermission = "halves";
    public const string QuakePermission = "quake";
    public const string SumpPermission = "sump";
    public const string RemissionPermission = "remission";
    public const string ShearingPermission = "shearing";
    public const string OsmosisPermission = "osmosis";
    public const string HarmonyPermission = "harmony";

    public const string SoriaPermission = "soria";

    public override void Load()
    {}

    public override void LoadContent()
    {
        Instance.Log.Info(LogPrefix + "Loading!");
        Atoms.AddAtomTypes();
        Components.AtomDecorationComponent.Register();
        Sounds.LoadSounds();
        Glyphs.DefineParts();
        Glyphs.DefinePartRendering();
        Glyphs.DefinePartBehaviors();
        Recipes.AddRecipes();

        // Add metallicity lookup
        AtomType[] metals = [Atoms.Beryl, AtomTypes.lead, Atoms.Wolfram, AtomTypes.tin, Atoms.Vulcan, AtomTypes.iron, Atoms.Nickel, AtomTypes.copper, Atoms.Zinc, AtomTypes.silver, Atoms.Sednum, AtomTypes.gold, Atoms.Osmium];
        for (int i = 0; i < metals.Length; i++)
        {
            API.AddAtomToRemissionTable(metals[i].Id, i + 1);
        }

        // Add permissions
        QApi.AddPuzzlePermission(Instance, HalvesPermission);
        QApi.AddPuzzlePermission(Instance, QuakePermission);
        QApi.AddPuzzlePermission(Instance, SumpPermission);
        QApi.AddPuzzlePermission(Instance, RemissionPermission);
        QApi.AddPuzzlePermission(Instance, ShearingPermission);
        QApi.AddPuzzlePermission(Instance, OsmosisPermission);
        QApi.AddPuzzlePermission(Instance, HarmonyPermission);

        QApi.AddPuzzlePermission(Instance, SoriaPermission);
        Instance.Log.Info(LogPrefix + "Loading complete, have fun!");
    }

    public override void LoadCompatContent()
    {
    }

    public override void FinaliseContent()
    {
    }

    public override void PostLoad()
    {
        //Quintessential.AtomTag.DumpTags();
    }

    public override void Unload()
    {
    }

    public override void ApplySettings()
    {
        HalvingMetallurgySettings hms = (HalvingMetallurgySettings)Settings;
        Atoms.quickcopperRadioactive = hms.quickcopperRadioactive;
    }
}