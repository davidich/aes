using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Aes;
using Aes.Types;
using Xunit;

namespace Xunit
{
    /// <summary>
    /// Tests per video https://www.youtube.com/watch?v=mlzxpkdXP58
    /// </summary>
    public class Tests
    {
        [Fact]
        public void CanExpandKey()
        {
            byte[] inputKey = Convert(new[]
            {
                0x2b, 0x7e, 0x15, 0x16,
                0x28, 0xae, 0xd2, 0xa6,
                0xab, 0xf7, 0x15, 0x88,
                0x09, 0xcf, 0x4f, 0x3c
            });

            byte[] expectedKey1 = Convert(new[]
            {
                0xa0, 0xfa, 0xfe, 0x17,
                0x88, 0x54, 0x2c, 0xb1,
                0x23, 0xa3, 0x39, 0x39,
                0x2a, 0x6c, 0x76, 0x05
            });

            byte[] expectedKey2 = Convert(new[]
            {
                0xf2, 0xc2, 0x95, 0xf2,
                0x7a, 0x96, 0xb9, 0x43,
                0x59, 0x35, 0x80, 0x7a,
                0x73, 0x59, 0xf6, 0x7f
            });

            byte[] expectedKey3= Convert(new[]
            {
                0x3d, 0x80, 0x47, 0x7d,
                0x47, 0x16, 0xfe, 0x3e,
                0x1e, 0x23, 0x7e, 0x44,
                0x6d, 0x7a, 0x88, 0x3b,
            });
            
            // key4 - key9 are skipped

            byte[] expectedKey10= Convert(new[]
            {
                0xd0, 0x14, 0xf9, 0xa8,
                0xc9, 0xee, 0x25, 0x89,
                0xe1, 0x3f, 0x0c, 0xc8,
                0xb6, 0x63, 0x0c, 0xa6,
            });


            WordArray[] keys = AesUtils.ExpandKey(inputKey);

            Assert.EqualBytes(inputKey, keys[0].GetBytes());            
            Assert.EqualBytes(expectedKey1, keys[1].GetBytes());            
            Assert.EqualBytes(expectedKey2, keys[2].GetBytes());            
            Assert.EqualBytes(expectedKey3, keys[3].GetBytes());            
            Assert.EqualBytes(expectedKey10, keys[10].GetBytes());            
        }

        [Fact]
        public void CanAddRoundKey()
        {
            WordArray state = Convert(new[]
            {
                0x04, 0x66, 0x81, 0xe5,
                0xe0, 0xcb, 0x19, 0x9a,
                0x48, 0xf8, 0xd3, 0x7a,
                0x28, 0x06, 0x26, 0x4c,
            });

            WordArray roundKey = Convert(new[]
            {
                0xa0, 0xfa, 0xfe, 0x17,
                0x88, 0x54, 0x2c, 0xb1,
                0x23, 0xa3, 0x39, 0x39,
                0x2a, 0x6c, 0x76, 0x05
            });

            AesUtils.AddRoundKey(state, roundKey);


            byte[] expectedState = Convert(new[]
            {
                0xa4, 0x9c, 0x7f, 0xf2,
                0x68, 0x9f, 0x35, 0x2b,
                0x6b, 0x5b, 0xea, 0x43,
                0x02, 0x6a, 0x50, 0x49,
            });
            Assert.EqualBytes(expectedState, state.GetBytes());

        }

        [Fact]
        public void CanShiftRows()
        {
            WordArray state = Convert(new[]
            {
                0xd4, 0x27, 0x11, 0xae,
                0xe0, 0xbf, 0x98, 0xf1,
                0xb8, 0xb4, 0x5d, 0xe5,
                0x1e, 0x41, 0x52, 0x30,
            });

            WordArray stateAfterShift = Convert(new[]
            {
                0xd4, 0xbf, 0x5d, 0x30,
                0xe0, 0xb4, 0x52, 0xae, 
                0xb8, 0x41, 0x11, 0xf1,
                0x1e, 0x27, 0x98, 0xe5,
            });

            AesUtils.ShiftRows(state);

            Assert.EqualBytes(stateAfterShift.GetBytes(), state.GetBytes());
        }

        [Fact]
        public void CanMixColumns()
        {
            WordArray state = Convert(new[]
            {
                0xd4, 0xbf, 0x5d, 0x30,
                0xe0, 0xb4, 0x52, 0xae,
                0xb8, 0x41, 0x11, 0xf1,
                0x1e, 0x27, 0x98, 0xe5,
            });

            WordArray expectedState = Convert(new[]
            {
                0x04, 0x66, 0x81, 0xe5,
                0xe0, 0xcb, 0x19, 0x9a,
                0x48, 0xf8, 0xd3, 0x7a,
                0x28, 0x06, 0x26, 0x4c,
            });

            AesUtils.MixColumns(state);

            Assert.EqualBytes(expectedState.GetBytes(), state.GetBytes());
        }
        
        [Fact]
        public void CanEncryp()
        {
            WordArray state = Convert(new[]
            {
                0x32, 0x43, 0xf6, 0xa8,
                0x88, 0x5a, 0x30, 0x8d,
                0x31, 0x31, 0x98, 0xa2,
                0xe0, 0x37, 0x07, 0x34,
            });

            WordArray inputKey = Convert(new[]
            {
                0x2b, 0x7e, 0x15, 0x16,
                0x28, 0xae, 0xd2, 0xa6,
                0xab, 0xf7, 0x15, 0x88,
                0x09, 0xcf, 0x4f, 0x3c
            });

            WordArray expectedState = Convert(new[]
            {
                0x39, 0x25, 0x84, 0x1d,
                0x02, 0xdc, 0x09, 0xfb,
                0xdc, 0x11, 0x85, 0x97,
                0x19, 0x6a, 0x0b, 0x32,
            });

            AesCore aes = new AesCore(state.GetBytes(), inputKey.GetBytes());
            aes.Encrypt();
            
            Assert.EqualBytes(expectedState.GetBytes(), aes.State.GetBytes());
        }

        private static byte[] Convert(IEnumerable<int> input)
        {
            return input.Select(val => (byte)val).ToArray();
        }
    }
}

