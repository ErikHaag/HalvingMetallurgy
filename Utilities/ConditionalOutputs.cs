using RecipePredicate = System.Func<Sim, Part, bool>;

namespace HalvingMetallurgy.Utilities;

public static class ConditionOutputHelpers
{
    public static bool AlwaysAllow(Sim sim, Part part) => true;
}


public class ShearingOutput
{
    public AtomType Split;
    public AtomType Eject;
    public RecipePredicate Predicate;

    public ShearingOutput(AtomType split, AtomType eject, RecipePredicate predicate = null)
    {
        Split = split;
        Eject = eject;
        Predicate = predicate ?? ConditionOutputHelpers.AlwaysAllow;
    }
}

public class OsmosisOutput
{
    public AtomType HalfDemotion;
    public RecipePredicate Predicate;

    public OsmosisOutput(AtomType halfDemotion, RecipePredicate predicate = null)
    {
        HalfDemotion = halfDemotion;
        Predicate = predicate ?? ConditionOutputHelpers.AlwaysAllow;
    }
}