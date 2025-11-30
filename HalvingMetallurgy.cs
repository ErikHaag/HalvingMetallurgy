using Quintessential;
using System;

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

    public const string SoriaPermission = "HalvingMetallurgy:soria";

    public static string contentPath;

    public override void Load()
    {
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
        Glyphs.quickcopperRadioactive = SET.quickcopperRadioactive;
        Wheel.quickcopperRadioactive = SET.quickcopperRadioactive;
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
        Logger.Log(LogPrefix + "Goodbye!");
    }

    public override void LoadPuzzleContent()
    {
        Logger.Log(LogPrefix + "Loading!");
        Atoms.AddAtomTypes();
        // Load sounds
        contentPath = Brimstone.API.GetContentPath("HalvingMetallurgy").method_1087();
        Glyphs.LoadSounds();
        Glyphs.AddPartTypes();

        Wheel.LoadWheel();

        // Edge-case Dictionaries
        API.ShearingDictionary.Add(Brimstone.API.VanillaAtoms.quicksilver, new(Atoms.Quickcopper, Atoms.Quickcopper));

        // Add metallicity lookup
        API.AddMetalToMetallicityDictionary(Atoms.Beryl, 1);
        API.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms.lead, 2);
        API.AddMetalToMetallicityDictionary(Atoms.Wolfram, 3);
        API.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms.tin, 4);
        API.AddMetalToMetallicityDictionary(Atoms.Vulcan, 5);
        API.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms.iron, 6);
        API.AddMetalToMetallicityDictionary(Atoms.Nickel, 7);
        API.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms.copper, 8);
        API.AddMetalToMetallicityDictionary(Atoms.Zinc, 9);
        API.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms.silver, 10);
        API.AddMetalToMetallicityDictionary(Atoms.Sednum, 11);
        API.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms.gold, 12);
        API.AddMetalToMetallicityDictionary(Atoms.Osmium, 13);

        // Add quicksilver's metallicity lookup
        API.AddQuicksilverToDictionary(Atoms.Quicklime, 0);
        API.AddQuicksilverToDictionary(Atoms.Quickcopper, 1);
        API.AddQuicksilverToDictionary(Brimstone.API.VanillaAtoms.quicksilver, 2);

        // Add permissions
        QApi.AddPuzzlePermission(HalvingPermission, "Glyph of Halves", "Halving Metallurgy");
        QApi.AddPuzzlePermission(QuakePermission, "Glyph of Quake", "Halving Metallurgy");
        QApi.AddPuzzlePermission(SumpPermission, "Quicksilver Sump", "Halving Metallurgy");
        QApi.AddPuzzlePermission(RemissionPermission, "Glyph of Remission", "Halving Metallurgy");
        QApi.AddPuzzlePermission(ShearingPermission, "Glyph of Shearing", "Halving Metallurgy");
        QApi.AddPuzzlePermission(OsmosisPermission, "Glyph of Osmosis", "Halving Metallurgy");

        QApi.AddPuzzlePermission(SoriaPermission, "Soria's Wheel", "Halving Metallurgy");

        // Hooking
        Glyphs.LoadHooking();

        if (FTSIGCTULoaded)
        {
            LoadMapRules();
        }
        if (ReductiveMetallurgyLoaded)
        {
            LoadReductiveMetallurgyRules();
            LoadReductiveMetallurgyMethods();
        }
        if (VacancyLoaded)
        {
            LoadVacancyGlyph();
            LoadVacancyRules();
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

    #region Reductive Metallurgy
    private static void LoadReductiveMetallurgyRules()
    {
        //Add Rejection Rules for new atoms
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Osmium, Atoms.Sednum);
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Sednum, Atoms.Zinc);
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Zinc, Atoms.Nickel);
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Nickel, Atoms.Vulcan);
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Vulcan, Atoms.Wolfram);
        // Add Deposition Rules
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Osmium, Atoms.Nickel, Brimstone.API.VanillaAtoms.iron);
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Sednum, Brimstone.API.VanillaAtoms.iron, Atoms.Vulcan);
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Zinc, Atoms.Vulcan, Brimstone.API.VanillaAtoms.tin);
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Nickel, Brimstone.API.VanillaAtoms.tin, Atoms.Wolfram);
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Vulcan, Atoms.Wolfram, Brimstone.API.VanillaAtoms.lead);
        // Add Proliferation
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Osmium);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Sednum);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Zinc);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Nickel);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Vulcan);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Wolfram);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Beryl);
    }

    private static void LoadReductiveMetallurgyMethods()
    {
        Glyphs.findRavariAtom = ReductiveMetallurgy.Wheel.maybeFindRavariWheelAtom;
        Glyphs.drawRavariFlash = ReductiveMetallurgy.Wheel.DrawRavariFlash;
    }
    #endregion
    #region Vacancy

    private static void LoadVacancyGlyph()
    {
        Glyphs.ExtractionGlyph = Vaca.CustomGlyphs.Extraction;
    }

    private static void LoadVacancyRules()
    {
        API.AddMetalToMetallicityDictionary(Vaca.MainClass.VacaAtom, 0);
        Glyphs.CalculateAdjacencies();
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
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Nickel, 80);
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Zinc, 160);
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Sednum, 320);
        ScaffoldingGlyphs.API.AddScaffold(Atoms.Osmium, 640);
    }
    #endregion

    #endregion


}
