using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Quintessential;

namespace HalvingMetallurgy;

public class HalvingMetallurgy : QuintessentialMod
{
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

    public static string contentPath;

    public static bool MirrorHalfProjectionPart(SolutionEditorScreen ses, Part part, bool mirrorVert, HexIndex pivot)
    {
        FTSIGCTU.MirrorTool.shiftRotation(part, HexRotation.Counterclockwise);
        FTSIGCTU.MirrorTool.mirrorSimplePart(ses, part, mirrorVert, pivot);
        FTSIGCTU.MirrorTool.shiftRotation(part, HexRotation.Clockwise);
        return true;
    }

    public override void Load()
    {
        if (FTSIGCTULoaded)
        {
            Quintessential.Logger.Log(LogPrefix + "Found FTSIGCTU");
        }
        if (ReductiveMetallurgyLoaded)
        {
            Quintessential.Logger.Log(LogPrefix + "Found Reductive Metallurgy");
        }
        if (VacancyLoaded)
        {
            Quintessential.Logger.Log(LogPrefix + "Found Vaca");
        }
        if (ScaffoldingLoaded)
        {
            Quintessential.Logger.Log(LogPrefix + "Found Scaffolding Glyphs");
        }
    }

    public override void PostLoad()
    {
        if (FTSIGCTULoaded)
        {
            FTSIGCTU.MirrorTool.addRule(HalvingMetallurgyParts.Halves, MirrorHalfProjectionPart);
            FTSIGCTU.MirrorTool.addRule(HalvingMetallurgyParts.Quake, FTSIGCTU.MirrorTool.mirrorSingleton);
            FTSIGCTU.MirrorTool.addRule(HalvingMetallurgyParts.Sump, FTSIGCTU.MirrorTool.mirrorHorizontalPart(0));
            FTSIGCTU.MirrorTool.addRule(HalvingMetallurgyParts.Remission, FTSIGCTU.MirrorTool.mirrorHorizontalPart(0));
            FTSIGCTU.MirrorTool.addRule(HalvingMetallurgyParts.Shearing, FTSIGCTU.MirrorTool.mirrorHorizontalPart(0));
            FTSIGCTU.MirrorTool.addRule(HalvingMetallurgyParts.Osmosis, FTSIGCTU.MirrorTool.mirrorHorizontalPart(0));
        }
    }

    public override void Unload()
    {
        HalvingMetallurgyParts.UnloadHooks();
        Quintessential.Logger.Log(LogPrefix + "Goodbye!");
    }

    public override void LoadPuzzleContent()
    {
        Quintessential.Logger.Log(LogPrefix + "Loading!");
        HalvingMetallurgyAtoms.AddAtomTypes();
        // Load sounds
        contentPath = Brimstone.API.GetContentPath("HalvingMetallurgy").method_1087();
        HalvingMetallurgyParts.LoadSounds();
        HalvingMetallurgyParts.AddPartTypes();

        // Edge-case Dictionaries
        HalvingMetallurgyAPI.ShearingDictionary.Add(Brimstone.API.VanillaAtoms["quicksilver"], new(HalvingMetallurgyAtoms.Quickcopper, HalvingMetallurgyAtoms.Quickcopper));

        // Add metallicity lookup
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Beryl, 1);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["lead"], 2);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Wolfram, 3);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["tin"], 4);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Vulcan, 5);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["iron"], 6);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Nickel, 7);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["copper"], 8);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Zinc, 9);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["silver"], 10);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Sednum, 11);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["gold"], 12);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Osmium, 13);

        // Atom Swapping
        HalvingMetallurgyAPI.ConvertBeforeConsumption.Add(HalvingMetallurgyAtoms.ActiveQuickcopper, HalvingMetallurgyAtoms.Quickcopper);
        HalvingMetallurgyAPI.ConvertAfterHalfstep.Add(HalvingMetallurgyAtoms.Quickcopper, HalvingMetallurgyAtoms.ActiveQuickcopper);

        // Add permissions
        QApi.AddPuzzlePermission(HalvingPermission, "Glyph of Halves", "Halving Metallurgy");
        QApi.AddPuzzlePermission(QuakePermission, "Glyph of Quake", "Halving Metallurgy");
        QApi.AddPuzzlePermission(SumpPermission, "Quicksilver Sump", "Halving Metallurgy");
        QApi.AddPuzzlePermission(RemissionPermission, "Glyph of Remission", "Halving Metallurgy");
        QApi.AddPuzzlePermission(ShearingPermission, "Glyph of Shearing", "Halving Metallurgy");
        QApi.AddPuzzlePermission(OsmosisPermission, "Glyph of Osmosis", "Halving Metallurgy");

        // Hooking
        HalvingMetallurgyParts.LoadHooking();

        if (FTSIGCTULoaded)
        {
            // Add Glyphs to FTSIGCTU's map
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Halves, FTSIGCTU.Navigation.PartsMap.glyphRule);
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Quake, FTSIGCTU.Navigation.PartsMap.glyphRule);
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Sump, FTSIGCTU.Navigation.PartsMap.glyphRule);
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Remission, FTSIGCTU.Navigation.PartsMap.glyphRule);
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Shearing, FTSIGCTU.Navigation.PartsMap.glyphRule);
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Osmosis, FTSIGCTU.Navigation.PartsMap.glyphRule);
        }
        if (ReductiveMetallurgyLoaded)
        {
            //Add Rejection Rules for new atoms
            ReductiveMetallurgy.API.addRejectionRule(HalvingMetallurgyAtoms.Osmium, HalvingMetallurgyAtoms.Sednum);
            ReductiveMetallurgy.API.addRejectionRule(HalvingMetallurgyAtoms.Sednum, HalvingMetallurgyAtoms.Zinc);
            ReductiveMetallurgy.API.addRejectionRule(HalvingMetallurgyAtoms.Zinc, HalvingMetallurgyAtoms.Nickel);
            ReductiveMetallurgy.API.addRejectionRule(HalvingMetallurgyAtoms.Nickel, HalvingMetallurgyAtoms.Vulcan);
            ReductiveMetallurgy.API.addRejectionRule(HalvingMetallurgyAtoms.Vulcan, HalvingMetallurgyAtoms.Wolfram);
            // Add Deposition Rules
            ReductiveMetallurgy.API.addDepositionRule(HalvingMetallurgyAtoms.Osmium, HalvingMetallurgyAtoms.Nickel, Brimstone.API.VanillaAtoms["iron"]);
            ReductiveMetallurgy.API.addDepositionRule(HalvingMetallurgyAtoms.Sednum, Brimstone.API.VanillaAtoms["iron"], HalvingMetallurgyAtoms.Vulcan);
            ReductiveMetallurgy.API.addDepositionRule(HalvingMetallurgyAtoms.Zinc, HalvingMetallurgyAtoms.Vulcan, Brimstone.API.VanillaAtoms["tin"]);
            ReductiveMetallurgy.API.addDepositionRule(HalvingMetallurgyAtoms.Nickel, Brimstone.API.VanillaAtoms["tin"], HalvingMetallurgyAtoms.Wolfram);
            ReductiveMetallurgy.API.addDepositionRule(HalvingMetallurgyAtoms.Vulcan, HalvingMetallurgyAtoms.Wolfram, Brimstone.API.VanillaAtoms["lead"]);
            // Add Proliferation
            ReductiveMetallurgy.API.addProliferationRule(HalvingMetallurgyAtoms.Osmium);
            ReductiveMetallurgy.API.addProliferationRule(HalvingMetallurgyAtoms.Sednum);
            ReductiveMetallurgy.API.addProliferationRule(HalvingMetallurgyAtoms.Zinc);
            ReductiveMetallurgy.API.addProliferationRule(HalvingMetallurgyAtoms.Nickel);
            ReductiveMetallurgy.API.addProliferationRule(HalvingMetallurgyAtoms.Vulcan);
            ReductiveMetallurgy.API.addProliferationRule(HalvingMetallurgyAtoms.Wolfram);
            ReductiveMetallurgy.API.addProliferationRule(HalvingMetallurgyAtoms.Beryl);
        }
        if (VacancyLoaded)
        {
            HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Vaca.MainClass.VacaAtom, 0);
            HalvingMetallurgyParts.CalculateAdjacencies();
        }
        if (ScaffoldingLoaded)
        {
            ScaffoldingGlyphs.API.AddScaffold(HalvingMetallurgyAtoms.Quickcopper, 10);
            ScaffoldingGlyphs.API.AddScaffold(HalvingMetallurgyAtoms.Beryl, 10);
            ScaffoldingGlyphs.API.AddScaffold(HalvingMetallurgyAtoms.Wolfram, 20);
            ScaffoldingGlyphs.API.AddScaffold(HalvingMetallurgyAtoms.Vulcan, 40);
            ScaffoldingGlyphs.API.AddScaffold(HalvingMetallurgyAtoms.Nickel, 80);
            ScaffoldingGlyphs.API.AddScaffold(HalvingMetallurgyAtoms.Zinc, 160);
            ScaffoldingGlyphs.API.AddScaffold(HalvingMetallurgyAtoms.Sednum, 320);
            ScaffoldingGlyphs.API.AddScaffold(HalvingMetallurgyAtoms.Osmium, 640);
        } 
        Quintessential.Logger.Log(LogPrefix + "Loading complete, have fun!");
    }
}
