using Brimstone;
using Quintessential;

namespace HalvingMetallurgy;

internal static class HalvingMetallurgyAtoms
{
    public static AtomType Wolfram, Vulcan, Nickel, Zinc, Sednum, Osmium;

    public static void AddAtomTypes()
    {
        Quintessential.Logger.Log("Creating atoms.");
        //Much Cleaner!
        Osmium = BrimstoneAPI.CreateMetal(130, "HalvingMetallurgy", "Osmium", "textures/atoms/erikhaag/HalvingMetallurgy/osmium_symbol", "textures/atoms/erikhaag/HalvingMetallurgy/osmium_lightramp");
        Sednum = BrimstoneAPI.CreateMetal(129, "HalvingMetallurgy", "Sednum", "textures/atoms/erikhaag/HalvingMetallurgy/sednum_symbol", "textures/atoms/erikhaag/HalvingMetallurgy/sednum_lightramp", Osmium);
        Zinc = BrimstoneAPI.CreateMetal(128, "HalvingMetallurgy", "Zinc", "textures/atoms/erikhaag/HalvingMetallurgy/zinc_symbol", "textures/atoms/erikhaag/HalvingMetallurgy/zinc_lightramp", Sednum);
        Nickel = BrimstoneAPI.CreateMetal(127, "HalvingMetallurgy", "Nickel", "textures/atoms/erikhaag/HalvingMetallurgy/nickel_symbol", "textures/atoms/erikhaag/HalvingMetallurgy/nickel_lightramp", Zinc);
        Vulcan = BrimstoneAPI.CreateMetal(126, "HalvingMetallurgy", "Vulcan", "textures/atoms/erikhaag/HalvingMetallurgy/vulcan_symbol", "textures/atoms/erikhaag/HalvingMetallurgy/vulcan_lightramp", Nickel);
        Wolfram = BrimstoneAPI.CreateMetal(125, "HalvingMetallurgy", "Wolfram", "textures/atoms/erikhaag/HalvingMetallurgy/wolfram_symbol", "textures/atoms/erikhaag/HalvingMetallurgy/wolfram_lightramp", Vulcan);
        QApi.AddAtomType(Wolfram);
        QApi.AddAtomType(Vulcan);
        QApi.AddAtomType(Nickel);
        QApi.AddAtomType(Zinc);
        QApi.AddAtomType(Sednum);
        QApi.AddAtomType(Osmium);
    }
}