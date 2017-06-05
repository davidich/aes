using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Aes.Types
{
    public class CipherBlock
    {
        public const int Size = 16;
        public byte[] Bytes { get; }

        public CipherBlock(byte[] bytes)
        {
            Bytes = bytes;
            Debug.Assert(Bytes.Length == Size);
        }

        

        public static CipherBlock CreateBlock(byte[] srcArray, int startIndex)
        {
            var bytes = new byte[Size];

            if (startIndex + Size <= srcArray.Length)
            {
                Array.Copy(srcArray, startIndex, bytes, 0, bytes.Length);
            }
            else
            {
                int usefullByteCount = srcArray.Length - startIndex;

                Array.Copy(srcArray, startIndex, bytes, 0, usefullByteCount);

                var paddingSize = bytes.Length - usefullByteCount;
                Array.Copy(Enumerable.Repeat((byte)paddingSize, paddingSize).ToArray(), 0, bytes, usefullByteCount, paddingSize);
                
            }

            return new CipherBlock(bytes);
        }

        public CipherBlock Xor(CipherBlock cipherBlock)
        {
            Debug.Assert(Bytes.Length == cipherBlock.Bytes.Length);

            var resultBytes = new byte[cipherBlock.Bytes.Length];
            for (int i = 0; i < Bytes.Length; i++)
            {
                resultBytes[i] = (byte)(Bytes[i] ^ cipherBlock.Bytes[i]);
            }
            return new CipherBlock(resultBytes);
        }
    }
}