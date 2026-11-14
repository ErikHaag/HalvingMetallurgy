using HalvingMetallurgy.Utilities;
using MonoMod.Utils;
using Quintessential;

namespace HalvingMetallurgy;

public static class Recipes
{
    public static void AddRecipes()
    {
        QApi.AddRecipe(HalvingMetallurgy.Instance, new GlyphRecipe(HalvesRecipes), HalvingMetallurgy.Instance.GetIdentifier("halves_native"), Glyphs.Halves.Id);

        QApi.AddRecipe(HalvingMetallurgy.Instance, new GlyphRecipe(RemissionRecipes), HalvingMetallurgy.Instance.GetIdentifier("remission_native"), Glyphs.Remission.Id);
        QApi.AddRecipe(HalvingMetallurgy.Instance, new GlyphRecipe(RemissionFullPromoteRecipes), HalvingMetallurgy.Instance.GetIdentifier("remission_fp_native"), HalvingMetallurgy.Instance.GetIdentifier("remission_full_promote"));
        QApi.AddRecipe(HalvingMetallurgy.Instance, new GlyphRecipe(RemissionHalfPromoteRecipes), HalvingMetallurgy.Instance.GetIdentifier("remission_hp_native"), HalvingMetallurgy.Instance.GetIdentifier("remission_half_promote"));
        QApi.AddRecipe(HalvingMetallurgy.Instance, new GlyphRecipe(RemissionHalfDemoteRecipes), HalvingMetallurgy.Instance.GetIdentifier("remission_hd_native"), HalvingMetallurgy.Instance.GetIdentifier("remission_half_demote"));

        QApi.AddRecipe(HalvingMetallurgy.Instance, new GlyphRecipe(ShearingRecipes), HalvingMetallurgy.Instance.GetIdentifier("shearing_native"), Glyphs.Shearing.Id);

        QApi.AddRecipe(HalvingMetallurgy.Instance, new GlyphRecipe(OsmosisRecipes), HalvingMetallurgy.Instance.GetIdentifier("osmosis_native"), Glyphs.Osmosis.Id);

        QApi.AddRecipe(HalvingMetallurgy.Instance, new(ProjectionRecipes), HalvingMetallurgy.Instance.GetIdentifier("quix_proj"), PartTypes.projectionGlyph.Id);
        QApi.AddRecipe(HalvingMetallurgy.Instance, new(ProjectionSoriaRecipes), HalvingMetallurgy.Instance.GetIdentifier("soria_proj"), HalvingMetallurgy.Instance.GetIdentifier("projection"));
    }

    public static bool HalvesRecipes(Sim sim, Part part)
    {
        if (!sim.FindAtomRelative(part, Glyphs.halvesInputHex).GetOrDefault(out AtomReference quicksilver) || quicksilver.inMultiAtomMolecule || quicksilver.isHeldByArm || quicksilver.atomType.Id != AtomTypes.quicksilver.Id)
        {
            return false;
        }
        if (!sim.FindAtomRelative(part, Glyphs.halvesMetal1Hex).GetOrDefault(out AtomReference metal1) || !sim.FindAtomRelative(part, Glyphs.halvesMetal2Hex).GetOrDefault(out AtomReference metal2))
        {
            return false;
        }
        var promotionTable = AtomTag.AtomTags["halving_metallurgy:$successor_half"];
        if (!promotionTable.HasAtom(metal1, out Identifier? newMetal1) || !promotionTable.HasAtom(metal2, out Identifier? newMetal2))
        {
            return false;
        }

        sim.RecipeInputs[Glyphs.halvesInputHex] = quicksilver;
        sim.RecipeInputs[Glyphs.halvesMetal1Hex] = metal1;
        sim.RecipeInputs[Glyphs.halvesMetal2Hex] = metal2;
        sim.RecipeOutputs[Glyphs.halvesMetal1Hex] = AtomTypes.GetByID(newMetal1.Value);
        sim.RecipeOutputs[Glyphs.halvesMetal2Hex] = AtomTypes.GetByID(newMetal2.Value);

        return true;
    }

    public static bool RemissionRecipes(Sim sim, Part part)
    {
        if (sim.FindAtomRelative(part, Glyphs.remissionOutputHex).HasValue())
        {
            return false;
        }

        if (!sim.FindAtomRelative(part, Glyphs.remissionInput1Hex).GetOrDefault(out AtomReference metal1) || metal1.inMultiAtomMolecule || metal1.isHeldByArm)
        {
            return false;
        }

        if (!sim.FindAtomRelative(part, Glyphs.remissionInput2Hex).GetOrDefault(out AtomReference metal2) || metal2.inMultiAtomMolecule || metal2.isHeldByArm)
        {
            return false;
        }

        if (metal1.atomType != metal2.atomType)
        {
            return false;
        }

        if (!API.remissionMetallicity.TryGetValue(metal1.atomType.Id, out int metallicity)
        || !AtomTag.AtomTags["om:$successor_proj"].HasAtom(metal1, out Identifier? promotion))
        {
            return false;
        }
        DynamicData data = new(sim.simulationDict[part]);
        object stateOb = data.Get("state");
        RemissionState state = stateOb is not null ? (RemissionState)stateOb : new RemissionState();
        sim.RecipeInputs[Glyphs.remissionInput1Hex] = metal1;
        sim.RecipeInputs[Glyphs.remissionInput2Hex] = metal2;
        sim.RecipeOutputs[Glyphs.remissionOutputHex] = AtomTypes.GetByID(promotion.Value);
        state.halfProjections = metallicity - 2;
        data.Set("state", state);
        return true;
    }
    public static bool RemissionFullPromoteRecipes(Sim sim, Part part)
    {
        if (!(sim.FindAtomRelative(part, Glyphs.remissionBowlHex).GetOrDefault(out AtomReference metal)
              && AtomTag.AtomTags["om:$successor_proj"].HasAtom(metal, out Identifier? proj)))
        {
            return false;
        }
        sim.RecipeInputs[Glyphs.remissionBowlHex] = metal;
        sim.RecipeOutputs[Glyphs.remissionBowlHex] = AtomTypes.GetByID(proj.Value);
        return true;
    }

    public static bool RemissionHalfPromoteRecipes(Sim sim, Part part)
    {
        if (!(sim.FindAtomRelative(part, Glyphs.remissionBowlHex).GetOrDefault(out AtomReference metal)
              && AtomTag.AtomTags["halving_metallurgy:$successor_half"].HasAtom(metal, out Identifier? proj)))
        {
            return false;
        }
        sim.RecipeInputs[Glyphs.remissionBowlHex] = metal;
        sim.RecipeOutputs[Glyphs.remissionBowlHex] = AtomTypes.GetByID(proj.Value);
        return true;
    }

    public static bool RemissionHalfDemoteRecipes(Sim sim, Part part)
    {
        if (!(sim.FindAtomRelative(part, Glyphs.remissionBowlHex).GetOrDefault(out AtomReference metal)
              && AtomTag.AtomTags["halving_metallurgy:$predecessor_half"].HasAtom(metal, out Identifier? proj)))
        {
            return false;
        }
        sim.RecipeInputs[Glyphs.remissionBowlHex] = metal;
        sim.RecipeOutputs[Glyphs.remissionBowlHex] = AtomTypes.GetByID(proj.Value);
        return true;
    }

    public static bool ShearingRecipes(Sim sim, Part part)
    {
        if (sim.FindAtomRelative(part, Glyphs.shearingOutputHex).HasValue())
        {
            return false;
        }
        if (!sim.FindAtomRelative(part, Glyphs.shearingBowlHex).GetOrDefault(out AtomReference metal))
        {
            return false;
        }
        if (!AtomTag.AtomTags["halving_metallurgy:$shearing_upper"].HasAtom(metal, out Identifier? cut) || !AtomTag.AtomTags["halving_metallurgy:$shearing_lower"].HasAtom(metal, out Identifier? swarf))
        {
            return false;
        }
        sim.RecipeInputs[Glyphs.shearingBowlHex] = metal;
        sim.RecipeOutputs[Glyphs.shearingBowlHex] = AtomTypes.GetByID(cut.Value);
        sim.RecipeOutputs[Glyphs.shearingOutputHex] = AtomTypes.GetByID(swarf.Value);
        return true;
    }

    public static bool OsmosisRecipes(Sim sim, Part part)
    {
        if (!(sim.FindAtomRelative(part, Glyphs.osmosisMetalHex).GetOrDefault(out AtomReference metal)
              && sim.FindAtomRelative(part, Glyphs.osmosisQuixHex).GetOrDefault(out AtomReference quix)))
        {
            return false;
        }
        if (!(AtomTag.AtomTags["halving_metallurgy:$predecessor_half"].HasAtom(metal, out Identifier? newMetal)
              && AtomTag.AtomTags["halving_metallurgy:$successor_half_quix"].HasAtom(quix, out Identifier? newQuix)))
        {
            return false;
        }
        sim.RecipeInputs[Glyphs.osmosisMetalHex] = metal;
        sim.RecipeInputs[Glyphs.osmosisQuixHex] = quix;
        sim.RecipeOutputs[Glyphs.osmosisMetalHex] = AtomTypes.GetByID(newMetal.Value);
        sim.RecipeOutputs[Glyphs.osmosisQuixHex] = AtomTypes.GetByID(newQuix.Value);
        return true;
    }

    public static bool ProjectionRecipes(Sim sim, Part part)
    {
        HexIndex quicksilverHole = new(0, 0);
        if (!sim.GetAtomReference(part, quicksilverHole, false, out AtomReference quicksilverRef) || quicksilverRef.isHeldByArm || quicksilverRef.inMultiAtomMolecule)
        {
            return false;
        }

        HexIndex metalBowl = new(1, 0);
        bool isMetalSoria = false;
        if (sim.GetAtomReference(part, metalBowl, false, out AtomReference metalRef))
        {
            if (quicksilverRef.atomType == AtomTypes.quicksilver)
            {
                // Handled by Quintessential
                return false;
            }
        }
        else if (Glyphs.Soria.TryFindWheelAtRelative(sim, part, metalBowl, out metalRef))
        {
            isMetalSoria = true;
        }
        else
        {
            return false;
        }

        if (metalRef is null)
        {
            HalvingMetallurgy.Instance.Log.Critical(HalvingMetallurgy.LogPrefix + "Unreachable code reached!");
            return false;
        }

        AtomType newMetal;
        if (quicksilverRef.atomType == AtomTypes.quicksilver && isMetalSoria)
        {
            if (!AtomTag.AtomTags["halving_metallurgy:$successor_proj_quix"].HasAtom(metalRef, out Identifier? mapped))
            {
                return false;
            }
            newMetal = AtomTypes.GetByID(mapped.Value);
        }
        else if (quicksilverRef.atomType == Atoms.Quickcopper)
        {
            if (!AtomTag.AtomTags[isMetalSoria ? "halving_metallurgy:$successor_half_quix" : "halving_metallurgy:$successor_half"].HasAtom(metalRef, out Identifier? mapped))
            {
                return false;
            }

            newMetal = AtomTypes.GetByID(mapped.Value);
        }
        else if (quicksilverRef.atomType == Atoms.Quicklime)
        {
            if (!AtomTag.AtomTags[isMetalSoria ? "halving_metallurgy:quicklime_proj_target_quix" : "halving_metallurgy:quicklime_proj_target"].HasAtom(metalRef))
            {
                return false;
            }
            newMetal = metalRef.atomType;
        }
        else
        {
            return false;
        }

        sim.RecipeInputs.Add(quicksilverHole, quicksilverRef);
        sim.RecipeInputs.Add(metalBowl, metalRef);
        sim.RecipeOutputs.Add(quicksilverHole, newMetal);

        return true;
    }

    public static bool ProjectionSoriaRecipes(Sim sim, Part part)
    {
        HexIndex quicksilverHole = new(0, 0);
        if (!Glyphs.Soria.TryFindWheelAtRelative(sim, part, quicksilverHole, out AtomReference quixRef))
        {
            return false;
        }
        if (quixRef.atomType == Atoms.Quicklime)
        {
            // crickets.
            return false;
        }

        HexIndex metalBowl = new(1, 0);
        bool isMetalSoria = false;
        if (sim.FindAtomRelative(part, metalBowl).GetOrDefault(out AtomReference metalRef))
        {
            // wow, much code.
        }
        else if (Glyphs.Soria.TryFindWheelAtRelative(sim, part, metalBowl, out metalRef))
        {
            isMetalSoria = true;
        }
        else
        {
            return false;
        }

        AtomType newMetal;
        if (quixRef.atomType == AtomTypes.quicksilver)
        {
            if (!AtomTag.AtomTags[isMetalSoria ? "halving_metallurgy:$successor_proj_quix" : "om:$successor_proj"].HasAtom(metalRef, out Identifier? mapped))
            {
                return false;
            }
            newMetal = AtomTypes.GetByID(mapped.Value);
        }
        else if (quixRef.atomType == Atoms.Quickcopper)
        {
            if (!AtomTag.AtomTags[isMetalSoria ? "halving_metallurgy:$successor_half_quix" : "halving_metallurgy:$successor_half"].HasAtom(metalRef, out Identifier? mapped))
            {
                return false;
            }
            newMetal = AtomTypes.GetByID(mapped.Value);
        }
        else
        {
            return false;
        }

        sim.RecipeInputs.Add(quicksilverHole, quixRef);
        sim.RecipeInputs.Add(metalBowl, metalRef);
        sim.RecipeOutputs.Add(quicksilverHole, Atoms.Quicklime);
        sim.RecipeOutputs.Add(metalBowl, newMetal);
        return true;
    }
}