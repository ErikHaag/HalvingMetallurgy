﻿using Quintessential;
using System;
using System.Collections.Generic;

namespace HalvingMetallurgy;

public static class HalvingMetallurgyAPI
{
    public static Dictionary<AtomType, AtomType> HalvesDictionary = new();
    public static Dictionary<AtomType, Pair<AtomType, AtomType>> ShearingDictionary = new();
    public static Dictionary<AtomType, AtomType> OsmosisDictionary = new();


    // Make this a brimstone feature?
    public static Dictionary<AtomType, AtomType> ConvertBeforeConsumption = new();
    public static Dictionary<AtomType, AtomType> ConvertAfterHalfstep = new();


    public static bool AddMetalToMetallicityDictionary(AtomType metal, int doubledMetallicity)
    {
        if (metalToDoubledMetallicity.ContainsKey(metal))
        {
            return false;
        }
        metalToDoubledMetallicity.Add(metal, doubledMetallicity);
        if (!doubledMetallicityToMetal.ContainsKey(doubledMetallicity))
        {
            doubledMetallicityToMetal.Add(doubledMetallicity, metal);
        }
        return true;
    }

    public static Brimstone.API.SuccessInfo ChangeMetallicity(AtomType metal, int deltaMetallicity, out AtomType changedMetal, Predicate<int> predicate = null) {
        changedMetal = metal;
        if (deltaMetallicity == 0)
        {
            return Brimstone.API.SuccessInfo.idempotent;
        }
        if (!metalToDoubledMetallicity.TryGetValue(metal, out int m))
        {
            return Brimstone.API.SuccessInfo.failure;
        }
        m += deltaMetallicity;
        if (m < 0)
        {
            // No negative metals, Crazybot27 !
            return Brimstone.API.SuccessInfo.failure;
        }
        
        if (predicate is not null && !predicate(m))
        {
            return Brimstone.API.SuccessInfo.failure;
        }

        if (!doubledMetallicityToMetal.TryGetValue(m, out AtomType temp)) {
            return Brimstone.API.SuccessInfo.failure;
        }
        changedMetal = temp;
        return Brimstone.API.SuccessInfo.success;
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


    public static Dictionary<AtomType, int> metalToDoubledMetallicity = new();
    public static Dictionary<int, AtomType> doubledMetallicityToMetal = new();
}