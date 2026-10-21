using Quintessential;
using Quintessential.Components;
using System;

namespace HalvingMetallurgy.Components;

public class AtomDecorationComponent : IAtomComponent
{
    static readonly Random rng = new(4); // Chosen by fair dice roll.
                                         // guarenteed to be random.

    private Atom referencedAtom = null;
    public int frameOffset = 0;
    public static readonly Identifier Id = HalvingMetallurgy.Instance.GetIdentifier("atom_decorations");
    Identifier IComponentBase<Atom>.Id => Id;

    public CycleEvent.CycleEventExecutionType CallbackType => CycleEvent.CycleEventExecutionType.None;

    public void OnAddToMolecule(Molecule molecule, HexIndex hexPos)
    {
    }
    public void OnBind(Atom self, Molecule arg1)
    {
        referencedAtom = self;
        frameOffset = rng.GetInt(0, 64);
    }

    public void OnClone(ref Atom cloned)
    {
    }

    public void OnCycleCallback(Sim sim, CycleEvent.CycleEventExecutionType executionType)
    {
    }

    public void OnRender(Action doRender, ref AtomType type, ref Vector2 translation, ref float scaleMultiplier, ref float opacityMultiplier, ref float height, ref float shadowStrength, ref float shadowOffset, ref float shadowAngle, Texture shadow, Texture overlayEffect, bool isOutputRender, SolutionEditorBase solutionEditor)
    {
        if (!isOutputRender && referencedAtom.atomType.Id == HalvingMetallurgy.Instance.GetIdentifier("quickcopper"))
        {
            int particleFrame = ((int)(Time.NowInSeconds() * 30f) + frameOffset) & 0x3f;
            TextureRenderer.Render(Textures.Atom.QuickcopperAnimation[particleFrame], translation - new Vector2(60f, 60f));
        }
        doRender();
        if (true)
        {
            TextureRenderer.Render(Textures.Atom.PartyHatTexture, translation - new Vector2(45f, 45f));
        }
    }

    public void OnRemoveFromMolecule(Molecule molecule, HexIndex hexPos)
    {
    }

    public void OnReplace(AtomType newType)
    {
        if (newType.Id.namespc != HalvingMetallurgy.Instance.ModId)
        {
            referencedAtom.RemoveComponent(Id);
        }
    }
    public void OnUnbind(Atom self, Molecule arg1)
    {
        referencedAtom = null;
    }

    public static void Register()
    {
        string[] atomNames = ["quicklime", "quickcopper", "beryl", "wolfram", "vulcan", "nickel", "zinc", "sednum", "osmium"];
        for (int i = 0; i < atomNames.Length; i++)
        {
            Identifier atomId = HalvingMetallurgy.Instance.GetIdentifier(atomNames[i]);
            QApi.AddComponentToAtom(atomId, (_) => new AtomDecorationComponent());
            QApi.AddComponentToAtomOnReplace(atomId, (_, _) => new AtomDecorationComponent());
        }
    }
}
