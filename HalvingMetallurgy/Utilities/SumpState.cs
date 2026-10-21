namespace HalvingMetallurgy.Utilities;

internal struct SumpState
{
    public SumpState()
    {
        state = 0;
    }

    private byte state;

    // bit manipulations!
    public byte QuicksilverCount
    {
        readonly get
        {
            return (byte)(state & 7);
        }
        set
        {
            state &= 0xf8; // ~7
            state |= (byte)(value & 7);
        }
    }

    public bool QuicksilverEject
    {
        readonly get
        {
            return (state & 0x08) == 0x08;
        }
        set
        {
            if (value)
            {
                state |= 0x08;
            }
            else
            {
                state &= 0xf7; // ~8
            }
        }
    }

    public bool DrainFlash
    {
        readonly get
        {
            return (state & 0x10) == 0x10;
        }
        set
        {
            if (value)
            {
                state |= 0x10;
            }
            else
            {
                state &= 0xef; // ~16
            }
        }
    }

}
