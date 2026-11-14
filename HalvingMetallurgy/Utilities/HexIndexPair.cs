using System;

namespace HalvingMetallurgy.Utilities;

internal struct HexIndexPair(HexIndex a, HexIndex b) : IEquatable<HexIndexPair>
{
    public static bool operator ==(HexIndexPair l, HexIndexPair r) => (l.Left == r.Left && l.Right == r.Right) || (l.Left == r.Right && l.Right == r.Left);

    public static bool operator !=(HexIndexPair l, HexIndexPair r) => !(l == r);

    public HexIndex Left = a;
    public HexIndex Right = b;
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

    public override readonly int GetHashCode() => HashCode.Combine(Q1, R1, Q2, R2);

    readonly bool IEquatable<HexIndexPair>.Equals(HexIndexPair other) => Equals(other);
}
