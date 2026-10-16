using System;
using Quintessential;

namespace HalvingMetallurgy;

public static class Textures
{
    private const string atomPath = "textures/atoms/erikhaag/HalvingMetallurgy/";
    private const string partPath = "textures/parts/erikhaag/HalvingMetallurgy/";
    private const string selectPath = "textures/select/erikhaag/HalvingMetallurgy/";

    public static class Atom
    {
        public static readonly Texture PartyHatTexture = AssetLoaderHelper.LoadTexture(atomPath + "party_hat");
        public static readonly Texture[] QuickcopperAnimation = new Texture[64];

        public static readonly Texture QuicklimeSymbol = AssetLoaderHelper.LoadTexture(atomPath + "quicklime_symbol");
        public static readonly Texture QuicklimeDiffuse = AssetLoaderHelper.LoadTexture(atomPath + "quicklime_diffuse");

        public static readonly Texture QuickcopperSymbol = AssetLoaderHelper.LoadTexture(atomPath + "quickcopper_symbol");
        public static readonly Texture QuickcopperDiffuse = AssetLoaderHelper.LoadTexture(atomPath + "quickcopper_diffuse");

        public static readonly Texture BerylSymbol = AssetLoaderHelper.LoadTexture(atomPath + "beryl_symbol");
        public static readonly Texture BerylLightramp = AssetLoaderHelper.LoadTexture(atomPath + "beryl_lightramp");
        public static readonly Texture BerylRimlight = AssetLoaderHelper.LoadTexture(atomPath + "beryl_rimlight");

        public static readonly Texture WolframSymbol = AssetLoaderHelper.LoadTexture(atomPath + "wolfram_symbol");
        public static readonly Texture WolframLightramp = AssetLoaderHelper.LoadTexture(atomPath + "wolfram_lightramp");
        public static readonly Texture WolframRimlight = AssetLoaderHelper.LoadTexture(atomPath + "wolfram_rimlight");

        public static readonly Texture VulcanSymbol = AssetLoaderHelper.LoadTexture(atomPath + "vulcan_symbol");
        public static readonly Texture VulcanLightramp = AssetLoaderHelper.LoadTexture(atomPath + "vulcan_lightramp");
        public static readonly Texture VulcanRimlight = AssetLoaderHelper.LoadTexture(atomPath + "vulcan_rimlight");

        public static readonly Texture NickelSymbol = AssetLoaderHelper.LoadTexture(atomPath + "nickel_symbol");
        public static readonly Texture NickelLightramp = AssetLoaderHelper.LoadTexture(atomPath + "nickel_lightramp");
        public static readonly Texture NickelRimlight = AssetLoaderHelper.LoadTexture(atomPath + "nickel_rimlight");

        public static readonly Texture ZincSymbol = AssetLoaderHelper.LoadTexture(atomPath + "zinc_symbol");
        public static readonly Texture ZincLightramp = AssetLoaderHelper.LoadTexture(atomPath + "zinc_lightramp");
        public static readonly Texture ZincRimlight = AssetLoaderHelper.LoadTexture(atomPath + "zinc_rimlight");

        public static readonly Texture SednumSymbol = AssetLoaderHelper.LoadTexture(atomPath + "sednum_symbol");
        public static readonly Texture SednumLightramp = AssetLoaderHelper.LoadTexture(atomPath + "sednum_lightramp");
        public static readonly Texture SednumRimlight = AssetLoaderHelper.LoadTexture(atomPath + "sednum_rimlight");

        public static readonly Texture OsmiumSymbol = AssetLoaderHelper.LoadTexture(atomPath + "osmium_symbol");
        public static readonly Texture OsmiumLightramp = AssetLoaderHelper.LoadTexture(atomPath + "osmium_lightramp");
        public static readonly Texture OsmiumRimlight = AssetLoaderHelper.LoadTexture(atomPath + "osmium_rimlight");
    }

    public static class Icons
    {
        public static readonly Texture Halves = AssetLoaderHelper.LoadTexture(partPath + "icons/halves_icon");
        public static readonly Texture HalvesHover = AssetLoaderHelper.LoadTexture(partPath + "icons/halves_icon_hover");
        public static readonly Texture Quake = AssetLoaderHelper.LoadTexture(partPath + "icons/quake_icon");
        public static readonly Texture QuakeHover = AssetLoaderHelper.LoadTexture(partPath + "icons/quake_icon_hover");
        public static readonly Texture Sump = AssetLoaderHelper.LoadTexture(partPath + "icons/sump_icon");
        public static readonly Texture SumpHover = AssetLoaderHelper.LoadTexture(partPath + "icons/sump_icon_hover");
        public static readonly Texture Remission = AssetLoaderHelper.LoadTexture(partPath + "icons/remission_icon");
        public static readonly Texture RemissionHover = AssetLoaderHelper.LoadTexture(partPath + "icons/remission_icon_hover");
        public static readonly Texture Shearing = AssetLoaderHelper.LoadTexture(partPath + "icons/shearing_icon");
        public static readonly Texture ShearingHover = AssetLoaderHelper.LoadTexture(partPath + "icons/shearing_icon_hover");
        public static readonly Texture Osmosis = AssetLoaderHelper.LoadTexture(partPath + "icons/osmosis_icon");
        public static readonly Texture OsmosisHover = AssetLoaderHelper.LoadTexture(partPath + "icons/osmosis_icon_hover");
        public static readonly Texture Soria = AssetLoaderHelper.LoadTexture(partPath + "icons/soria_icon");
        public static readonly Texture SoriaHover = AssetLoaderHelper.LoadTexture(partPath + "icons/soria_icon_hover");
    }

    public static class GlyphCommon
    {
        public static readonly Texture DoubleBase = AssetLoaderHelper.LoadTexture(partPath + "double_base");
    }

    public static class Halves
    {
        public static readonly Texture Base = AssetLoaderHelper.LoadTexture(partPath + "halves/base");
        public static readonly Texture Glow = UI.AssignOffset(UI.AssignScale(AssetLoaderHelper.LoadTexture(selectPath + "halves_glow"), new Vector2(96, 58)), new Vector2(-1, -1));
        public static readonly Texture Stroke = UI.AssignOffset(UI.AssignScale(AssetLoaderHelper.LoadTexture(selectPath + "halves_stroke"), new Vector2(83, 51)), new Vector2(-1, -1));
        public static readonly Texture Connectors = AssetLoaderHelper.LoadTexture(partPath + "halves/connectors");
        public static readonly Texture GlossMask = AssetLoaderHelper.LoadTexture(partPath + "halves/gloss_mask");

        public static readonly Texture[] ConnectorFlash = Brimstone.API.GetAnimation(partPath + "halves_connector_glow.array", "glow", 16);
        public static readonly Texture[] BowlFlashAnimation = Brimstone.API.GetAnimation(partPath + "halves_bowl_flash.array", "halves_bowl", 10);
    }

    public static class Quake
    {
        public static readonly Texture Base = AssetLoaderHelper.LoadTexture(partPath + "quake/base");
        public static readonly Texture Bowl = AssetLoaderHelper.LoadTexture(partPath + "quake/bowl");
        public static readonly Texture BowlShaking = AssetLoaderHelper.LoadTexture(partPath + "quake/bowl_shaking");
        public static readonly Texture SednumSymbol = AssetLoaderHelper.LoadTexture(partPath + "quake/sednum_symbol");
        public static readonly Texture[] UnbondResistedAnimation = Brimstone.API.GetAnimation("textures/bonds/erikhaag/HalvingMetallurgy/unbond_resist.array", "unbond_resist", 22);
    }

    public static class Sump
    {
        public static readonly Texture[] DrainFlashAnimation = Brimstone.API.GetAnimation(partPath + "sump_drain_flash.array", "sump_flash", 8);
    }

    public static class Remission
    {
        public static readonly Texture Arrow = AssetLoaderHelper.LoadTexture(partPath + "remission/arrow");
        public static readonly Texture Connectors = AssetLoaderHelper.LoadTexture(partPath + "remission/connectors");
        public static readonly Texture[] BowlGlow = new Texture[64];
    }

    public static class Shearing
    {

        public static readonly Texture SednumLine = AssetLoaderHelper.LoadTexture(partPath + "shearing/sednum_line");
        public static readonly Texture[] BowlFlash = Brimstone.API.GetAnimation(partPath + "shearing_flash.array", "shearing_flash", 8);
        // not to be confused with a banana split.
        public static Texture[] Split = Brimstone.API.GetAnimation(atomPath + "shearing_split.array", "split", 16);
        public static readonly Texture[] Spawn = Brimstone.API.GetAnimation(atomPath + "shearing_spawn.array", "spawn", 16);
    }

    public static class Osmosis
    {
        public static readonly Texture Connector = AssetLoaderHelper.LoadTexture(partPath + "osmosis/connector");
        public static readonly Texture OsmiumDemote = AssetLoaderHelper.LoadTexture(partPath + "osmosis/osmium_symbol");
        public static readonly Texture QuickcopperPromote = AssetLoaderHelper.LoadTexture(partPath + "osmosis/quickcopper_symbol");
    }

    public static class Harmony
    {
        public static readonly class_126 TrefoilIndent = Brimstone.API.GetLighting(partPath + "trefoil_marker.lighting");
    }

    public static class Soria
    {
        public static readonly Texture[] Flash = Brimstone.API.GetAnimation(partPath + "soria_flash.array", "soria_flash", 10);
    }

    public static class Irises
    {
        public static readonly Texture[] Crossout = Brimstone.API.GetAnimation(partPath + "iris_full_crossout.array", "iris", 16);
        public static readonly Texture[] Quicksilver = Brimstone.API.GetAnimation(partPath + "iris_full_quicksilver.array", "iris_full_quicksilver", 16);
    }

    public static void LoadOtherTextures()
    {
        string remissionGlowPath = (DateTime.Now.Month == 6 ? "remission_bowl_glow_rainbow.array" : "remission_bowl_glow.array");
        for (int i = 0; i < 64; i++)
        {
            string index = (i + 1).ToString().PadLeft(4, '0');
            Atom.QuickcopperAnimation[i] = AssetLoaderHelper.LoadTexture(atomPath + "cloud_trails.array/trails_" + index);
            Remission.BowlGlow[i] = AssetLoaderHelper.LoadTexture(partPath + remissionGlowPath + "/glow_" + index);
        }    
    
    }
}