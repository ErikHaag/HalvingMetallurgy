using Quintessential;
using System.Collections.Generic;

namespace HalvingMetallurgy;

public static class API
{
    public static readonly Dictionary<Identifier, int> remissionMetallicity = [];

    public static bool AddAtomToRemissionTable(Identifier atomId, int doubleMetallicity)
    {
        bool replaced = remissionMetallicity.ContainsKey(atomId);
        remissionMetallicity[atomId] = doubleMetallicity;
        return replaced;
    }
}
