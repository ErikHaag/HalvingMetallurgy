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
        Quicklime = new AtomType() {
            symbol = Textures.Atom.QuicklimeSymbol,
            matteTextures = new MatteAtomTextures() {
                diffuse = Textures.Atom.QuicklimeDiffuse,
                shade = Assets.textures.atoms.salt_shade
            },
            shadow = Assets.textures.atoms.shadow
        };

        Quickcopper = new AtomType()
        {
            symbol = Textures.Atom.QuickcopperSymbol,
            matteTextures = new MatteAtomTextures() {
                diffuse = Textures.Atom.QuickcopperDiffuse,
                shade = Assets.textures.atoms.salt_shade
            },
            shadow = Assets.textures.atoms.shadow
        };

        Beryl = new AtomType()
        {
            isMetallic = true,
            symbol = Textures.Atom.BerylSymbol,
            metallicTextures = new MetallicAtomTextures()
            {
                diffuse = Assets.textures.atoms.copper_diffuse,
                lightramp = Textures.Atom.BerylLightramp,
                rimlight = Textures.Atom.BerylRimlight
            },
            shadow = Assets.textures.atoms.shadow
        };

        Wolfram = new AtomType()
        {
            isMetallic = true,
            symbol = Textures.Atom.WolframSymbol,
            metallicTextures = new MetallicAtomTextures()
            {
                diffuse = Assets.textures.atoms.copper_diffuse,
                lightramp = Textures.Atom.WolframLightramp,
                rimlight = Textures.Atom.WolframRimlight
            },
            shadow = Assets.textures.atoms.shadow
        };

        Vulcan = new AtomType()
        {
            isMetallic = true,
            symbol = Textures.Atom.VulcanSymbol,
            metallicTextures = new MetallicAtomTextures()
            {
                diffuse = Assets.textures.atoms.copper_diffuse,
                lightramp = Textures.Atom.VulcanLightramp,
                rimlight = Textures.Atom.VulcanRimlight
            },
            shadow = Assets.textures.atoms.shadow
        };

        Nickel = new AtomType()
        {
            isMetallic = true,
            symbol = Textures.Atom.NickelSymbolOxide,
            metallicTextures = new MetallicAtomTextures()
            {
                diffuse = Assets.textures.atoms.copper_diffuse,
                lightramp = Textures.Atom.NickelLightramp,
                rimlight = Textures.Atom.NickelRimlight
            },
            shadow = Assets.textures.atoms.shadow
        };

        Zinc = new AtomType()
        {
            isMetallic = true,
            symbol = Textures.Atom.ZincSymbol,
            metallicTextures = new MetallicAtomTextures()
            {
                diffuse = Assets.textures.atoms.copper_diffuse,
                lightramp = Textures.Atom.ZincLightramp,
                rimlight = Textures.Atom.ZincRimlight
            },
            shadow = Assets.textures.atoms.shadow
        };

        Sednum = new AtomType()
        {
            isMetallic = true,
            symbol = Textures.Atom.SednumSymbol,
            metallicTextures = new MetallicAtomTextures()
            {
                diffuse = Assets.textures.atoms.copper_diffuse,
                lightramp = Textures.Atom.SednumLightramp,
                rimlight = Textures.Atom.SednumRimlight
            },
            shadow = Assets.textures.atoms.shadow
        };

        Osmium = new AtomType()
        {
            isMetallic = true,
            symbol = Textures.Atom.OsmiumSymbol,
            metallicTextures = new MetallicAtomTextures()
            {
                diffuse = Assets.textures.atoms.copper_diffuse,
                lightramp = Textures.Atom.OsmiumLightramp,
                rimlight = Textures.Atom.OsmiumRimlight
            },
            shadow = Assets.textures.atoms.shadow
        };

        QApi.AddAtomType(HalvingMetallurgy.Instance, Quicklime, "quicklime");
        QApi.AddAtomType(HalvingMetallurgy.Instance, Quickcopper, "quickcopper");
        QApi.AddAtomType(HalvingMetallurgy.Instance, Beryl, "beryl");
        QApi.AddAtomType(HalvingMetallurgy.Instance, Wolfram, "wolfram");
        QApi.AddAtomType(HalvingMetallurgy.Instance, Vulcan, "vulcan");
        QApi.AddAtomType(HalvingMetallurgy.Instance, Nickel, "nickel");
        QApi.AddAtomType(HalvingMetallurgy.Instance, Zinc, "zinc");
        QApi.AddAtomType(HalvingMetallurgy.Instance, Sednum, "sednum");
        QApi.AddAtomType(HalvingMetallurgy.Instance, Osmium, "osmium");
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