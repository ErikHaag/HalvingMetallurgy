#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable IDE0059 // Unnecessary assignment of a value

using HalvingMetallurgy.Utilities;
using Lithos;
using MonoMod.Utils;
using Quintessential;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using static Sim;

namespace HalvingMetallurgy;

public static class Glyphs
{

    internal static List<HexIndexPair> QuakeNormalBonders = [];

    static readonly ulong timerFactor = SDL.SDL_GetPerformanceFrequency() / 30;

    #region Glyphs

    public static PartType Halves;
    public static PartType Quake;
    public static PartType Sump;
    public static PartType Remission;
    public static PartType Shearing;
    public static PartType Osmosis;

    public static MutableWheel Soria;
    public static PartType Harmony;
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
    public static readonly HexIndex osmosisQuixHex = new(1, 0);

    /*
    public static HexIndex[] DoubleNeighbors;
    public static HexIndex[] DiamondNeighbors;

    public static void CalculateAdjacencies()
    {
        DoubleNeighbors = API.GetGlyphNeighbors(Osmosis.field_1540);
        DiamondNeighbors = API.GetGlyphNeighbors(Remission.field_1540);
    }
    */

    #endregion

    public static void PopulateQuakeNormalBonders(Sim sim)
    {
        QuakeNormalBonders.Clear();
        Solution solution = sim.GetSolution();
        foreach (Part part in solution.parts)
        {
            foreach (BonderInfo bonder in part.GetType().bonders)
            {
                if (bonder.isUnbond
                    || !bonder.bondTypes.Contains(BondTypes.standard)
                    || bonder.uniqueAtoms.Count != 0)
                {
                    continue;
                }
                QuakeNormalBonders.Add(new HexIndexPair(part.GlobalPosFromLocal(bonder.hexPos1), part.GlobalPosFromLocal(bonder.hexPos2)));
            }
        }
    }

    public static void DefineParts()
    {

        #region Glyph Definitions

        Halves = new()
        {
            Id = HalvingMetallurgy.Instance.GetIdentifier("halves"),
            cost = 30,
            glowTexture = Textures.Halves.Glow,
            strokeTexture = Textures.Halves.Stroke,
            iconTexture = Textures.Icons.Halves,
            iconHoverTexture = Textures.Icons.HalvesHover,
            glyphHexes = [
                halvesInputHex,
                halvesMetal1Hex,
                halvesMetal2Hex
            ],
            isFullHexCover = true,
            CustomPermissionCheck = (ids) => ids.Contains(HalvingMetallurgy.Instance.GetIdentifier(HalvingMetallurgy.HalvesPermission))
        };

        Quake = new()
        {
            Id = HalvingMetallurgy.Instance.GetIdentifier("quake"),
            cost = 15,
            glowTexture = Assets.textures.select.single_glow,
            strokeTexture = Assets.textures.select.single_stroke,
            iconTexture = Textures.Icons.Quake,
            iconHoverTexture = Textures.Icons.QuakeHover,
            glyphHexes = [
                quakeBowlHex
            ],
            isFullHexCover = true,
            CustomPermissionCheck = (ids) => ids.Contains(HalvingMetallurgy.Instance.GetIdentifier(HalvingMetallurgy.QuakePermission))
        };

        Sump = new()
        {
            Id = HalvingMetallurgy.Instance.GetIdentifier("sump"),
            cost = 15,
            glowTexture = Assets.textures.select.double_glow,
            strokeTexture = Assets.textures.select.double_stroke,
            iconTexture = Textures.Icons.Sump,
            iconHoverTexture = Textures.Icons.SumpHover,
            glyphHexes = [
                sumpInputHex,
                sumpOutputHex
            ],
            isFullHexCover = true,
            isUnique = true,
            CustomPermissionCheck = (ids) => ids.Contains(HalvingMetallurgy.Instance.GetIdentifier(HalvingMetallurgy.SumpPermission))
        };

        Remission = new()
        {
            Id = HalvingMetallurgy.Instance.GetIdentifier("remission"),
            cost = 30,
            glowTexture = Assets.textures.select.diamond_glow,
            strokeTexture = Assets.textures.select.diamond_stroke,
            iconTexture = Textures.Icons.Remission,
            iconHoverTexture = Textures.Icons.RemissionHover,
            glyphHexes = [
                remissionInput1Hex,
                remissionInput2Hex,
                remissionBowlHex,
                remissionOutputHex
            ],
            isFullHexCover = true,
            CustomPermissionCheck = (ids) => ids.Contains(HalvingMetallurgy.Instance.GetIdentifier(HalvingMetallurgy.RemissionPermission))
        };

        Shearing = new()
        {

            Id = HalvingMetallurgy.Instance.GetIdentifier("shearing"),
            cost = 25,
            glowTexture = Assets.textures.select.double_glow,
            strokeTexture = Assets.textures.select.double_stroke,
            iconTexture = Textures.Icons.Shearing,
            iconHoverTexture = Textures.Icons.ShearingHover,
            glyphHexes = [
                shearingBowlHex,
                shearingOutputHex
            ],
            isFullHexCover = true,
            CustomPermissionCheck = (ids) => ids.Contains(HalvingMetallurgy.Instance.GetIdentifier(HalvingMetallurgy.ShearingPermission))
        };

        Osmosis = new()
        {
            Id = HalvingMetallurgy.Instance.GetIdentifier("osmosis"),
            cost = 25,
            glowTexture = Assets.textures.select.double_glow,
            strokeTexture = Assets.textures.select.double_stroke,
            iconTexture = Textures.Icons.Osmosis,
            iconHoverTexture = Textures.Icons.OsmosisHover,
            glyphHexes = [
                osmosisMetalHex,
                osmosisQuixHex
            ],
            isFullHexCover = true,
            CustomPermissionCheck = (ids) => ids.Contains(HalvingMetallurgy.Instance.GetIdentifier(HalvingMetallurgy.OsmosisPermission))
        };

        Soria = new(
            HalvingMetallurgy.Instance,
            "soria",
            Textures.Icons.Soria,
            Textures.Icons.SoriaHover,
            [
                AtomTypes.quicksilver,
                Atoms.Quickcopper,
                AtomTypes.quicksilver,
                Atoms.Quickcopper,
                AtomTypes.quicksilver,
                Atoms.Quickcopper
            ],
            30,
            HalvingMetallurgy.SoriaPermission
        );

        /*
        Harmony = Brimstone.API.CreateSimpleGlyph(
            ID: "halving-metallurgy-harmony",
            name: "Glyph of Harmony",
            description: "The glyph of harmony does nothing expect stay where it is placed, some alchemists may place it in an appeasing spot.",
            cost: 0,
            glow: class_238.field_1989.field_97.field_382,
            stroke: class_238.field_1989.field_97.field_383,
            icon: class_238.field_1989.field_90.field_245.field_309,
            hoveredIcon: class_238.field_1989.field_90.field_245.field_310,
            usedHexes: new HexIndex[] { new(0, 0) },
            customPermission: HalvingMetallurgy.HarmonyPermission
        );
        */
        #endregion

        QApi.AddPartTypeToPanel(Halves, false);
        QApi.AddPartTypeToPanel(Quake, false);
        QApi.AddPartTypeToPanel(Sump, false);
        QApi.AddPartTypeToPanel(Remission, false);
        QApi.AddPartTypeToPanel(Shearing, false);
        QApi.AddPartTypeToPanel(Osmosis, false);
        //QApi.AddPartTypeToPanel(Harmony, false);

        QApi.AddPartType(HalvingMetallurgy.Instance, Halves, "halves");
        QApi.AddPartType(HalvingMetallurgy.Instance, Quake, "quake");
        QApi.AddPartType(HalvingMetallurgy.Instance, Sump, "sump");
        QApi.AddPartType(HalvingMetallurgy.Instance, Remission, "remission");
        QApi.AddPartType(HalvingMetallurgy.Instance, Shearing, "shearing");
        QApi.AddPartType(HalvingMetallurgy.Instance, Osmosis, "osmosis");
        //QApi.AddPartType(Harmony, HarmonyRenderer);
    }

    public static void DefinePartRendering()
    {
        QApi.AddPartTypesRenderer(HalvingMetallurgy.Instance, HalvesRenderer, (part) => part.GetType() == Halves);
        QApi.AddPartTypesRenderer(HalvingMetallurgy.Instance, QuakeRenderer, (part) => part.GetType() == Quake);
        QApi.AddPartTypesRenderer(HalvingMetallurgy.Instance, SumpRenderer, (part) => part.GetType() == Sump);
        QApi.AddPartTypesRenderer(HalvingMetallurgy.Instance, RemissionRenderer, (part) => part.GetType() == Remission);
        QApi.AddPartTypesRenderer(HalvingMetallurgy.Instance, ShearingRenderer, (part) => part.GetType() == Shearing);
        QApi.AddPartTypesRenderer(HalvingMetallurgy.Instance, OsmosisRenderer, (part) => part.GetType() == Osmosis);
    }

    public static void DefinePartBehaviors()
    {
        QApi.AddCycleEvent(new(CycleEvent.CycleEventExecutionType.BeforeEarlyGlyphs, ExtraBehavior));
        QApi.BindGlyphCycle(Halves, new PartCycleDelegate(PartCycleDelegate.PartCycleExecutionType.Normal, HalvesBehavior));
        LApi.AddHandler(Sump.Id, HalvingMetallurgy.Instance.GetIdentifier("sump_native_handler"), SumpBehavior);
        LApi.AddHandler(Remission.Id, HalvingMetallurgy.Instance.GetIdentifier("remission_projection_native_handler"), RemissionStateMachineBehavior);
        QApi.BindGlyphCycle(Remission, new PartCycleDelegate(PartCycleDelegate.PartCycleExecutionType.Normal, RemissionBehavior));
        QApi.BindGlyphCycle(Shearing, new PartCycleDelegate(PartCycleDelegate.PartCycleExecutionType.Normal, ShearingBehavior));
        QApi.BindGlyphCycle(Osmosis, new PartCycleDelegate(PartCycleDelegate.PartCycleExecutionType.Normal, OsmosisBehavior));

        LApi.AddHandler(PartTypes.projectionGlyph.Id, HalvingMetallurgy.Instance.GetIdentifier("soria_handler"), ProjectionSoriaBehavior);
    }

    private static void ExtraBehavior(Sim sim, CycleEvent.CycleEventExecutionType executionType)
    {
        SolutionEditorBase seb = sim.solutionEditor;
        foreach (Part part in sim.GetSolution().parts)
        {
            PartType partType = part.GetType();
            PartSimState pss = sim.simulationDict[part];
            if (partType == Quake)
            {
                QuakeBehavior(sim, part, pss, seb);
            }
        }
    }

    public static void HalvesRenderer(Part part, Vector2 pos, SolutionEditorBase editor, PartRenderer renderer)
    {
        PartSimState pss = editor.GetSimulation().GetSimState(part);

        Vector2 pivot = new(82f, 48f);
        Vector2 offset = new(-1f, -1f);

        renderer.RenderBase(Textures.Halves.Base, offset, pivot, 0f);
        // quicksilver
        renderer.RenderRotating(Assets.textures.parts.projection_glyph.quicksilver_input, halvesInputHex, Vector2.Zero);
        renderer.RenderUpright(Assets.textures.parts.projection_glyph.quicksilver_symbol, halvesInputHex, Vector2.Zero);
        // bowls
        renderer.RenderRotating(Assets.textures.parts.projection_glyph.metal_bowl, halvesMetal1Hex, Vector2.Zero);
        renderer.RenderUpright(Assets.textures.parts.projection_glyph.lead_symbol, halvesMetal1Hex, Vector2.Zero);
        renderer.RenderRotating(Assets.textures.parts.projection_glyph.metal_bowl, halvesMetal2Hex, Vector2.Zero);
        renderer.RenderUpright(Assets.textures.parts.projection_glyph.lead_symbol, halvesMetal2Hex, Vector2.Zero);

        renderer.RenderBase(Textures.Halves.Connectors, new Vector2(-32f, 10f), 0f);
        renderer.RenderBase(Textures.Halves.Connectors, new Vector2(-33f, 10f), (float)(2d * Math.PI / 3d));
        renderer.RenderGloss(Assets.textures.parts.dispersion.gloss, Textures.Halves.GlossMask, pivot);
    }
    public static bool HalvesBehavior(Sim sim, Part part, PartSimState pss, GlyphRecipe recipe, bool first)
    {
        if (!recipe.Predicate.InvokeAndClear(sim, part))
        {
            return false;
        }
        SolutionEditorBase seb = sim.solutionEditor;
        AtomReference quicksilver = (AtomReference)sim.RecipeInputs[halvesInputHex];
        if (sim.RecipeOutputs.TryGetValue(halvesInputHex, out IRecipeOutput temp) && temp is AtomType newQuicksilver)
        {
            // assume Soria's wheel
            quicksilver.molecule.ReplaceAtom(newQuicksilver, quicksilver.pos);
            quicksilver.atom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, quicksilver.atomType, Assets.textures.atoms.rejection_effect_metal, 7.5f, 5);
            seb.glyphEffects.Add(new GlyphEffect(
                seb,
                EffectTimescaleType.RealTime,
                HexGrid.standardGrid.ToPixelCoords(part.GetHexPos()),
                Textures.Soria.Flash,
                30f,
                Vector2.Zero,
                0f
            ));
        }
        else
        {
            sim.molecules.Remove(quicksilver.molecule);
            seb.DrawFallingAtom(quicksilver);
        }
        AtomReference metal1Ref = (AtomReference)sim.RecipeInputs[halvesMetal1Hex];
        AtomType newMetal1 = (AtomType)sim.RecipeOutputs[halvesMetal1Hex];
        AtomReference metal2Ref = (AtomReference)sim.RecipeInputs[halvesMetal2Hex];
        AtomType newMetal2 = (AtomType)sim.RecipeOutputs[halvesMetal2Hex];
        // Promote the metals
        metal1Ref.molecule.ReplaceAtom(newMetal1, metal1Ref.pos);
        metal2Ref.molecule.ReplaceAtom(newMetal2, metal2Ref.pos);
        // Play promotion 
        HexIndex bowl1 = part.GlobalPosFromLocal(halvesMetal1Hex);
        HexIndex bowl2 = part.GlobalPosFromLocal(halvesMetal2Hex);
        metal1Ref.atom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, metal1Ref.atomType, Assets.textures.atoms.projection_effect, 7.5f, 7);
        metal2Ref.atom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, metal2Ref.atomType, Assets.textures.atoms.projection_effect, 7.5f, 7);

        // bowl effects
        seb.glyphEffects.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, HexGrid.standardGrid.ToPixelCoords(bowl2), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
        seb.glyphEffects.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, HexGrid.standardGrid.ToPixelCoords(bowl1), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
        //play engraving flash
        float theta = part.GetRotation().ToRadians();
        Vector2 pivot = HexGrid.standardGrid.ToPixelCoords(part.GetHexPos());

        seb.glyphEffects.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, pivot + new Vector2(41f, 1f).Rotated(theta), Textures.Halves.ConnectorFlash, 30f, Vector2.Zero, theta) { Color = Atoms.QuickcopperColor });
        theta += (float)(2 * Math.PI / 3);
        seb.glyphEffects.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, pivot + new Vector2(42f, 1f).Rotated(theta), Textures.Halves.ConnectorFlash, 30f, Vector2.Zero, theta) { Color = Atoms.QuickcopperColor });

        // Play custom sound
        sim.PlaySound(Sounds.Halves);
        return true;
    }

    public static void QuakeRenderer(Part part, Vector2 pos, SolutionEditorBase editor, PartRenderer renderer)
    {
        PartSimState pss = editor.GetSimulation().GetSimState(part);
        float time = editor.GetCycleTime();
        Vector2 pivot = new(41f, 49f);
        Vector2 offset = new(0f, 0f);
        renderer.RenderBase(Textures.Quake.Base, Vector2.Zero, pivot, 0f);
        if (pss.isProcessing && time < 0.75f)
        {
            float x = (float)(10.6666667 * time % 4.0);
            // "I want a sinusoid!"
            // "We have sinusoid at home"
            // Sinusoid at home:
            float shakeX = (float)(-Math.Pow(x, 5) + (10 * Math.Pow(x, 4)) - (28 * Math.Pow(x, 3)) + (8 * Math.Pow(x, 2)) + (32 * x)) / 7;
            offset += new Vector2(shakeX, 0f);
        }
        renderer.RenderRotating(Textures.Quake.Bowl, quakeBowlHex, offset);
        renderer.RenderUpright(Textures.Quake.SednumSymbol, quakeBowlHex, offset);
    }
    public static void QuakeBehavior(Sim sim, Part part, PartSimState pss, SolutionEditorBase seb)
    {
        HexIndex bowl = part.GlobalPosFromLocal(quakeBowlHex);
        Molecule moleculeAboveBowl = null;
        foreach (Molecule molecule in sim.molecules)
        {
            if (molecule.GetAtoms().Keys.Contains(bowl))
            {
                moleculeAboveBowl = molecule;
                break;
            }
        }
        if (moleculeAboveBowl is not null)
        {
            // are any parts holding this molecule?
            if (sim.HeldGrippers.Any(p => sim.GetHeldMolecule(p).GetOrDefault(out Molecule held) && held == moleculeAboveBowl))
            {
                return;
            }

            List<HexIndexPair> removals = [];
            List<HexIndexPair> additions = [];
            List<HexIndexPair> resists = [];
            // for each atom in a molecule
            IReadOnlyDictionary<HexIndex, Atom> atoms = moleculeAboveBowl.GetAtoms();
            foreach (KeyValuePair<HexIndex, Atom> entry in atoms)
            {
                if (AtomTag.AtomTags["halving_metallurgy:quake_sednum_like"].HasAtom(entry.Value.atomType))
                {
                    List<HexIndexPair> toRemove = [];
                    foreach (var bond in moleculeAboveBowl.GetBonds().Where(bond => bond.hexPos1 == entry.Key || bond.hexPos2 == entry.Key))
                    {
                        if (atoms.TryGetValue(bond.hexPos1 == entry.Key ? bond.hexPos2 : bond.hexPos1, out Atom other))
                        {
                            if (AtomTag.AtomTags["halving_metallurgy:quake_sednum_suppressor"].HasAtom(other.atomType))
                            {
                                // sednum is bonded to an atom that suppresses it   
                                toRemove.Clear();
                                break;
                            }
                            else if (AtomTag.AtomTags["halving_metallurgy:quake_zinc_like"].HasAtom(other.atomType) || AtomTag.AtomTags["halving_metallurgy:quake_sednum_immune"].HasAtom(other.atomType))
                            {
                                continue;
                            }
                        }
                        IReadOnlyList<BondType> bondTypes = bond.GetBondTypes();
                        if (bondTypes.Count >= 3)
                        {
                            continue;
                        }
                        HexIndexPair pair = new(bond.hexPos1, bond.hexPos2);
                        if (bondTypes.Count == 1 && bondTypes[0].Id == "om:standard" && QuakeNormalBonders.Any(p => p == pair))
                        {
                            resists.Add(pair);
                        }
                        else
                        {
                            toRemove.Add(pair);
                        }
                    }
                    foreach (var removed in toRemove)
                    {
                        removals.Add(removed);
                    }
                }
                else if (AtomTag.AtomTags["halving_metallurgy:quake_zinc_like"].HasAtom(entry.Value.atomType))
                {
                    foreach (HexIndex offset in HexIndex.AdjacentOffsets)
                    {
                        HexIndex zincPos = entry.Key;
                        HexIndex neighborPos = zincPos + offset;
                        if (!sim.GetMoleculeAt(neighborPos).GetOrDefault(out Molecule otherMolecule))
                        {
                            // no molecule adjacent.
                            continue;
                        }
                        Atom neighbor = otherMolecule.GetAtoms()[neighborPos];
                        if (AtomTag.AtomTags["halving_metallurgy:quake_sednum_like"].HasAtom(neighbor.atomType))
                        {
                            // Sednum vs. Zinc is tie
                            continue;
                        }
                        if (moleculeAboveBowl != otherMolecule || !moleculeAboveBowl.GetBondAt(zincPos, neighborPos).HasValue())
                        {
                            additions.Add(new(zincPos, neighborPos));
                        }
                    }
                }
            }

            // only active if not idempotent
            if (removals.Count == 0 && additions.Count == 0)
            {
                return;
            }
            foreach (HexIndexPair pair in resists.Distinct())
            {
                Vector2 midpoint = Utils.InterpolateVect(HexGrid.standardGrid.ToPixelCoords(pair.Left), HexGrid.standardGrid.ToPixelCoords(pair.Right), 0.5f);
                float angle = HexGrid.standardGrid.ToPixelCoords(pair.Right - pair.Left).Angle();
                seb.glyphEffects.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, midpoint, Textures.Quake.UnbondResistedAnimation, 75f, new Vector2(1.5f, -5f), angle));
            }
            foreach (HexIndexPair pair in additions.Distinct())
            {
                if (!sim.GetMoleculeAt(pair.Left).GetOrDefault(out Molecule L) || !sim.GetMoleculeAt(pair.Right).GetOrDefault(out Molecule R))
                {
                    continue;
                }
                moleculeAboveBowl = sim.CombineMolecules(L, R);
                moleculeAboveBowl.AddBond(BondTypes.standard, pair.Left, pair.Right, new BondEffect(seb, BondTypes.standard.bondTexture.bondAnim, 60f, BondTypes.standard.bondTexture.textureOffset));
            }

            foreach (HexIndexPair pair in removals.Distinct())
            {
                if (moleculeAboveBowl.GetBondAt(pair.Left, pair.Right).GetOrDefault(out Bond bond))
                {
                    foreach (BondType bondType in bond.GetBondTypes())
                    {
                        Vector2 midpoint = Utils.InterpolateVect(HexGrid.standardGrid.ToPixelCoords(pair.Left), HexGrid.standardGrid.ToPixelCoords(pair.Right), 0.5f);
                        float angle = HexGrid.standardGrid.ToPixelCoords(pair.Right - pair.Left).Angle();
                        seb.underneathPartsGlyphEffects.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, midpoint, bondType.unbondAnim, 75f, bondType.bondTexture.textureOffset, angle));
                    }
                    moleculeAboveBowl.RemoveBond(pair.Left, pair.Right);
                }
            }
            moleculeAboveBowl.shouldRecomputeConnectivity = true;

            pss.isProcessing = true;
            // play a buzzing sound
            sim.PlaySound(Sounds.Quake);
            return;
        }
        return;
    }

    public static void SumpRenderer(Part part, Vector2 pos, SolutionEditorBase editor, PartRenderer renderer)
    {
        PartSimState pss = editor.GetSimulation().GetSimState(part);
        IntermediatePartState ips = editor.GetIntermState(part, pos);
        float time = editor.GetCycleTime();

        // A dictionary that acts like the original object, and also allow extra data to be added,
        // It also magically remembers data put in it, even across multiple instantiations.
        DynamicData dyn_pss = new(pss);
        object stateOb = dyn_pss.Get("state");
        SumpState state = stateOb is not null ? (SumpState)stateOb : new();
        Vector2 pivot = new(41f, 48f);
        renderer.RenderBase(Textures.GlyphCommon.DoubleBase, new(-1f, -1f), pivot, 0f);
        // quicksilver
        renderer.RenderRotating(Assets.textures.parts.projection_glyph.quicksilver_input, sumpInputHex, Vector2.Zero);
        renderer.RenderUpright(Assets.textures.parts.projection_glyph.quicksilver_symbol, sumpInputHex, Vector2.Zero);

        // input ring
        if (state.DrainFlash)
        {
            int flashFrame = (int)(8 * time);
            flashFrame = flashFrame > 7 ? 7 : flashFrame;
            renderer.RenderUpright(Textures.Sump.DrainFlashAnimation[flashFrame], sumpInputHex, Vector2.Zero);
        }

        renderer.RenderIris(ips, sumpOutputHex, time, Textures.Irises.Quicksilver, state.QuicksilverEject ? AtomTypes.quicksilver : null);
    }
    public static void SumpBehavior(Sim sim, Part part, PartSimState pss, bool first)
    {
        SolutionEditorBase seb = sim.solutionEditor;
        DynamicData data = new(pss);
        object stateOb = data.Get("state");
        SumpState state = stateOb is not null ? (SumpState)stateOb : new();
        if (first)
        {
            if (!sim.FindAtomRelative(part, sumpInputHex).GetOrDefault(out AtomReference quicksilver) || quicksilver.isHeldByArm || quicksilver.inMultiAtomMolecule)
            {
                goto trySumpDrain;
            }

            // Delete the quicksilver
            quicksilver.molecule.RemoveAtom(quicksilver.pos);
            // Play deletion animation
            seb.DrawFallingAtom(quicksilver);

            if (state.QuicksilverCount < 5)
            {
                state.QuicksilverCount++;
                sim.PlaySound(Sounds.SumpConsume);
            }
            else
            {
                state.DrainFlash = true;
                sim.PlaySound(Sounds.SumpDiscard);
            }
        trySumpDrain:
            if (state.QuicksilverCount > 0 && !sim.FindAtomRelative(part, sumpOutputHex).HasValue())
            {
                state.QuicksilverEject = true;
                state.QuicksilverCount--;
                sim.SpawnColliderRelative(part, sumpOutputHex);
                sim.PlaySound(Sounds.SumpEject);
            }
        }
        else
        {
            state.DrainFlash = false;
            if (state.QuicksilverEject)
            {
                state.QuicksilverEject = false;
                sim.SpawnAtomRelative(AtomTypes.quicksilver, part, sumpOutputHex);
            }
        }
        data.Set("state", state);
        return;
    }

    public static void RemissionRenderer(Part part, Vector2 pos, SolutionEditorBase editor, PartRenderer renderer)
    {
        PartSimState pss = editor.GetSimulation().GetSimState(part);
        IntermediatePartState ips = editor.GetIntermState(part, pos);
        float time = editor.GetCycleTime();

        DynamicData data = new(pss);
        object stateOb = data.Get("state");
        RemissionState state = (stateOb is null) ? new RemissionState() : (RemissionState)stateOb;
        Vector2 pivot = new(41f, 120f);
        Vector2 offset = new(-1f, -1f);
        renderer.RenderBase(Assets.textures.parts.animismus.@base, offset, pivot, 0f);
        Texture leadSymbol = Assets.textures.parts.purification_glyph.lead_symbol;
        int frame = 0;
        bool beforeIrisCloses = false;
        Molecule mol = null;
        if (pss.isProcessing)
        {
            mol = Molecule.CreateMonoatomic(pss.processingAtoms[0]);
        }

        if (state.irisState == 3)
        {
            // falling and closing
            frame = Utils.Clamp((int)(Utils.InterpolateLinear(-1f, 1f, time) * 16), 0, 15);
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
            frame = Utils.Clamp((int)(Utils.InterpolateLinear(1f, -1f, time) * 16), 0, 15);
        }

        // input
        foreach (HexIndex inputHex in new HexIndex[] { remissionInput1Hex, remissionInput2Hex })
        {
            renderer.RenderRotating(Assets.textures.parts.animismus.input, inputHex, Vector2.Zero);
            if (state.irisState != 2)
            {
                renderer.RenderUpright(leadSymbol, inputHex, Vector2.Zero);
            }

            Vector2 molOffset = HexGrid.standardGrid.ToPixelCoords(inputHex).Rotated(ips.rotation) + ips.pos;
            if (pss.isProcessing && !beforeIrisCloses)
            {
                Editor.RenderMolecule(mol, molOffset, new(0, 0), 0f, 1f, 1f - time, 1f, false, null);
            }
            renderer.RenderUpright(Textures.Irises.Crossout[frame], inputHex, Vector2.Zero);
            if (pss.isProcessing && beforeIrisCloses)
            {
                Editor.RenderMolecule(mol, molOffset, new(0, 0), 0f, 1f, 1f - time, 1f, false, null);
            }
        }
        // bowl
        renderer.RenderRotating(Assets.textures.parts.projection_glyph.metal_bowl, remissionBowlHex, Vector2.Zero);
        renderer.RenderUpright(Assets.textures.parts.projection_glyph.lead_symbol, remissionBowlHex, Vector2.Zero);
        if (!state.IsEmpty)
        {
            int glowFrame = (int)(Time.Now().Ticks / timerFactor) & 0x3f;
            renderer.RenderRotating(Textures.Remission.BowlGlow[glowFrame], remissionBowlHex, Vector2.Zero);
        }
        // iris
        renderer.RenderIris(ips, remissionOutputHex, time, pss.isProcessing ? pss.processingAtoms[1] : null);

        renderer.RenderBase(Textures.Remission.Connectors, pivot);
    }
    public static bool RemissionBehavior(Sim sim, Part part, PartSimState pss, GlyphRecipe recipe, bool first)
    {
        if (!pss.isProcessing)
        {
            DynamicData data = new(pss);
            object stateOb = data.Get("state");
            RemissionState state = stateOb is not null ? (RemissionState)stateOb : new RemissionState();
            if (state.irisState == 0 && first && recipe.Predicate.InvokeAndClear(sim, part))
            {
                AtomReference metal1 = (AtomReference)sim.RecipeInputs[remissionInput1Hex];
                AtomReference metal2 = (AtomReference)sim.RecipeInputs[remissionInput2Hex];
                AtomType promotion = (AtomType)sim.RecipeOutputs[remissionOutputHex];
                metal1.molecule.RemoveAtom(metal1.pos);
                metal2.molecule.RemoveAtom(metal2.pos);
                pss.isProcessing = true;
                pss.processingAtoms = [metal1.atomType, promotion];
                data = new(pss);
                stateOb = data.Get("state");
                state = stateOb is not null ? (RemissionState)stateOb : new RemissionState();
                sim.PlaySound(Sounds.Remission);
                sim.SpawnColliderRelative(part, remissionOutputHex);
                if (!state.IsEmpty)
                {
                    state.irisState = 3;
                }
                data.Set("state", state);
                return true;
            }
        }
        else if (!pss.wasActivated)
        {
            sim.SpawnAtomRelative(pss.processingAtoms[1], part, remissionOutputHex);
            return true;
        }
        return false;
    }
    public static void RemissionStateMachineBehavior(Sim sim, Part part, PartSimState pss, bool first)
    {
        SolutionEditorBase seb = sim.solutionEditor;
        DynamicData data = new(pss);
        object stateOb = data.Get("state");
        RemissionState state = stateOb is not null ? (RemissionState)stateOb : new RemissionState();
        if (first)
        {
            if (state.IsEmpty)
            {
                return;
            }

            if (state.CanFullPromote)
            {
                if (GlyphRecipe.Recipes.TryGetValue(HalvingMetallurgy.Instance.GetIdentifier("remission_full_promote"), out OrderedDictionary<Identifier, GlyphRecipe> fullPromotionRecipes))
                {
                    foreach (GlyphRecipe recipe in fullPromotionRecipes.Values)
                    {
                        if (recipe.Predicate.InvokeAndClear(sim, part))
                        {
                            state.halfProjections -= 2;
                            sim.PlaySound(Sounds.RemissionPromote);
                            goto remissionChangeBowl;
                        }
                    }
                }
            }
            if (state.CanHalfPromote)
            {
                if (GlyphRecipe.Recipes.TryGetValue(HalvingMetallurgy.Instance.GetIdentifier("remission_half_promote"), out OrderedDictionary<Identifier, GlyphRecipe> halfPromotionRecipes))
                {
                    foreach (GlyphRecipe recipe in halfPromotionRecipes.Values)
                    {
                        if (recipe.Predicate.InvokeAndClear(sim, part))
                        {
                            state.halfProjections -= 1;
                            sim.PlaySound(Sounds.RemissionHalfPromote);
                            goto remissionChangeBowl;
                        }
                    }
                }
            }
            if (state.CanHalfDemote)
            {
                if (!GlyphRecipe.Recipes.TryGetValue(HalvingMetallurgy.Instance.GetIdentifier("remission_half_demote"), out OrderedDictionary<Identifier, GlyphRecipe> halfDemotionRecipes))
                {
                    foreach (GlyphRecipe recipe in halfDemotionRecipes.Values)
                    {
                        if (recipe.Predicate.InvokeAndClear(sim, part))
                        {
                            state.halfProjections += 1;
                            sim.PlaySound(Sounds.RemissionHalfDemote);
                            goto remissionChangeBowl;
                        }
                    }
                }
            }
            goto remissionEnd;

        remissionChangeBowl:
            if (state.IsEmpty)
            {
                state.irisState = state.irisState == 2 ? 1 : 0;
            }
            AtomReference metalOnBowl = (AtomReference)sim.RecipeInputs[remissionBowlHex];
            AtomType projectionResult = (AtomType)sim.RecipeOutputs[remissionBowlHex];
            // Promote the metal
            metalOnBowl.molecule.ReplaceAtom(projectionResult, metalOnBowl.pos);
            // Play promotion animations
            seb.underneathPartsGlyphEffects.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, HexGrid.standardGrid.ToPixelCoords(part.GlobalPosFromLocal(remissionBowlHex)), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
            metalOnBowl.atom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, metalOnBowl.atomType, Assets.textures.atoms.projection_effect, 7.5f, 7);
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
        }
    remissionEnd:
        data.Set("state", state);
    }

    public static void ShearingRenderer(Part part, Vector2 pos, SolutionEditorBase editor, PartRenderer renderer)
    {
        Vector2 pivot = new(41f, 48f);
        Vector2 offset = new(-1f, -1f);

        renderer.RenderBase(Textures.GlyphCommon.DoubleBase, offset, pivot, 0f);
        renderer.RenderRotating(Assets.textures.parts.projection_glyph.metal_bowl, shearingBowlHex, Vector2.Zero);
        renderer.RenderUpright(Textures.Shearing.SednumLine, osmosisMetalHex, Vector2.Zero);

        renderer.RenderRotating(Assets.textures.parts.projection_glyph.metal_bowl, shearingOutputHex, Vector2.Zero);
    }

    public static bool ShearingBehavior(Sim sim, Part part, PartSimState pss, GlyphRecipe recipe, bool first)
    {
        if (!recipe.Predicate.InvokeAndClear(sim, part))
        {
            return false;
        }

        AtomReference bowlAtom = (AtomReference)sim.RecipeInputs[shearingBowlHex];
        AtomType newBowlAtom = (AtomType)sim.RecipeOutputs[shearingBowlHex];
        AtomType outputAtom = (AtomType)sim.RecipeOutputs[shearingOutputHex];
        bool madeQuickcopper = Atoms.quickcopperRadioactive && (newBowlAtom == Atoms.Quickcopper || outputAtom == Atoms.Quickcopper);

        SolutionEditorBase seb = sim.solutionEditor;

        bowlAtom.molecule.ReplaceAtom(newBowlAtom, bowlAtom.pos);
        bowlAtom.atom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, bowlAtom.atomType, Textures.Shearing.Split, 7f, 7);
        seb.underneathPartsGlyphEffects.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, HexGrid.standardGrid.ToPixelCoords(part.GetHexPos()), Textures.Shearing.BowlFlash, 30f, Vector2.Zero, 0f));
        Atom spawnedAtom = sim.SpawnAtomRelative(outputAtom, part, shearingOutputHex);
        spawnedAtom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, outputAtom, Textures.Shearing.Spawn, 7.5f);

        sim.PlaySound(madeQuickcopper ? Sounds.ShearingMakingQuickcopper : Sounds.Shearing);
        return true;
    }
    public static void OsmosisRenderer(Part part, Vector2 pos, SolutionEditorBase editor, PartRenderer renderer)
    {
        Vector2 pivot = new(41f, 48f);
        Vector2 offset = new(-1f, -1f);

        renderer.RenderBase(Textures.GlyphCommon.DoubleBase, offset, pivot, 0f);
        renderer.RenderRotating(Assets.textures.parts.projection_glyph.metal_bowl, osmosisMetalHex, Vector2.Zero);
        renderer.RenderUpright(Textures.Osmosis.OsmiumDemote, osmosisMetalHex, Vector2.Zero);
        renderer.RenderRotating(Assets.textures.parts.projection_glyph.metal_bowl, osmosisQuixHex, Vector2.Zero);
        renderer.RenderUpright(Textures.Osmosis.QuickcopperPromote, osmosisQuixHex, Vector2.Zero);

        renderer.RenderBase(Textures.Osmosis.Connector, new Vector2(-28f, 23f), 0f);
    }
    public static bool OsmosisBehavior(Sim sim, Part part, PartSimState pss, GlyphRecipe recipe, bool first)
    {
        if (first)
        {
            return true;
        }
        if (recipe.Predicate.InvokeAndClear(sim, part))
        {
            AtomReference metal = (AtomReference)sim.RecipeInputs[osmosisMetalHex];
            AtomReference quix = (AtomReference)sim.RecipeInputs[osmosisQuixHex];
            AtomType newMetal = (AtomType)sim.RecipeOutputs[osmosisMetalHex];
            AtomType newQuix = (AtomType)sim.RecipeOutputs[osmosisQuixHex];

            metal.molecule.ReplaceAtom(newMetal, metal.pos);
            quix.molecule.ReplaceAtom(newQuix, quix.pos);

            SolutionEditorBase seb = sim.solutionEditor;
            pss.isProcessing = true;
            seb.glyphEffects.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, HexGrid.standardGrid.ToPixelCoords(part.GlobalPosFromLocal(osmosisMetalHex)), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
            metal.atom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, metal.atomType, Assets.textures.atoms.rejection_effect_metal, 7f, 7);
            seb.glyphEffects.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, HexGrid.standardGrid.ToPixelCoords(part.GlobalPosFromLocal(osmosisQuixHex)), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
            quix.atom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, quix.atomType, Assets.textures.atoms.projection_effect, 7f, 7);
            sim.PlaySound(Sounds.Osmosis);
            return true;
        }

        return false;
    }
    /*
    public static void HarmonyRenderer(Part part, Vector2 pos, SolutionEditorBase editor, class_195 renderer)
    {
        Brimstone.API.GetRenderingHelpers(part, pos, editor, out PartSimState pss, out class_236 partDataWrapper, out float time);
        Vector2 pivot = new(42f, 49f);
        renderer.method_521(class_238.field_1989.field_90.field_185, pivot);
        HexIndex center = new(0, 0);
        renderer.RenderRotating(class_238.field_1989.field_90.field_187, center, Vector2.Zero);
        renderer.method_527(Textures.Harmony.TrefoilIndent, center, Vector2.Zero, pivot, 0f);
    }
    */
    public static void ProjectionSoriaBehavior(Sim sim, Part part, PartSimState pss, bool first)
    {
        SolutionEditorBase seb = sim.solutionEditor;
        if (pss.wasActivated)
        {
            // don't double-activate
            return;
        }
        if (GlyphRecipe.Recipes.TryGetValue(HalvingMetallurgy.Instance.GetIdentifier("projection"), out OrderedDictionary<Identifier, GlyphRecipe> recipes)) {
            HexIndex hole = new(0, 0);
            HexIndex bowl = new(1, 0);
            foreach (GlyphRecipe recipe in recipes.Values)
            {
                if (recipe.Predicate.InvokeAndClear(sim, part))
                {
                    AtomReference quix = (AtomReference)sim.RecipeInputs[hole];
                    AtomReference metal = (AtomReference)sim.RecipeInputs[bowl];
                    AtomType newQuix = (AtomType)sim.RecipeOutputs[hole];
                    AtomType newMetal = (AtomType)sim.RecipeOutputs[bowl];

                    quix.molecule.ReplaceAtom(newQuix, quix.pos);
                    metal.molecule.ReplaceAtom(newMetal, metal.pos);

                    quix.atom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, quix.atomType, Assets.textures.atoms.rejection_effect_metal, 7.5f, 5);
                    seb.glyphEffects.Add(new GlyphEffect(
                        seb,
                        EffectTimescaleType.RealTime,
                        HexGrid.standardGrid.ToPixelCoords(part.GetHexPos()),
                        Textures.Soria.Flash,
                        30f,
                        Vector2.Zero,
                        0f
                    ));

                    metal.atom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, metal.atomType, Assets.textures.atoms.projection_effect, 7.5f, 7);
                    seb.glyphEffects.Add(new GlyphEffect(
                        seb,
                        EffectTimescaleType.RealTime,
                        HexGrid.standardGrid.ToPixelCoords(part.GlobalPosFromLocal(bowl)),
                        Assets.textures.parts.projection_glyph_flash,
                        30f,
                        Vector2.Zero,
                        part.GetRotation().ToRadians()
                    ));

                    pss.isProcessing = true;
                    sim.PlaySound(Assets.sounds.glyph_projection);
                    pss.wasActivated = true;
                    break;
                }
            }
        }
    }

}