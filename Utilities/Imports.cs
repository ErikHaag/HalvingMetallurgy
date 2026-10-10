using Quintessential;
using System;
using System.Collections.Generic;
using System.Linq;
using PartType = class_139;

namespace HalvingMetallurgy.Utilities;

internal static class ReductiveMetallurgyImports
{
    public static Func<Sim, Part, HexIndex, Maybe<AtomReference>> findRavariAtom = (_, _, _) => struct_18.field_1431;
    public static Action<SolutionEditorBase, Part, HexIndex> drawRavariFlash = (_, _, _) => { };
    public static WeakReference<Dictionary<AtomType, AtomType>> Rejections;

    internal static void Load()
    {
        findRavariAtom = ReductiveMetallurgy.Wheel.maybeFindRavariWheelAtom;
        drawRavariFlash = ReductiveMetallurgy.Wheel.DrawRavariFlash;
        object targetObj = typeof(ReductiveMetallurgy.API).GetField("rejectDict", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null);
        if (targetObj is Dictionary<AtomType, AtomType> target)
        {
            Logger.Log(HalvingMetallurgy.LogPrefix + "Got rejection dictionary!");
            Rejections = new(target);
        }
        else
        {
            Rejections = default;
        }
    }

    internal static Maybe<AtomType> TryRejectAtom(AtomType subject)
    {
        if (Rejections == default)
        {
            return struct_18.field_1431;
        }
        if (Rejections.TryGetTarget(out Dictionary<AtomType, AtomType> dictionary) && dictionary.TryGetValue(subject, out AtomType traumatized))
        {
            return traumatized;
        }
        return struct_18.field_1431;
    }

    internal static void LoadRules()
    {
        //Add Rejection Rules for new atoms
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Osmium, Atoms.Sednum);
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Sednum, Atoms.Zinc);
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Zinc, Atoms.Nickel);
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Nickel, Atoms.Vulcan);
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Vulcan, Atoms.Wolfram);
        ReductiveMetallurgy.API.addRejectionRule(Atoms.Wolfram, Atoms.Beryl);
        // Add Deposition Rules
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Osmium, Atoms.Nickel, Brimstone.API.VanillaAtoms.iron);
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Sednum, Brimstone.API.VanillaAtoms.iron, Atoms.Vulcan);
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Zinc, Atoms.Vulcan, Brimstone.API.VanillaAtoms.tin);
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Nickel, Brimstone.API.VanillaAtoms.tin, Atoms.Wolfram);
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Vulcan, Atoms.Wolfram, Brimstone.API.VanillaAtoms.lead);
        ReductiveMetallurgy.API.addDepositionRule(Atoms.Wolfram, Brimstone.API.VanillaAtoms.lead, Atoms.Beryl);
        // Add Proliferation
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Osmium);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Sednum);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Zinc);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Nickel);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Vulcan);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Wolfram);
        ReductiveMetallurgy.API.addProliferationRule(Atoms.Beryl);
    }
}

internal static class VacancyImports
{
    internal static PartType ExtractionGlyph = default;

    public static bool ExtractionPresent(List<Part> parts, Part part, HexIndex[] neighborhood)
    {
        if (!HalvingMetallurgy.VacancyLoaded)
        {
            return false;
        }
        foreach (Part extraction in parts.Where(p => p.method_1159() == ExtractionGlyph))
        {
            HexIndex test = extraction.method_1161();
            foreach (HexIndex neighbor in neighborhood)
            {
                if (test == part.method_1184(neighbor))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool VacaTest(Sim sim, Part part)
    {
        PartType partType = part.method_1159();
        if (partType == Glyphs.Osmosis || partType == Glyphs.Shearing)
        {
            return ExtractionPresent(sim.field_3818.method_502().field_3919, part, Glyphs.DoubleNeighbors);
        }
        else if (partType == Glyphs.Remission)
        {
            return ExtractionPresent(sim.field_3818.method_502().field_3919, part, Glyphs.DiamondNeighbors);
        }
        return false;
    }

    internal static void Load()
    {
        ExtractionGlyph = Vaca.CustomGlyphs.Extraction;
    }

    internal static void LoadRules()
    {
        Glyphs.CalculateAdjacencies();
        API.AddMetalToMetallicityDictionary(Vaca.MainClass.VacaAtom, 0);
        API.HalvesDictionary.Add(Vaca.MainClass.VacaAtom, Atoms.Beryl);
        API.OsmosisDictionary.Add(Atoms.Beryl, new(Vaca.MainClass.VacaAtom, VacaTest));
        API.ShearingDictionary.Add(Atoms.Beryl, new(Atoms.Beryl, Vaca.MainClass.VacaAtom, VacaTest));
        // If you have Vaca, Shearing just duplicates it without fuss.
        API.ShearingDictionary.Add(Vaca.MainClass.VacaAtom, new(Vaca.MainClass.VacaAtom, Vaca.MainClass.VacaAtom));
    }
}