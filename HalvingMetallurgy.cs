using Brimstone;
using Quintessential;

namespace HalvingMetallurgy;

public class HalvingMetallurgy : QuintessentialMod
{
    // optional dependencies
    public static readonly bool ReductiveMetallurgyLoaded = Brimstone.API.IsModLoaded("ReductiveMetallurgy");
    public static readonly bool FTSIGCTULoaded = Brimstone.API.IsModLoaded("FTSIGCTU");
    public static readonly bool VacancyLoaded = Brimstone.API.IsModLoaded("Vacancy");

    // permissions
    public const string HalvingPermission = "HalvingMetallurgy:halving";
    public const string QuakePermission = "HalvingMetallurgy:quake";
    public const string SumpPermission = "HalvingMetallurgy:sump";
    public const string RemissionPermission = "HalvingMetallurgy:remission";
    public const string ShearingPermission = "HalvingMetallurgy:shearing";

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
            Quintessential.Logger.Log("Halving Metallurgy: Found FTSIGCTU");
        }
        if (ReductiveMetallurgyLoaded)
        {
            Quintessential.Logger.Log("HalvingMetallurgy: Found Reductive Metallurgy");
        }
        if (VacancyLoaded)
        {
            Quintessential.Logger.Log("HalvingMetallurgy: Found Vaca");
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
        }
    }

    public override void Unload()
    {
        Quintessential.Logger.Log("Halving Metallurgy: Goodbye!");
    }

    public override void LoadPuzzleContent()
    {
        Quintessential.Logger.Log("HalvingMetallurgy: Loading!");
        HalvingMetallurgyAtoms.AddAtomTypes();
        // Load sounds
        contentPath = Brimstone.API.GetContentPath("HalvingMetallurgy").method_1087();
        HalvingMetallurgyParts.LoadSounds();
        HalvingMetallurgyParts.AddPartTypes();
        // Add glyph of halves promotions
        HalvingMetallurgyAPI.HalvingPromotions.Add(Brimstone.API.VanillaAtoms["lead"], HalvingMetallurgyAtoms.Wolfram);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Wolfram, Brimstone.API.VanillaAtoms["tin"]);
        HalvingMetallurgyAPI.HalvingPromotions.Add(Brimstone.API.VanillaAtoms["tin"], HalvingMetallurgyAtoms.Vulcan);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Vulcan, Brimstone.API.VanillaAtoms["iron"]);
        HalvingMetallurgyAPI.HalvingPromotions.Add(Brimstone.API.VanillaAtoms["iron"], HalvingMetallurgyAtoms.Nickel);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Nickel, Brimstone.API.VanillaAtoms["copper"]);
        HalvingMetallurgyAPI.HalvingPromotions.Add(Brimstone.API.VanillaAtoms["copper"], HalvingMetallurgyAtoms.Zinc);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Zinc, Brimstone.API.VanillaAtoms["silver"]);
        HalvingMetallurgyAPI.HalvingPromotions.Add(Brimstone.API.VanillaAtoms["silver"], HalvingMetallurgyAtoms.Sednum);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Sednum, Brimstone.API.VanillaAtoms["gold"]);
        HalvingMetallurgyAPI.HalvingPromotions.Add(Brimstone.API.VanillaAtoms["gold"], HalvingMetallurgyAtoms.Osmium);
        // Add metallicity lookup
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["lead"], 2);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Wolfram, 3);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["tin"], 4);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Vulcan, 5);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["iron"],6);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Nickel, 7);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["copper"], 8);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Zinc, 9);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["silver"], 10);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Sednum, 11);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Brimstone.API.VanillaAtoms["gold"], 12);
        HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(HalvingMetallurgyAtoms.Osmium, 13);

        // Add permissions
        QApi.AddPuzzlePermission(HalvingPermission, "Glyph of Halves", "Halving Metallurgy");
        QApi.AddPuzzlePermission(QuakePermission, "Glyph of Quake", "Halving Metallurgy");
        QApi.AddPuzzlePermission(SumpPermission, "Quicksilver Sump", "Halving Metallurgy");
        QApi.AddPuzzlePermission(RemissionPermission, "Glyph of Remission", "Halving Metallurgy");
        QApi.AddPuzzlePermission(ShearingPermission, "Glyph of Shearing", "Halving Metallurgy");

        // Hooking
        IL.SolutionEditorScreen.method_2097 += HalvingMetallurgyParts.InjectSimStartup;

        if (FTSIGCTULoaded)
        {
            // Add Glyphs to FTSIGCTU's map
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Halves, FTSIGCTU.Navigation.PartsMap.glyphRule);
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Quake, FTSIGCTU.Navigation.PartsMap.glyphRule);
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Sump, FTSIGCTU.Navigation.PartsMap.glyphRule);
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Remission, FTSIGCTU.Navigation.PartsMap.glyphRule);
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Shearing, FTSIGCTU.Navigation.PartsMap.glyphRule);
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
        }
        if (VacancyLoaded)
        {
            HalvingMetallurgyAPI.AddMetalToMetallicityDictionary(Vaca.MainClass.VacaAtom, 0);
        }
        Quintessential.Logger.Log("Loading complete, have fun!");
    }
}
