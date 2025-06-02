using System.Collections.Generic;
using Quintessential;
using ReductiveMetallurgy;
using PartType = class_139;
using Permissions = enum_149;
using Texture = class_256;
using System.Reflection;
using System.Linq;
using System;
using MonoMod.Utils;

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
        quicksilverCount = 0;
        drainFlash = false;
        quicksilverEject = false;
    }
    // could compress these into one byte, but readablity would suck.
    public byte quicksilverCount;
    public bool drainFlash;
    public bool quicksilverEject;
}

public static class HalvingMetallurgyParts
{
    public static PartType Halves;

    public static Texture halvesBase = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/halves_base");
    public static Texture halvesGlow = Brimstone.API.GetTexture("textures/select/erikhaag/HalvingMetallurgy/halves_glow");
    public static Texture halvesOutline = Brimstone.API.GetTexture("textures/select/erikhaag/HalvingMetallurgy/halves_outline");
    public static Texture halvesIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/halves_icon");
    public static Texture halvesIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/halves_icon_hover");
    public static Texture[] halvesEngravingFlashAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/halves_engraving_flash.array", "halves_engraving", 6);

    public static PartType Quake;

    public static Texture quakeBase = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_base");
    public static Texture quakeBowl = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_bowl");
    public static Texture quakeBowlShaking = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_bowl_shaking");
    public static Texture quakeIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/quake_icon");
    public static Texture quakeIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/quake_icon_hover");
    public static Texture quakeGlow = class_238.field_1989.field_97.field_382;
    public static Texture quakeOutline = class_238.field_1989.field_97.field_383;
    public static Texture[] quakeUnbondResistedAnimation = Brimstone.API.GetAnimation("textures/bonds/erikhaag/HalvingMetallurgy/unbond_resist.array", "unbond_resist", 22);

    public static PartType Sump;

    public static Texture sumpBase = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/sump_base");
    public static Texture sumpIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/sump_icon");
    public static Texture sumpIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/icons/sump_icon_hover");
    public static Texture sumpGlow = class_238.field_1989.field_97.field_374;
    public static Texture sumpOutline = class_238.field_1989.field_97.field_375;
    public static Texture[] quicksilverIrisAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/iris_full_quicksilver.array", "iris_full_quicksilver", 16);
    public static Texture[] sumpDrainFlashAnimation = Brimstone.API.GetAnimation("textures/parts/erikhaag/HalvingMetallurgy/sump_drain_flash.array", "sump_flash", 8);

    public static HexIndex[] HexMoves = {
        new(1,0),
        new(0,1),
        new(-1,1),
        new(-1,0),
        new(0,-1),
        new(1,-1)
    };

    public static Sound halvesSound;
    public static Sound quakeSound;

    public static void LoadSounds()
    {
        halvesSound = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/halves");
        quakeSound = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/quake");

        FieldInfo field = typeof(class_11).GetField("field_52", BindingFlags.Static | BindingFlags.NonPublic);
        Dictionary<string, float> volumeDictionary = (Dictionary<string, float>)field.GetValue(null);

        volumeDictionary.Add("halves", 0.7f);
        volumeDictionary.Add("quake", 0.4f);

        void Method_540(On.class_201.orig_method_540 orig, class_201 self)
        {
            orig(self);
            halvesSound.field_4062 = false;
            quakeSound.field_4062 = false;
        }

        On.class_201.method_540 += Method_540;
    }

    public static readonly HexIndex halvesInputHex = new(0, 0);
    public static readonly HexIndex halvesMetal1Hex = new(1, 0);
    public static readonly HexIndex halvesMetal2Hex = new(-1, 1);

    public static readonly HexIndex quakeBowlHex = new(0, 0);

    public static readonly HexIndex sumpInputHex = new(0, 0);
    public static readonly HexIndex sumpOutputHex = new(1, 0);

    public static void AddPartTypes()
    {
        // Todo: Add a Brimstone function for this
        Halves = new()
        {
            field_1528 = "halving-metallurgy-halves", // ID
            field_1529 = class_134.method_253("Glyph of Halves", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of halves consumes an atom of quicksilver and half-promotes two metal atoms.", string.Empty), // Description
            field_1531 = 30, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = halvesGlow, // Shadow/glow
            field_1550 = halvesOutline, // Stroke/outline
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

        Quake = new()
        {
            field_1528 = "halving-metallurgy-quake",
            field_1529 = class_134.method_253("Glyph of Quake", string.Empty),
            field_1530 = class_134.method_253("The glyph of quake shakes a molecule vigorously, breaking all the weak sednum bonds.", string.Empty),
            field_1531 = 20,
            field_1539 = true,
            field_1549 = quakeGlow,
            field_1550 = quakeOutline,
            field_1547 = quakeIcon,
            field_1548 = quakeIconHover,
            field_1540 = new HexIndex[]
            {
                quakeBowlHex
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains(HalvingMetallurgy.QuakePermission)
        };

        Sump = new()
        {
            field_1528 = "halving-metallurgy-sump",
            field_1529 = class_134.method_253("Quicksilver Sump", string.Empty),
            field_1530 = class_134.method_253("The quicksilver sump can hold 6 quicksilver for storage, excess quicksilver is drained out of the engine.", string.Empty),
            field_1531 = 15,
            field_1539 = true,
            field_1549 = sumpGlow,
            field_1550 = sumpOutline,
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

        QApi.AddPartTypeToPanel(Halves, false);
        QApi.AddPartTypeToPanel(Quake, false);
        QApi.AddPartTypeToPanel(Sump, false);

        QApi.AddPartType(Halves, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            float time = editor.method_504();
            int frame = 0;
            Vector2 offset = new(83f, 50f);
            if (pss.field_2743)
            {
                frame = (int)(11f * time);
                frame = frame >= 6 ? 10 - frame : frame;
            }
            renderer.method_523(halvesBase, Vector2.Zero, offset, 0f);
            renderer.method_523(halvesEngravingFlashAnimation[frame], Vector2.Zero, offset, 0f);
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
            renderer.method_523(quakeBase, Vector2.Zero, offset, 0f);
            if (pss.field_2743 && time < 0.75f)
            {
                double x = (10.6666667 * time) % 4.0;
                const double commonDenominator = 1.0 / 21.0;
                // "I want a sinusoid!"
                // "We have sinusoid at home"
                // Sinusoid at home:
                float shakeX = 3 * (float)(-commonDenominator * Math.Pow(x, 5) + 10 * commonDenominator * Math.Pow(x, 4) - 28 * commonDenominator * Math.Pow(x, 3) + 8 * commonDenominator * Math.Pow(x, 2) + 32 * commonDenominator * x);
                renderer.method_529(quakeBowlShaking, quakeBowlHex, new Vector2(shakeX, 0));
            }
            else
            {
                renderer.method_529(quakeBowl, quakeBowlHex, Vector2.Zero);
            }
        });

        QApi.AddPartType(Sump, static (part, pos, editor, renderer) =>
        {
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
            Vector2 offset = new(41f, 49f);
            renderer.method_523(sumpBase, Vector2.Zero, offset, 0f);
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
            }
            if (state.quicksilverEject && !afterIrisOpens)
            {
                // show quicksilver rising behind iris
                Editor.method_925(risingQuicksilver, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
            renderer.method_529(quicksilverIrisAnimation[irisFrame], sumpOutputHex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_228.field_271, sumpOutputHex, Vector2.Zero);
            if (state.quicksilverEject && afterIrisOpens)
            {
                // show quicksilver rising infront of iris
                Editor.method_925(risingQuicksilver, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        });


        QApi.RunAfterCycle((sim, first) =>
        {
            SolutionEditorBase seb = sim.field_3818;
            Dictionary<Part, PartSimState> pss = sim.field_3821;
            List<Part> parts = seb.method_502().field_3919;

            List<HexIndexPair> bonders = new();
            // should technically pre-compute at startup, but I don't exactly know where that is, also I really don't want to deal with ILCursor right now, maybe later!
            foreach (Part potential_bonder in parts)
            {
                if (potential_bonder.method_1159().field_1528 == "bonder")
                {
                    bonders.Add(new HexIndexPair(potential_bonder.method_1184(new HexIndex(0, 0)), potential_bonder.method_1184(new HexIndex(1, 0))));
                }
                else if (potential_bonder.method_1159().field_1528 == "bonder-speed")
                {
                    HexIndex center = potential_bonder.method_1184(new HexIndex(0, 0));
                    bonders.Add(new HexIndexPair(center, potential_bonder.method_1184(new HexIndex(1, 0))));
                    bonders.Add(new HexIndexPair(center, potential_bonder.method_1184(new HexIndex(-1, 1))));
                    bonders.Add(new HexIndexPair(center, potential_bonder.method_1184(new HexIndex(0, -1))));
                }

            }

            foreach (Part part in parts)
            {
                PartType type = part.method_1159();
                if (type == Halves)
                {
                    // Oh hey, it is
                    if (!first)
                    {
                        continue;
                    }

                    bool quicksilverExists = false;
                    bool metal1Exists = false;
                    bool metal2Exists = false;

                    bool isQuicksilverRavari = false;
                    bool isMetal1Ravari = false;
                    bool isMetal2Ravari = false;

                    Dictionary<AtomType, AtomType> rejectionRules = new();
                    AtomType rejectionResult = null;

                    if (HalvingMetallurgy.ReductiveMetallurgyLoaded)
                    {
                        rejectionRules = (Dictionary<AtomType, AtomType>)Brimstone.API.PrivateField(typeof(ReductiveMetallurgy.API), "rejectDict").GetValue(null);
                    }

                    if (sim.FindAtomRelative(part, halvesInputHex).method_99(out AtomReference quicksilver))
                    {
                        // Is the quicksilver singular and not held
                        quicksilverExists = !quicksilver.field_2281 && !quicksilver.field_2282;
                    }
                    else if (HalvingMetallurgy.ReductiveMetallurgyLoaded && Wheel.maybeFindRavariWheelAtom(sim, part, halvesInputHex).method_99(out quicksilver))
                    {
                        // Is the metal able to be demoted
                        if (rejectionRules.TryGetValue(quicksilver.field_2280, out rejectionResult))
                        {
                            quicksilverExists = true;
                            isQuicksilverRavari = true;
                        }
                    }
                    if (sim.FindAtomRelative(part, halvesMetal1Hex).method_99(out AtomReference metal1))
                    {
                        metal1Exists = true;
                    }
                    else if (HalvingMetallurgy.ReductiveMetallurgyLoaded && Wheel.maybeFindRavariWheelAtom(sim, part, halvesMetal1Hex).method_99(out metal1))
                    {
                        metal1Exists = true;
                        isMetal1Ravari = true;
                    }
                    if (sim.FindAtomRelative(part, halvesMetal2Hex).method_99(out AtomReference metal2))
                    {
                        metal2Exists = true;
                    }
                    else if (HalvingMetallurgy.ReductiveMetallurgyLoaded && Wheel.maybeFindRavariWheelAtom(sim, part, halvesMetal2Hex).method_99(out metal2))
                    {
                        metal2Exists = true;
                        isMetal2Ravari = true;
                    }

                    // Are their atoms in the right spots?
                    if (quicksilverExists && metal1Exists && metal2Exists)
                    {

                        // Are they valid atoms
                        if (HalvingMetallurgyAPI.HalvingPromotions.TryGetValue(metal1.field_2280, out AtomType hp1)
                        && HalvingMetallurgyAPI.HalvingPromotions.TryGetValue(metal2.field_2280, out AtomType hp2))
                        {
                            if (isQuicksilverRavari)
                            {
                                Brimstone.API.ChangeAtom(quicksilver, rejectionResult);
                                Wheel.DrawRavariFlash(seb, part, halvesMetal1Hex);
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
                            // Play promotion animations
                            if (isMetal1Ravari)
                            {
                                Wheel.DrawRavariFlash(seb, part, halvesMetal1Hex);
                            }
                            metal1.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal1.field_2280, class_238.field_1989.field_81.field_614, 30f);
                            if (isMetal2Ravari)
                            {
                                Wheel.DrawRavariFlash(seb, part, halvesMetal2Hex);
                            }
                            metal2.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal2.field_2280, class_238.field_1989.field_81.field_614, 30f);

                            pss[part].field_2743 = true;
                            // Play custom sound
                            Brimstone.API.PlaySound(sim, halvesSound);
                        }
                    }
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
                            if (entry.Value.field_2275 == HalvingMetallurgyAtoms.Sednum)
                            {
                                foreach (HexIndex offset in HexMoves)
                                {
                                    HexIndex sednumPos = entry.Key;
                                    HexIndex sednumNeighbor = sednumPos + offset;
                                    bool willBeRegularBonded = bonders.Contains(new HexIndexPair(sednumPos, sednumNeighbor)) || bonders.Contains(new HexIndexPair(sednumNeighbor, sednumPos));
                                    enum_126 bondType = moleculeAboveBowl.method_1113(sednumPos, sednumNeighbor);
                                    if (bondType != enum_126.None)
                                    {
                                        Vector2 midpoint = class_162.method_413(class_187.field_1742.method_492(sednumPos), class_187.field_1742.method_492(sednumNeighbor), 0.5f);
                                        if ((bondType & enum_126.Standard) == enum_126.Standard && willBeRegularBonded)
                                        {
                                            resistingBonds.Add(new Pair<Vector2, float>(midpoint, class_187.field_1742.method_492(offset).Angle()));
                                        }
                                        else
                                        {
                                            hasRemovableBond = true;
                                            moleculeAboveBowl.method_1114(sednumPos, sednumNeighbor);
                                            Texture[] bondBreakAnimation = ((bondType & enum_126.Standard) != enum_126.Standard) ? class_238.field_1989.field_83.field_156 : class_238.field_1989.field_83.field_154;
                                            seb.field_3935.Add(new class_228(seb, (enum_7)1, midpoint, bondBreakAnimation, 75f, new Vector2(1.5f, -5f), class_187.field_1742.method_492(offset).Angle()));
                                        }
                                    }
                                }
                            }
                        }

                        if (hasRemovableBond)
                        {
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
                    SumpState state = new();
                    if (stateOb is not null)
                    {
                        state = (SumpState)stateOb;
                    }
                    if (first)
                    {
                        if (sim.FindAtomRelative(part, sumpInputHex).method_99(out AtomReference quicksilver) && !quicksilver.field_2281 && !quicksilver.field_2282)
                        {
                            // Delete the quicksilver
                            Brimstone.API.RemoveAtom(quicksilver);
                            // Play deletion animation
                            seb.field_3937.Add(new(seb, quicksilver.field_2278, Brimstone.API.VanillaAtoms["quicksilver"]));
                            if (state.quicksilverCount < 5)
                            {
                                state.quicksilverCount++;
                            }
                            else
                            {
                                state.drainFlash = true;
                            }
                        }
                        if (state.quicksilverCount > 0 && !sim.FindAtomRelative(part, sumpOutputHex).method_99(out _))
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
                            Brimstone.API.AddAtom(sim, Brimstone.API.VanillaAtoms["quicksilver"], part, sumpOutputHex);
                        }
                    }
                    dyn_pss.Set("state", state);
                }
            nextGlyph:;
            }
        });
    }
}
