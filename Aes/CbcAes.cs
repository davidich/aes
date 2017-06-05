using System;
using System.Threading;
using Aes.Types;

namespace Aes
{
    public class CbcAes
    {
        private readonly byte[] _iv;
        private readonly byte[] _key;

        public CbcAes(byte[] iv, byte[] key)
        {
            _iv = iv;
            _key = key;
        }

        public byte[] Encode(byte[] message)
        {
            var blocks = SplitMessage(message);
            var encryptedBytes = new byte[blocks.Length * CipherBlock.Size];

            var prevResult = _iv;
            for (int i = 0; i < blocks.Length; i++)
            {
                var cipherInput = new CipherBlock(prevResult).Xor(blocks[i]);
                var cipherOutput = new AesCore(cipherInput.Bytes, _key).Encrypt();
                Array.Copy(cipherOutput, 0, encryptedBytes, i * CipherBlock.Size, CipherBlock.Size);

                prevResult = cipherOutput;
            }

            return encryptedBytes;
        }

        public static CipherBlock[] SplitMessage(byte[] messageBytes)
        {
            var blockCount = messageBytes.Length / CipherBlock.Size + 1;

            var blocks = new CipherBlock[blockCount];
            for (int i = 0; i < blockCount; i++)
            {
                blocks[i] = CipherBlock.CreateBlock(messageBytes, i * CipherBlock.Size);
            }
            return blocks;
        }
    }
}