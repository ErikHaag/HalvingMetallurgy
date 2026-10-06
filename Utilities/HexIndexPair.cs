using System;

namespace HalvingMetallurgy.Utilities;

internal struct HexIndexPair : IEquatable<HexIndexPair>
{
    public HexIndexPair(HexIndex a, HexIndex b)
    {
        Left = a;
        Right = b;
    }

    public static bool operator ==(HexIndexPair l, HexIndexPair r) => (l.Left == r.Left && l.Right == r.Right) || (l.Left == r.Right && l.Right == r.Left);

    public static bool operator !=(HexIndexPair l, HexIndexPair r) => !(l == r);

    public HexIndex Left;
    public HexIndex Right;
    public readonly int Q1
    {
        get => Left.Q;
    }
    public readonly int R1
    {
        get => Left.R;
    }
    public readonly int Q2
    {
        get => Right.Q;
    }
    public readonly int R2
    {
        get => Right.R;
    }

    public override readonly bool Equals(object obj) => obj is HexIndexPair pair && this == pair;

    public override readonly int GetHashCode()
    {
        int hashCode = 898849194;
        hashCode = hashCode * -1521134295 + Q1.GetHashCode();
        hashCode = hashCode * -1521134295 + R1.GetHashCode();
        hashCode = hashCode * -1521134295 + Q2.GetHashCode();
        hashCode = hashCode * -1521134295 + R2.GetHashCode();
        return hashCode;
    }

    readonly bool IEquatable<HexIndexPair>.Equals(HexIndexPair other) => Equals(other);
}
