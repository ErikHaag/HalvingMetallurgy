using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Quintessential;
using ReductiveMetallurgy;

namespace HalvingMetallurgy;

public class HalvingMetallurgy : QuintessentialMod
{
    public override void Load()
    {

    }

    public override void PostLoad()
    {

    }

    public override void Unload()
    {
        Quintessential.Logger.Log("icwass made the dictionaries private and didn't add a remove method.\nSo my atoms are still in there, if this is a problem, reload the game.");
        Atoms.Unload();
    }

    private static bool contentLoaded = false;
    public override void LoadPuzzleContent()
    {
        if (contentLoaded)
        {
            return;
        }
        contentLoaded = true;
        Quintessential.Logger.Log("HalvingMetallurgy: Loading!");
        Atoms.AddAtomTypes();
        //ReductiveMetallurgy API
        //Add Rejection Rules for new atoms
        API.addRejectionRule(Atoms.Osmium, Atoms.Sednum);
        API.addRejectionRule(Atoms.Sednum, Atoms.Zinc);
        API.addRejectionRule(Atoms.Zinc, Atoms.Nickel);
        API.addRejectionRule(Atoms.Nickel, Atoms.Vulcan);
        API.addRejectionRule(Atoms.Vulcan, Atoms.Wolfram);
        Quintessential.Logger.Log("HalvingMetallurgy: Loading complete, have fun!");
    }
}
