namespace HalvingMetallurgy.Utilities;

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
