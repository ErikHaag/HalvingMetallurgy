using System.Collections.Generic;

namespace HalvingMetallurgy;

public static class HalvingMetallurgyAPI
{
    public static Dictionary<AtomType, AtomType> HalvingPromotions = new();

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

    public static Dictionary<AtomType, int> metalToDoubledMetallicity = new();
    public static Dictionary<int, AtomType> doubledMetallicityToMetal = new();
}