using System;

namespace Aes.Types
{
    public struct Word
    {
        public byte B0;
        public byte B1;
        public byte B2;
        public byte B3;

        public byte this[int byteIndex]
        {
            get
            {
                switch (byteIndex)
                {
                    case 0:
                        return B0;
                    case 1:
                        return B1;
                    case 2:
                        return B2;
                    case 3:
                        return B3;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (byteIndex)
                {
                    case 0:
                        B0 = value;
                        break;
                    case 1:
                        B1 = value;
                        break;                        
                    case 2:
                        B2 = value;
                        break;
                    case 3:
                        B3 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public Word(byte b0, byte b1, byte b2, byte b3)
            : this()
        {
            B0 = b0;
            B1 = b1;
            B2 = b2;
            B3 = b3;
        }

        public Word(byte[] srcBytes, int startIndex)
        {
            B0 = srcBytes[startIndex + 0];
            B1 = srcBytes[startIndex + 1];
            B2 = srcBytes[startIndex + 2];
            B3 = srcBytes[startIndex + 3];
        }

        public Word Rotate()
        {
            return new Word(B1, B2, B3, B0);
        }

        public Word Substitute(byte[] sbox)
        {
            return new Word(sbox[B0],sbox[B1],sbox[B2],sbox[B3]);
        }

        public Word Xor(Word other)
        {
            return new Word(
                (byte)(B0 ^ other.B0),
                (byte)(B1 ^ other.B1),
                (byte)( B2 ^ other.B2),
                (byte)( B3 ^ other.B3));
        }

        public override string ToString()
        {
            return $"{B3:X},{B2:X},{B1:X},{B0:X}";
        }
    }
}