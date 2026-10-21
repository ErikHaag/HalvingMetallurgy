namespace HalvingMetallurgy.Utilities;

internal struct RemissionState
{
    public int halfProjections = 0;
    public int irisState = 0;

    public RemissionState()
    {
    }

    public readonly bool CanHalfDemote => this.halfProjections <= -1;
    public readonly bool IsEmpty => this.halfProjections == 0;
    public readonly bool CanHalfPromote => this.halfProjections >= 1;
    public readonly bool CanFullPromote => this.halfProjections >= 2;
}
