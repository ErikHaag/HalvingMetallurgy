using MonoMod.Utils;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PartType = class_139;
using Texture = class_256;

namespace HalvingMetallurgy;

// Borrowed from Reductive Metallurgy
public static class Wheel
{
    public static bool quickcopperRadioactive = true;

    const string SoriaStateString = "HalvingMetallurgy_SoriaWheelAtoms";
    const float sixtyDegrees = (float)Math.PI / 3f;

    static Molecule SoriaMolecule()
    {
        Molecule molecule = new();
        molecule.method_1105(new(Brimstone.API.VanillaAtoms["quicksilver"]), new HexIndex(0, 1));
        molecule.method_1105(new Atom(Atoms.Quickcopper), new HexIndex(1, 0));
        molecule.method_1105(new Atom(Brimstone.API.VanillaAtoms["quicksilver"]), new HexIndex(1, -1));
        molecule.method_1105(new Atom(Atoms.Quickcopper), new HexIndex(0, -1));
        molecule.method_1105(new Atom(Brimstone.API.VanillaAtoms["quicksilver"]), new HexIndex(-1, 0));
        molecule.method_1105(new Atom(Atoms.Quickcopper), new HexIndex(-1, 1));
        return molecule;
    }

    public static PartType Soria;

    public static void LoadWheel()
    {
        Soria = new() {
            /*ID*/
            field_1528 = "halving-metallurgy-soria",
            /*Name*/
            field_1529 = class_134.method_253("Soria's Wheel", string.Empty),
            /*Desc*/
            field_1530 = class_134.method_253("By using Soria's wheel with various metal glyphs, metals can be promoted and demoted in full or half increments.", string.Empty),
            /*Cost*/
            field_1531 = 30,
            /*Type*/
            field_1532 = (enum_2)1,
            /*Programmable?*/
            field_1533 = true,
            /*Force-rotatable*/
            field_1536 = true,
            /*Berlo Atoms*/
            field_1544 = new Dictionary<HexIndex, AtomType>(),
            /*Icon*/
            field_1547 = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/soria_icon"),
            /*Hover Icon*/
            field_1548 = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/soria_icon_hover"),
            /*Only One Allowed?*/
            field_1552 = true,
            CustomPermissionCheck = perms => perms.Contains(HalvingMetallurgy.SoriaPermission)
        };
        foreach (HexIndex hex in HexIndex.AdjacentOffsets)
            Soria.field_1544.Add(hex, Brimstone.API.VanillaAtoms["quicksilver"]);


        QApi.AddPartTypeToPanel(Soria, HalvingMetallurgy.ReductiveMetallurgyLoaded ? ReductiveMetallurgy.Wheel.Ravari : class_191.field_1771);
        QApi.AddPartType(Soria, DrawSoriaWheel);
    }

    private static void SetSoriaWheelData<T>(PartSimState state, string field, T data) => new DynamicData(state).Set(field, data);
    private static T GetSoriaWheelData<T>(PartSimState state, string field, T initial)
    {
        var data = new DynamicData(state).Get(field);
        if (data == null)
        {
            SetSoriaWheelData(state, field, initial);
            return initial;
        }
        else
        {
            return (T)data;
        }
    }

    public static void DrawSoriaAtoms(SolutionEditorBase seb_self, Part part, Vector2 pos, bool active = false)
    {
        if (part.method_1159() != Soria)
            return;
        PartSimState partSimState = seb_self.method_507().method_481(part);

        class_236 class236 = seb_self.method_1989(part, pos);
        Molecule molecule = GetSoriaWheelAtoms(partSimState);
        if (active && quickcopperRadioactive)
        {
            int frame = (int)(new struct_27(Time.Now().Ticks).method_603() * 30f) & 0x3f;
            foreach (KeyValuePair<HexIndex, Atom> atoms in molecule.method_1100().Where((p) => p.Value.field_2275 == Atoms.Quickcopper))
            {
                Vector2 position = class236.field_1984 + class_187.field_1742.method_492(atoms.Key).Rotated(class236.field_1985);
                class_135.method_272(Atoms.quickcopperAnimation[frame], position - new Vector2(60, 60));
            }
        }
        Editor.method_925(molecule, class236.field_1984, new HexIndex(0, 0), class236.field_1985, 1f, 1f, 1f, false, seb_self);
    }

    public static Texture[] SoriaFlashAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/soria_flash.array", "soria_flash", 10);

    public static void DrawSoriaFlash(SolutionEditorBase seb, Part part, HexIndex hex)
    {
        DrawSoriaFlash(seb, part.method_1184(hex));
    }

    public static void DrawSoriaFlash(SolutionEditorBase seb, HexIndex hex)
    {
        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(hex), SoriaFlashAnimation, 30f, Vector2.Zero, 0f));
    }

    private static Molecule GetSoriaWheelAtoms(PartSimState state) => GetSoriaWheelData(state, SoriaStateString, SoriaMolecule());

    static void DrawSoriaWheel(Part part, Vector2 pos, SolutionEditorBase editor, class_195 renderer)
    {
        // draw atoms, if the simulation is stopped - otherwise, the running simulation will draw them
        if (editor.method_503() == enum_128.Stopped)
        {
            DrawSoriaAtoms(editor, part, pos);
        }

        // draw arm stubs
        class_236 class236 = editor.method_1989(part, pos);
        typeof(SolutionEditorBase).GetMethod("method_2005", BindingFlags.NonPublic | BindingFlags.Static).Invoke(editor, new object[] { part.method_1165(), class_191.field_1767.field_1534, class236 });

        // draw cages
        PartSimState partSimState = editor.method_507().method_481(part);
        for (int i = 0; i < 6; i++)
        {
            float radians = renderer.field_1798 + (i * sixtyDegrees);
            Vector2 vector2_9 = renderer.field_1797 + class_187.field_1742.method_492(new HexIndex(1, 0)).Rotated(radians);
            typeof(SolutionEditorBase).GetMethod("method_2003", BindingFlags.NonPublic | BindingFlags.Static).Invoke(editor, new object[] { class_238.field_1989.field_90.field_232, vector2_9, new Vector2(39f, 33f), radians });
        }
    }

    public static Maybe<AtomReference> MaybeFindSoriaWheelAtom(Sim sim_self, Part part, HexIndex offset) => MaybeFindSoriaWheelAtom(sim_self, part.method_1184(offset));

    public static Maybe<AtomReference> MaybeFindSoriaWheelAtom(Sim sim_self, HexIndex hex)
    {
        var SEB = sim_self.field_3818;
        var solution = SEB.method_502();
        var partList = solution.field_3919;
        var partSimStates = sim_self.field_3821;

        foreach (var soria in partList.Where(x => x.method_1159() == Soria))
        {
            var partSimState = partSimStates[soria];
            Molecule soriaAtoms = GetSoriaWheelAtoms(partSimState);
            var hexIndex = partSimState.field_2724;
            var rotation = partSimState.field_2726;
            var hexKey = (hex - hexIndex).Rotated(rotation.Negative());

            if (soriaAtoms.method_1100().TryGetValue(hexKey, out Atom atom))
            {
                return new AtomReference(soriaAtoms, hexKey, atom.field_2275, atom, true);
            }
        }
        return struct_18.field_1431;
    }

}
