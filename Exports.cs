using MonoMod.ModInterop;
using MonoMod.Utils;
using System;
using PartType = class_139;
using Texture = class_256;

namespace HalvingMetallurgy;

public static class Exports
{
    // should've done this forever ago...
    internal static void ExportTextures()
    {
        typeof(AtomTextureExports).ModInterop();
    }
    internal static void ExportContent()
    {
        typeof(AtomExports).ModInterop();

    }

    [ModExportName("HalvingMetallurgy.Textures.Atom")]
    public static class AtomTextureExports
    {
        public static Texture[] GetQuickcopperRadiation() => Textures.Atom.QuickcopperAnimation;
        public static Texture GetPartyHat() => Textures.Atom.PartyHatTexture;
    }

    // todo

    [ModExportName("HalvingMetallurgy.Atoms")]
    public static class AtomExports
    {
        public static bool GetQuickcopperRadioactivity() => Atoms.quickcopperRadioactive;
        public static AtomType GetQuicklime() => Atoms.Quicklime;
        // Active quickcopper is decorative, and is substituted for normal quickcopper in every molecule before the glyphs do their thing
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

    [ModExportName("HalvingMetallurgy.Glyphs")]
    public static class GlyphExports
    {
        public static PartType GetHalves() => Glyphs.Halves;
        public static PartType GetQuake() => Glyphs.Quake;
        public static PartType GetSump() => Glyphs.Sump;
        public static PartType GetRemission() => Glyphs.Remission;
        public static PartType GetShearing() => Glyphs.Shearing;
        public static PartType GetOsmosis() => Glyphs.Osmosis;
    }

    // todo
}
