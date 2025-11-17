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
using Permissions = enum_149;
using Texture = class_256;

namespace HalvingMetallurgy;

internal struct HexIndexPair
{
    public HexIndexPair(HexIndex a, HexIndex b)
    {
        Q1 = a.Q;
        R1 = a.R;
        Q2 = b.Q;
        R2 = b.R;
    }
    public int Q1;
    public int R1;
    public int Q2;
    public int R2;
}

internal struct SumpState
{
    public SumpState()
    {
        state = 0;
    }

    private byte state;

    // bit manipulations!
    public byte quicksilverCount
    {
        readonly get {
            return (byte)(state & 7);
        }
        set {
            state &= 0xf8; // ~7
            state |= (byte)(value & 7);
        }
    }

    public bool quicksilverEject
    {
        readonly get {
            return (state & 0x08) == 0x08;
        }
        set {
            if (value)
            {
                state |= 0x08;
            }
            else
            {
                state &= 0xf7; // ~8
            }
        }
    }

    public bool drainFlash
    {
        readonly get {
            return (state & 0x10) == 0x10;
        }
        set {
            if (value)
            {
                state |= 0x10;
            }
            else
            {
                state &= 0xef; // ~16
            }
        }
    }

}

public static class Glyphs
{
    public static bool quickcopperRadioactive = true;

    #region Glyphs
    public static PartType Halves;
    public static PartType Quake;
    public static PartType Sump;
    public static PartType Remission;
    public static PartType Shearing;
    // Techinally reverse osmosis, but osmosis is shorter
    public static PartType Osmosis;
    #endregion

    #region Glyph Textures
    public static Texture halvesBase = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/halves_base");
    public static Texture halvesGlow = Brimstone.API.GetTexture("textures/select/erikhaag/HalvingMetallurgy/halves_glow");
    public static Texture halvesStroke = Brimstone.API.GetTexture("textures/select/erikhaag/HalvingMetallurgy/halves_stroke");
    public static Texture halvesIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/halves_icon");
    public static Texture halvesIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/halves_icon_hover");
    public static Texture[] halvesEngravingFlashAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/halves_engraving_flash.array", "halves_engraving", 6);
    public static Texture[] halvesBowlFlashAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/halves_bowl_flash.array", "halves_bowl", 10);

    public static Texture quakeBase = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_base");
    public static Texture quakeBowl = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_bowl");
    public static Texture quakeBowlShaking = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_bowl_shaking");
    public static Texture quakeIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/quake_icon");
    public static Texture quakeIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/quake_icon_hover");
    public static Texture[] quakeUnbondResistedAnimation = Brimstone.API.GetAnimation("textures/bonds/erikhaag/HalvingMetallurgy/unbond_resist.array", "unbond_resist", 22);

    public static Texture sumpBase = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/sump_base");
    public static Texture sumpIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/sump_icon");
    public static Texture sumpIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/sump_icon_hover");
    public static Texture[] quicksilverIrisAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/iris_full_quicksilver.array", "iris_full_quicksilver", 16);
    public static Texture[] sumpDrainFlashAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/sump_drain_flash.array", "sump_flash", 8);

    public static Texture remissionArrow = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/remission_arrow");
    public static Texture remissionConnectors = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/remission_connectors");
    public static Texture remissionIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/remission_icon");
    public static Texture remissionIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/remission_icon_hover");

    public static Texture shearingBase = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/shearing_base");
    public static Texture shearingBowl = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/shearing_bowl");
    public static Texture shearingIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/shearing_icon");
    public static Texture shearingIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/shearing_icon_hover");
    public static Texture[] shearingFlashAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/shearing_flash.array", "shearing_flash", 8);
    public static Texture[] shearingSplitAnimation = Brimstone.API.GetAnimation("textures/atoms/erikhaag/HalvingMetallurgy/split_effect.array", "split", 12);

    public static Texture osmosisBase = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/osmosis_base");
    public static Texture osmosisIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/osmosis_icon");
    public static Texture osmosisIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/osmosis_icon_hover");
    public static Texture osmiumDemoteSymbol = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/osmium_symbol");
    public static Texture quickcopperPromoteSymbol = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quickcopper_symbol");

    #endregion

    #region Sounds
    public static Sound halvesSound;
    public static Sound quakeSound;
    public static Sound shearingSound;
    public static Sound quickcopperSound;
    public static Sound osmosisSound;
    public static void LoadSounds()
    {
        halvesSound = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/halves").method_1087();
        quakeSound = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/quake").method_1087();
        shearingSound = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/shearing").method_1087();

        /* Yoinked and modified from https://en.wikipedia.org/wiki/File:Geiger_counter_sound_KCl.oga,
         * because trying to recreate a geiger counter is hard. */
        quickcopperSound = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/shearing_making_quickcopper").method_1087();
        osmosisSound = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/osmosis").method_1087();

        FieldInfo field = typeof(class_11).GetField("field_52", BindingFlags.Static | BindingFlags.NonPublic);
        Dictionary<string, float> volumeDictionary = (Dictionary<string, float>)field.GetValue(null);

        volumeDictionary.Add("halves", 0.5f);
        volumeDictionary.Add("quake", 0.3f);
        volumeDictionary.Add("shearing", 0.3f);
        volumeDictionary.Add("shearing_making_quickcopper", 0.2f);
        volumeDictionary.Add("osmosis", 0.5f);

        void Method_540(On.class_201.orig_method_540 orig, class_201 self)
        {
            orig(self);
            halvesSound.field_4062 = false;
            quakeSound.field_4062 = false;
            shearingSound.field_4062 = false;
            quickcopperSound.field_4062 = false;
            osmosisSound.field_4062 = false;
        }

        On.class_201.method_540 += Method_540;
    }
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

    public static bool ExtractionPresent(List<Part> parts, Part part, HexIndex[] neighborhood)
    {
        if (!HalvingMetallurgy.VacancyLoaded)
        {
            return false;
        }
        foreach (Part extraction in parts.Where(p => p.method_1159() == Vaca.CustomGlyphs.Extraction))
        {
            HexIndex test = extraction.method_1161();
            foreach (HexIndex neighbor in neighborhood)
            {
                if (test == part.method_1184(neighbor))
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion

    #region Code Modification

    internal static List<HexIndexPair> simBonders = new();

    private static Hook AtomSwapHook;

    internal static void LoadHooking()
    {
        Logger.Log(HalvingMetallurgy.LogPrefix + "Hooking!");
        IL.SolutionEditorBase.method_1984 += InjectDrawSoriaAtom;
        IL.SolutionEditorScreen.method_2097 += InjectSimStartup;
        On.Editor.method_927 += Atoms.OnAtomRender;
        AtomSwapHook = new(typeof(Sim).GetMethod("method_1832", BindingFlags.Instance | BindingFlags.NonPublic), AtomSwap);
    }

    internal static void UnloadHooks()
    {
        Logger.Log(HalvingMetallurgy.LogPrefix + "Removing Hooks!");
        IL.SolutionEditorBase.method_1984 -= InjectDrawSoriaAtom;
        IL.SolutionEditorScreen.method_2097 -= InjectSimStartup;
        On.Editor.method_927 -= Atoms.OnAtomRender;
        AtomSwapHook.Dispose();
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
        gremlin.EmitDelegate<Action<Sim>>(sim => {
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
        cursor.EmitDelegate<Action<SolutionEditorBase, SolutionEditorBase.class_423>>((self, uco) => {
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
        if (param_5369)
        {
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
        // Todo: Add a Brimstone function for this
        Halves = new() {
            field_1528 = "halving-metallurgy-halves", // ID
            field_1529 = class_134.method_253("Glyph of Halves", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of halves consumes an atom of quicksilver and half-promotes two metal atoms.", string.Empty), // Description
            field_1531 = 30, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = halvesGlow, // Shadow/glow
            field_1550 = halvesStroke, // Stroke/outline
            field_1547 = halvesIcon, // Panel icon
            field_1548 = halvesIconHover, // Hovered panel icon
            field_1540 = new HexIndex[]
            {
                halvesInputHex,
                halvesMetal1Hex,
                halvesMetal2Hex
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains(HalvingMetallurgy.HalvingPermission)
        };

        Quake = new() {
            field_1528 = "halving-metallurgy-quake",
            field_1529 = class_134.method_253("Glyph of Quake", string.Empty),
            field_1530 = class_134.method_253("The glyph of quake shakes a molecule vigorously, breaking all the weak sednum bonds.", string.Empty),
            field_1531 = 15,
            field_1539 = true,
            field_1549 = class_238.field_1989.field_97.field_382,
            field_1550 = class_238.field_1989.field_97.field_383,
            field_1547 = quakeIcon,
            field_1548 = quakeIconHover,
            field_1540 = new HexIndex[]
            {
                quakeBowlHex
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains(HalvingMetallurgy.QuakePermission)
        };

        Sump = new() {
            field_1528 = "halving-metallurgy-sump",
            field_1529 = class_134.method_253("Quicksilver Sump", string.Empty),
            field_1530 = class_134.method_253("The quicksilver sump can hold 5 quicksilver internally for storage, excess quicksilver is drained out of the engine.", string.Empty),
            field_1531 = 15,
            field_1539 = true,
            field_1549 = class_238.field_1989.field_97.field_374,
            field_1550 = class_238.field_1989.field_97.field_375,
            field_1547 = sumpIcon,
            field_1548 = sumpIconHover,
            field_1540 = new HexIndex[]
            {
                sumpInputHex,
                sumpOutputHex
            },
            field_1551 = Permissions.None,
            field_1552 = true, // only one is allowed.
            CustomPermissionCheck = perms => perms.Contains(HalvingMetallurgy.SumpPermission)
        };

        Remission = new() {
            field_1528 = "halving-metallurgy-remission", // ID
            field_1529 = class_134.method_253("Glyph of Remission", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of remission transmutes two metal atoms to their next higher form, assuming the metal in the bowl is able to handle the influx of quicksilver", string.Empty), // Description
            field_1531 = 30, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = class_238.field_1989.field_97.field_368, // Shadow/glow
            field_1550 = class_238.field_1989.field_97.field_369, // Stroke/outline
            field_1547 = remissionIcon, // Panel icon
            field_1548 = remissionIconHover, // Hovered panel icon
            field_1540 = new HexIndex[]
            {
                remissionInput1Hex,
                remissionInput2Hex,
                remissionBowlHex,
                remissionOutputHex
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains(HalvingMetallurgy.RemissionPermission)
        };

        Shearing = new() {
            field_1528 = "halving-metallurgy-shearing", // ID
            field_1529 = class_134.method_253("Glyph of Shearing", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of shearing cuts a metal in half, ejecting the leftovers", string.Empty), // Description
            field_1531 = 25, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
            field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
            field_1547 = shearingIcon, // Panel icon
            field_1548 = shearingIconHover, // Hovered panel icon
            field_1540 = new HexIndex[]
            {
                shearingBowlHex,
                shearingOutputHex
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains(HalvingMetallurgy.ShearingPermission)
        };

        Osmosis = new() {
            field_1528 = "halving-metallurgy-osmosis", // ID
            field_1529 = class_134.method_253("Glyph of Osmosis", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of osmosis half-demotes a metal to transmute an atom of quickcopper into quicksilver", string.Empty), // Description
            field_1531 = 25, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
            field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
            field_1547 = osmosisIcon, // Panel icon
            field_1548 = osmosisIconHover, // Hovered panel icon
            field_1540 = new HexIndex[]
            {
                shearingBowlHex,
                shearingOutputHex
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains(HalvingMetallurgy.OsmosisPermission)
        };

        #endregion

        QApi.AddPartTypeToPanel(Halves, false);
        QApi.AddPartTypeToPanel(Quake, false);
        QApi.AddPartTypeToPanel(Sump, false);
        QApi.AddPartTypeToPanel(Remission, false);
        QApi.AddPartTypeToPanel(Shearing, false);
        QApi.AddPartTypeToPanel(Osmosis, false);

        #region Glyph Renderers

        QApi.AddPartType(Halves, static (part, pos, editor, renderer) => {
            PartSimState pss = editor.method_507().method_481(part);
            float time = editor.method_504();
            int frame = 0;
            Vector2 offset = new(82f, 48f);
            if (pss.field_2743)
            {
                frame = (int)(11f * time);
                frame = frame >= 6 ? 10 - frame : frame;
            }
            renderer.method_523(halvesBase, new(-1f, -1f), offset, 0f);
            renderer.method_523(halvesEngravingFlashAnimation[frame], new(-1f, -1f), offset, 0f);
            // quicksilver
            renderer.method_530(class_238.field_1989.field_90.field_255.field_293, halvesInputHex, 0);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_294, halvesInputHex, Vector2.Zero);
            // bowls
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, halvesMetal1Hex, Vector2.Zero);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_291, halvesMetal1Hex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, halvesMetal2Hex, Vector2.Zero);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_291, halvesMetal2Hex, Vector2.Zero);
        });

        QApi.AddPartType(Quake, static (part, pos, editor, renderer) => {
            PartSimState pss = editor.method_507().method_481(part);
            float time = editor.method_504();
            Vector2 offset = new(41f, 49f);
            renderer.method_523(quakeBase, Vector2.Zero, offset, 0f);
            if (pss.field_2743 && time < 0.75f)
            {
                double x = (10.6666667 * time) % 4.0;
                // "I want a sinusoid!"
                // "We have sinusoid at home"
                // Sinusoid at home:
                float shakeX = (float)(-Math.Pow(x, 5) + 10 * Math.Pow(x, 4) - 28 * Math.Pow(x, 3) + 8 * Math.Pow(x, 2) + 32 * x) / 7;
                renderer.method_529(quakeBowlShaking, quakeBowlHex, new Vector2(shakeX, 0));
            }
            else
            {
                renderer.method_529(quakeBowl, quakeBowlHex, Vector2.Zero);
            }
        });

        QApi.AddPartType(Sump, static (part, pos, editor, renderer) => {
            PartSimState pss = editor.method_507().method_481(part);

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
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();
            Vector2 offset = new(41f, 48f);
            renderer.method_523(sumpBase, new(-1f, -1f), offset, 0f);
            // quicksilver
            renderer.method_530(class_238.field_1989.field_90.field_255.field_293, sumpInputHex, 0);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_294, sumpInputHex, Vector2.Zero);

            // input ring
            if (state.drainFlash)
            {
                int flashFrame = (int)(8 * time);
                flashFrame = flashFrame > 7 ? 7 : flashFrame;
                renderer.method_529(sumpDrainFlashAnimation[flashFrame], sumpInputHex, Vector2.Zero);
            }
            // output iris
            int irisFrame = 15;
            bool afterIrisOpens = false;

            Molecule risingQuicksilver = Molecule.method_1121(Brimstone.API.VanillaAtoms["quicksilver"]);
            // from SolutionEditorBase.method_1999 which was a private static method,
            // and I kept getting a null reference exception when trying the access modifier ignoring method.
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(sumpOutputHex).Rotated(uco.field_1985);

            if (state.quicksilverEject)
            {
                irisFrame = class_162.method_404((int)(class_162.method_411(1f, -1f, time) * 16f), 0, 15);
                afterIrisOpens = time > 0.5f;
                if (!afterIrisOpens)
                {
                    // show quicksilver rising behind iris
                    Editor.method_925(risingQuicksilver, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }
            renderer.method_529(quicksilverIrisAnimation[irisFrame], sumpOutputHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, sumpOutputHex, Vector2.Zero);
            if (state.quicksilverEject && afterIrisOpens)
            {
                // show quicksilver rising infront of iris
                Editor.method_925(risingQuicksilver, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        });

        QApi.AddPartType(Remission, static (part, pos, editor, renderer) => {
            PartSimState pss = editor.method_507().method_481(part);
            // unknown class object
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();
            Vector2 offset = new(41f, 120f);
            // input
            renderer.method_523(class_238.field_1989.field_90.field_228.field_265, new Vector2(-1f, -1f), offset, 0f);
            renderer.method_523(remissionArrow, new Vector2(-1f, -1f), offset, 0f);
            renderer.method_530(class_238.field_1989.field_90.field_255.field_293, remissionInput1Hex, 0f);
            renderer.method_530(class_238.field_1989.field_90.field_255.field_293, remissionInput2Hex, 0f);
            Texture leadSymbol = class_238.field_1989.field_90.field_257.field_363;
            class_135.method_272(leadSymbol, (class_187.field_1742.method_491(remissionInput1Hex, Vector2.Zero).Rotated(uco.field_1985) + uco.field_1984 - leadSymbol.field_2056.ToVector2() / 2).Rounded());
            class_135.method_272(leadSymbol, (class_187.field_1742.method_491(remissionInput2Hex, Vector2.Zero).Rotated(uco.field_1985) + uco.field_1984 - leadSymbol.field_2056.ToVector2() / 2).Rounded());
            // bowl
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, remissionBowlHex, Vector2.Zero);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_291, remissionBowlHex, Vector2.Zero);
            // iris
            renderer.method_528(class_238.field_1989.field_90.field_228.field_272, remissionOutputHex, Vector2.Zero);

            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingMetal = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(remissionOutputHex).Rotated(uco.field_1985);
            if (pss.field_2743)
            {
                irisFrame = class_162.method_404((int)(class_162.method_411(1f, -1f, time) * 16f), 0, 15);
                afterIrisOpens = time > 0.5f;
                risingMetal = Molecule.method_1121(pss.field_2744[0]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.method_925(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }
            renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], remissionOutputHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, remissionOutputHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
            renderer.method_521(remissionConnectors, offset);
        });

        QApi.AddPartType(Shearing, static (part, pos, editor, renderer) => {
            PartSimState pss = editor.method_507().method_481(part);
            // unknown class object
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();
            Vector2 offset = new(41f, 48f);

            renderer.method_523(shearingBase, new(-1f, -1f), offset, 0f);
            renderer.method_529(shearingBowl, shearingBowlHex, Vector2.Zero);

            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingMetal = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(shearingOutputHex).Rotated(uco.field_1985);
            if (pss.field_2743)
            {
                irisFrame = class_162.method_404((int)(class_162.method_411(1f, -1f, time) * 16f), 0, 15);
                afterIrisOpens = time > 0.5f;
                risingMetal = Molecule.method_1121(pss.field_2744[0]);
                if (!afterIrisOpens)
                {
                    // show quicksilver rising behind iris
                    Editor.method_925(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }
            renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], shearingOutputHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, shearingOutputHex, Vector2.Zero);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show quicksilver rising infront of iris
                Editor.method_925(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        });

        QApi.AddPartType(Osmosis, static (part, pos, editor, renderer) => {
            Vector2 offset = new(41f, 48f);
            renderer.method_523(osmosisBase, new(-1f, -1f), offset, 0);
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, osmosisMetalHex, Vector2.Zero);
            renderer.method_529(osmiumDemoteSymbol, osmosisMetalHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, osmosisQuickcopperHex, Vector2.Zero);
            renderer.method_529(quickcopperPromoteSymbol, osmosisQuickcopperHex, Vector2.Zero);
        });
        #endregion

        #region Part Behavior
        QApi.RunAfterCycle(static (sim, first) => {
            SolutionEditorBase seb = sim.field_3818;
            Dictionary<Part, PartSimState> pss = sim.field_3821;
            List<Part> parts = seb.method_502().field_3919;

            foreach (Part part in parts)
            {
                PartType type = part.method_1159();
                if (type == Halves)
                {
                    if (!first)
                    {
                        continue;
                    }

                    HexIndex bowl1 = part.method_1184(halvesMetal1Hex);
                    HexIndex bowl2 = part.method_1184(halvesMetal2Hex);

                    bool quicksilverExists = false;

                    bool isQuicksilverSoria = false;
                    bool isQuicksilverRavari = false;

                    AtomType rejectionResult = null;

                    if (sim.FindAtomRelative(part, halvesInputHex).method_99(out AtomReference quicksilver))
                    {
                        // Is the quicksilver singular and not held
                        quicksilverExists = quicksilver.field_2280 == Brimstone.API.VanillaAtoms["quicksilver"] && !quicksilver.field_2281 && !quicksilver.field_2282;
                    }
                    else if (Wheel.MaybeFindSoriaWheelAtom(sim, part, halvesInputHex).method_99(out quicksilver))
                    {
                        if (API.QuicksilverProjectionBehavior(quicksilver.field_2280, -2).method_99(out rejectionResult))
                        {
                            quicksilverExists = true;
                            isQuicksilverSoria = true;
                        }
                    }
                    else if (HalvingMetallurgy.ReductiveMetallurgyLoaded && ReductiveMetallurgy.Wheel.maybeFindRavariWheelAtom(sim, part, halvesInputHex).method_99(out quicksilver))
                    {
                        if (API.ChangeMetallicity(quicksilver.field_2280, -2, out rejectionResult, static i => i >= 2) != Brimstone.API.SuccessInfo.failure)
                        {
                            quicksilverExists = true;
                            isQuicksilverRavari = true;
                        }
                    }

                    if (!quicksilverExists)
                    {
                        continue;
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
                    else if (HalvingMetallurgy.ReductiveMetallurgyLoaded && ReductiveMetallurgy.Wheel.maybeFindRavariWheelAtom(sim, part, halvesMetal1Hex).method_99(out metal1))
                    {
                        metal1Exists = true;
                    }

                    if (!metal1Exists)
                    {
                        continue;
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
                    else if (HalvingMetallurgy.ReductiveMetallurgyLoaded && ReductiveMetallurgy.Wheel.maybeFindRavariWheelAtom(sim, part, halvesMetal2Hex).method_99(out metal2))
                    {
                        metal2Exists = true;
                    }

                    if (!metal2Exists)
                    {
                        continue;
                    }


                    // Are they valid atoms
                    if (metal1IsSoria ? !API.QuicksilverProjectionBehavior(metal1.field_2280, 1).method_99(out AtomType hp1) : // Soria's Wheel
                    (!API.HalvesDictionary.TryGetValue(metal1.field_2280, out hp1) // override
                    && API.ChangeMetallicity(metal1.field_2280, 1, out hp1, static i => i <= 13) == Brimstone.API.SuccessInfo.failure)) // metallicity
                    {
                        continue;
                    }
                    if (metal2IsSoria ? !API.QuicksilverProjectionBehavior(metal2.field_2280, 1).method_99(out AtomType hp2) : // Soria's Wheel
                    (!API.HalvesDictionary.TryGetValue(metal2.field_2280, out hp2) // override
                    && API.ChangeMetallicity(metal2.field_2280, 1, out hp2, static i => i <= 13) == Brimstone.API.SuccessInfo.failure)) // metallicity
                    {
                        continue;
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
                        ReductiveMetallurgy.Wheel.DrawRavariFlash(seb, part, halvesInputHex);
                        quicksilver.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, quicksilver.field_2280, class_238.field_1989.field_81.field_614, 30f);
                    }
                    else
                    {
                        // Delete the quicksilver
                        Brimstone.API.RemoveAtom(quicksilver);
                        // Play deletion animation
                        seb.field_3937.Add(new(seb, quicksilver.field_2278, Brimstone.API.VanillaAtoms["quicksilver"]));
                    }
                    // Promote the metals
                    Brimstone.API.ChangeAtom(metal1, hp1);
                    Brimstone.API.ChangeAtom(metal2, hp2);
                    // Play promotion 
                    seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(bowl1), halvesBowlFlashAnimation, 30f, Vector2.Zero, 0f));
                    metal1.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal1.field_2280, class_238.field_1989.field_81.field_614, 30f);
                    seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(bowl2), halvesBowlFlashAnimation, 30f, Vector2.Zero, 0f));
                    metal2.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal2.field_2280, class_238.field_1989.field_81.field_614, 30f);
                    pss[part].field_2743 = true;
                    // Play custom sound
                    Brimstone.API.PlaySound(sim, halvesSound);
                }
                else if (type == Quake)
                {
                    if (!first)
                    {
                        continue;
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
                                goto nextGlyph;
                            }
                        }

                        bool hasRemovableBond = false;
                        List<Pair<Vector2, float>> resistingBonds = new();
                        // for each atom in a molecule
                        foreach (KeyValuePair<HexIndex, Atom> entry in moleculeAboveBowl.method_1100())
                        {
                            if (entry.Value.field_2275 == Atoms.Sednum)
                            {
                                foreach (HexIndex offset in HexIndex.AdjacentOffsets)
                                {
                                    HexIndex sednumPos = entry.Key;
                                    HexIndex sednumNeighbor = sednumPos + offset;
                                    bool willBeRegularBonded = simBonders.Contains(new HexIndexPair(sednumPos, sednumNeighbor)) || simBonders.Contains(new HexIndexPair(sednumNeighbor, sednumPos));
                                    enum_126 bondType = Brimstone.API.FindBondType(moleculeAboveBowl, sednumPos, sednumNeighbor);

                                    if (bondType != enum_126.None && bondType != (enum_126.Prisma0 | enum_126.Prisma1 | enum_126.Prisma2))
                                    {
                                        Vector2 midpoint = class_162.method_413(class_187.field_1742.method_492(sednumPos), class_187.field_1742.method_492(sednumNeighbor), 0.5f);
                                        if ((bondType & enum_126.Standard) == enum_126.Standard && willBeRegularBonded)
                                        {
                                            resistingBonds.Add(new Pair<Vector2, float>(midpoint, class_187.field_1742.method_492(offset).Angle()));
                                        }
                                        else
                                        {
                                            hasRemovableBond = true;
                                            Brimstone.API.RemoveBonds(sim, moleculeAboveBowl, sednumPos, sednumNeighbor, true, false);
                                        }
                                    }
                                }
                            }
                        }

                        // only active if not idempotent
                        if (hasRemovableBond)
                        {
                            Brimstone.API.ForceRecomputeBonds(moleculeAboveBowl);
                            pss[part].field_2743 = true;
                            foreach (Pair<Vector2, float> pair in resistingBonds)
                            {
                                seb.field_3936.Add(new class_228(seb, (enum_7)1, pair.Left, quakeUnbondResistedAnimation, 75f, new Vector2(1.5f, -5f), pair.Right));
                            }

                            // play a buzzing sound
                            Brimstone.API.PlaySound(sim, quakeSound);
                        }
                    }
                }
                else if (type == Sump)
                {
                    DynamicData dyn_pss = new(pss[part]);
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

                        if (!quicksilverExists || quicksilver.field_2280 != Brimstone.API.VanillaAtoms["quicksilver"])
                        {
                            goto trySumpDrain;
                        }
                        if (quicksilverIsSoria)
                        {
                            if (state.quicksilverCount == 5)
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
                            seb.field_3937.Add(new(seb, quicksilver.field_2278, Brimstone.API.VanillaAtoms["quicksilver"]));
                        }

                        if (state.quicksilverCount < 5)
                        {
                            state.quicksilverCount++;
                        }
                        else
                        {
                            state.drainFlash = true;
                        }
                    trySumpDrain:
                        if (state.quicksilverCount > 0 && !sim.FindAtomRelative(part, sumpOutputHex).method_1085())
                        {
                            state.quicksilverEject = true;
                            state.quicksilverCount--;
                            Brimstone.API.AddSmallCollider(sim, part, sumpOutputHex);
                        }
                    }
                    else
                    {
                        state.drainFlash = false;
                        if (state.quicksilverEject)
                        {
                            state.quicksilverEject = false;
                            Brimstone.API.AddAtom(sim, part, sumpOutputHex, Brimstone.API.VanillaAtoms["quicksilver"]);
                        }
                    }
                    dyn_pss.Set("state", state);
                }
                else if (type == Remission)
                {
                    if (first)
                    {
                        if (sim.FindAtomRelative(part, remissionOutputHex).method_1085())
                        {
                            // blocked!
                            continue;
                        }

                        HexIndex input1 = part.method_1184(remissionInput1Hex);
                        HexIndex input2 = part.method_1184(remissionInput2Hex);
                        HexIndex bowl = part.method_1184(remissionBowlHex);

                        bool metal1Exists = false;
                        bool metal2Exists = false;
                        bool metalBowlExists = false;

                        if (sim.FindAtom(input1).method_99(out AtomReference metal1))
                        {
                            metal1Exists = !metal1.field_2281 && !metal1.field_2282;
                        }
                        if (sim.FindAtom(input2).method_99(out AtomReference metal2))
                        {
                            metal2Exists = !metal2.field_2281 && !metal2.field_2282;
                        }
                        if (sim.FindAtom(bowl).method_99(out AtomReference metalOnBowl) || (HalvingMetallurgy.ReductiveMetallurgyLoaded && ReductiveMetallurgy.Wheel.maybeFindRavariWheelAtom(sim, part, remissionBowlHex).method_99(out metalOnBowl)))
                        {
                            metalBowlExists = true;
                        }

                        if (!metal1Exists || !metal2Exists || !metalBowlExists)
                        {
                            continue;
                        }
                        if (metal1.field_2280 != metal2.field_2280)
                        {
                            continue;
                        }
                        if (!API.metalToDoubledMetallicity.TryGetValue(metal1.field_2280, out int inputMetallicity) || !API.metalToDoubledMetallicity.TryGetValue(metalOnBowl.field_2280, out int bowlMetallicity))
                        {
                            continue;
                        }

                        int outputMetallicity = inputMetallicity + 2;
                        bowlMetallicity += inputMetallicity - 2;

                        if (bowlMetallicity < 0 || outputMetallicity > 13)
                        {
                            continue;
                        }

                        if (bowlMetallicity == 0 && !ExtractionPresent(parts, part, DiamondNeighbors))
                        {
                            continue;
                        }


                        if (!API.doubledMetallicityToMetal.TryGetValue(outputMetallicity, out AtomType outputAtom) || !API.doubledMetallicityToMetal.TryGetValue(bowlMetallicity, out AtomType projectedMetal))
                        {
                            continue;
                        }

                        // Delete the metals
                        Brimstone.API.RemoveAtom(metal1);
                        Brimstone.API.RemoveAtom(metal2);
                        // Play deletion animation
                        seb.field_3937.Add(new(seb, metal1.field_2278, metal1.field_2280));
                        seb.field_3937.Add(new(seb, metal2.field_2278, metal2.field_2280));
                        // Promote the metal
                        Brimstone.API.ChangeAtom(metalOnBowl, projectedMetal);
                        // Play promotion animations
                        seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(bowl), halvesBowlFlashAnimation, 30f, Vector2.Zero, 0f));
                        metalOnBowl.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metalOnBowl.field_2280, class_238.field_1989.field_81.field_614, 30f);
                        // Update Glyph
                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[1] { outputAtom };
                        Brimstone.API.AddSmallCollider(sim, part, remissionOutputHex);
                        // Play sound
                        Brimstone.API.PlaySound(sim, class_238.field_1991.field_1845);
                    }
                    else if (pss[part].field_2743)
                    {

                        Brimstone.API.AddAtom(sim, part, remissionOutputHex, pss[part].field_2744[0]);
                    }
                }
                else if (type == Shearing)
                {
                    if (first)
                    {
                        if (sim.FindAtomRelative(part, shearingOutputHex).method_1085())
                        {
                            // blocked!
                            continue;
                        }
                        if (!sim.FindAtomRelative(part, shearingBowlHex).method_99(out AtomReference bowlAtom)
                        && !Wheel.MaybeFindSoriaWheelAtom(sim, part, shearingBowlHex).method_99(out bowlAtom))
                        {
                            continue;
                        }

                        bool madeQuickcopper = false;

                        AtomType newBowlAtom = null;
                        AtomType outputAtom = null;
                        if (API.ShearingDictionary.TryGetValue(bowlAtom.field_2280, out Pair<AtomType, AtomType> result))
                        {
                            newBowlAtom = result.Left;
                            outputAtom = result.Right;
                            madeQuickcopper = quickcopperRadioactive && (newBowlAtom == Atoms.Quickcopper || outputAtom == Atoms.Quickcopper);
                        }
                        else if (API.metalToDoubledMetallicity.TryGetValue(bowlAtom.field_2280, out int inputMetallicity))
                        {
                            int remainder = (inputMetallicity + 1) >> 1;
                            int leftovers = inputMetallicity - remainder;

                            if (leftovers < 0)
                            {
                                // no subvacants
                                continue;
                            }

                            if (leftovers == 1)
                            {
                                // don't create beryl
                                continue;
                            }
                            else if (leftovers == 0 && !ExtractionPresent(parts, part, DoubleNeighbors))
                            {
                                // only create vaca if extraction is present
                                continue;
                            }

                            if (!API.doubledMetallicityToMetal.TryGetValue(remainder, out newBowlAtom) || !API.doubledMetallicityToMetal.TryGetValue(leftovers, out outputAtom))
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }

                        Brimstone.API.ChangeAtom(bowlAtom, newBowlAtom);
                        bowlAtom.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, bowlAtom.field_2280, shearingSplitAnimation, 30f);
                        seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1161()) + new Vector2(0f, 0f), shearingFlashAnimation, 30f, Vector2.Zero, 0f));
                        // Update Glyph
                        pss[part].field_2743 = true;
                        pss[part].field_2744 = new AtomType[1] { outputAtom };
                        Brimstone.API.PlaySound(sim, madeQuickcopper ? quickcopperSound : shearingSound);
                        Brimstone.API.AddSmallCollider(sim, part, shearingOutputHex);
                    }
                    else if (pss[part].field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, shearingOutputHex, pss[part].field_2744[0]);
                    }
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

                    if (!quickcopperExists || quickcopper.field_2280 != Atoms.Quickcopper)
                    {
                        continue;
                    }

                    if (!sim.FindAtom(metalHex).method_99(out AtomReference metal)
                    && !(HalvingMetallurgy.ReductiveMetallurgyLoaded && ReductiveMetallurgy.Wheel.maybeFindRavariWheelAtom(sim, part, osmosisMetalHex).method_99(out metal)))
                    {
                        continue;
                    }

                    AtomType output = null;
                    if (quickcopperIsSoria && metal.field_2280 == Atoms.Quickcopper)
                    {
                        output = Atoms.Quicklime;
                    }
                    else if (!API.OsmosisDictionary.TryGetValue(metal.field_2280, out output)
                    && API.ChangeMetallicity(metal.field_2280, -1, out output, i => (i != 0 || ExtractionPresent(parts, part, DoubleNeighbors))) == Brimstone.API.SuccessInfo.failure)
                    {
                        continue;
                    }
                    Brimstone.API.ChangeAtom(metal, output);
                    Brimstone.API.ChangeAtom(quickcopper, Brimstone.API.VanillaAtoms["quicksilver"]);
                    seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(metalHex), halvesBowlFlashAnimation, 30f, Vector2.Zero, 0f));
                    metal.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal.field_2280, class_238.field_1989.field_81.field_614, 30f);
                    seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(quickcopperHex), halvesBowlFlashAnimation, 30f, Vector2.Zero, 0f));
                    quickcopper.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, quickcopper.field_2280, class_238.field_1989.field_81.field_614, 30f);
                    Brimstone.API.PlaySound(sim, osmosisSound);
                }
                else if (type == class_191.field_1775) /* Triplex bonder */
                {
                    foreach (class_222 bonder in type.field_1538)
                    {
                        if (!sim.FindAtomRelative(part, bonder.field_1920).method_99(out AtomReference leftAtom) || !sim.FindAtomRelative(part, bonder.field_1921).method_99(out AtomReference rightAtom))
                        {
                            continue;
                        }
                        if ((leftAtom.field_2280 == Atoms.Vulcan && rightAtom.field_2280 == Brimstone.API.VanillaAtoms["fire"]) || (leftAtom.field_2280 == Brimstone.API.VanillaAtoms["fire"] && rightAtom.field_2280 == Atoms.Vulcan) || (leftAtom.field_2280 == Atoms.Vulcan && rightAtom.field_2280 == Atoms.Vulcan))
                        {
                            Brimstone.API.JoinMoleculesAtHexes(sim, part, bonder.field_1920, bonder.field_1921);
                            Brimstone.API.AddBond(sim, part, bonder.field_1920, bonder.field_1921, bonder.field_1922);
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
                            else if (quickcopper.field_2280 == Brimstone.API.VanillaAtoms["quicksilver"])
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
                        else if (quickcopper.field_2280 == Brimstone.API.VanillaAtoms["quicksilver"])
                        {
                            projectAmount = 2;
                            isQuicksilverSoria = true;
                        }
                    }
                    else if (HalvingMetallurgy.ReductiveMetallurgyLoaded && ReductiveMetallurgy.Wheel.maybeFindRavariWheelAtom(sim, part, new(0, 0)).method_99(out quickcopper))
                    {
                        if (API.ChangeMetallicity(quickcopper.field_2280, -2, out rejectionResult, static i => i >= 2) != Brimstone.API.SuccessInfo.failure)
                        {
                            projectAmount = 2;
                            isQuicksilverRavari = true;
                        }
                    }

                    if (projectAmount == 0)
                    {
                        continue;
                    }

                    bool metalExists = false;
                    bool isMetalSoria = false;

                    if (sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference metal))
                    {
                        if (projectAmount == 2 && !isQuicksilverSoria)
                        {
                            // already handled by vanilla or RM
                            continue;
                        }
                        metalExists = true;
                    }
                    else if (Wheel.MaybeFindSoriaWheelAtom(sim, part, new(1, 0)).method_99(out metal))
                    {
                        metalExists = true;
                        isMetalSoria = true;
                    }
                    else if (HalvingMetallurgy.ReductiveMetallurgyLoaded && ReductiveMetallurgy.Wheel.maybeFindRavariWheelAtom(sim, part, new(1, 0)).method_99(out metal))
                    {
                        if (projectAmount == 2 && !isQuicksilverSoria)
                        {
                            // already handled by RM
                            continue;
                        }
                        metalExists = true;
                    }

                    if (!metalExists)
                    {
                        continue;
                    }

                    if (!(isMetalSoria && API.QuicksilverProjectionBehavior(metal.field_2280, projectAmount).method_99(out AtomType projection))
                    && !API.HalvesDictionary.TryGetValue(metal.field_2280, out projection)
                    && API.ChangeMetallicity(metal.field_2280, projectAmount, out projection, static i => i <= 13) == Brimstone.API.SuccessInfo.failure)
                    {
                        continue;
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
                        ReductiveMetallurgy.Wheel.DrawRavariFlash(seb, part, new(0, 0));
                    }
                    else
                    {
                        // quickcopper falling
                        Brimstone.API.RemoveAtom(quickcopper);
                        seb.field_3937.Add(new(seb, quickcopper.field_2278, quickcopper.field_2280));
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
                    if (first && pss[part].field_2743 && pss[part].field_2744[0] == Atoms.Beryl)
                    {
                        // Switch-a-roo
                        pss[part].field_2744 = new AtomType[1] { Atoms.PurificationBeryl };
                    }
                }
            nextGlyph:
                ;
            }
        });
        #endregion
    }
}
