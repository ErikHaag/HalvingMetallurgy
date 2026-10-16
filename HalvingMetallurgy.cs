using HalvingMetallurgy.Utilities;
using Quintessential;
using System;
using System.Reflection;
using VanillaAtoms = Brimstone.API.VanillaAtoms;

namespace HalvingMetallurgy;

public class HalvingMetallurgy : QuintessentialMod
{
    public static QuintessentialMod self;
    public override Type SettingsType => typeof(MySettings);

    // optional dependencies
    public static readonly bool ReductiveMetallurgyLoaded = Brimstone.API.IsModLoaded("ReductiveMetallurgy");
    public static readonly bool FTSIGCTULoaded = Brimstone.API.IsModLoaded("FTSIGCTU");
    public static readonly bool VacancyLoaded = Brimstone.API.IsModLoaded("Vacancy");
    public static readonly bool ScaffoldingLoaded = Brimstone.API.IsModLoaded("ScaffoldingGlyphs");

    public const string LogPrefix = "Halving Metallurgy: ";

    // permissions
    public const string HalvingPermission = "HalvingMetallurgy:halving";
    public const string QuakePermission = "HalvingMetallurgy:quake";
    public const string SumpPermission = "HalvingMetallurgy:sump";
    public const string RemissionPermission = "HalvingMetallurgy:remission";
    public const string ShearingPermission = "HalvingMetallurgy:shearing";
    public const string OsmosisPermission = "HalvingMetallurgy:osmosis";
    public const string HarmonyPermission = "HalvingMetallurgy:harmony";

    public const string SoriaPermission = "HalvingMetallurgy:soria";

    public static string contentPath;

    public override void Load()
    {
        Exports.ExportTextures();
        self = this;
        Settings = new MySettings();

        if (FTSIGCTULoaded)
        {
            Logger.Log(LogPrefix + "Found FTSIGCTU");
        }
        if (ReductiveMetallurgyLoaded)
        {
            Logger.Log(LogPrefix + "Found Reductive Metallurgy");
        }
        if (VacancyLoaded)
        {
            Logger.Log(LogPrefix + "Found Vaca");
        }
        if (ScaffoldingLoaded)
        {
            Logger.Log(LogPrefix + "Found Scaffolding Glyphs");
        }
    }
    public override void ApplySettings()
    {
        base.ApplySettings();
        MySettings SET = (MySettings)Settings;

        Atoms.quickcopperRadioactive = SET.quickcopperRadioactive;
    }

    public override void PostLoad()
    {
        if (FTSIGCTULoaded)
        {
            LoadMirrorRules();
        }
    }

    public override void Unload()
    {
        Glyphs.UnloadHooks();
        Wheel.UnloadHooks();
        Sounds.Unload();
        Logger.Log(LogPrefix + "Goodbye!");
    }

    public override void LoadPuzzleContent()
    {
        Logger.Log(LogPrefix + "Loading!");
        Atoms.AddAtomTypes();
        // Load sounds
        contentPath = Brimstone.API.GetContentPath("HalvingMetallurgy").method_1087();
        Sounds.LoadSounds();
        Glyphs.DefineParts();
        Glyphs.DefinePartRendering();
        Glyphs.DefinePartBehaviors();
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

        // Add permissions
        QApi.AddPuzzlePermission(HalvingPermission, "Glyph of Halves", "Halving Metallurgy");
        QApi.AddPuzzlePermission(QuakePermission, "Glyph of Quake", "Halving Metallurgy");
        QApi.AddPuzzlePermission(SumpPermission, "Quicksilver Sump", "Halving Metallurgy");
        QApi.AddPuzzlePermission(RemissionPermission, "Glyph of Remission", "Halving Metallurgy");
        QApi.AddPuzzlePermission(ShearingPermission, "Glyph of Shearing", "Halving Metallurgy");
        QApi.AddPuzzlePermission(OsmosisPermission, "Glyph of Osmosis", "Halving Metallurgy");
        QApi.AddPuzzlePermission(HarmonyPermission, "Glyph of Harmony", "Halving Metallurgy");

        QApi.AddPuzzlePermission(SoriaPermission, "Soria's Wheel", "Halving Metallurgy");

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
        Logger.Log(LogPrefix + "Loading complete, have fun!");
    }

    #region External Method calls
    #region FTSIGCTU
    private static bool MirrorHalves(SolutionEditorScreen ses, Part part, bool mirrorVert, HexIndex pivot)
    {
        FTSIGCTU.MirrorTool.shiftRotation(part, HexRotation.Counterclockwise);
        FTSIGCTU.MirrorTool.mirrorSimplePart(ses, part, mirrorVert, pivot);
        FTSIGCTU.MirrorTool.shiftRotation(part, HexRotation.Clockwise);
        return true;
    }

    private static void LoadMirrorRules()
    {
        FTSIGCTU.MirrorTool.addRule(Glyphs.Halves, MirrorHalves);
        FTSIGCTU.MirrorTool.addRule(Glyphs.Quake, FTSIGCTU.MirrorTool.mirrorSingleton);
        FTSIGCTU.MirrorTool.addRule(Glyphs.Sump, FTSIGCTU.MirrorTool.mirrorSimplePart);
        FTSIGCTU.MirrorTool.addRule(Glyphs.Remission, FTSIGCTU.MirrorTool.mirrorSimplePart);
        FTSIGCTU.MirrorTool.addRule(Glyphs.Shearing, FTSIGCTU.MirrorTool.mirrorSimplePart);
        FTSIGCTU.MirrorTool.addRule(Glyphs.Osmosis, FTSIGCTU.MirrorTool.mirrorSimplePart);

        FTSIGCTU.MirrorTool.addRule(Wheel.Soria, FTSIGCTU.MirrorTool.mirrorVanillaArm);
    }

    private static void LoadMapRules()
    {
        // Add Glyphs to FTSIGCTU's map
        FTSIGCTU.Navigation.PartsMap.addPartHexRule(Glyphs.Halves, FTSIGCTU.Navigation.PartsMap.glyphRule);
        FTSIGCTU.Navigation.PartsMap.addPartHexRule(Glyphs.Quake, FTSIGCTU.Navigation.PartsMap.glyphRule);
        FTSIGCTU.Navigation.PartsMap.addPartHexRule(Glyphs.Sump, FTSIGCTU.Navigation.PartsMap.glyphRule);
        FTSIGCTU.Navigation.PartsMap.addPartHexRule(Glyphs.Remission, FTSIGCTU.Navigation.PartsMap.glyphRule);
        FTSIGCTU.Navigation.PartsMap.addPartHexRule(Glyphs.Shearing, FTSIGCTU.Navigation.PartsMap.glyphRule);
        FTSIGCTU.Navigation.PartsMap.addPartHexRule(Glyphs.Osmosis, FTSIGCTU.Navigation.PartsMap.glyphRule);

        FTSIGCTU.Navigation.PartsMap.addPartHexRule(Wheel.Soria, FTSIGCTU.Navigation.PartsMap.armHexRule);
    }

    #endregion

    #region Scaffolding Glyphs
    private static void LoadScaffoldingGlyphs()
    {
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Quicklime, 0);
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Quickcopper, 10);
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Beryl, 10);
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Wolfram, 20);
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Vulcan, 40);
        // worst than expected, but oh well.
        MethodInfo m = typeof(ScaffoldingGlyphs.API).GetMethod("generateSymbolFromAtomtype", BindingFlags.NonPublic | BindingFlags.Static);
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Nickel, 80, (class_256)m.Invoke(null, new object[] { new AtomType { field_2287 = Brimstone.API.GetTexture("textures/atoms/erikhaag/HalvingMetallurgy/nickel_symbol") } }));
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Zinc, 160);
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Sednum, 320);
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Osmium, 640);
    }
    #endregion

    #endregion
}