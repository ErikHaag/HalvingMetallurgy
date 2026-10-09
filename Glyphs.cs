using HalvingMetallurgy.Utilities;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PartType = class_139;
using Texture = class_256;

namespace HalvingMetallurgy;

public static class Glyphs
{
    public static bool quickcopperRadioactive = true;

    #region Glyphs
    public static PartType Halves;
    public static PartType Quake;
    public static PartType Sump;
    public static PartType Remission;
    public static PartType Shearing;
    public static PartType Osmosis;
    #endregion

    #region Hexes
    public static readonly HexIndex halvesInputHex = new(0, 0);
    public static readonly HexIndex halvesMetal1Hex = new(1, 0);
    public static readonly HexIndex halvesMetal2Hex = new(-1, 1);

    public static readonly HexIndex quakeBowlHex = new(0, 0);

    public static readonly HexIndex sumpInputHex = new(0, 0);
    public static readonly HexIndex sumpOutputHex = new(1, 0);

    public static readonly HexIndex remissionInput1Hex = new(0, 1);
    public static readonly HexIndex remissionInput2Hex = new(1, -1);
    public static readonly HexIndex remissionOutputHex = new(1, 0);
    public static readonly HexIndex remissionBowlHex = new(0, 0);

    public static readonly HexIndex shearingBowlHex = new(0, 0);
    public static readonly HexIndex shearingOutputHex = new(1, 0);

    public static readonly HexIndex osmosisMetalHex = new(0, 0);
    public static readonly HexIndex osmosisQuickcopperHex = new(1, 0);

    public static HexIndex[] DoubleNeighbors;
    public static HexIndex[] DiamondNeighbors;

    public static void CalculateAdjacencies()
    {
        DoubleNeighbors = API.GetGlyphNeighbors(Osmosis.field_1540);
        DiamondNeighbors = API.GetGlyphNeighbors(Remission.field_1540);
    }

    #endregion

    #region Code Modification

    public static ValueTweaker tweaker;

    internal static List<HexIndexPair> simBonders = new();

    private static Hook AtomSwapHook;

    internal static void LoadHooking()
    {
        Logger.Log(HalvingMetallurgy.LogPrefix + "Hooking!");
        IL.SolutionEditorBase.method_1984 += InjectDrawSoriaAtom;
        IL.SolutionEditorScreen.method_2097 += InjectSimStartup;
        On.Editor.method_927 += Atoms.OnAtomRender;
        AtomSwapHook = new(typeof(Sim).GetMethod("method_1832", BindingFlags.Instance | BindingFlags.NonPublic), AtomSwap);
        tweaker = new ValueTweaker(false)
        {
            OnUpdateCallback = OnTweak
        };
    }

    internal static void OnTweak(ValueTweaker t)
    {
        //Vector2 pivot = new(t.V1, t.V2);
        //Vector2 offset = new(t.V3, t.V4);
    }

    internal static void UnloadHooks()
    {
        Logger.Log(HalvingMetallurgy.LogPrefix + "Removing Hooks!");
        IL.SolutionEditorBase.method_1984 -= InjectDrawSoriaAtom;
        IL.SolutionEditorScreen.method_2097 -= InjectSimStartup;
        On.Editor.method_927 -= Atoms.OnAtomRender;
        AtomSwapHook.Dispose();
        tweaker.Unload();
    }


    internal static void InjectSimStartup(ILContext context)
    {
        ILCursor gremlin = new(context);
        if (!gremlin.TryGotoNext(MoveType.Before,
            instr => instr.OpCode == OpCodes.Ret
            ))
        {
            throw new Exception("Could not find end of startup function");
        }
        gremlin.Emit(OpCodes.Ldarg_1); // get sim object
        gremlin.EmitDelegate<Action<Sim>>(sim =>
        {
            SimStartup(sim);
        });
    }

    internal static void SimStartup(Sim sim)
    {
        simBonders.Clear();
        List<Part> parts = sim.field_3818.method_502().field_3919;
        foreach (Part potential_bonder in parts)
        {
            class_222[] pT = potential_bonder.method_1159().field_1538;
            foreach (class_222 bonder in pT)
            {
                if (bonder.field_1922 != enum_126.Standard)
                {
                    continue;
                }

                if (!bonder.field_1923.method_99(out AtomType requires) || requires == Atoms.Sednum)
                {
                    simBonders.Add(new(potential_bonder.method_1184(bonder.field_1920), potential_bonder.method_1184(bonder.field_1921)));
                }
            }
        }
    }

    internal static void InjectDrawSoriaAtom(ILContext context)
    {
        ILCursor cursor = new(context);
        if (!cursor.TryGotoNext(MoveType.After,
            instr => instr.MatchCallvirt("SolutionEditorBase", "method_2015")))
        {
            Logger.Log(HalvingMetallurgy.LogPrefix + "Failed to inject draw call (no method_2015 call)");
            return;
        }

        if (!cursor.TryGotoNext(MoveType.After,
            instr => instr.MatchEndfinally()))
        {
            Logger.Log(HalvingMetallurgy.LogPrefix + "Fail to inject draw call (no loop end)");
            return;
        }

        cursor.Index++;
        cursor.Emit(OpCodes.Ldarg_0);
        cursor.Emit(OpCodes.Ldloc_0);
        cursor.EmitDelegate<Action<SolutionEditorBase, SolutionEditorBase.class_423>>((self, uco) =>
        {
            if (self.method_503() != enum_128.Stopped)
            {
                var partList = self.method_502().field_3919;
                foreach (var soria in partList.Where(x => x.method_1159() == Wheel.Soria))
                {
                    Wheel.DrawSoriaAtoms(self, soria, uco.field_3959, true);
                }
            }
        });
    }

    public delegate void orig_method_1832(Sim self, bool param_5369);

    public static void AtomSwap(orig_method_1832 orig, Sim self, bool param_5369)
    {
        if (!quickcopperRadioactive)
        {
            orig(self, param_5369);
            return;
        }
        foreach (Molecule m in self.field_3823)
        {
            foreach (KeyValuePair<HexIndex, Atom> kvp in m.method_1100())
            {
                if (kvp.Value.field_2275 == Atoms.ActiveQuickcopper)
                {
                    m.method_1106(Atoms.Quickcopper, kvp.Key);
                }
            }
        }
        orig(self, param_5369);
        foreach (Molecule m in self.field_3823)
        {
            foreach (KeyValuePair<HexIndex, Atom> kvp in m.method_1100())
            {
                if (kvp.Value.field_2275 == Atoms.Quickcopper)
                {
                    m.method_1106(Atoms.ActiveQuickcopper, kvp.Key);
                }
            }
        }
    }

    #endregion

    public static void AddPartTypes()
    {

        #region Glyph Definitions

        Halves = Brimstone.API.CreateSimpleGlyph(
            ID: "halving-metallurgy-halves",
            name: "Glyph of Halves",
            description: "The glyph of halves consumes an atom of quicksilver to half-promote two metal atoms.",
            cost: 30,
            glow: Textures.Halves.Glow,
            stroke: Textures.Halves.Stroke,
            icon: Textures.Icons.Halves,
            hoveredIcon: Textures.Icons.HalvesHover,
            usedHexes: new HexIndex[]
            {
                halvesInputHex,
                halvesMetal1Hex,
                halvesMetal2Hex
            },
            customPermission: HalvingMetallurgy.HalvingPermission
        );

        Quake = Brimstone.API.CreateSimpleGlyph(
            ID: "halving-metallurgy-quake",
            name: "Glyph of Quake",
            description: "The glyph of quake shakes a molecule vigorously, breaking most bonds that connect sednum, and creating bonds that connect to zinc.",
            cost: 15,
            glow: class_238.field_1989.field_97.field_382,
            stroke: class_238.field_1989.field_97.field_383,
            icon: Textures.Icons.Quake,
            hoveredIcon: Textures.Icons.QuakeHover,
            usedHexes: new HexIndex[]
            {
                quakeBowlHex
            },
            customPermission: HalvingMetallurgy.QuakePermission
        );

        Sump = Brimstone.API.CreateSimpleGlyph(
            ID: "halving-metallurgy-sump",
            name: "Quicksilver Sump",
            description: "The quicksilver sump can hold 5 quicksilver internally for storage, excess quicksilver is drained out of the engine.",
            cost: 15,
            glow: class_238.field_1989.field_97.field_374,
            stroke: class_238.field_1989.field_97.field_375,
            icon: Textures.Icons.Sump,
            hoveredIcon: Textures.Icons.SumpHover,
            usedHexes: new HexIndex[]
            {
                sumpInputHex,
                sumpOutputHex
            },
            customPermission: HalvingMetallurgy.SumpPermission
        );
        Sump.field_1552 = true; // only one is allowed.

        Remission = Brimstone.API.CreateSimpleGlyph(
            ID: "halving-metallurgy-remission",
            name: "Glyph of Remission",
            description: "The glyph of remission transmutes two metal atoms to their next higher form, preventing further transmutations until the metallicity reserve is restored through the bowl.",
            cost: 30,
            glow: class_238.field_1989.field_97.field_368,
            stroke: class_238.field_1989.field_97.field_369,
            icon: Textures.Icons.Remission,
            hoveredIcon: Textures.Icons.RemissionHover,
            usedHexes: new HexIndex[]
            {
                remissionInput1Hex,
                remissionInput2Hex,
                remissionBowlHex,
                remissionOutputHex
            },
            customPermission: HalvingMetallurgy.RemissionPermission
        );

        Shearing = Brimstone.API.CreateSimpleGlyph(
            ID: "halving-metallurgy-shearing",
            name: "Glyph of Shearing",
            description: "The glyph of shearing cuts a metal in half, and ejects the remainder.",
            cost: 25,
            glow: class_238.field_1989.field_97.field_374,
            stroke: class_238.field_1989.field_97.field_375,
            icon: Textures.Icons.Shearing,
            hoveredIcon: Textures.Icons.ShearingHover,
            usedHexes: new HexIndex[]
            {
                shearingBowlHex,
                shearingOutputHex
            },
            customPermission: HalvingMetallurgy.ShearingPermission
        );

        Osmosis = Brimstone.API.CreateSimpleGlyph(
            ID: "halving-metallurgy-osmosis",
            name: "Glyph of Osmosis",
            description: "The glyph of osmosis half-demotes a metal to promote an atom of quix.",
            cost: 25,
            glow: class_238.field_1989.field_97.field_374,
            stroke: class_238.field_1989.field_97.field_375,
            icon: Textures.Icons.Osmosis,
            hoveredIcon: Textures.Icons.OsmosisHover,
            usedHexes: new HexIndex[]
            {
                osmosisMetalHex,
                osmosisQuickcopperHex
            },
            customPermission: HalvingMetallurgy.OsmosisPermission
        );

        #endregion

        QApi.AddPartTypeToPanel(Halves, false);
        QApi.AddPartTypeToPanel(Quake, false);
        QApi.AddPartTypeToPanel(Sump, false);
        QApi.AddPartTypeToPanel(Remission, false);
        QApi.AddPartTypeToPanel(Shearing, false);
        QApi.AddPartTypeToPanel(Osmosis, false);

        #region Glyph Renderers

        QApi.AddPartType(Halves, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            float time = editor.method_504();
            Vector2 offset = new(82f, 48f);
            renderer.method_523(Textures.Halves.Base, new(-1f, -1f), offset, 0f);
            renderer.method_523(Textures.Halves.EngravingFlashAnimation[0], new(-1f, -1f), offset, 0f);
            // quicksilver
            renderer.method_530(class_238.field_1989.field_90.field_255.field_293, halvesInputHex, 0);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_294, halvesInputHex, Vector2.Zero);
            // bowls
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, halvesMetal1Hex, Vector2.Zero);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_291, halvesMetal1Hex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, halvesMetal2Hex, Vector2.Zero);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_291, halvesMetal2Hex, Vector2.Zero);
        });

        QApi.AddPartType(Quake, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            float time = editor.method_504();
            Vector2 offset = new(41f, 49f);
            renderer.method_523(Textures.Quake.Base, Vector2.Zero, offset, 0f);
            if (pss.field_2743 && time < 0.75f)
            {
                double x = (10.6666667 * time) % 4.0;
                // "I want a sinusoid!"
                // "We have sinusoid at home"
                // Sinusoid at home:
                float shakeX = (float)(-Math.Pow(x, 5) + 10 * Math.Pow(x, 4) - 28 * Math.Pow(x, 3) + 8 * Math.Pow(x, 2) + 32 * x) / 7;
                renderer.method_529(Textures.Quake.BowlShaking, quakeBowlHex, new Vector2(shakeX, 0));
            }
            else
            {
                renderer.method_529(Textures.Quake.Bowl, quakeBowlHex, Vector2.Zero);
            }
        });

        QApi.AddPartType(Sump, static (part, pos, editor, renderer) =>
        {
            Brimstone.API.GetRenderingHelpers(part, pos, editor, out PartSimState pss, out class_236 partDataWrapper, out float time);

            // A dictionary that acts like the original object, and also allow extra data to be added,
            // It also magically remembers data put in it, even across multiple instantiations.
            DynamicData dyn_pss = new(pss);
            object stateOb = dyn_pss.Get("state");
            SumpState state = new();
            if (stateOb is not null)
            {
                // if there was something there, use that
                state = (SumpState)stateOb;
            }
            else
            {
                // if there isn't, set it to a default value
                dyn_pss.Set("state", state);
            }
            // unknown class object
            Vector2 offset = new(41f, 48f);
            renderer.method_523(Textures.GlyphCommon.DoubleBase, new(-1f, -1f), offset, 0f);
            // quicksilver
            renderer.method_528(class_238.field_1989.field_90.field_255.field_293, sumpInputHex, Vector2.Zero);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_294, sumpInputHex, Vector2.Zero);

            // input ring
            if (state.DrainFlash)
            {
                int flashFrame = (int)(8 * time);
                flashFrame = flashFrame > 7 ? 7 : flashFrame;
                renderer.method_529(Textures.Sump.DrainFlashAnimation[flashFrame], sumpInputHex, Vector2.Zero);
            }

            Brimstone.API.DrawIris(renderer, partDataWrapper, sumpOutputHex, time, Textures.Irises.Quicksilver, Brimstone.API.ConvertToMaybe(pss.field_2743 ? Brimstone.API.VanillaAtoms.quicksilver : null));
        });

        QApi.AddPartType(Remission, static (part, pos, editor, renderer) =>
        {
            Brimstone.API.GetRenderingHelpers(part, pos, editor, out PartSimState pss, out class_236 partDataWrapper, out float time);

            DynamicData data = new(pss);
            object stateOb = data.Get("state");
            RemissionState state = (stateOb is null) ? new RemissionState() : (RemissionState)stateOb;
            Vector2 pivot = new(41f, 120f);
            Vector2 offset = new(-1f, -1f);
            renderer.method_523(class_238.field_1989.field_90.field_228.field_265, offset, pivot, 0f);
            renderer.method_523(Textures.Remission.Arrow, offset, pivot, 0f);
            Texture leadSymbol = class_238.field_1989.field_90.field_257.field_363;
            int frame = 0;
            bool beforeIrisCloses = false;
            Molecule mol = null;
            if (pss.field_2743)
            {
                mol = Molecule.method_1121(pss.field_2744[0]);
            }

            if (state.irisState == 3)
            {
                // falling and closing
                frame = class_162.method_404((int)(class_162.method_411(-1f, 1f, time) * 16), 0, 15);
                beforeIrisCloses = time <= 0.5;
            }
            else if (state.irisState == 2)
            {
                // closed
                frame = 15;
            }
            else if (state.irisState == 1)
            {
                // reopening
                frame = class_162.method_404((int)(class_162.method_411(1f, -1f, time) * 16), 0, 15);
            }

            // input
            foreach (HexIndex inputHex in new HexIndex[] { remissionInput1Hex, remissionInput2Hex })
            {
                renderer.method_528(class_238.field_1989.field_90.field_255.field_293, inputHex, Vector2.Zero);
                class_135.method_272(leadSymbol, (class_187.field_1742.method_491(inputHex, Vector2.Zero).Rotated(partDataWrapper.field_1985) + partDataWrapper.field_1984 - (leadSymbol.field_2056.ToVector2() / 2)).Rounded());

                Vector2 molOffset = partDataWrapper.field_1984 + Brimstone.API.HexIndexToVector2(inputHex).Rotated(partDataWrapper.field_1985);
                if (pss.field_2743 && !beforeIrisCloses)
                {
                    Editor.method_925(mol, molOffset, new(0, 0), 0f, 1f, 1f - time, 1f, false, null);
                }
                // todo: create custom iris with red x on it.
                renderer.method_529(class_238.field_1989.field_90.field_246[frame], inputHex, Vector2.Zero);
                if (pss.field_2743 && beforeIrisCloses)
                {
                    Editor.method_925(mol, molOffset, new(0, 0), 0f, 1f, 1f - time, 1f, false, null);
                }
            }
            // bowl
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, remissionBowlHex, Vector2.Zero);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_291, remissionBowlHex, Vector2.Zero);
            // iris
            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, remissionOutputHex, Vector2.Zero);

            Brimstone.API.DrawIris(renderer, partDataWrapper, remissionOutputHex, time, Brimstone.API.ConvertToMaybe(pss.field_2743 ? pss.field_2744[1] : null));
            renderer.method_521(Textures.Remission.Connectors, pivot);
        });

        QApi.AddPartType(Shearing, static (part, pos, editor, renderer) =>
        {
            Brimstone.API.GetRenderingHelpers(part, pos, editor, out PartSimState pss, out class_236 partDataWrapper, out float time);
            Vector2 offset = new(41f, 48f);

            renderer.method_523(Textures.GlyphCommon.DoubleBase, new(-1f, -1f), offset, 0f);
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, shearingBowlHex, Vector2.Zero);
            renderer.method_529(Textures.Shearing.SednumLine, osmosisMetalHex, Vector2.Zero);

            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, shearingOutputHex, Vector2.Zero);
        });

        QApi.AddPartType(Osmosis, static (part, pos, editor, renderer) =>
        {
            Vector2 offset = new(41f, 48f);
            renderer.method_523(Textures.GlyphCommon.DoubleBase, new(-1f, -1f), offset, 0);
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, osmosisMetalHex, Vector2.Zero);
            renderer.method_529(Textures.GlyphSymbols.OsmiumDemote, osmosisMetalHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, osmosisQuickcopperHex, Vector2.Zero);
            renderer.method_529(Textures.GlyphSymbols.QuickcopperPromote, osmosisQuickcopperHex, Vector2.Zero);
        });
        #endregion

        #region Part Behavior
        QApi.RunDuringCycle(static (sim, part, pss, first) =>
        {
            SolutionEditorBase seb = sim.field_3818;
            List<Part> parts = seb.method_502().field_3919;
            PartType type = part.method_1159();
            if (type == Halves)
            {
                HexIndex bowl1 = part.method_1184(halvesMetal1Hex);
                HexIndex bowl2 = part.method_1184(halvesMetal2Hex);

                bool quicksilverExists = false;

                bool isQuicksilverSoria = false;
                bool isQuicksilverRavari = false;

                AtomType rejectionResult = null;

                if (sim.FindAtomRelative(part, halvesInputHex).method_99(out AtomReference quicksilver))
                {
                    // Is the quicksilver singular and not held
                    quicksilverExists = quicksilver.field_2280 == Brimstone.API.VanillaAtoms.quicksilver && !quicksilver.field_2281 && !quicksilver.field_2282;
                }
                else if (Wheel.MaybeFindSoriaWheelAtom(sim, part, halvesInputHex).method_99(out quicksilver))
                {
                    if (API.QuicksilverProjectionBehavior(quicksilver.field_2280, -2).method_99(out rejectionResult))
                    {
                        quicksilverExists = true;
                        isQuicksilverSoria = true;
                    }
                }
                else if (ReductiveMetallurgyImports.findRavariAtom(sim, part, halvesInputHex).method_99(out quicksilver))
                {
                    if (ReductiveMetallurgyImports.TryRejectAtom(quicksilver.field_2280).method_99(out rejectionResult))
                    {
                        quicksilverExists = true;
                        isQuicksilverRavari = true;
                    }
                    else if (API.OsmosisDictionary.TryGetValue(quicksilver.field_2280, out OsmosisOutput temp1)
                        && API.OsmosisDictionary.TryGetValue(temp1.HalfDemotion, out OsmosisOutput temp2)
                        && temp1.Predicate(sim, part)
                        && temp2.Predicate(sim, part))
                    {
                        rejectionResult = temp2.HalfDemotion;
                        quicksilverExists = true;
                        isQuicksilverRavari = true;
                    }
                }

                if (!quicksilverExists)
                {
                    return;
                }

                bool metal1Exists = false;
                bool metal1IsSoria = false;

                if (sim.FindAtom(bowl1).method_99(out AtomReference metal1))
                {
                    metal1Exists = true;
                }
                else if (Wheel.MaybeFindSoriaWheelAtom(sim, bowl1).method_99(out metal1))
                {
                    metal1Exists = true;
                    metal1IsSoria = true;
                }
                else if (ReductiveMetallurgyImports.findRavariAtom(sim, part, halvesMetal1Hex).method_99(out metal1))
                {
                    metal1Exists = true;
                }

                if (!metal1Exists)
                {
                    return;
                }

                bool metal2Exists = false;
                bool metal2IsSoria = false;

                if (sim.FindAtom(bowl2).method_99(out AtomReference metal2))
                {
                    metal2Exists = true;
                }
                else if (Wheel.MaybeFindSoriaWheelAtom(sim, bowl2).method_99(out metal2))
                {
                    metal2Exists = true;
                    metal2IsSoria = true;
                }
                else if (ReductiveMetallurgyImports.findRavariAtom(sim, part, halvesMetal2Hex).method_99(out metal2))
                {
                    metal2Exists = true;
                }

                if (!metal2Exists)
                {
                    return;
                }


                // Are they valid atoms
                if (metal1IsSoria ? !API.QuicksilverProjectionBehavior(metal1.field_2280, 1).method_99(out AtomType hp1) : // Soria's Wheel
                !API.HalvesDictionary.TryGetValue(metal1.field_2280, out hp1)) // override
                {
                    return;
                }
                if (metal2IsSoria ? !API.QuicksilverProjectionBehavior(metal2.field_2280, 1).method_99(out AtomType hp2) : // Soria's Wheel
                !API.HalvesDictionary.TryGetValue(metal2.field_2280, out hp2))
                {
                    return;
                }

                if (isQuicksilverSoria)
                {
                    Brimstone.API.ChangeAtom(quicksilver, rejectionResult);
                    Wheel.DrawSoriaFlash(seb, part, halvesInputHex);
                    quicksilver.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, quicksilver.field_2280, class_238.field_1989.field_81.field_614, 30f);
                }
                else if (isQuicksilverRavari)
                {
                    Brimstone.API.ChangeAtom(quicksilver, rejectionResult);
                    ReductiveMetallurgyImports.drawRavariFlash(seb, part, halvesInputHex);
                    quicksilver.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, quicksilver.field_2280, class_238.field_1989.field_81.field_614, 30f);
                }
                else
                {
                    // Delete the quicksilver
                    Brimstone.API.RemoveAtom(quicksilver);
                    // Play deletion animation
                    Brimstone.API.DrawFallingAtom(seb, quicksilver);
                }
                // Promote the metals
                Brimstone.API.ChangeAtom(metal1, hp1);
                Brimstone.API.ChangeAtom(metal2, hp2);
                // Play promotion 
                seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(bowl1), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
                metal1.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal1.field_2280, class_238.field_1989.field_81.field_614, 30f);
                seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(bowl2), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
                metal2.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal2.field_2280, class_238.field_1989.field_81.field_614, 30f);
                //play engraving flash
                float theta = part.method_1163().ToRadians();
                Vector2 offset = class_187.field_1742.method_492(part.method_1161()) + new Vector2(20f, 35f).Rotated(theta);
                seb.field_3935.Add(new class_228(seb, (enum_7)1, offset, Textures.Halves.EngravingFlashAnimation, 30f, Vector2.Zero, theta));

                // Play custom sound
                Brimstone.API.PlaySound(sim, Sounds.Halves);
            }
            else if (type == Quake)
            {
                if (!first)
                {
                    return;
                }
                HexIndex bowl = part.method_1184(quakeBowlHex);
                Molecule moleculeAboveBowl = null;
                foreach (Molecule molecule in sim.field_3823)
                {
                    if (molecule.method_1100().Keys.Contains(bowl))
                    {
                        moleculeAboveBowl = molecule;
                        break;
                    }
                }
                if (moleculeAboveBowl is not null)
                {

                    foreach (HexIndex hex in moleculeAboveBowl.method_1100().Keys)
                    {
                        sim.FindAtom(hex).method_99(out AtomReference atom);
                        if (atom.field_2282)
                        {
                            // Molecule can't be gripped
                            return;
                        }
                    }

                    List<HexIndexPair> removals = new();
                    List<HexIndexPair> additions = new();
                    List<HexIndexPair> resists = new();
                    // for each atom in a molecule
                    foreach (KeyValuePair<HexIndex, Atom> entry in moleculeAboveBowl.method_1100())
                    {
                        if (entry.Value.field_2275 == Atoms.Sednum)
                        {
                            foreach (HexIndex offset in HexIndex.AdjacentOffsets)
                            {
                                HexIndex sednumPos = entry.Key;
                                HexIndex neighborPos = sednumPos + offset;
                                if (moleculeAboveBowl.method_1100().TryGetValue(neighborPos, out Atom neighborAtom) && neighborAtom.field_2275 == Atoms.Zinc)
                                {
                                    // Sednum vs. Zinc is tie
                                    continue;
                                }

                                bool willBeRegularBonded = simBonders.Contains(new HexIndexPair(sednumPos, neighborPos)) || simBonders.Contains(new HexIndexPair(neighborPos, sednumPos));
                                enum_126 bondType = Brimstone.API.FindBondType(moleculeAboveBowl, sednumPos, neighborPos);

                                if (bondType is not enum_126.None and not (enum_126.Prisma0 | enum_126.Prisma1 | enum_126.Prisma2))
                                {
                                    if ((bondType & enum_126.Standard) == enum_126.Standard && willBeRegularBonded)
                                    {
                                        resists.Add(new(sednumPos, neighborPos));
                                    }
                                    else
                                    {
                                        removals.Add(new(sednumPos, neighborPos));
                                    }
                                }
                            }
                        }
                        else if (entry.Value.field_2275 == Atoms.Zinc)
                        {
                            foreach (HexIndex offset in HexIndex.AdjacentOffsets)
                            {
                                HexIndex zincPos = entry.Key;
                                HexIndex neighborPos = zincPos + offset;
                                if (!Brimstone.API.FindMolecule(sim, neighborPos).method_99(out Molecule otherMolecule))
                                {
                                    // no molecule adjacent.
                                    continue;
                                }
                                Atom neighbor = otherMolecule.method_1100()[neighborPos];
                                if (neighbor.field_2275 == Atoms.Sednum)
                                {
                                    // Sednum vs. Zinc is tie
                                    continue;
                                }
                                if (moleculeAboveBowl != otherMolecule || Brimstone.API.FindBondType(moleculeAboveBowl, zincPos, neighborPos) == enum_126.None)
                                {
                                    additions.Add(new(zincPos, neighborPos));
                                }
                            }
                        }
                    }

                    // only active if not idempotent
                    if (removals.Any() || additions.Any())
                    {
                        foreach (HexIndexPair pair in resists.Distinct())
                        {
                            Vector2 midpoint = class_162.method_413(class_187.field_1742.method_492(pair.Left), class_187.field_1742.method_492(pair.Right), 0.5f);
                            float angle = class_187.field_1742.method_492(pair.Right - pair.Left).Angle();
                            seb.field_3936.Add(new class_228(seb, (enum_7)1, midpoint, Textures.Quake.UnbondResistedAnimation, 75f, new Vector2(1.5f, -5f), angle));
                        }
                        foreach (HexIndexPair pair in additions.Distinct())
                        {
                            if (!Brimstone.API.FindMolecule(sim, pair.Left).method_99(out Molecule L) || !Brimstone.API.FindMolecule(sim, pair.Right).method_99(out Molecule R))
                            {
                                continue;
                            }
                            Brimstone.API.JoinMolecules(sim, L, R, out moleculeAboveBowl);
                            Brimstone.API.AddBond(sim, moleculeAboveBowl, pair.Left, pair.Right, enum_126.Standard);
                        }

                        foreach (HexIndexPair pair in removals.Distinct())
                        {
                            Brimstone.API.RemoveBonds(sim, moleculeAboveBowl, pair.Left, pair.Right);
                        }
                        Brimstone.API.ForceRecomputeBonds(moleculeAboveBowl);

                        pss.field_2743 = true;
                        // play a buzzing sound
                        Brimstone.API.PlaySound(sim, Sounds.Quake);
                    }
                }
            }
            else if (type == Sump)
            {
                DynamicData dyn_pss = new(pss);
                object stateOb = dyn_pss.Get("state");
                SumpState state;
                if (stateOb is not null)
                {
                    state = (SumpState)stateOb;
                }
                else
                {
                    state = new();
                }
                if (first)
                {
                    bool quicksilverExists = false;
                    bool quicksilverIsSoria = false;
                    if (sim.FindAtomRelative(part, sumpInputHex).method_99(out AtomReference quicksilver))
                    {
                        quicksilverExists = !quicksilver.field_2281 && !quicksilver.field_2282;
                    }
                    else if (Wheel.MaybeFindSoriaWheelAtom(sim, part, sumpInputHex).method_99(out quicksilver))
                    {
                        quicksilverExists = true;
                        quicksilverIsSoria = true;
                    }

                    if (!quicksilverExists || quicksilver.field_2280 != Brimstone.API.VanillaAtoms.quicksilver)
                    {
                        goto trySumpDrain;
                    }
                    if (quicksilverIsSoria)
                    {
                        if (state.QuicksilverCount == 5)
                        {
                            // Soria can't overfill a sump
                            goto trySumpDrain;
                        }
                        Brimstone.API.ChangeAtom(quicksilver, Atoms.Quicklime);
                        Wheel.DrawSoriaFlash(seb, part, sumpInputHex);
                        quicksilver.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, quicksilver.field_2280, class_238.field_1989.field_81.field_614, 30f);
                    }
                    else
                    {
                        // Delete the quicksilver
                        Brimstone.API.RemoveAtom(quicksilver);
                        // Play deletion animation
                        seb.field_3937.Add(new(seb, quicksilver.field_2278, Brimstone.API.VanillaAtoms.quicksilver));
                    }

                    if (state.QuicksilverCount < 5)
                    {
                        state.QuicksilverCount++;
                        Brimstone.API.PlaySound(sim, Sounds.SumpConsume);
                    }
                    else
                    {
                        state.DrainFlash = true;
                        Brimstone.API.PlaySound(sim, Sounds.SumpDiscard);
                    }
                trySumpDrain:
                    if (state.QuicksilverCount > 0 && !sim.FindAtomRelative(part, sumpOutputHex).method_1085())
                    {
                        state.QuicksilverEject = true;
                        state.QuicksilverCount--;
                        Brimstone.API.AddSmallCollider(sim, part, sumpOutputHex);
                        Brimstone.API.PlaySound(sim, Sounds.SumpEject);
                    }
                }
                else
                {
                    state.DrainFlash = false;
                    if (state.QuicksilverEject)
                    {
                        state.QuicksilverEject = false;
                        Brimstone.API.AddAtom(sim, part, sumpOutputHex, Brimstone.API.VanillaAtoms.quicksilver);
                    }
                }
                dyn_pss.Set("state", state);
            }
            else if (type == Remission)
            {
                DynamicData data = new(pss);
                object stateOb = data.Get("state");
                RemissionState state = (stateOb is null) ? new RemissionState() : (RemissionState)stateOb;
                if (first)
                {
                    if (!state.IsEmpty)
                    {
                        // still full
                        goto tryProject;
                    }
                    if (state.irisState != 0)
                    {
                        // empty, but has closed iris.
                        return;
                    }

                    if (sim.FindAtomRelative(part, remissionOutputHex).method_1085())
                    {
                        // blocked!
                        goto tryProject;
                    }

                    HexIndex input1 = part.method_1184(remissionInput1Hex);
                    HexIndex input2 = part.method_1184(remissionInput2Hex);

                    bool metal1Exists = false;
                    bool metal2Exists = false;

                    if (sim.FindAtom(input1).method_99(out AtomReference metal1))
                    {
                        metal1Exists = !metal1.field_2281 && !metal1.field_2282;
                    }
                    if (sim.FindAtom(input2).method_99(out AtomReference metal2))
                    {
                        metal2Exists = !metal2.field_2281 && !metal2.field_2282;
                    }

                    if (!metal1Exists
                        || !metal2Exists
                        || metal1.field_2280 != metal2.field_2280
                        || !API.metalToDoubledMetallicity.TryGetValue(metal1.field_2280, out int inputMetallicity)
                        || !metal1.field_2280.field_2297.method_99(out AtomType outputAtom))
                    {
                        goto tryProject;
                    }

                    state.halfProjections = inputMetallicity - 2;
                    // closing
                    if (!state.IsEmpty)
                    {
                        state.irisState = 3;
                    }

                    // Delete the metals
                    Brimstone.API.RemoveAtom(metal1);
                    Brimstone.API.RemoveAtom(metal2);

                    // Update Glyph
                    pss.field_2743 = true;
                    pss.field_2744 = new AtomType[2] { metal1.field_2280, outputAtom };
                    Brimstone.API.AddSmallCollider(sim, part, remissionOutputHex);
                    // Play sound
                    Brimstone.API.PlaySound(sim, Sounds.Remission);
                tryProject:
                    if (state.IsEmpty)
                    {
                        goto remissionEnd;
                    }
                    HexIndex bowl = part.method_1184(remissionBowlHex);
                    if (!sim.FindAtom(bowl).method_99(out AtomReference metalOnBowl) && !ReductiveMetallurgyImports.findRavariAtom(sim, part, remissionBowlHex).method_99(out metalOnBowl))
                    {
                        goto remissionEnd;
                    }
                    AtomType bowlType = metalOnBowl.field_2280;
                    if (state.CanFullPromote && bowlType.field_2297.method_99(out AtomType projectedMetal))
                    {
                        state.halfProjections -= 2;
                    }
                    else if (state.CanHalfPromote && API.HalvesDictionary.TryGetValue(bowlType, out projectedMetal))
                    {
                        state.halfProjections -= 1;
                    }
                    else if (state.CanHalfDemote && API.OsmosisDictionary.TryGetValue(bowlType, out OsmosisOutput output) && output.Predicate(sim, part))
                    {
                        projectedMetal = output.HalfDemotion;
                        state.halfProjections += 1;
                    }
                    else
                    {
                        goto remissionEnd;

                    }
                    if (state.IsEmpty)
                    {
                        state.irisState = state.irisState == 2 ? 1 : 0;
                    }
                    // Promote the metal
                    Brimstone.API.ChangeAtom(metalOnBowl, projectedMetal);
                    // Play promotion animations
                    seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(bowl), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
                    metalOnBowl.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metalOnBowl.field_2280, class_238.field_1989.field_81.field_614, 30f);
                }
                else
                {
                    if (state.irisState == 1)
                    {
                        state.irisState = 0;
                    }
                    else if (state.irisState == 3)
                    {
                        state.irisState = 2;
                    }
                    if (pss.field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, remissionOutputHex, pss.field_2744[1]);
                    }
                }
            remissionEnd:
                data.Set("state", state);
            }
            else if (type == Shearing)
            {

                if (sim.FindAtomRelative(part, shearingOutputHex).method_1085())
                {
                    // blocked!
                    return;
                }
                if (!sim.FindAtomRelative(part, shearingBowlHex).method_99(out AtomReference bowlAtom)
                && !Wheel.MaybeFindSoriaWheelAtom(sim, part, shearingBowlHex).method_99(out bowlAtom))
                {
                    return;
                }

                if (!API.ShearingDictionary.TryGetValue(bowlAtom.field_2280, out ShearingOutput result) || !result.Predicate(sim, part))
                {
                    return;
                }

                AtomType newBowlAtom = result.Split;
                AtomType outputAtom = result.Eject;
                bool madeQuickcopper = quickcopperRadioactive && (newBowlAtom == Atoms.Quickcopper || outputAtom == Atoms.Quickcopper);

                Brimstone.API.ChangeAtom(bowlAtom, newBowlAtom);
                bowlAtom.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, bowlAtom.field_2280, Textures.Shearing.Split, 30f);
                seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1161()) + new Vector2(0f, 0f), Textures.Shearing.BowlFlash, 30f, Vector2.Zero, 0f));
                Atom spawnedAtom = new(outputAtom)
                {
                    field_2276 = new class_168(seb, 0, (enum_132)1, outputAtom, Textures.Shearing.Spawn, 30f)
                };


                Molecule molecule = new();
                molecule.method_1105(spawnedAtom, part.method_1184(shearingOutputHex));
                sim.field_3823.Add(molecule);
                Brimstone.API.PlaySound(sim, madeQuickcopper ? Sounds.ShearingMakingQuickcopper : Sounds.Shearing);
            }
            else if (type == Osmosis)
            {
                HexIndex metalHex = part.method_1184(osmosisMetalHex);
                HexIndex quickcopperHex = part.method_1184(osmosisQuickcopperHex);

                bool quickcopperIsSoria = false;
                bool quickcopperExists = false;
                if (sim.FindAtom(quickcopperHex).method_99(out AtomReference quickcopper))
                {
                    quickcopperExists = true;
                }
                else if (Wheel.MaybeFindSoriaWheelAtom(sim, quickcopperHex).method_99(out quickcopper))
                {
                    quickcopperExists = true;
                    quickcopperIsSoria = true;
                }

                if (!quickcopperExists)
                {
                    return;
                }

                bool metalIsSoria = false;

                if (Wheel.MaybeFindSoriaWheelAtom(sim, metalHex).method_99(out AtomReference metal))
                {
                    metalIsSoria = true;
                }
                else if (!sim.FindAtom(metalHex).method_99(out metal)
                && !ReductiveMetallurgyImports.findRavariAtom(sim, part, osmosisMetalHex).method_99(out metal))
                {
                    return;
                }

                if (quickcopperIsSoria && metalIsSoria && first)
                {
                    // "Soria special" only allow one transfer per cycle.
                    return;
                }

                bool dilutionSpecial = false;
                AtomType output = null;
                if (quickcopperIsSoria && quickcopper.field_2280 == Atoms.Quickcopper && metal.field_2280 == Atoms.Quickcopper)
                {
                    output = Atoms.Quicklime;
                }
                else if (quickcopper.field_2280 == Atoms.Quicklime && metal.field_2280 == Brimstone.API.VanillaAtoms.quicksilver)
                {
                    dilutionSpecial = true;
                    output = Atoms.Quickcopper;
                }
                else if (API.OsmosisDictionary.TryGetValue(metal.field_2280, out OsmosisOutput temp) && temp.Predicate(sim, part))
                {
                    if (quickcopper.field_2280 == Atoms.Quickcopper)
                    {
                        output = temp.HalfDemotion;
                    }
                    else if (quickcopper.field_2280 == Atoms.Quicklime)
                    {
                        output = temp.HalfDemotion;
                        dilutionSpecial = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
                Brimstone.API.ChangeAtom(metal, output);
                Brimstone.API.ChangeAtom(quickcopper, dilutionSpecial ? Atoms.Quickcopper : Brimstone.API.VanillaAtoms.quicksilver);
                seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(metalHex), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
                metal.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal.field_2280, class_238.field_1989.field_81.field_614, 30f);
                seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(quickcopperHex), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
                quickcopper.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, quickcopper.field_2280, class_238.field_1989.field_81.field_614, 30f);
                Brimstone.API.PlaySound(sim, Sounds.Osmosis);
            }
            else if (type == class_191.field_1775) /* Triplex bonder */
            {
                foreach (class_222 bonder in type.field_1538)
                {
                    if (!sim.FindAtomRelative(part, bonder.field_1920).method_99(out AtomReference leftAtom) || !sim.FindAtomRelative(part, bonder.field_1921).method_99(out AtomReference rightAtom))
                    {
                        continue;
                    }
                    if ((leftAtom.field_2280 == Atoms.Vulcan && rightAtom.field_2280 == Brimstone.API.VanillaAtoms.fire) || (leftAtom.field_2280 == Brimstone.API.VanillaAtoms.fire && rightAtom.field_2280 == Atoms.Vulcan) || (leftAtom.field_2280 == Atoms.Vulcan && rightAtom.field_2280 == Atoms.Vulcan))
                    {
                        Brimstone.API.JoinMolecules(sim, leftAtom.field_2277, rightAtom.field_2277, out Molecule joined);
                        Brimstone.API.AddBond(sim, joined, part.method_1184(bonder.field_1920), part.method_1184(bonder.field_1921), bonder.field_1922);
                    }
                }
            }
            else if (type == class_191.field_1778) /* Projection */
            {
                int projectAmount = 0;
                bool isQuicksilverSoria = false;
                bool isQuicksilverRavari = false;

                AtomType rejectionResult = null;

                if (sim.FindAtomRelative(part, new(0, 0)).method_99(out AtomReference quickcopper))
                {
                    if (!quickcopper.field_2281 && !quickcopper.field_2282)
                    {
                        if (quickcopper.field_2280 == Atoms.Quickcopper)
                        {
                            projectAmount = 1;
                        }
                        else if (quickcopper.field_2280 == Brimstone.API.VanillaAtoms.quicksilver)
                        {
                            projectAmount = 2;
                        }
                    }
                }
                else if (Wheel.MaybeFindSoriaWheelAtom(sim, part, new(0, 0)).method_99(out quickcopper))
                {
                    if (quickcopper.field_2280 == Atoms.Quickcopper)
                    {
                        projectAmount = 1;
                        isQuicksilverSoria = true;
                    }
                    else if (quickcopper.field_2280 == Brimstone.API.VanillaAtoms.quicksilver)
                    {
                        projectAmount = 2;
                        isQuicksilverSoria = true;
                    }
                }
                else if (ReductiveMetallurgyImports.findRavariAtom(sim, part, new(0, 0)).method_99(out quickcopper))
                {
                    if (ReductiveMetallurgyImports.TryRejectAtom(quickcopper.field_2280).method_99(out rejectionResult))
                    {
                        projectAmount = 2;
                        isQuicksilverRavari = true;
                    }
                    else if (API.OsmosisDictionary.TryGetValue(quickcopper.field_2280, out OsmosisOutput temp1)
                        && API.OsmosisDictionary.TryGetValue(temp1.HalfDemotion, out OsmosisOutput temp2)
                        && temp1.Predicate(sim, part)
                        && temp2.Predicate(sim, part))
                    {
                        rejectionResult = temp2.HalfDemotion;
                        projectAmount = 2;
                        isQuicksilverRavari = true;
                    }
                }

                if (projectAmount == 0)
                {
                    return;
                }

                bool metalExists = false;
                bool isMetalSoria = false;

                if (sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference metal))
                {
                    if (projectAmount == 2 && !isQuicksilverSoria)
                    {
                        // already handled by vanilla or RM
                        return;
                    }
                    metalExists = true;
                }
                else if (Wheel.MaybeFindSoriaWheelAtom(sim, part, new(1, 0)).method_99(out metal))
                {
                    metalExists = true;
                    isMetalSoria = true;
                }
                else if (ReductiveMetallurgyImports.findRavariAtom(sim, part, new(1, 0)).method_99(out metal))
                {
                    if (projectAmount == 2 && !isQuicksilverSoria)
                    {
                        // already handled by RM
                        return;
                    }
                    metalExists = true;
                }

                if (!metalExists)
                {
                    return;
                }

                AtomType projection = null;
                if (isMetalSoria)
                {
                    if (!API.QuicksilverProjectionBehavior(metal.field_2280, projectAmount).method_99(out projection))
                    {
                        return;
                    }
                }
                else if (projectAmount == 1)
                {
                    if (!API.HalvesDictionary.TryGetValue(metal.field_2280, out projection))
                    {
                        return;
                    }
                }
                else if (projectAmount == 2)
                {
                    if (!metal.field_2280.field_2297.method_99(out projection))
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (isQuicksilverSoria)
                {
                    Brimstone.API.ChangeAtom(quickcopper, Atoms.Quicklime);
                    Wheel.DrawSoriaFlash(seb, part, new(0, 0));
                    quickcopper.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, quickcopper.field_2280, class_238.field_1989.field_81.field_614, 30f);
                }
                else if (isQuicksilverRavari)
                {
                    Brimstone.API.ChangeAtom(quickcopper, rejectionResult);
                    quickcopper.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, quickcopper.field_2280, class_238.field_1989.field_81.field_614, 30f);
                    ReductiveMetallurgyImports.drawRavariFlash(seb, part, new(0, 0));
                }
                else
                {
                    // quickcopper falling
                    Brimstone.API.RemoveAtom(quickcopper);
                    Brimstone.API.DrawFallingAtom(seb, quickcopper);
                }

                // metal promoting
                Brimstone.API.ChangeAtom(metal, projection);
                metal.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal.field_2280, class_238.field_1989.field_81.field_614, 30f);
                // glyph animation
                Vector2 param_5378 = class_187.field_1742.method_492(part.method_1161() + new HexIndex(1, 0).Rotated(part.method_1163()));
                seb.field_3935.Add(new class_228(seb, (enum_7)1, param_5378, class_238.field_1989.field_90.field_256, 30f, Vector2.Zero, part.method_1163().ToRadians()));
                Brimstone.API.PlaySound(sim, class_238.field_1991.field_1844);
            }
            else if (type == class_191.field_1779) /* Purification */
            {
                if (first && pss.field_2743 && pss.field_2744[0] == Atoms.Beryl)
                {
                    // Switch-a-roo
                    pss.field_2744 = new AtomType[1] { Atoms.PurificationBeryl };
                }
            }
        });
        #endregion
    }
}
