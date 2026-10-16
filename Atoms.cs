using Quintessential;

namespace HalvingMetallurgy;

public static class Atoms
{
    internal static bool quickcopperRadioactive = true;
    internal static bool wearPartyHat = System.DateTime.Now.Month == 4 && System.DateTime.Now.Day == 5;
    internal static AtomType Quicklime, Quickcopper, Beryl, Wolfram, Vulcan, Nickel, Zinc, Sednum, Osmium;
    internal static readonly Color QuickcopperColor = Color.FromHex(0xee9966);

    public static void AddAtomTypes()
    {
        Quicklime = Brimstone.API.CreateNormalAtom(
            ID: 132,
            modName: "HalvingMetallurgy",
            name: "Quicklime",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/quicklime_symbol",
            pathToDiffuse: "textures/atoms/erikhaag/HalvingMetallurgy/quicklime_diffuse"
        );

        Osmium = new AtomType()
        {
            isMetallic = true,
            symbol = Textures.Atom.OsmiumSymbol,
            metallicTextures = new MetallicAtomTextures()
            {
                diffuse = Assets.textures.atoms.copper_diffuse,
                lightramp = Textures.Atom.OsmiumLightramp,
                rimlight = Textures.Atom.OsmiumRimlight
            }
        };

        Sednum = Brimstone.API.CreateMetalAtom(
            ID: 129,
            modName: "HalvingMetallurgy",
            name: "Sednum",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/sednum_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/sednum_lightramp",
            pathToRimlight: "textures/atoms/erikhaag/HalvingMetallurgy/sednum_rimlight",
            promotesTo: Osmium
        );

        Zinc = Brimstone.API.CreateMetalAtom(
            ID: 128,
            modName: "HalvingMetallurgy",
            name: "Zinc",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/zinc_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/zinc_lightramp",
            pathToRimlight: "textures/atoms/erikhaag/HalvingMetallurgy/zinc_rimlight",
            promotesTo: Sednum
        );

        Nickel = Brimstone.API.CreateMetalAtom(
            ID: 127,
            modName: "HalvingMetallurgy",
            name: "Nickel",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/nickel_symbol_oxide",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/nickel_lightramp",
            pathToRimlight: "textures/atoms/erikhaag/HalvingMetallurgy/nickel_rimlight",
            promotesTo: Zinc
        );

        Vulcan = Brimstone.API.CreateMetalAtom(
            ID: 126,
            modName: "HalvingMetallurgy",
            name: "Vulcan",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/vulcan_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/vulcan_lightramp",
            pathToRimlight: "textures/atoms/erikhaag/HalvingMetallurgy/vulcan_rimlight",
            promotesTo: Nickel
        );

        Wolfram = Brimstone.API.CreateMetalAtom(
            ID: 125,
            modName: "HalvingMetallurgy",
            name: "Wolfram",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/wolfram_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/wolfram_lightramp",
            pathToRimlight: "textures/atoms/erikhaag/HalvingMetallurgy/wolfram_rimlight",
            promotesTo: Vulcan
        );

        Beryl = Brimstone.API.CreateMetalAtom(
            ID: 124,
            modName: "HalvingMetallurgy",
            name: "Beryl",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/beryl_symbol",
            pathToLightramp: "textures/atoms/erikhaag/HalvingMetallurgy/beryl_lightramp",
            pathToRimlight: "textures/atoms/erikhaag/HalvingMetallurgy/beryl_rimlight",
            promotesTo: Wolfram
        );

        Quickcopper = Brimstone.API.CreateNormalAtom(
            ID: 131,
            modName: "HalvingMetallurgy",
            name: "Quickcopper",
            pathToSymbol: "textures/atoms/erikhaag/HalvingMetallurgy/quickcopper_symbol",
            pathToDiffuse: "textures/atoms/erikhaag/HalvingMetallurgy/quickcopper_diffuse"
        );

        QApi.AddAtomType(Quicklime);
        QApi.AddAtomType(Quickcopper);
        QApi.AddAtomType(Beryl);
        QApi.AddAtomType(Wolfram);
        QApi.AddAtomType(Vulcan);
        QApi.AddAtomType(Nickel);
        QApi.AddAtomType(Zinc);
        QApi.AddAtomType(Sednum);
        QApi.AddAtomType(Osmium);
    }

    //internal static void OnAtomRender(On.Editor.orig_method_927 orig, AtomType type, Vector2 position, float param_4582, float param_4583, float param_4584, float param_4585, float param_4586, float param_4587, Texture overrideShadow, Texture maskM, bool param_4590)
    //{
    //    if (quickcopperRadioactive && type.QuintAtomType == "HalvingMetallurgy:aqc")
    //    {
    //        int frame = (int)(new struct_27(Time.Now().Ticks).method_603() * 30f) & 0x3f;
    //        class_135.method_272(Textures.Atom.QuickcopperAnimation[frame], position - new Vector2(60, 60));
    //    }
    //    orig(type, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);

    //    if (wearPartyHat && (type.QuintAtomType ?? "").StartsWith("HalvingMetallurgy"))
    //    {
    //        class_135.method_272(Textures.Atom.PartyHatTexture, position - new Vector2(45, 45));
    //    }
    //}
}