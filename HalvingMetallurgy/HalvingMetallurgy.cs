using HalvingMetallurgy.Utilities;
using Quintessential;
using System;
using System.Reflection;

namespace HalvingMetallurgy;

public class HalvingMetallurgy : QuintessentialMod, IDataMod
{
    public static HalvingMetallurgy Instance { get; }

    public override string ModId => "halving_metallurgy";

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
        Logger.Log(LogPrefix + "Loading!");
        Atoms.AddAtomTypes();
        Components.AtomDecorationComponent.Register();
        Sounds.LoadSounds();
        Glyphs.DefineParts();
        Glyphs.DefinePartRendering();
        Glyphs.DefinePartBehaviors();
        Recipes.AddRecipes();

        /*
        Wheel.LoadWheel();

        Exports.ExportContent();

        // Add metallicity lookup
        AtomType[] metals = new AtomType[] { Atoms.Beryl, VanillaAtoms.lead, Atoms.Wolfram, VanillaAtoms.tin, Atoms.Vulcan, VanillaAtoms.iron, Atoms.Nickel, VanillaAtoms.copper, Atoms.Zinc, VanillaAtoms.silver, Atoms.Sednum, VanillaAtoms.gold, Atoms.Osmium };
        for (int i = 0; i < metals.Length; i++)
        {
            API.AddMetalToMetallicityDictionary(metals[i], i + 1);
        }

        for (int i = 0; i < metals.Length - 1; i++)
        {
            API.HalvesDictionary.Add(metals[i], metals[i + 1]);
            API.OsmosisDictionary.Add(metals[i + 1], new(metals[i]));
            API.ShearingDictionary.Add(metals[i + 1], new(metals[(i + 1) >> 1], metals[i >> 1]));
        }
        API.ShearingDictionary.Add(VanillaAtoms.quicksilver, new(Atoms.Quickcopper, Atoms.Quickcopper));
        API.ShearingDictionary.Add(Atoms.Quickcopper, new(Atoms.Quickcopper, Atoms.Quicklime));
        API.ShearingDictionary.Add( Atoms.Quicklime, new(Atoms.Quicklime, Atoms.Quicklime));

        // Add quicksilver's metallicity lookup
        API.AddQuicksilverToDictionary(Atoms.Quicklime, 0);
        API.AddQuicksilverToDictionary(Atoms.Quickcopper, 1);
        API.AddQuicksilverToDictionary(VanillaAtoms.quicksilver, 2);
        */
        // Add permissions
        QApi.AddPuzzlePermission(Instance, HalvesPermission);
        QApi.AddPuzzlePermission(Instance, QuakePermission);
        QApi.AddPuzzlePermission(Instance, SumpPermission);
        QApi.AddPuzzlePermission(Instance, RemissionPermission);
        //QApi.AddPuzzlePermission(ShearingPermission, "Glyph of Shearing", "Halving Metallurgy");
        //QApi.AddPuzzlePermission(OsmosisPermission, "Glyph of Osmosis", "Halving Metallurgy");
        //QApi.AddPuzzlePermission(HarmonyPermission, "Glyph of Harmony", "Halving Metallurgy");

        //QApi.AddPuzzlePermission(SoriaPermission, "Soria's Wheel", "Halving Metallurgy");
        /*
        // Hooking
        Logger.Log(LogPrefix + "Hooking!");
        Glyphs.LoadHooking();
        Wheel.LoadHooks();

        if (FTSIGCTULoaded)
        {
            LoadMapRules();
        }
        if (ReductiveMetallurgyLoaded)
        {
            ReductiveMetallurgyImports.Load();
            ReductiveMetallurgyImports.LoadRules();
        }
        if (VacancyLoaded)
        {
            VacancyImports.Load();
            VacancyImports.LoadRules();
        }
        if (ScaffoldingLoaded)
        {
            LoadScaffoldingGlyphs();
        }
        */
        Logger.Log(LogPrefix + "Loading complete, have fun!");
    }

    public override void LoadCompatContent()
    {
    }

    public override void FinaliseContent()
    {
    }

    public override void PostLoad()
    {
        Quintessential.AtomTag.DumpTags();
    }

    public override void Unload()
    {
    }
}