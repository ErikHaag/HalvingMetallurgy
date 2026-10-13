using System;
using Quintessential;
using Texture = class_256;

namespace HalvingMetallurgy;

public static class Textures
{
    private const string atomPath = "textures/atoms/erikhaag/HalvingMetallurgy/";
    private const string partPath = "textures/parts/erikhaag/HalvingMetallurgy/";
    private const string selectPath = "textures/select/erikhaag/HalvingMetallurgy/";

    public static class Atom
    {
        public static Texture PartyHatTexture = Brimstone.API.GetTexture(atomPath + "party_hat");
        public static Texture[] QuickcopperAnimation = Brimstone.API.GetAnimation(atomPath + "cloud_trails.array", "trails", 64);
    }

    public static class Icons
    {
        public static Texture Halves = Brimstone.API.GetTexture(partPath + "icons/halves_icon");
        public static Texture HalvesHover = Brimstone.API.GetTexture(partPath + "icons/halves_icon_hover");
        public static Texture Quake = Brimstone.API.GetTexture(partPath + "icons/quake_icon");
        public static Texture QuakeHover = Brimstone.API.GetTexture(partPath + "icons/quake_icon_hover");
        public static Texture Sump = Brimstone.API.GetTexture(partPath + "icons/sump_icon");
        public static Texture SumpHover = Brimstone.API.GetTexture(partPath + "icons/sump_icon_hover");
        public static Texture Remission = Brimstone.API.GetTexture(partPath + "icons/remission_icon");
        public static Texture RemissionHover = Brimstone.API.GetTexture(partPath + "icons/remission_icon_hover");
        public static Texture Shearing = Brimstone.API.GetTexture(partPath + "icons/shearing_icon");
        public static Texture ShearingHover = Brimstone.API.GetTexture(partPath + "icons/shearing_icon_hover");
        public static Texture Osmosis = Brimstone.API.GetTexture(partPath + "icons/osmosis_icon");
        public static Texture OsmosisHover = Brimstone.API.GetTexture(partPath + "icons/osmosis_icon_hover");
        public static Texture Soria = Brimstone.API.GetTexture(partPath + "icons/soria_icon");
        public static Texture SoriaHover = Brimstone.API.GetTexture(partPath + "icons/soria_icon_hover");
    }

    public static class GlyphCommon
    {
        public static Texture DoubleBase = Brimstone.API.GetTexture(partPath + "double_base");
    }

    public static class Halves
    {
        public static Texture Base = Brimstone.API.GetTexture(partPath + "halves/base");
        public static Texture Glow = UI.AssignOffset(UI.AssignCentre(Brimstone.API.GetTexture(selectPath + "halves_glow"), new Vector2(96, 58)), new Vector2(-1, -1));
        public static Texture Stroke = UI.AssignOffset(UI.AssignCentre(Brimstone.API.GetTexture(selectPath + "halves_stroke"), new Vector2(83, 51)), new Vector2(-1, -1));
        public static Texture Connectors = Brimstone.API.GetTexture(partPath + "halves/connectors");
        public static Texture GlossMask = Brimstone.API.GetTexture(partPath + "halves/gloss_mask");

        public static Texture[] ConnectorFlash = Brimstone.API.GetAnimation(partPath + "halves_connector_glow.array", "glow", 6);
        public static Texture[] BowlFlashAnimation = Brimstone.API.GetAnimation(partPath + "halves_bowl_flash.array", "halves_bowl", 10);
    }

    public static class Quake
    {
        public static Texture Base = Brimstone.API.GetTexture(partPath + "quake/base");
        public static Texture Bowl = Brimstone.API.GetTexture(partPath + "quake/bowl");
        public static Texture BowlShaking = Brimstone.API.GetTexture(partPath + "quake/bowl_shaking");
        public static Texture[] UnbondResistedAnimation = Brimstone.API.GetAnimation("textures/bonds/erikhaag/HalvingMetallurgy/unbond_resist.array", "unbond_resist", 22);
    }

    public static class Sump
    {
        public static Texture[] DrainFlashAnimation = Brimstone.API.GetAnimation(partPath + "sump_drain_flash.array", "sump_flash", 8);
    }

    public static class Remission
    {
        public static Texture Arrow = Brimstone.API.GetTexture(partPath + "remission/arrow");
        public static Texture Connectors = Brimstone.API.GetTexture(partPath + "remission/connectors");
        public static Texture[] BowlGlow = Brimstone.API.GetAnimation(partPath + (DateTime.Now.Month == 6 ? "remission_bowl_glow_rainbow.array" : "remission_bowl_glow.array"), "glow", 64);
    }

    public static class Shearing
    {

        public static Texture SednumLine = Brimstone.API.GetTexture(partPath + "shearing/sednum_line");
        public static Texture[] BowlFlash = Brimstone.API.GetAnimation(partPath + "shearing_flash.array", "shearing_flash", 8);
        // not to be confused with a banana split.
        public static Texture[] Split = Brimstone.API.GetAnimation(atomPath + "shearing_split.array", "split", 16);
        public static Texture[] Spawn = Brimstone.API.GetAnimation(atomPath + "shearing_spawn.array", "spawn", 16);
    }

    public static class Osmosis
    {
        public static Texture Connector = Brimstone.API.GetTexture(partPath + "osmosis/connector");
    }

    public static class Soria
    {
        public static Texture[] Flash = Brimstone.API.GetAnimation(partPath + "soria_flash.array", "soria_flash", 10);
    }

    public static class GlyphSymbols
    {
        public static Texture OsmiumDemote = Brimstone.API.GetTexture(partPath + "osmium_symbol");
        public static Texture QuickcopperPromote = Brimstone.API.GetTexture(partPath + "quickcopper_symbol");
    }

    public static class Irises
    {
        public static Texture[] Crossout = Brimstone.API.GetAnimation(partPath + "iris_full_crossout.array", "iris", 16);
        public static Texture[] Quicksilver = Brimstone.API.GetAnimation(partPath + "iris_full_quicksilver.array", "iris_full_quicksilver", 16);
    }
    
    public static void TexturesPostProcessing()
    {
    }
}