using Quintessential;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;

namespace HalvingMetallurgy;

public class ValueTweaker
{
    public bool Enabled;
    public Action<ValueTweaker> OnUpdateCallback;
    public ValueTweaker(bool enabled)
    {
        Enabled = enabled;
        IL.SolutionEditorBase.method_1984 += ValueTweakerPhage;
    }

    public void Unload()
    {
        IL.SolutionEditorBase.method_1984 -= ValueTweakerPhage;
    }

    private static void ValueTweakerPhage(ILContext context)
    {
        ILCursor gremlin = new(context);

        if (!gremlin.TryGotoNext(MoveType.After,
            instr => instr.MatchLdloc(4),
            instr => instr.MatchCallvirt("SolutionEditorBase", "method_1993"),
            instr => instr.MatchLdloc(9)))
        {
            throw new Exception("Could not find part draw loop");
        }

        if (!gremlin.TryGotoNext(MoveType.After,
            instr => instr.OpCode == OpCodes.Blt_S,
            instr => instr.MatchLdloc(3),
            instr => instr.MatchStloc(26)))
        {
            throw new Exception("Could not find end of loop");
        }
        gremlin.EmitDelegate(() =>
        {
            Glyphs.tweaker.Update();
            Glyphs.tweaker.Display(new(500, 500));
        });

    }
    public int V1 = 0;
    public int V2 = 0;
    public int V3 = 0;
    public int V4 = 0;

    public void Update()
    {
        if (!Enabled)
        {
            return;
        }
        bool hasChanged = false;
        if (class_115.method_200(SDL2.SDL.enum_160.SDLK_y))
        {
            V1 += 1;
            hasChanged = true;
        }
        else if (class_115.method_200(SDL2.SDL.enum_160.SDLK_h))
        {
            V1 -= 1;
            hasChanged = true;
        }

        if (class_115.method_200(SDL2.SDL.enum_160.SDLK_u))
        {
            V2 += 1;
            hasChanged = true;
        }
        else if (class_115.method_200(SDL2.SDL.enum_160.SDLK_j))
        {
            V2 -= 1;
            hasChanged = true;
        }

        if (class_115.method_200(SDL2.SDL.enum_160.SDLK_i))
        {
            V3 += 1;
            hasChanged = true;
        }
        else if (class_115.method_200(SDL2.SDL.enum_160.SDLK_k))
        {
            V3 -= 1;
            hasChanged = true;
        }

        if (class_115.method_200(SDL2.SDL.enum_160.SDLK_o))
        {
            V4 += 1;
            hasChanged = true;
        }
        else if (class_115.method_200(SDL2.SDL.enum_160.SDLK_l))
        {
            V4 -= 1;
            hasChanged = true;
        }
        
        if (hasChanged && OnUpdateCallback is not null)
        {
            OnUpdateCallback(this);
        }
    }

    public void Display(Vector2 pos)
    {
        if (!Enabled)
        {
            return;
        }
        Vector2 lineOffset = new(0, UI.DrawText($"{V1}, {V2}, {V3}, {V4}", pos, UI.Text, UI.TextColor, TextAlignment.Centred).Height);
        UI.DrawText($"y, u, i, o", pos + lineOffset, UI.Text, UI.TextColor, TextAlignment.Centred);
        UI.DrawText($"h, j, k, l", pos - lineOffset, UI.Text, UI.TextColor, TextAlignment.Centred);
    }
}