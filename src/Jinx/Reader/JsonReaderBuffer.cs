using System;
using System.IO;

namespace Jinx.Reader
{
    public struct JsonReaderBuffer
    {
        public TextReader Stream;

        public char[] Data;
        public int Size;
        public int Expanded;

        public int Offset;
        public int Length;

        public int Index;
        public int Total;

        public void Ensure(bool consistent)
        {
            if (Offset >= Length)
            {
                if (consistent == false)
                {
                    Length = Stream.ReadBlock(Data, 0, Size);
                    Index += Length;
                    Total += Length;
                    Offset = 0;
                }
                else
                {
                    int available = Expanded + Size - Length;

                    if (available == 0)
                    {
                        Array.Resize(ref Data, Expanded + Size + Size);
                        Expanded += Size;
                        available += Size;
                    }

                    int loaded = Stream.ReadBlock(Data, Offset, available);

                    Length += loaded;
                    Index += loaded;
                    Total += loaded;
                }
            }
            else if (consistent == false && Offset > Size)
            {
                var diff = Length - Offset;
                var copy = new char[Size];

                Array.Copy(Data, Offset, copy, 0, diff);

                Length -= Offset;
                Offset = 0;
                Data = copy;
                Expanded = 0;
            }
        }

        public void Forward(bool consistent)
        {
            Offset++;
            Ensure(consistent);
        }
    }
}