using Quintessential;
using System.Collections.Generic;
using System.Linq;
using Texture = class_256;

namespace HalvingMetallurgy;

public static class Textures
{
    public static class Atom
    {
        public static Texture PartyHatTexture = Brimstone.API.GetTexture("textures/atoms/erikhaag/HalvingMetallurgy/party_hat");
        public static Texture[] QuickcopperAnimation = Brimstone.API.GetAnimation("textures/atoms/erikhaag/HalvingMetallurgy/cloud_trails.array", "trails", 64);
    }

    public static class Icons
    {
        public static Texture Halves = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/halves_icon");
        public static Texture HalvesHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/halves_icon_hover");
        public static Texture Quake = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/quake_icon");
        public static Texture QuakeHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/quake_icon_hover");
        public static Texture Sump = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/sump_icon");
        public static Texture SumpHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/sump_icon_hover");
        public static Texture Remission = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/remission_icon");
        public static Texture RemissionHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/remission_icon_hover");
        public static Texture Shearing = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/shearing_icon");
        public static Texture ShearingHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/shearing_icon_hover");
        public static Texture Osmosis = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/osmosis_icon");
        public static Texture OsmosisHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/osmosis_icon_hover");
        public static Texture Soria = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/soria_icon");
        public static Texture SoriaHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/soria_icon_hover");
    }

    public static class Halves
    {
        public static Texture Base = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/halves_base");
        public static Texture Glow = Brimstone.API.GetTexture("textures/select/erikhaag/HalvingMetallurgy/halves_glow");
        public static Texture Stroke = Brimstone.API.GetTexture("textures/select/erikhaag/HalvingMetallurgy/halves_stroke");
        public static Texture[] EngravingFlashAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/halves_engraving_flash.array", "halves_engraving", 6);
        public static Texture[] BowlFlashAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/halves_bowl_flash.array", "halves_bowl", 10);
    }

    public static class Quake
    {
        public static Texture Base = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_base");
        public static Texture Bowl = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_bowl");
        public static Texture BowlShaking = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_bowl_shaking");
        public static Texture[] UnbondResistedAnimation = Brimstone.API.GetAnimation("textures/bonds/erikhaag/HalvingMetallurgy/unbond_resist.array", "unbond_resist", 22);
    }

    public static class Sump
    {
        public static Texture Base = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/sump_base");
        public static Texture[] DrainFlashAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/sump_drain_flash.array", "sump_flash", 8);
    }

    public static class Remission
    {
        public static Texture Arrow = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/remission_arrow");
        public static Texture Connectors = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/remission_connectors");
    }

    public static class Shearing
    {
        public static Texture Base = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/shearing_base");
        public static Texture Bowl = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/shearing_bowl");
        public static Texture[] BowlFlash = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/shearing_flash.array", "shearing_flash", 8);
        // not to be confused with a banana split.
        public static Texture[] AtomicSplit = Brimstone.API.GetAnimation("textures/atoms/erikhaag/HalvingMetallurgy/split_effect.array", "split", 12);
    }

    public static class Osmosis
    {
        public static Texture Base = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/osmosis_base");
    }

    public static class Soria
    {
        public static Texture[] Flash = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/soria_flash.array", "soria_flash", 10);
    }

    public static class GlyphSymbols
    {
        public static Texture OsmiumDemote = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/osmium_symbol");
        public static Texture QuickcopperPromote = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quickcopper_symbol");
    }

    public static class Irises
    {
        public static Texture[] Quicksilver = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/iris_full_quicksilver.array", "iris_full_quicksilver", 16);
    }
    
    public static void TexturesPostProcessing()
    {
        Halves.EngravingFlashAnimation = Halves.EngravingFlashAnimation.ToList().Concat(Halves.EngravingFlashAnimation.Skip(1).Reverse().Skip(1)).ToArray();
    }
}