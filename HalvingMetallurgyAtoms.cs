using Quintessential;

namespace HalvingMetallurgy;

internal static class HalvingMetallurgyAtoms
{
    public static AtomType Wolfram, Vulcan, Nickel, Zinc, Sednum, Osmium;

    public static void AddAtomTypes()
    {
        Quintessential.Logger.Log("HalvingMetallurgy: Creating atoms.");
        Osmium = new()
        {
            field_2283 = 130, //Id
            field_2284 = class_134.method_254("Osmium"), //Non local name
            field_2285 = class_134.method_253("Elemental Osmium", string.Empty), //Atomic name
            field_2286 = class_134.method_253("Osmium", string.Empty), //Local name
            field_2287 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/osmium_symbol"), //Symbol
            field_2288 = class_235.method_615("textures/atoms/shadow"), //Shadow
            field_2294 = true, //Metal, Enables field_2297
            field_2291 = new()
            {
                field_13 = class_238.field_1989.field_81.field_577, //Diffuse
                field_14 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/osmium_lightramp"), //Lightramp
                field_15 = class_238.field_1989.field_81.field_613.field_633 //Shiny!
            },
            QuintAtomType = "HalvingMetallurgy:osmium"
        };
        QApi.AddAtomType(Osmium);
        Sednum = new()
        {
            field_2283 = 129, //Id
            field_2284 = class_134.method_254("Sednum"), //Non local name
            field_2285 = class_134.method_253("Elemental Sednum", string.Empty), //Atomic name
            field_2286 = class_134.method_253("Sednum", string.Empty), //Local name
            field_2287 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/sednum_symbol"), //Symbol
            field_2288 = class_235.method_615("textures/atoms/shadow"), //Shadow
            field_2294 = true, //Metal, Enables field_2297
            field_2291 = new()
            {
                field_13 = class_238.field_1989.field_81.field_577, //Diffuse
                field_14 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/sednum_lightramp"), //Lightramp
                field_15 = class_238.field_1989.field_81.field_613.field_633 //Shiny!
            },
            field_2297 = Maybe<AtomType>.method_1089(HalvingMetallurgyAtoms.Osmium), //Promotion Atom
            QuintAtomType = "HalvingMetallurgy:sednum"
        };
        QApi.AddAtomType(Sednum);
        Zinc = new()
        {
            field_2283 = 128, //Id
            field_2284 = class_134.method_254("Zinc"), //Non local name
            field_2285 = class_134.method_253("Elemental Zinc", string.Empty), //Atomic name
            field_2286 = class_134.method_253("Zinc", string.Empty), //Local name
            field_2287 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/zinc_symbol"), //Symbol
            field_2288 = class_235.method_615("textures/atoms/shadow"), //Shadow
            field_2294 = true, //Metal, Enables field_2297
            field_2291 = new()
            {
                field_13 = class_238.field_1989.field_81.field_577, //Diffuse
                field_14 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/zinc_lightramp"), //Lightramp
                field_15 = class_238.field_1989.field_81.field_613.field_633 //Shiny!
            },
            field_2297 = Maybe<AtomType>.method_1089(HalvingMetallurgyAtoms.Sednum), //Promotion Atom
            QuintAtomType = "HalvingMetallurgy:zinc"
        };
        QApi.AddAtomType(Zinc);
        Nickel = new()
        {
            field_2283 = 127, //Id
            field_2284 = class_134.method_254("Nickel"), //Non local name
            field_2285 = class_134.method_253("Elemental Nickel", string.Empty), //Atomic name
            field_2286 = class_134.method_253("Nickel", string.Empty), //Local name
            field_2287 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/nickel_symbol"), //Symbol
            field_2288 = class_235.method_615("textures/atoms/shadow"), //Shadow
            field_2294 = true, //Metal, Enables field_2297
            field_2291 = new()
            {
                field_13 = class_238.field_1989.field_81.field_577, //Diffuse
                field_14 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/nickel_lightramp"), //Lightramp
                field_15 = class_238.field_1989.field_81.field_613.field_633 //Shiny!
            },
            field_2297 = Maybe<AtomType>.method_1089(HalvingMetallurgyAtoms.Zinc), //Promotion Atom
            QuintAtomType = "HalvingMetallurgy:nickel"
        };
        QApi.AddAtomType(Nickel);
        Vulcan = new()
        {
            field_2283 = 126, //Id
            field_2284 = class_134.method_254("Vulcan"), //Non local name
            field_2285 = class_134.method_253("Elemental Vulcan", string.Empty), //Atomic name
            field_2286 = class_134.method_253("Vulcan", string.Empty), //Local name
            field_2287 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/vulcan_symbol"), //Symbol
            field_2288 = class_235.method_615("textures/atoms/shadow"), //Shadow
            field_2294 = true, //Metal, Enables field_2297
            field_2291 = new()
            {
                field_13 = class_238.field_1989.field_81.field_577, //Diffuse
                field_14 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/vulcan_lightramp"), //Lightramp
                field_15 = class_238.field_1989.field_81.field_613.field_633 //Shiny!
            },
            field_2297 = Maybe<AtomType>.method_1089(HalvingMetallurgyAtoms.Nickel), //Promotion Atom
            QuintAtomType = "HalvingMetallurgy:vulcan"
        };
        QApi.AddAtomType(Vulcan);
        Wolfram = new()
        {
            field_2283 = 125, //Id
            field_2284 = class_134.method_254("Wolfram"), //Non local name
            field_2285 = class_134.method_253("Elemental Wolfram", string.Empty), //Atomic name
            field_2286 = class_134.method_253("Wolfram", string.Empty), //Local name
            field_2287 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/wolfram_symbol"), //Symbol
            field_2288 = class_235.method_615("textures/atoms/shadow"), //Shadow
            field_2294 = true, //Metal, Enables field_2297
            field_2291 = new()
            {
                field_13 = class_238.field_1989.field_81.field_577, //Diffuse
                field_14 = class_235.method_615("textures/atoms/erikhaag/HalvingMetallurgy/wolfram_lightramp"), //Lightramp
                field_15 = class_238.field_1989.field_81.field_613.field_633 //Shiny! (textures/atoms/elements/specular_highlight.png)
            },
            field_2297 = Maybe<AtomType>.method_1089(HalvingMetallurgyAtoms.Vulcan), //Promotion Atom
            QuintAtomType = "HalvingMetallurgy:wolfram"
        };
        QApi.AddAtomType(Wolfram);
        Quintessential.Logger.Log("HalvingMetallurgy: Atoms added.");
    }
}