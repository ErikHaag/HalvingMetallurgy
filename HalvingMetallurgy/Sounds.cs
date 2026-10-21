using System.Collections.Generic;
using System.Reflection;

namespace HalvingMetallurgy;

internal static class Sounds
{
    public static Sound Halves;
    public static Sound Quake;
    public static Sound SumpConsume;
    public static Sound SumpEject;
    public static Sound SumpDiscard;
    public static Sound Remission;
    public static Sound RemissionHalfDemote;
    public static Sound RemissionHalfPromote;
    public static Sound RemissionPromote;
    public static Sound Shearing;
    public static Sound ShearingMakingQuickcopper;
    public static Sound Osmosis;

    public static void LoadSounds()
    {
        Halves = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_halves");
        Quake = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_quake");
        SumpConsume = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_sump_consume");
        SumpEject = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_sump_eject");
        SumpDiscard = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_sump_discard");
        Remission = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_remission");
        RemissionHalfDemote = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_remission_half_demote");
        RemissionHalfPromote = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_remission_half_promote");
        RemissionPromote = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_remission_promote");
        Shearing = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_shearing");
        ShearingMakingQuickcopper = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_shearing_making_quickcopper");
        Osmosis = AssetLoaderHelper.LoadSound("sounds/erikhaag/HalvingMetallurgy/glyph_hm_osmosis");

        Volume.SetVoulme("halves", 0.5f);
        Volume.SetVoulme("quake", 0.3f);
        Volume.SetVoulme("sump_consume", 0.3f);
        Volume.SetVoulme("sump_eject", 0.3f);
        Volume.SetVoulme("sump_discard", 0.3f);
        Volume.SetVoulme("remission", 0.3f);
        Volume.SetVoulme("remission_half_demote", 0.3f);
        Volume.SetVoulme("remission_half_promote", 0.3f);
        Volume.SetVoulme("remission_promote", 0.3f);
        Volume.SetVoulme("shearing", 0.3f);
        Volume.SetVoulme("shearing_making_quickcopper", 0.2f);
        Volume.SetVoulme("osmosis", 0.3f);
    }

    public static void ResetHMSounds()
    {
        Halves.isPlaying = false;
        Quake.isPlaying = false;
        SumpConsume.isPlaying = false;
        SumpEject.isPlaying = false;
        SumpDiscard.isPlaying = false;
        Remission.isPlaying = false;
        RemissionHalfDemote.isPlaying = false;
        RemissionHalfPromote.isPlaying = false;
        RemissionPromote.isPlaying = false;
        Shearing.isPlaying = false;
        ShearingMakingQuickcopper.isPlaying = false;
        Osmosis.isPlaying = false;
    }
}