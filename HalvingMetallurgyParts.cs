using System.Collections.Generic;
using System.Media;
using Quintessential;
using ReductiveMetallurgy;
using PartType = class_139;
using Permissions = enum_149;
using Texture = class_256;

namespace HalvingMetallurgy;

internal static class HalvingMetallurgyParts
{
    public static PartType Halves;

    public static Texture halvesBase = class_235.method_615("textures/parts/erikhaag/HalvingMetallurgy/halves_base");
    public static Texture halvesGlow = class_235.method_615("textures/select/erikhaag/HalvingMetallurgy/halves_glow");
    public static Texture halvesOutline = class_235.method_615("textures/select/erikhaag/HalvingMetallurgy/halves_outline");
    public static Texture halvesIcon = class_235.method_615("textures/parts/erikhaag/HalvingMetallurgy/halves_icon");
    public static Texture halvesIconHover = class_235.method_615("textures/parts/erikhaag/HalvingMetallurgy/halves_icon_hover");

    public static readonly HexIndex halvesInputHex = new(0, 0);
    public static readonly HexIndex halvesMetal1Hex = new(1, 0);
    public static readonly HexIndex halvesMetal2Hex = new(-1, 1);
    public static void AddPartTypes()
    {
        Quintessential.Logger.Log("HalvingMetallurgy: Creating glyph.");
        Halves = new()
        {
            field_1528 = "halving-metallurgy-halves", // ID
            field_1529 = class_134.method_253("Glyph of Halves", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of halves consumes an atom of quicksilver and half-promotes two metal atoms.", string.Empty), // Description
            field_1531 = 30, // Cost
            field_1539 = true, // Is a glyph (?)
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
        QApi.AddPartTypeToPanel(Halves, false);
        QApi.AddPartType(Halves, static (part, pos, editor, renderer) => {
            Vector2 offset = new(82f, 48f);
            renderer.method_523(halvesBase, new Vector2(0f, 0f), offset, 0f);
            // quicksilver
            renderer.method_530(class_238.field_1989.field_90.field_255.field_293, halvesInputHex, 0);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_294, halvesInputHex, Vector2.Zero);
            // bowls
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, halvesMetal1Hex, Vector2.Zero);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_291, halvesMetal1Hex, Vector2.Zero);
            renderer.method_528(class_238.field_1989.field_90.field_255.field_292, halvesMetal2Hex, Vector2.Zero);
            renderer.method_529(class_238.field_1989.field_90.field_255.field_291, halvesMetal2Hex, Vector2.Zero);
        });
        QApi.RunAfterCycle((sim, first) =>
        {
            SolutionEditorBase seb = sim.field_3818;
            List<Part> parts = seb.method_502().field_3919;
            Dictionary<Part,PartSimState> simStates = sim.field_3821;
            // If there are a lot of part-adding mods, having each one loop over all the parts could get slow.
            foreach (Part part in parts) {
                PartType type = part.method_1159();
                // Is it mine?
                if (type == Halves) {
                    // Oh hey, it is
                    bool metal1Exists = false;
                    bool metal2Exists = false;
                    bool isMetal1Ravari = false;
                    bool isMetal2Ravari = false;
                    if (sim.FindAtomRelative(part, halvesMetal1Hex).method_99(out AtomReference metal1))
                    {
                        metal1Exists = true;
                    } else if (!first && Wheel.maybeFindRavariWheelAtom(sim, part, halvesMetal1Hex).method_99(out metal1))
                    {
                        metal1Exists = true;
                        isMetal1Ravari = true;
                    }
                    if (sim.FindAtomRelative(part, halvesMetal2Hex).method_99(out AtomReference metal2))
                    {
                        metal2Exists = true;
                    }
                    else if (!first && Wheel.maybeFindRavariWheelAtom(sim, part, halvesMetal2Hex).method_99(out metal2))
                    {
                        metal2Exists = true;
                        isMetal2Ravari = true;
                    }
                    // Are their atoms in the right spots?
                    if (sim.FindAtomRelative(part, halvesInputHex).method_99(out AtomReference quicksilver) && metal1Exists && metal2Exists)
                    {
                        // Is the quicksilver singular and not held?
                        if (!quicksilver.field_2281 && !quicksilver.field_2282) {
                            // Are they valid atoms
                            if (quicksilver.field_2280 == API.quicksilverAtomType
                            && HalvingMetallurgyAPI.HalvingPromotions.TryGetValue(metal1.field_2280, out AtomType hp1)
                            && HalvingMetallurgyAPI.HalvingPromotions.TryGetValue(metal2.field_2280, out AtomType hp2))
                            {
                                // Delete the quicksilver
                                quicksilver.field_2277.method_1107(quicksilver.field_2278);
                                // Promote the metals
                                metal1.field_2277.method_1106(hp1, metal1.field_2278);
                                metal2.field_2277.method_1106(hp2, metal2.field_2278);
                                // Play deletion animation
                                seb.field_3937.Add(new(seb, quicksilver.field_2278, API.quicksilverAtomType)); 
                                // Play promotion animations
                                if (isMetal1Ravari) {
                                    Wheel.DrawRavariFlash(seb, part, halvesMetal1Hex);
                                } else {
                                    metal1.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal1.field_2280, class_238.field_1989.field_81.field_614, 30f);
                                }
                                if (isMetal2Ravari) {
                                    Wheel.DrawRavariFlash(seb, part, halvesMetal2Hex);
                                } else {
                                    metal2.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, metal2.field_2280, class_238.field_1989.field_81.field_614, 30f);
                                }
                                // Play promotion sound
                                API.PrivateMethod<Sim>("method_1856").Invoke(sim, new object[] { class_238.field_1991.field_1844 });
                            }
                        }
                    }
                }
            }
        });
        Quintessential.Logger.Log("HalvingMetallurgy: Glyph added.");
    }
}
