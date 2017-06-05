using System.CodeDom;

namespace Aes.Types
{
    public class WordArray
    {
        public Word[] Words;

        public WordArray(int wordCount)
        {
            Words = new Word[wordCount];
            for (int i = 0; i < wordCount; i++)
            {
                Words[i] = new Word();
            }            
        }

        public WordArray(Word[] words)
        {
            Words = words;
        }

        public WordArray Xor(WordArray other)
        {
            for (int i = 0; i < Words.Length; i++)
            {
                Words[i] = Words[i].Xor(other.Words[i]);
            }
            return this;
        }

        public WordArray Substitude(byte[] sbox)
        {
            for (int i = 0; i < Words.Length; i++)
            {
                Words[i] = Words[i].Substitute(sbox);
            }
            return this;
        }

        public byte[] GetBytes()
        {
            var bytes = new byte[4 * Words.Length];
            for (int i = 0; i < 4; i++)
            {
                bytes[i * 4 + 0] = Words[i].B0;
                bytes[i * 4 + 1] = Words[i].B1;
                bytes[i * 4 + 2] = Words[i].B2;
                bytes[i * 4 + 3] = Words[i].B3;
            }

            return bytes;
        }

        public static implicit operator WordArray (byte[] data)
        {
            return FromBytes(data);
        }

        public static WordArray FromBytes(byte[] bytes)
        {
            Word[] words = new Word[bytes.Length / 4];

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = new Word(bytes, i * 4);
            }

            return new WordArray(words);
        }
    }
}