using HalvingMetallurgy;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod;
using MonoMod.Cil;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Linq;

[MonoModPatch("Sim")]
class patch_Sim
{

    [MonoModILInject("ProcessInputs")]
    public static void PatchProcessInputs(MethodDefinition method, CustomAttribute attr)
    {

        Console.WriteLine("[HM] Patching ProcessInputs.");
        ILCursor gremlin = new(new ILContext(method));
        Instruction isInputIfEnd = null;
        if (!gremlin.TryGotoNext(MoveType.After,
            (instr) => instr.MatchLdfld(out FieldReference f) && f.Name == "isInput",
            (instr) =>
            {
                if (instr.OpCode != OpCodes.Brfalse_S)
                {
                    return false;
                }
                isInputIfEnd = (Instruction)instr.Operand;
                return true;
            }
        ))
        {
            Console.WriteLine("[HM] Failed to find first if statement.");
            throw new Exception();
        }
        int isInputIndex = gremlin.Index - 1;
        gremlin.Goto(isInputIfEnd);
        int isInputIfEndIndex = gremlin.Index;

        int rejectionIndex = -1;
        if (!gremlin.TryGotoNext(MoveType.After,
            (instr) => instr.MatchLdsfld(out FieldReference f) && f.Name == "rejectionGlyph",
            (instr) => instr.OpCode == OpCodes.Bne_Un
        ))
        {
            Console.WriteLine("[HM] Failed to find rejection if statement.");
            throw new Exception();
        }
        rejectionIndex = gremlin.Index - 1;

        gremlin.Goto(isInputIfEnd);

        int errorIndex = -1;
        if (!gremlin.TryGotoNext(MoveType.After,
            (instr) => instr.MatchCallvirt(out MethodReference m) && m.Name == "HasError",
            (instr) => instr.OpCode == OpCodes.Brtrue
        ))
        {
            Console.WriteLine("[HM] Failed to find error if statement.");
            throw new Exception();
        }
        errorIndex = gremlin.Index - 1;

        if (rejectionIndex < errorIndex)
        {
            Console.WriteLine("[HM] Swapping && operands.");
            int errorConditionLength = errorIndex - rejectionIndex;
            for (int i = 0; i < errorConditionLength; i++)
            {
                Instruction instr = gremlin.Instrs[errorIndex];
                gremlin.Instrs.RemoveAt(errorIndex);
                gremlin.Instrs.Insert(isInputIfEndIndex, instr);
            }
            gremlin.Instrs[isInputIndex].Operand = gremlin.Instrs[isInputIfEndIndex];

            int temp = rejectionIndex;
            rejectionIndex = errorIndex;
            errorIndex = temp;
            Console.WriteLine("[HM] Swapped.");
        }

        Instruction rejectionIfEnd = (Instruction)gremlin.Instrs[rejectionIndex].Operand;
        gremlin.Goto(rejectionIfEnd);

        TypeDefinition simRef = method.DeclaringType.Resolve();
        MethodReference ProcessInputAdditionsRef = simRef.Methods.First(method => method.Name == nameof(ProcessInputAdditions));
        gremlin.Emit(OpCodes.Br, gremlin.Next);
        gremlin.Emit(OpCodes.Ldarg_0);
        gremlin.Instrs[rejectionIndex].Operand = gremlin.Prev;
        gremlin.Emit(OpCodes.Ldloc, 4);
        gremlin.EmitCallvirt(ProcessInputAdditionsRef);
        Console.WriteLine("[HM] Ok.");
    }

    public void ProcessInputAdditions(Part part)
    {
        bool isProcessing = ((Sim)(object)this).simulationDict[part].isProcessing;
        if (part.GetType() == Glyphs.Shearing)
        {
            foreach (KeyValuePair<Identifier, GlyphRecipe> recipe in GlyphRecipe.Recipes[Glyphs.Shearing.Id])
            {
                if (recipe.Value.Predicate.InvokeAndClear((Sim)(object)this, part))
                {
                    ((Sim)(object)this).solutionEditor.glyphEffects.Add(new GlyphEffect(
                        ((Sim)(object)this).solutionEditor, EffectTimescaleType.SimTime,
                        HexGrid.standardGrid.ToPixelCoords(part.GlobalPosFromLocal(Glyphs.shearingOutputHex)),
                        Textures.Shearing.SpawnBloom, 7.5f, Vector2.Zero, part.GetRotation().ToRadians(), 0.7f));
                    break;
                }
            }
        }
        // reset pss
        ((Sim)(object)this).simulationDict[part].isProcessing = isProcessing;
    }
}