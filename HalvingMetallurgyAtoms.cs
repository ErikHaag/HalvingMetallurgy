using Brimstone;
using Quintessential;

namespace HalvingMetallurgy;

internal static class HalvingMetallurgyAtoms
{
    public static AtomType Wolfram, Vulcan, Nickel, Zinc, Sednum, Osmium;

    public static void AddAtomTypes()
    {
        Quintessential.Logger.Log("Creating atoms.");
        Osmium = Brimstone.API.CreateMetalAtom(
            ID: 130, 
            modName: "HalvingMetallurgy",
            name: "Osmium",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/osmium_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/osmium_lightramp"
        );
        Sednum = Brimstone.API.CreateMetalAtom(
            ID: 129,
            modName: "HalvingMetallurgy",
            name: "Sednum",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/sednum_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/sednum_lightramp",
            promotesTo: Osmium
        );
        Zinc = Brimstone.API.CreateMetalAtom(
            ID: 128,
            modName: "HalvingMetallurgy",
            name: "Zinc",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/zinc_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/zinc_lightramp",
            promotesTo: Sednum
        );
        Nickel = Brimstone.API.CreateMetalAtom(
            ID: 127,
            modName: "HalvingMetallurgy",
            name: "Nickel",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/nickel_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/nickel_lightramp",
            promotesTo: Zinc
        );
        Vulcan = Brimstone.API.CreateMetalAtom(
            ID: 126,
            modName: "HalvingMetallurgy",
            name: "Vulcan",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/vulcan_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/vulcan_lightramp",
            promotesTo: Nickel);
        Wolfram = Brimstone.API.CreateMetalAtom(
            ID: 125,
            modName: "HalvingMetallurgy",
            name: "Wolfram",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/wolfram_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/wolfram_lightramp",
            promotesTo: Vulcan
        );
        QApi.AddAtomType(Wolfram);
        QApi.AddAtomType(Vulcan);
        QApi.AddAtomType(Nickel);
        QApi.AddAtomType(Zinc);
        QApi.AddAtomType(Sednum);
        QApi.AddAtomType(Osmium);
    }
}