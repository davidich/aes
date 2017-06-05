using Aes.Types;

namespace Aes
{
    class AesCore
    {
        private readonly byte[] _key;
        private const int Nr = 10;   // number of rounds
        private const int Nk = 4;    // number of words in key
        private const int Nb = 4;    // number state columns

        public WordArray State { get; }

        public AesCore(byte[] state, byte[] key)
        {
            _key = key;
            State = CreateState(state);
        }
        
        public byte[] Encrypt()
        {
            var roundKeys = AesUtils.ExpandKey(_key);
            for (int i = 0; i < 11; i++)
            {
                if (i > 0)
                    AesUtils.SubBytes(State);

                if(i > 0)
                    AesUtils.ShiftRows(State);

                if (i > 0 && i < 10)
                    AesUtils.MixColumns(State);

                AesUtils.AddRoundKey(State, roundKeys[i]);
            }
            
            return State.GetBytes();
        }

        private static WordArray CreateState(byte[] input)
        {
            var words = new Word[4];
            
            for (int row = 0; row < 4; row++)
            {
                words[row] = new Word(input, row * 4);
            }

            return new WordArray(words);
        }       
    }
}