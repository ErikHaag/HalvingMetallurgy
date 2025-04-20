using Brimstone;
using Quintessential;
using ReductiveMetallurgy;

namespace HalvingMetallurgy;

public class HalvingMetallurgy : QuintessentialMod
{
    public const string HalvingPermission = "HalvingMetallurgy:halving";
    public static string contentPath;

    public override void Load()
    {

    }

    public override void PostLoad()
    {

    }

    public override void Unload()
    {
        Quintessential.Logger.Log("Halving Metallurgy: Goodbye!");
    }

    public override void LoadPuzzleContent()
    {
        Quintessential.Logger.Log("HalvingMetallurgy: Loading!");
        HalvingMetallurgyAtoms.AddAtomTypes();
        //ReductiveMetallurgy API
        Quintessential.Logger.Log("HalvingMetallurgy: Adding reduction metallurgy rules.");
        //Add Rejection Rules for new atoms
        API.addRejectionRule(HalvingMetallurgyAtoms.Osmium, HalvingMetallurgyAtoms.Sednum);
        API.addRejectionRule(HalvingMetallurgyAtoms.Sednum, HalvingMetallurgyAtoms.Zinc);
        API.addRejectionRule(HalvingMetallurgyAtoms.Zinc, HalvingMetallurgyAtoms.Nickel);
        API.addRejectionRule(HalvingMetallurgyAtoms.Nickel, HalvingMetallurgyAtoms.Vulcan);
        API.addRejectionRule(HalvingMetallurgyAtoms.Vulcan, HalvingMetallurgyAtoms.Wolfram);
        // Add Deposition Rules
        API.addDepositionRule(HalvingMetallurgyAtoms.Osmium, HalvingMetallurgyAtoms.Nickel, BrimstoneAPI.VanillaAtoms["iron"]);
        API.addDepositionRule(HalvingMetallurgyAtoms.Sednum, BrimstoneAPI.VanillaAtoms["iron"], HalvingMetallurgyAtoms.Vulcan);
        API.addDepositionRule(HalvingMetallurgyAtoms.Zinc, HalvingMetallurgyAtoms.Vulcan, BrimstoneAPI.VanillaAtoms["tin"]);
        API.addDepositionRule(HalvingMetallurgyAtoms.Nickel, BrimstoneAPI.VanillaAtoms["tin"], HalvingMetallurgyAtoms.Wolfram);
        API.addDepositionRule(HalvingMetallurgyAtoms.Vulcan, HalvingMetallurgyAtoms.Wolfram, BrimstoneAPI.VanillaAtoms["lead"]);
        // Add Proliferation
        API.addProliferationRule(HalvingMetallurgyAtoms.Osmium);
        API.addProliferationRule(HalvingMetallurgyAtoms.Sednum);
        API.addProliferationRule(HalvingMetallurgyAtoms.Zinc);
        API.addProliferationRule(HalvingMetallurgyAtoms.Nickel);
        API.addProliferationRule(HalvingMetallurgyAtoms.Vulcan);
        API.addProliferationRule(HalvingMetallurgyAtoms.Wolfram);
        Quintessential.Logger.Log("HalvingMetallurgy: Reduction metallurgy rules added.");
        Quintessential.Logger.Log("HalvingMetallurgy: Loading Sounds");
        contentPath = BrimstoneAPI.GetContentPath("HalvingMetallurgy");
        HalvingMetallurgyParts.LoadSounds();
        Quintessential.Logger.Log("HalvingMetallurgy: Sounds loaded");
        HalvingMetallurgyParts.AddPartTypes();
        Quintessential.Logger.Log("HalvingMetallurgy: Adding own rules.");
        HalvingMetallurgyAPI.HalvingPromotions.Add(BrimstoneAPI.VanillaAtoms["lead"], HalvingMetallurgyAtoms.Wolfram);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Wolfram, BrimstoneAPI.VanillaAtoms["tin"]);
        HalvingMetallurgyAPI.HalvingPromotions.Add(BrimstoneAPI.VanillaAtoms["tin"], HalvingMetallurgyAtoms.Vulcan);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Vulcan, BrimstoneAPI.VanillaAtoms["iron"]);
        HalvingMetallurgyAPI.HalvingPromotions.Add(BrimstoneAPI.VanillaAtoms["iron"], HalvingMetallurgyAtoms.Nickel);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Nickel, BrimstoneAPI.VanillaAtoms["copper"]);
        HalvingMetallurgyAPI.HalvingPromotions.Add(BrimstoneAPI.VanillaAtoms["copper"], HalvingMetallurgyAtoms.Zinc);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Zinc, BrimstoneAPI.VanillaAtoms["silver"]);
        HalvingMetallurgyAPI.HalvingPromotions.Add(BrimstoneAPI.VanillaAtoms["silver"], HalvingMetallurgyAtoms.Sednum);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Sednum, BrimstoneAPI.VanillaAtoms["gold"]);
        HalvingMetallurgyAPI.HalvingPromotions.Add(BrimstoneAPI.VanillaAtoms["gold"], HalvingMetallurgyAtoms.Osmium);
        Quintessential.Logger.Log("HalvingMetallurgy: Own rules added.");
        Quintessential.Logger.Log("HalvingMetallurgy: Adding permission.");
        QApi.AddPuzzlePermission(HalvingPermission, "Glyph of Halves", "Halving Metallurgy");
        Quintessential.Logger.Log("HalvingMetallurgy: Permission added.");
        Quintessential.Logger.Log("HalvingMetallurgy: Loading complete, have fun!");
    }
}
