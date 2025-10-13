using Brimstone;
using Mono.Cecil;
using Quintessential;
using Texture = class_256;

namespace HalvingMetallurgy;

public static class HalvingMetallurgyAtoms
{
    public static Texture[] quickcopperAnimation = Brimstone.API.GetAnimation("textures/atoms/erikhaag/HalvingMetallurgy/cloud_trails.array", "trails", 64);


    public static AtomType Quickcopper, ActiveQuickcopper, Beryl, PurificationBeryl, Wolfram, Vulcan, Nickel, Zinc, Sednum, Osmium;

    public static void AddAtomTypes()
    {
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
            promotesTo: Nickel
        );
        Wolfram = Brimstone.API.CreateMetalAtom(
            ID: 125,
            modName: "HalvingMetallurgy",
            name: "Wolfram",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/wolfram_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/wolfram_lightramp",
            promotesTo: Vulcan
        );
        Beryl = Brimstone.API.CreateMetalAtom(
            ID: 124,
            modName: "HalvingMetallurgy",
            name: "Beryl",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/beryl_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/beryl_lightramp",
            promotesTo: Wolfram
        );
        PurificationBeryl = Brimstone.API.CreateMetalAtom(
            ID: 124,
            modName: "HalvingMetallurgy",
            name: "PB", // do not spread on bread, this isn't peanut butter nor strawberry jam.
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/beryl_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/beryl_lightramp",
            promotesTo: Brimstone.API.VanillaAtoms["lead"]
        );

        Quickcopper = Brimstone.API.CreateNormalAtom(
            ID: 132,
            modName: "HalvingMetallurgy",
            name: "Quickcopper",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/quickcopper_symbol",
            pathToDiffuse: "textures/atoms/erikhaag/HalvingMetallurgy/quickcopper_diffuse"
        );

        ActiveQuickcopper = Brimstone.API.CreateNormalAtom(
            ID: 132,
            modName: "HalvingMetallurgy",
            name: "Aqc",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/quickcopper_symbol",
            pathToDiffuse: "textures/atoms/erikhaag/HalvingMetallurgy/quickcopper_diffuse"
        );

        QApi.AddAtomType(Quickcopper);
        QApi.AddAtomType(Beryl);
        QApi.AddAtomType(Wolfram);
        QApi.AddAtomType(Vulcan);
        QApi.AddAtomType(Nickel);
        QApi.AddAtomType(Zinc);
        QApi.AddAtomType(Sednum);
        QApi.AddAtomType(Osmium);
    }

    internal static void OnAtomRender(On.Editor.orig_method_927 orig, AtomType type, Vector2 position, float param_4582, float param_4583, float param_4584, float param_4585, float param_4586, float param_4587, Texture overrideShadow, Texture maskM, bool param_4590)
    {
        if (type.QuintAtomType == "HalvingMetallurgy:aqc")
        {
            int frame = (int)(new struct_27(Time.Now().Ticks).method_603() * 30f) & 0x3f;
            class_135.method_272(quickcopperAnimation[frame], position - new Vector2(60, 60));
        }
        orig(type, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
    }
}