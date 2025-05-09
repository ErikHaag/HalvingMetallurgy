using System.Collections.Generic;
using Quintessential;
using ReductiveMetallurgy;
using PartType = class_139;
using Permissions = enum_149;
using Texture = class_256;
using System.Reflection;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Threading;

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

internal static class HalvingMetallurgyParts
{
    public static PartType Halves;

    public static Texture halvesBase = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/halves_base");
    public static Texture halvesEngraving = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/halves_engraving");
    public static Texture halvesGlow = Brimstone.API.GetTexture("textures/select/erikhaag/HalvingMetallurgy/halves_glow");
    public static Texture halvesOutline = Brimstone.API.GetTexture("textures/select/erikhaag/HalvingMetallurgy/halves_outline");
    public static Texture halvesIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/halves_icon");
    public static Texture halvesIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/halves_icon_hover");

    public static PartType Quake;

    public static Texture quakeBase = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_base");
    public static Texture quakeBowl = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_bowl");
    public static Texture quakeBowlShaking = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_bowl_shaking");
    public static Texture quakeIcon = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_icon");
    public static Texture quakeIconHover = Brimstone.API.GetTexture("textures/parts/erikhaag/HalvingMetallurgy/quake_icon_hover");
    public static Texture quakeGlow = class_238.field_1989.field_97.field_382;
    public static Texture quakeOutline = class_238.field_1989.field_97.field_383;
    public static Texture[] quakeUnbondResistedAnimation = Brimstone.API.GetAnimation("textures/bonds/erikhaag/HalvingMetallurgy/unbond_resist.array", "unbond_resist", 22);


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
    public static Sound quakeUnbondSound;
    public static Sound quakeUnbondResistSound;

    public static void LoadSounds()
    {
        halvesSound = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/halves");
        quakeSound = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/quake");

        FieldInfo field = typeof(class_11).GetField("field_52", BindingFlags.Static | BindingFlags.NonPublic);
        Dictionary<string, float> dictionary = (Dictionary<string, float>)field.GetValue(null);

        dictionary.Add("halves", 0.7f);
        dictionary.Add("quake", 0.4f);

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

        QApi.AddPartTypeToPanel(Halves, false);
        QApi.AddPartTypeToPanel(Quake, false);

        QApi.AddPartType(Halves, static (part, pos, editor, renderer) =>
        {
            Vector2 offset = new(83f, 50f);
            renderer.method_523(halvesBase, new Vector2(0f, 0f), offset, 0f);
            renderer.method_523(halvesEngraving, new Vector2(0f, 0f), offset, 0f);
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
            renderer.method_523(quakeBase, new Vector2(0f, 0f), offset, 0f);
            if (pss.field_2743 && time < 0.75f)
            {
                double x = (10.6666667 * time) % 4.0;
                const double commonDenominator = 1.0 / 21.0;
                // "I want a sinusoid!"
                // "We have sinusoid at home"
                // Sinusoid at home:
                float shakeX = 3 * (float)(-commonDenominator * Math.Pow(x, 5) + 10 * commonDenominator * Math.Pow(x, 4) - 28 * commonDenominator * Math.Pow(x, 3) + 8 * commonDenominator * Math.Pow(x, 2) + 32 * commonDenominator * x);
                renderer.method_529(quakeBowlShaking, quakeBowlHex, new Vector2(shakeX, 0));
            } else
            {
                renderer.method_529(quakeBowl, quakeBowlHex, Vector2.Zero);
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
                    bool metal1Exists = false;
                    bool metal2Exists = false;
                    bool isMetal1Ravari = false;
                    bool isMetal2Ravari = false;
                    if (sim.FindAtomRelative(part, halvesMetal1Hex).method_99(out AtomReference metal1))
                    {
                        metal1Exists = true;
                    }
                    else if (HalvingMetallurgy.ReductiveMetallurgyLoaded && !first && Wheel.maybeFindRavariWheelAtom(sim, part, halvesMetal1Hex).method_99(out metal1))
                    {
                        metal1Exists = true;
                        isMetal1Ravari = true;
                    }
                    if (sim.FindAtomRelative(part, halvesMetal2Hex).method_99(out AtomReference metal2))
                    {
                        metal2Exists = true;
                    }
                    else if (HalvingMetallurgy.ReductiveMetallurgyLoaded && !first && Wheel.maybeFindRavariWheelAtom(sim, part, halvesMetal2Hex).method_99(out metal2))
                    {
                        metal2Exists = true;
                        isMetal2Ravari = true;
                    }
                    // Are their atoms in the right spots?
                    if (sim.FindAtomRelative(part, halvesInputHex).method_99(out AtomReference quicksilver) && metal1Exists && metal2Exists)
                    {
                        // Is the quicksilver singular and not held?
                        if (!quicksilver.field_2281 && !quicksilver.field_2282)
                        {
                            // Are they valid atoms
                            if (quicksilver.field_2280 == Brimstone.API.VanillaAtoms["quicksilver"]
                            && HalvingMetallurgyAPI.HalvingPromotions.TryGetValue(metal1.field_2280, out AtomType hp1)
                            && HalvingMetallurgyAPI.HalvingPromotions.TryGetValue(metal2.field_2280, out AtomType hp2))
                            {
                                // Delete the quicksilver
                                Brimstone.API.RemoveAtom(quicksilver);
                                // Promote the metals
                                Brimstone.API.ChangeAtom(metal1, hp1);
                                Brimstone.API.ChangeAtom(metal2, hp2);
                                // Play deletion animation
                                seb.field_3937.Add(new(seb, quicksilver.field_2278, Brimstone.API.VanillaAtoms["quicksilver"]));
                                // Play promotion animations
                                if (isMetal1Ravari)
                                {
                                    Wheel.DrawRavariFlash(seb, part, halvesMetal1Hex);
                                }
                                else
                                {
                                    metal1.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal1.field_2280, class_238.field_1989.field_81.field_614, 30f);
                                }
                                if (isMetal2Ravari)
                                {
                                    Wheel.DrawRavariFlash(seb, part, halvesMetal2Hex);
                                }
                                else
                                {
                                    metal2.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal2.field_2280, class_238.field_1989.field_81.field_614, 30f);
                                }
                                // Play custom sound
                                Brimstone.API.PlaySound(sim, halvesSound);
                            }
                        }
                    }
                }
                else if (type == Quake)
                {
                    if (!first)
                    {
                        goto nextGlyph;
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
                                        hasRemovableBond = true;
                                        Vector2 midpoint = class_162.method_413(class_187.field_1742.method_492(sednumPos), class_187.field_1742.method_492(sednumNeighbor), 0.5f);
                                        if ((bondType & enum_126.Standard) == enum_126.Standard && willBeRegularBonded)
                                        {
                                            seb.field_3936.Add(new class_228(seb, (enum_7)1, midpoint, quakeUnbondResistedAnimation, 75f, new Vector2(1.5f, -5f), class_187.field_1742.method_492(sednumNeighbor - sednumPos).Angle()));
                                        }
                                        else
                                        {
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
                            // play a buzzing sound
                            Brimstone.API.PlaySound(sim, quakeSound);
                        }
                    }
                }
            nextGlyph:;
            }
        });
    }
}
