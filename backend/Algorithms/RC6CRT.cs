using System;
using System.Text;

namespace Algorithms
{

    class RC6CRT : RC6
    {
        private byte[] IV;
        public RC6CRT(string key, byte[]? iv = null) : base(key)
        {
            if (iv != null && iv.Length == 4 * sizeof(uint))
                IV = iv;
            else
            {
                IV = new byte[4 * sizeof(uint)];
                IV[0] = 131;
                int i = 1;
                for (; i < 4 * sizeof(uint); i++)
                {
                    IV[i] = (byte)(IV[i - 1] * 17 / 2);
                }
            }
        }

        public byte[] EncryptByteArrayCRT(byte[] input)
        {
            int missing = input.Length % (4 * sizeof(uint));
            missing = missing == 0 ? 0 : 4 * sizeof(uint) - missing;
            int len = input.Length + missing;

            byte[] inputExtended = new byte[len];
            Buffer.BlockCopy(input, 0, inputExtended, 0, input.Length);

            byte[] enc = new byte[len];

            uint[] data = new uint[4];

            uint[] currIV = new uint[4];
            Buffer.BlockCopy(IV, 0, currIV, 0, 4 * sizeof(uint));

            for (int i = 0; i < len / (4 * sizeof(uint)); i++)
            {
                Buffer.BlockCopy(currIV, 0, data, 0, 4 * sizeof(uint));
                uint[] rez = Encrypt4Regs(data);
                Buffer.BlockCopy(inputExtended, i * 4 * sizeof(uint), data, 0, 4 * sizeof(uint));
                for (int j = 0; j < 4; j++)
                {
                    rez[j] = rez[j] ^ data[j];
                }
                Buffer.BlockCopy(rez, 0, enc, i * 4 * sizeof(uint), 4 * sizeof(uint));
                for (int j = 0; j < 4; j++)
                {
                    currIV[j] = (currIV[j] + ((uint)i + 1));
                }
            }

            return enc;
        }

        public byte[] DecryptByteArrayCRT(byte[] input)
        {
            byte[] dec = new byte[input.Length];
            uint[] data = new uint[4];

            uint[] currIV = new uint[4];
            Buffer.BlockCopy(IV, 0, currIV, 0, 4 * sizeof(uint));

            for (int i = 0; i < input.Length / (4 * sizeof(uint)); i++)
            {
                Buffer.BlockCopy(currIV, 0, data, 0, 4 * sizeof(uint));
                uint[] rez = Encrypt4Regs(data);
                Buffer.BlockCopy(input, i * 4 * sizeof(uint), data, 0, 4 * sizeof(uint));
                for (int j = 0; j < 4; j++)
                {
                    rez[j] = rez[j] ^ data[j];
                }
                Buffer.BlockCopy(rez, 0, dec, i * 4 * sizeof(uint), 4 * sizeof(uint));
                for (int j = 0; j < 4; j++)
                {
                    currIV[j] = (currIV[j] + ((uint)i + 1));
                }
            }

            return dec;
        }


    }
}