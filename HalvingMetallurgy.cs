using Quintessential;
using ReductiveMetallurgy;

namespace HalvingMetallurgy;

public class HalvingMetallurgy : QuintessentialMod
{
    public const string HalvingPermission = "HalvingMetallurgy:halving";

    public override void Load()
    {

    }

    public override void PostLoad()
    {

    }

    public override void Unload()
    {
        Quintessential.Logger.Log("icwass made the dictionaries private and didn't add a remove method.\nSo my atoms are still in there, if this is a problem, reload the game.");
    }

    private static bool contentLoaded = false;
    public override void LoadPuzzleContent()
    {
        if (contentLoaded)
        {
            return;
        }
        contentLoaded = true;
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
        API.addDepositionRule(HalvingMetallurgyAtoms.Osmium, HalvingMetallurgyAtoms.Nickel, HalvingMetallurgyAtoms.Nickel);
        API.addDepositionRule(HalvingMetallurgyAtoms.Sednum, HalvingMetallurgyAtoms.Nickel, HalvingMetallurgyAtoms.Vulcan);
        API.addDepositionRule(HalvingMetallurgyAtoms.Zinc, HalvingMetallurgyAtoms.Vulcan, HalvingMetallurgyAtoms.Vulcan);
        API.addDepositionRule(HalvingMetallurgyAtoms.Nickel, HalvingMetallurgyAtoms.Vulcan, HalvingMetallurgyAtoms.Wolfram);
        API.addDepositionRule(HalvingMetallurgyAtoms.Vulcan, HalvingMetallurgyAtoms.Wolfram, HalvingMetallurgyAtoms.Wolfram);
        // Add Proliferation
        API.addProliferationRule(HalvingMetallurgyAtoms.Osmium);
        API.addProliferationRule(HalvingMetallurgyAtoms.Sednum);
        API.addProliferationRule(HalvingMetallurgyAtoms.Zinc);
        API.addProliferationRule(HalvingMetallurgyAtoms.Nickel);
        API.addProliferationRule(HalvingMetallurgyAtoms.Vulcan);
        API.addProliferationRule(HalvingMetallurgyAtoms.Wolfram);
        Quintessential.Logger.Log("HalvingMetallurgy: Reduction metallurgy rules added."); 
        HalvingMetallurgyParts.AddPartTypes();
        Quintessential.Logger.Log("HalvingMetallurgy: Adding own rules.");
        HalvingMetallurgyAPI.HalvingPromotions.Add(API.leadAtomType, HalvingMetallurgyAtoms.Wolfram);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Wolfram, API.tinAtomType);
        HalvingMetallurgyAPI.HalvingPromotions.Add(API.tinAtomType, HalvingMetallurgyAtoms.Vulcan);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Vulcan, API.ironAtomType);
        HalvingMetallurgyAPI.HalvingPromotions.Add(API.ironAtomType, HalvingMetallurgyAtoms.Nickel);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Nickel, API.copperAtomType);
        HalvingMetallurgyAPI.HalvingPromotions.Add(API.copperAtomType, HalvingMetallurgyAtoms.Zinc);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Zinc, API.silverAtomType);
        HalvingMetallurgyAPI.HalvingPromotions.Add(API.silverAtomType, HalvingMetallurgyAtoms.Sednum);
        HalvingMetallurgyAPI.HalvingPromotions.Add(HalvingMetallurgyAtoms.Sednum, API.goldAtomType);
        HalvingMetallurgyAPI.HalvingPromotions.Add(API.goldAtomType, HalvingMetallurgyAtoms.Osmium);
        Quintessential.Logger.Log("HalvingMetallurgy: Own rules added.");
        Quintessential.Logger.Log("HalvingMetallurgy: Adding permission.");
        QApi.AddPuzzlePermission(HalvingPermission, "Glyph of Halves", "Halving Metallurgy");
        Quintessential.Logger.Log("HalvingMetallurgy: Permission added.");
        Quintessential.Logger.Log("HalvingMetallurgy: Loading complete, have fun!");
    }
}
