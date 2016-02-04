using System.IO;

namespace Jinx.Reader
{
    public struct JsonReaderBuffer
    {
        public TextReader Stream;

        public char[] Data;
        public int Size;

        public int Offset;
        public int Length;

        public int Index;
        public int Total;

        public void Ensure()
        {
            if (Offset >= Length)
            {
                Length = Stream.ReadBlock(Data, 0, Size);
                Index += Length;
                Total += Length;
                Offset = 0;
            }
        }

        public void Forward()
        {
            Offset++;
            Ensure();
        }
    }
}