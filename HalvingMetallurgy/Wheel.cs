//using HalvingMetallurgy.Utilities;
//using Mono.Cecil.Cil;
//using MonoMod.Cil;
//using MonoMod.Utils;
//using Quintessential;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace HalvingMetallurgy;

//// Borrowed from Reductive Metallurgy

//public static class Wheel
//{
//    const string SoriaStateString = "HalvingMetallurgy_SoriaWheelAtoms";
//    const float sixtyDegrees = (float)Math.PI / 3f;

//    static Molecule SoriaMolecule()
//    {
//        Molecule molecule = new();
//        molecule.method_1105(new(Brimstone.API.VanillaAtoms.quicksilver), new HexIndex(0, 1));
//        molecule.method_1105(new Atom(Atoms.Quickcopper), new HexIndex(1, 0));
//        molecule.method_1105(new Atom(Brimstone.API.VanillaAtoms.quicksilver), new HexIndex(1, -1));
//        molecule.method_1105(new Atom(Atoms.Quickcopper), new HexIndex(0, -1));
//        molecule.method_1105(new Atom(Brimstone.API.VanillaAtoms.quicksilver), new HexIndex(-1, 0));
//        molecule.method_1105(new Atom(Atoms.Quickcopper), new HexIndex(-1, 1));
//        return molecule;
//    }

//    public static PartType Soria;

//    public static void LoadWheel()
//    {
//        Soria = new()
//        {
//            /*ID*/
//            field_1528 = "halving-metallurgy-soria",
//            /*Name*/
//            field_1529 = class_134.method_253("Soria's Wheel", string.Empty),
//            /*Desc*/
//            field_1530 = class_134.method_253("By using Soria's wheel with various metal glyphs, metals can be promoted and demoted in full or half increments.", string.Empty),
//            /*Cost*/
//            field_1531 = 30,
//            /*Type*/
//            field_1532 = (enum_2)1,
//            /*Programmable?*/
//            field_1533 = true,
//            /*Force-rotatable*/
//            field_1536 = true,
//            /*Berlo Atoms*/
//            field_1544 = new Dictionary<HexIndex, AtomType>(),
//            /*Icon*/
//            field_1547 = Textures.Icons.Soria,
//            /*Hover Icon*/
//            field_1548 = Textures.Icons.SoriaHover,
//            /*Only One Allowed?*/
//            field_1552 = true,
//            CustomPermissionCheck = perms => perms.Contains(HalvingMetallurgy.SoriaPermission)
//        };
//        foreach (HexIndex hex in HexIndex.AdjacentOffsets)
//        {
//            Soria.field_1544.Add(hex, Brimstone.API.VanillaAtoms.quicksilver);
//        }

//        QApi.AddPartTypeToPanel(Soria, class_191.field_1771);
//        QApi.AddPartType(Soria, DrawSoriaWheel);
//    }

//    public static void LoadHooks()
//    {
//        IL.SolutionEditorBase.method_1984 += InjectDrawSoriaAtom;
//        On.SolutionEditorBase.method_1997 += DrawPartSelectionGlows;
//    }

//    public static void UnloadHooks()
//    {
//        IL.SolutionEditorBase.method_1984 -= InjectDrawSoriaAtom;
//        On.SolutionEditorBase.method_1997 -= DrawPartSelectionGlows;
//    }

//    internal static void InjectDrawSoriaAtom(ILContext context)
//    {
//        ILCursor cursor = new(context);
//        if (!cursor.TryGotoNext(MoveType.After,
//            instr => instr.MatchCallvirt("SolutionEditorBase", "method_2015")))
//        {
//            Logger.Log(HalvingMetallurgy.LogPrefix + "Failed to inject draw call (no method_2015 call)");
//            return;
//        }

//        if (!cursor.TryGotoNext(MoveType.After,
//            instr => instr.MatchEndfinally()))
//        {
//            Logger.Log(HalvingMetallurgy.LogPrefix + "Fail to inject draw call (no loop end)");
//            return;
//        }

//        cursor.Index++;
//        cursor.Emit(OpCodes.Ldarg_0);
//        cursor.Emit(OpCodes.Ldloc_0);
//        cursor.EmitDelegate<Action<SolutionEditorBase, SolutionEditorBase.class_423>>(static (self, uco) =>
//        {
//            if (self.method_503() != enum_128.Stopped)
//            {
//                var partList = self.method_502().field_3919;
//                foreach (Part soria in partList.Where(x => x.method_1159() == Soria))
//                {
//                    DrawSoriaAtoms(self, soria, uco.field_3959, true);
//                }
//            }
//        });
//    }

//    internal static void DrawPartSelectionGlows(On.SolutionEditorBase.orig_method_1997 orig, SolutionEditorBase self, Part param_5565, Vector2 param_5566, float param_5567)
//    {
//        if (param_5565.method_1159() == Soria)
//        {
//            DrawSelectionGlow(self, param_5565, param_5566, param_5567);
//        }
//        orig(self, param_5565, param_5566, param_5567);
//    }

//    public static void DrawSelectionGlow(SolutionEditorBase seb_self, Part part, Vector2 pos, float alpha)
//    {
//        var cageSelectGlowTexture = class_238.field_1989.field_97.field_367;
//        int armLength = 1; // part.method_1165()
//        class_236 class236 = seb_self.method_1989(part, pos);
//        Color color = Color.White.WithAlpha(alpha);

//        typeof(SolutionEditorBase).GetMethod("method_2006", BindingFlags.NonPublic | BindingFlags.Static).Invoke(seb_self, new object[] { armLength, class_191.field_1767.field_1534, class236, color });
//        MethodInfo cageDrawer = typeof(SolutionEditorBase).GetMethod("method_2016", BindingFlags.NonPublic | BindingFlags.Static);
//        float angle = 0;
//        for (int i = 0; i < 6; i++)
//        {
//            cageDrawer.Invoke(seb_self, new object[] { cageSelectGlowTexture, color, class236.field_1984, class236.field_1985 + angle });
//            angle += sixtyDegrees;
//        }
//    }

//    private static void SetSoriaWheelData<T>(PartSimState state, string field, T data) => new DynamicData(state).Set(field, data);
//    private static T GetSoriaWheelData<T>(PartSimState state, string field, T initial)
//    {
//        var data = new DynamicData(state).Get(field);
//        if (data == null)
//        {
//            SetSoriaWheelData(state, field, initial);
//            return initial;
//        }
//        else
//        {
//            return (T)data;
//        }
//    }

//    public static void DrawSoriaAtoms(SolutionEditorBase seb_self, Part part, Vector2 pos, bool active = false)
//    {
//        if (part.method_1159() != Soria)
//            return;
//        PartSimState partSimState = seb_self.method_507().method_481(part);

//        class_236 class236 = seb_self.method_1989(part, pos);
//        Molecule molecule = GetSoriaWheelAtoms(partSimState);
//        if (active && Atoms.quickcopperRadioactive)
//        {
//            int frame = (int)(new struct_27(Time.Now().Ticks).method_603() * 30f) & 0x3f;
//            foreach (KeyValuePair<HexIndex, Atom> atoms in molecule.method_1100().Where((p) => p.Value.field_2275 == Atoms.Quickcopper))
//            {
//                Vector2 position = class236.field_1984 + class_187.field_1742.method_492(atoms.Key).Rotated(class236.field_1985);
//                class_135.method_272(Textures.Atom.QuickcopperAnimation[frame], position - new Vector2(60, 60));
//            }
//        }
//        Editor.method_925(molecule, class236.field_1984, new HexIndex(0, 0), class236.field_1985, 1f, 1f, 1f, false, seb_self);
//    }

//    public static void DrawSoriaFlash(SolutionEditorBase seb, Part part, HexIndex hex)
//    {
//        DrawSoriaFlash(seb, part.method_1184(hex));
//    }

//    public static void DrawSoriaFlash(SolutionEditorBase seb, HexIndex hex)
//    {
//        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(hex), Textures.Soria.Flash, 30f, Vector2.Zero, 0f));
//    }

//    private static Molecule GetSoriaWheelAtoms(PartSimState state) => GetSoriaWheelData(state, SoriaStateString, SoriaMolecule());

//    static void DrawSoriaWheel(Part part, Vector2 pos, SolutionEditorBase editor, class_195 renderer)
//    {
//        // draw atoms, if the simulation is stopped - otherwise, the running simulation will draw them
//        if (editor.method_503() == enum_128.Stopped)
//        {
//            DrawSoriaAtoms(editor, part, pos);
//        }

//        // draw arm stubs
//        class_236 class236 = editor.method_1989(part, pos);
//        typeof(SolutionEditorBase).GetMethod("method_2005", BindingFlags.NonPublic | BindingFlags.Static).Invoke(editor, new object[] { part.method_1165(), class_191.field_1767.field_1534, class236 });

//        // draw cages
//        for (int i = 0; i < 6; i++)
//        {
//            float radians = renderer.field_1798 + (i * sixtyDegrees);
//            Vector2 vector2_9 = renderer.field_1797 + class_187.field_1742.method_492(new HexIndex(1, 0)).Rotated(radians);
//            typeof(SolutionEditorBase).GetMethod("method_2003", BindingFlags.NonPublic | BindingFlags.Static).Invoke(editor, new object[] { class_238.field_1989.field_90.field_232, vector2_9, new Vector2(39f, 33f), radians });
//        }
//    }

//    public static Maybe<AtomReference> MaybeFindSoriaWheelAtom(Sim sim_self, Part part, HexIndex offset) => MaybeFindSoriaWheelAtom(sim_self, part.method_1184(offset));

//    public static Maybe<AtomReference> MaybeFindSoriaWheelAtom(Sim sim_self, HexIndex hex)
//    {
//        var SEB = sim_self.field_3818;
//        var solution = SEB.method_502();
//        var partList = solution.field_3919;
//        var partSimStates = sim_self.field_3821;

//        foreach (var soria in partList.Where(x => x.method_1159() == Soria))
//        {
//            var partSimState = partSimStates[soria];
//            Molecule soriaAtoms = GetSoriaWheelAtoms(partSimState);
//            var hexIndex = partSimState.field_2724;
//            var rotation = partSimState.field_2726;
//            var hexKey = (hex - hexIndex).Rotated(rotation.Negative());

//            if (soriaAtoms.method_1100().TryGetValue(hexKey, out Atom atom))
//            {
//                return new AtomReference(soriaAtoms, hexKey, atom.field_2275, atom, true);
//            }
//        }
//        return struct_18.field_1431;
//    }

//}
