using HalvingMetallurgy.Utilities;
using Quintessential;
using System;
using System.Collections.Generic;

namespace HalvingMetallurgy;

public static class API
{
    public static Dictionary<AtomType, AtomType> HalvesDictionary = new();
    public static Dictionary<AtomType, ShearingOutput> ShearingDictionary = new();
    public static Dictionary<AtomType, OsmosisOutput> OsmosisDictionary = new();

    public static bool AddMetalToMetallicityDictionary(AtomType metal, int doubledMetallicity)
    {
        if (metalToDoubledMetallicity.ContainsKey(metal))
        {
            return false;
        }
        metalToDoubledMetallicity.Add(metal, doubledMetallicity);
        return true;
    }

    public static HexIndex[] GetGlyphNeighbors(HexIndex[] glyphHexes)
    {
        HexIndex[] output = new HexIndex[6 * glyphHexes.Length];
        int i = 0;
        foreach (HexIndex hex in glyphHexes)
        {
            foreach (HexIndex offset in HexIndex.AdjacentOffsets)
            {
                HexIndex potential = hex + offset;
                foreach (HexIndex test in glyphHexes)
                {
                    if (test == potential)
                    {
                        goto nextOffset;
                    }
                }
                for (int j = 0; j < i; j++)
                {
                    if (output[j] == potential)
                    {
                        goto nextOffset;
                    }
                }
                output[i] = potential;
                i++;
            nextOffset:;
            }
        }
        Array.Resize(ref output, i);
        return output;
    }


    public static readonly Dictionary<AtomType, int> metalToDoubledMetallicity = new();

    internal static void AddQuicksilverToDictionary(AtomType qs, int doubledMetallicity)
    {
        quicksilverToDoubledMetallicity.Add(qs, doubledMetallicity);
        doubledMetallicityToQuicksilver.Add(doubledMetallicity, qs);
    }

    private static readonly Dictionary<AtomType, int> quicksilverToDoubledMetallicity = new();
    private static readonly Dictionary<int, AtomType> doubledMetallicityToQuicksilver = new();
    public static Maybe<AtomType> QuicksilverProjectionBehavior(AtomType qs, int delta)
    {
        if (quicksilverToDoubledMetallicity.TryGetValue(qs, out int m) && doubledMetallicityToQuicksilver.TryGetValue(m + delta, out AtomType newQs))
        {
            return newQs;
        }
        return struct_18.field_1431;
    }
}