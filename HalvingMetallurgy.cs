using Brimstone;
using Quintessential;

namespace HalvingMetallurgy;

public class HalvingMetallurgy : QuintessentialMod
{
    public static readonly bool ReductiveMetallurgyLoaded = Brimstone.API.IsModLoaded("ReductiveMetallurgy");
    public static readonly bool FTSIGCTULoaded = Brimstone.API.IsModLoaded("FTSIGCTU");
    public const string HalvingPermission = "HalvingMetallurgy:halving";
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
    }

    public override void PostLoad()
    {
        if (FTSIGCTULoaded)
        {
            Quintessential.Logger.Log("Halving Metallurgy: Adding mirroring compatiblity.");
            FTSIGCTU.MirrorTool.addRule(HalvingMetallurgyParts.Halves, MirrorHalfProjectionPart);
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
        Quintessential.Logger.Log("Loading sounds");
        contentPath = Brimstone.API.GetContentPath("HalvingMetallurgy");
        HalvingMetallurgyParts.LoadSounds();
        Quintessential.Logger.Log("Sounds loaded");
        HalvingMetallurgyParts.AddPartTypes();
        Quintessential.Logger.Log("Adding own rules.");
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
        Quintessential.Logger.Log("Own rules added.");
        Quintessential.Logger.Log("Adding permission.");
        QApi.AddPuzzlePermission(HalvingPermission, "Glyph of Halves", "Halving Metallurgy");
        Quintessential.Logger.Log("Permission added.");
        if (FTSIGCTULoaded)
        {
            Quintessential.Logger.Log("Adding Glyph of halves to FTSIGCTU's map");
            FTSIGCTU.Navigation.PartsMap.addPartHexRule(HalvingMetallurgyParts.Halves, FTSIGCTU.Navigation.PartsMap.glyphRule);
        }
        if (ReductiveMetallurgyLoaded)
        {
            Quintessential.Logger.Log("Adding reduction metallurgy rules.");
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
        Quintessential.Logger.Log("Loading complete, have fun!");
    }
}
