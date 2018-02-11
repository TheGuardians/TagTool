namespace TagTool.Common
{
    public struct Bitmask
    {
        public bool[] Values;

        public Bitmask(byte Value)
        {
            Values = new bool[8];

            for (int i = 0; i < 8; i++)
                Values[i] = (Value & (1 << i)) != 0;
        }

        public Bitmask(short Value)
        {
            Values = new bool[16];

            for (int i = 0; i < 16; i++)
                Values[i] = (Value & (1 << i)) != 0;
        }

        public Bitmask(int Value)
        {
            Values = new bool[32];

            for (int i = 0; i < 32; i++)
                Values[i] = (Value & (1 << i)) != 0;
        }
    }

}
