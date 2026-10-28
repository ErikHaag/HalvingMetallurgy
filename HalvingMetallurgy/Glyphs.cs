#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable IDE0059 // Unnecessary assignment of a value

using HalvingMetallurgy.Utilities;
using MonoMod.Utils;
using Quintessential;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using static StringLoader;

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
    public static readonly HexIndex osmosisQuickcopperHex = new(1, 0);

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
                QuakeNormalBonders.Add(new HexIndexPair(part.InFrontBy(bonder.hexPos1), part.InFrontBy(bonder.hexPos2)));
            }
        }
    }

    public static void DefineParts()
    {

        #region Glyph Definitions

        Halves = new PartType()
        {
            Id = HalvingMetallurgy.Instance.GetIdentifier("halves"),
            cost = 30,
            glowTexture = Textures.Halves.Glow,
            strokeTexture = Textures.Halves.Stroke,
            baseTexture = Textures.Icons.Halves,
            hoverTexture = Textures.Icons.HalvesHover,
            glyphHexes = [
                halvesInputHex,
                halvesMetal1Hex,
                halvesMetal2Hex
            ],
            isFullHexCover = true,
            CustomPermissionCheck = (ids) => ids.Contains(HalvingMetallurgy.Instance.GetIdentifier(HalvingMetallurgy.HalvesPermission))
        };

        Quake = new PartType()
        {
            Id = HalvingMetallurgy.Instance.GetIdentifier("quake"),
            cost = 15,
            glowTexture = Assets.textures.select.single_glow,
            strokeTexture = Assets.textures.select.single_stroke,
            baseTexture = Textures.Icons.Quake,
            hoverTexture = Textures.Icons.QuakeHover,
            glyphHexes = [
                quakeBowlHex
            ],
            isFullHexCover = true,
            CustomPermissionCheck = (ids) => ids.Contains(HalvingMetallurgy.Instance.GetIdentifier(HalvingMetallurgy.QuakePermission))
        };

        Sump = new PartType()
        {
            Id = HalvingMetallurgy.Instance.GetIdentifier("sump"),
            cost = 15,
            glowTexture = Assets.textures.select.double_glow,
            strokeTexture = Assets.textures.select.double_stroke,
            baseTexture = Textures.Icons.Sump,
            hoverTexture = Textures.Icons.SumpHover,
            glyphHexes = [
                sumpInputHex,
                sumpOutputHex
            ],
            isFullHexCover = true,
            isUnique = true,
            CustomPermissionCheck = (ids) => ids.Contains(HalvingMetallurgy.Instance.GetIdentifier(HalvingMetallurgy.SumpPermission))
        };

        Remission = new PartType()
        {
            Id = HalvingMetallurgy.Instance.GetIdentifier("remission"),
            cost = 30,
            glowTexture = Assets.textures.select.diamond_glow,
            strokeTexture = Assets.textures.select.diamond_stroke,
            baseTexture = Textures.Icons.Remission,
            hoverTexture = Textures.Icons.RemissionHover,
            glyphHexes = [
                remissionInput1Hex,
                remissionInput2Hex,
                remissionBowlHex,
                remissionOutputHex
            ],
            isFullHexCover = true,
            CustomPermissionCheck = (ids) => ids.Contains(HalvingMetallurgy.RemissionPermission)
        };
        /*
        Shearing = Brimstone.API.CreateSimpleGlyph(
            ID: "halving-metallurgy-shearing",
            name: "Glyph of Shearing",
            description: "The glyph of shearing cuts a metal in half, collecting the swarf into the adjacent bowl.",
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
        //QApi.AddPartTypeToPanel(Remission, false);
        //QApi.AddPartTypeToPanel(Shearing, false);
        //QApi.AddPartTypeToPanel(Osmosis, false);
        //QApi.AddPartTypeToPanel(Harmony, false);

        QApi.AddPartType(HalvingMetallurgy.Instance, Halves, "halves");
        QApi.AddPartType(HalvingMetallurgy.Instance, Quake, "quake");
        QApi.AddPartType(HalvingMetallurgy.Instance, Sump, "sump");
        //QApi.AddPartType(Remission, RemissionRenderer);
        //QApi.AddPartType(Shearing, ShearingRenderer);
        //QApi.AddPartType(Osmosis, OsmosisRenderer);
        //QApi.AddPartType(Harmony, HarmonyRenderer);
    }

    public static void DefinePartRendering()
    {
        QApi.AddPartTypesRenderer(HalvingMetallurgy.Instance, HalvesRenderer, (part) => part.GetType() == Halves);
        QApi.AddPartTypesRenderer(HalvingMetallurgy.Instance, QuakeRenderer, (part) => part.GetType() == Quake);
        QApi.AddPartTypesRenderer(HalvingMetallurgy.Instance, SumpRenderer, (part) => part.GetType() == Sump);
    }

    public static void DefinePartBehaviors()
    {
        QApi.AddCycleEvent(new(CycleEvent.CycleEventExecutionType.BeforeEarlyGlyphs | CycleEvent.CycleEventExecutionType.AfterEarlyGlyphs | CycleEvent.CycleEventExecutionType.AfterLateGlyphs, ExtraBehavior));
        QApi.BindGlyphCycle(Halves, new PartCycleDelegate(PartCycleDelegate.PartCycleExecutionType.Normal, HalvesBehavior));
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
                if (executionType == CycleEvent.CycleEventExecutionType.BeforeEarlyGlyphs)
                {
                    QuakeBehavior(sim, part, pss, seb);
                }
            }
            else if (partType == Sump)
            {
                if (executionType == CycleEvent.CycleEventExecutionType.AfterEarlyGlyphs)
                {
                    SumpBehavior(sim, part, pss, true, seb);
                }
                else if (executionType == CycleEvent.CycleEventExecutionType.AfterLateGlyphs)
                {
                    SumpBehavior(sim, part, pss, false, seb);
                }
            }
            else if (partType == Remission)
            {
                if (executionType == CycleEvent.CycleEventExecutionType.AfterEarlyGlyphs)
                {
                    RemissionStateMachineBehaviour(sim, part, pss, true, seb);
                }
                else if (executionType == CycleEvent.CycleEventExecutionType.AfterLateGlyphs)
                {
                    RemissionStateMachineBehaviour(sim, part, pss, false, seb);
                }
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
            quicksilver.molecule.ReplaceAtom(newQuicksilver, quicksilver.pos);
            quicksilver.atom.transmutationEffect = new TransmutationEffect(seb, TransmutationEffectRenderMode.AsEffect, quicksilver.atomType, Assets.textures.atoms.projection_effect, 30f);
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
        HexIndex bowl1 = part.InFrontBy(halvesMetal1Hex);
        HexIndex bowl2 = part.InFrontBy(halvesMetal2Hex);
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
        HexIndex bowl = part.InFrontBy(quakeBowlHex);
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
                if (entry.Value.atomType == Atoms.Sednum)
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
                            else if (AtomTag.AtomTags["halving_metallurgy:quake_sednum_immune"].HasAtom(other.atomType))
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
                else if (entry.Value.atomType == Atoms.Zinc)
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
                        if (neighbor.atomType == Atoms.Sednum)
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
                        seb.field_3936.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, midpoint, bondType.unbondAnim, 75f, bondType.bondTexture.textureOffset, angle));
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
    public static void SumpBehavior(Sim sim, Part part, PartSimState pss, bool first, SolutionEditorBase seb)
    {
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

    public static void RemissionRenderer(Part part, Vector2 pos, SolutionEditorBase editor, class_195 renderer)
    {
        Brimstone.API.GetRenderingHelpers(part, pos, editor, out PartSimState pss, out class_236 partDataWrapper, out float time);

        DynamicData data = new(pss);
        object stateOb = data.Get("state");
        RemissionState state = (stateOb is null) ? new RemissionState() : (RemissionState)stateOb;
        Vector2 pivot = new(41f, 120f);
        Vector2 offset = new(-1f, -1f);
        renderer.method_523(class_238.field_1989.field_90.field_228.field_265, offset, pivot, 0f);
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
            renderer.RenderRotating(Assets.textures.parts., inputHex, Vector2.Zero);
            if (state.irisState != 2)
            {
                class_135.method_272(leadSymbol, (class_187.field_1742.method_491(inputHex, Vector2.Zero).Rotated(partDataWrapper.field_1985) + partDataWrapper.field_1984 - (leadSymbol.field_2056.ToVector2() / 2)).Rounded());
            }

            Vector2 molOffset = partDataWrapper.field_1984 + Brimstone.API.HexIndexToVector2(inputHex).Rotated(partDataWrapper.field_1985);
            if (pss.field_2743 && !beforeIrisCloses)
            {
                Editor.method_925(mol, molOffset, new(0, 0), 0f, 1f, 1f - time, 1f, false, null);
            }
            renderer.RenderUpright(Textures.Irises.Crossout[frame], inputHex, Vector2.Zero);
            if (pss.field_2743 && beforeIrisCloses)
            {
                Editor.method_925(mol, molOffset, new(0, 0), 0f, 1f, 1f - time, 1f, false, null);
            }
        }
        // bowl
        renderer.RenderRotating(class_238.field_1989.field_90.field_255.field_292, remissionBowlHex, Vector2.Zero);
        renderer.RenderUpright(class_238.field_1989.field_90.field_255.field_291, remissionBowlHex, Vector2.Zero);
        if (!state.IsEmpty)
        {
            int glowFrame = (int)(Time.Now().Ticks / timerFactor) & 0x3f;
            renderer.RenderRotating(Textures.Remission.BowlGlow[glowFrame], remissionBowlHex, Vector2.Zero);
        }
        // iris
        Brimstone.API.DrawIris(renderer, partDataWrapper, remissionOutputHex, time, Brimstone.API.ConvertToMaybe(pss.field_2743 ? pss.field_2744[1] : null));

        renderer.method_521(Textures.Remission.Connectors, pivot);
    }
    public static void RemissionBehavior(Sim sim, Part part, PartSimState pss, bool first, SolutionEditorBase seb)
    {
        DynamicData data = new(pss);
        object stateOb = data.Get("state");
        RemissionState state = stateOb is not null ? (RemissionState)stateOb : new RemissionState();
        if (first)
        {
            if (state.IsEmpty)
            {
                goto remissionEnd;
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
            seb.field_3936.Add(new GlyphEffect(seb, EffectTimescaleType.RealTime, HexGrid.solitaireGrid.ToPixelCoords(part.InFrontBy(remissionBowlHex)), Textures.Halves.BowlFlashAnimation, 30f, Vector2.Zero, 0f));
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

    /*
        public static void ShearingRenderer(Part part, Vector2 pos, SolutionEditorBase editor, class_195 renderer)
        {
            Brimstone.API.GetRenderingHelpers(part, pos, editor, out PartSimState pss, out class_236 partDataWrapper, out float time);
            Vector2 pivot = new(41f, 48f);
            Vector2 offset = new(-1f, -1f);

            renderer.method_523(Textures.GlyphCommon.DoubleBase, offset, pivot, 0f);
            renderer.RenderRotating(class_238.field_1989.field_90.field_255.field_292, shearingBowlHex, Vector2.Zero);
            renderer.RenderUpright(Textures.Shearing.SednumLine, osmosisMetalHex, Vector2.Zero);

            renderer.RenderRotating(class_238.field_1989.field_90.field_255.field_292, shearingOutputHex, Vector2.Zero);
        }
        public static void ShearingBehavior(Sim sim, Part part, PartSimState pss, bool first, SolutionEditorBase seb)
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
            bool madeQuickcopper = Atoms.quickcopperRadioactive && (newBowlAtom == Atoms.Quickcopper || outputAtom == Atoms.Quickcopper);

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
            return;
        }

        public static void OsmosisRenderer(Part part, Vector2 pos, SolutionEditorBase editor, class_195 renderer)
        {
            Brimstone.API.GetRenderingHelpers(part, pos, editor, out PartSimState pss, out class_236 partDataWrapper, out float time);
            Vector2 pivot = new(41f, 48f);
            Vector2 offset = new(-1f, -1f);

            renderer.method_523(Textures.GlyphCommon.DoubleBase, offset, pivot, 0f);
            renderer.RenderRotating(class_238.field_1989.field_90.field_255.field_292, osmosisMetalHex, Vector2.Zero);
            renderer.RenderUpright(Textures.Osmosis.OsmiumDemote, osmosisMetalHex, Vector2.Zero);
            renderer.RenderRotating(class_238.field_1989.field_90.field_255.field_292, osmosisQuickcopperHex, Vector2.Zero);
            renderer.RenderUpright(Textures.Osmosis.QuickcopperPromote, osmosisQuickcopperHex, Vector2.Zero);

            renderer.method_522(Textures.Osmosis.Connector, new Vector2(-28f, 23f), 0f);
        }
        public static void OsmosisBehavior(Sim sim, Part part, PartSimState pss, bool first, SolutionEditorBase seb)
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
            return;
        }

        public static void HarmonyRenderer(Part part, Vector2 pos, SolutionEditorBase editor, class_195 renderer)
        {
            Brimstone.API.GetRenderingHelpers(part, pos, editor, out PartSimState pss, out class_236 partDataWrapper, out float time);
            Vector2 pivot = new(42f, 49f);
            renderer.method_521(class_238.field_1989.field_90.field_185, pivot);
            HexIndex center = new(0, 0);
            renderer.RenderRotating(class_238.field_1989.field_90.field_187, center, Vector2.Zero);
            renderer.method_527(Textures.Harmony.TrefoilIndent, center, Vector2.Zero, pivot, 0f);
        }

        public static void TriplexBonderBehavior(Sim sim, Part part, PartSimState pss, bool first, SolutionEditorBase seb)
        {
            foreach (class_222 bonder in class_191.field_1775.field_1538)
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
    */
}