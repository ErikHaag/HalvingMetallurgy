using HalvingMetallurgy;
using HalvingMetallurgy.Utilities;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

[MonoModPatch("SolutionEditorScreen")]
class patch_SolutionEditorScreen
{
    public List<HexIndexPair> HMQuakeNormalBonders;

    // patch the instance constructor, if you need to patch the class constructor (the static field assignments), use .cctor instead.
    [MonoModILInject(".ctor")]
    public static void PatchCtor(MethodDefinition method, CustomAttribute attrib)
    {
        Console.WriteLine("[HM] Patching SES constructor");
        ILCursor gremlin = new(new ILContext(method));
        if (!gremlin.TryGotoNext(
            instr => instr.OpCode == OpCodes.Ret
        ))
        {
            throw new Exception("[HM] Could not find end of method.");
        }

        // I hate that this is the first thing that genuinely works to instantiate a List object correctly.
        TypeDefinition sesType = method.DeclaringType;
        FieldDefinition bonderListF = sesType.Fields.First(f => f.Name == nameof(HMQuakeNormalBonders));
        // grab the aux method
        MethodDefinition initMD = sesType.Methods.First(m => m.Name == nameof(InitializeHMQuakeNormalBonders));
        // yoink the correct argument.
        MethodReference listCtor = (MethodReference)new ILContext(initMD).Body.Instructions.First(i => i.OpCode == OpCodes.Newobj).Operand;

        gremlin.Emit(OpCodes.Ldarg_0);
        gremlin.EmitNewobj(listCtor);
        gremlin.EmitStfld(bonderListF);

        // eviscerate the aux method.
        sesType.Methods.Remove(initMD);
    }

    public void InitializeHMQuakeNormalBonders()
    {
        HMQuakeNormalBonders = [];
    }

    [MonoModILInject("StartPlayback")]
    public static void PatchStartPlayback(MethodDefinition method, CustomAttribute attrib)
    {
        Console.WriteLine("[HM] Patching SES Start Playback.");
        ILCursor gremlin = new(new ILContext(method));
        if (!gremlin.TryGotoNext(
            instr => instr.OpCode == OpCodes.Ret
        ))
        {
            throw new Exception("[HM] Could not find end of method.");
        }

        TypeDefinition sesType = method.DeclaringType;
        MethodDefinition PopulateHMQuakeNormalBondersMethod = sesType.Methods.First(m => m.Name == nameof(PopulateHMQuakeNormalBonders));

        gremlin.Emit(OpCodes.Ldarg_0);
        gremlin.Emit(OpCodes.Callvirt, PopulateHMQuakeNormalBondersMethod);
    }

    public void PopulateHMQuakeNormalBonders()
    {
        HMQuakeNormalBonders.Clear();
        Solution solution = ((SolutionEditorScreen)(object)this).GetSolution();
        foreach (Part part in solution.parts)
        {
            foreach (BonderInfo bonder in part.GetType().bonders)
            {
                if (bonder.isUnbond
                    || !bonder.bondTypes.Contains(BondTypes.standard)
                    || (bonder.uniqueAtoms.Count != 0 && !bonder.uniqueAtoms.Contains(Atoms.Sednum)))
                {
                    continue;
                }
                HMQuakeNormalBonders.Add(new HexIndexPair(bonder.hexPos1, bonder.hexPos2));
            }
        }
    }
}