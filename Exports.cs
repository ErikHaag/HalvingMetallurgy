using MonoMod.ModInterop;
using MonoMod.Utils;


namespace HalvingMetallurgy;

public static class Exports
{
    // should've done this forever ago...
    internal static void Initialize()
    {
        typeof(AtomExports).ModInterop();
    }

    [ModExportName("HalvingMetallurgy.Atoms")]
    public static class AtomExports
    {
        public static bool GetQuickcopperRadioactivity() => Atoms.quickcopperRadioactive;
        public static AtomType GetQuicklime() => Atoms.Quicklime;
        public static AtomType GetQuickCopper() => Atoms.Quickcopper;
        public static AtomType GetBeryl() => Atoms.Beryl;
        public static AtomType GetPurificationBeryl() => Atoms.PurificationBeryl;
        public static AtomType GetWolfram() => Atoms.Wolfram;
        public static AtomType GetVulcan() => Atoms.Vulcan;
        public static AtomType GetNickel() => Atoms.Nickel;
        public static AtomType GetZinc() => Atoms.Zinc;
        public static AtomType GetSednum() => Atoms.Sednum;
        public static AtomType GetOsmium() => Atoms.Osmium;
    }

    [ModExportName("HalvingMetallurgy.Textures.Atom")]
    public static class AtomTextureExports
    {

    }
}
