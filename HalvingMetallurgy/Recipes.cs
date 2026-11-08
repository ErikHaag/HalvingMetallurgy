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

        QApi.AddRecipe(HalvingMetallurgy.Instance, new(ProjectionRecipes), HalvingMetallurgy.Instance.GetIdentifier("quix_proj"), PartTypes.projectionGlyph.Id);
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
        || !AtomTag.AtomTags["om:$successor_proj"].HasAtom(metal1.atomType, out Identifier? promotion))
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
              && AtomTag.AtomTags["om:$successor_proj"].HasAtom(metal.atomType, out Identifier? proj)))
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
              && AtomTag.AtomTags["halving_metallurgy:$successor_half"].HasAtom(metal.atomType, out Identifier? proj)))
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
              && AtomTag.AtomTags["halving_metallurgy:$predecessor_half"].HasAtom(metal.atomType, out Identifier? proj)))
        {
            return false;
        }
        sim.RecipeInputs[Glyphs.remissionBowlHex] = metal;
        sim.RecipeOutputs[Glyphs.remissionBowlHex] = AtomTypes.GetByID(proj.Value);
        return true;
    }

    public static bool ProjectionRecipes(Sim sim, Part part)
    {
        if (!sim.GetAtomReference(part, new HexIndex(0, 0), false, out AtomReference quicksilverRef) || quicksilverRef.isHeldByArm || quicksilverRef.inMultiAtomMolecule)
        {
            return false;
        }

        if (!sim.GetAtomReference(part, new(1, 0), false, out AtomReference metalRef))
        {
            return false;
        }
        if (quicksilverRef.atomType.Id == "om:quicksilver")
        {
            // Handled by Quintessential
            return false;
        }

        AtomType newMetal;
        if (quicksilverRef.atomType.Id == "halving_metallurgy:quickcopper")
        {
            if (!AtomTag.AtomTags["halving_metallurgy:$successor_half"].HasAtom(metalRef, out Identifier? mapped))
            {
                return false;
            }
            newMetal = AtomTypes.GetByID(mapped.Value);
        }
        else if (quicksilverRef.atomType.Id == "halving_metallurgy:quicklime")
        {
            if (!AtomTag.AtomTags["halving_metallurgy:quicklime_proj_target"].HasEntry(metalRef.atomType.Id))
            {
                return false;
            }
            newMetal = metalRef.atomType;
        }
        else
        {
            return false;
        }
        sim.RecipeInputs.Add(new HexIndex(0, 0), quicksilverRef);
        sim.RecipeInputs.Add(new HexIndex(1, 0), metalRef);
        sim.RecipeOutputs.Add(new HexIndex(0, 0), newMetal);

        return true;
    }
}