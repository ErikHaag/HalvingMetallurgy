using Quintessential;

namespace HalvingMetallurgy;

public static class Recipes
{
    public static void AddRecipes()
    {
        QApi.AddRecipe(HalvingMetallurgy.Instance, new GlyphRecipe(HalvesRecipes), HalvingMetallurgy.Instance.GetIdentifier("halves_native"), Glyphs.Halves.Id);

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