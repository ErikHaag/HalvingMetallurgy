using MonoMod;

[MonoModPatch("SolutionEditorScreen")]
class patch_SolutionEditorScreen
{
    [MonoModIgnore]
    public extern void orig_StartPlayback(Sim sim);
    public void StartPlayback(Sim sim)
    {
        orig_StartPlayback(sim);
        HalvingMetallurgy.Glyphs.PopulateQuakeNormalBonders(sim);
    }
}