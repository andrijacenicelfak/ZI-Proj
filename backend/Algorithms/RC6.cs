using System;
using System.Text;

namespace Algorithms
{

    class RC6
    {
        private uint W = 32;
        private uint LOG_W = 5;
        private uint R = 20;
        private uint[] S;
        private uint[] L;
        private ulong mod;
        private uint Pw, Qw;
        public RC6(string key)
        {
            Pw = (uint)Math.Ceiling((Math.E - 2) * Math.Pow(2, W));
            Qw = (uint)((1.618033988749895 - 1) * Math.Pow(2, W));
            S = new uint[2 * R + 4];
            mod = (ulong)Math.Pow(2, W);
            SetKey(key);
            if (L == null) L = new uint[10];
        }

        public void SetKey(string key)
        {
            uint w_bytes = (uint)Math.Ceiling((float)W / 8);
            uint c = (uint)Math.Ceiling((float)key.Length / w_bytes);

            L = new uint[c];

            char[] keychar = key.ToCharArray();
            char[] normkey = new char[c * 4];
            Buffer.BlockCopy(keychar, 0, normkey, 0, keychar.Length * sizeof(char));
            Buffer.BlockCopy(normkey, 0, L, 0, (int)(c * 4));

            S[0] = Pw;
            for (int _i = 1; _i < 2 * R + 4; _i++)
            {
                S[_i] = (uint)((S[_i - 1] + Qw) % mod);
            }

            uint A = 0, B = 0, i = 0, j = 0;
            uint v = 3 * uint.Max(c, 2 * R + 4);
            for (uint s = 1; s <= v; s++)
            {
                A = S[i] = ROL((uint)((S[i] + A + B) % mod), 3);
                B = L[j] = ROL((uint)((L[j] + A + B) % mod), (int)(A + B));
                i = (i + 1) % (2 * R + 4);
                j = (j + 1) % c;
            }

        }
        private uint ROL(uint v, int num)
        {
            return (v << num) | (v >> (32 - num));
        }
        private uint ROR(uint v, int num)
        {
            return (v >> num) | (v << (32 - num));
        }

        public uint[] Encrypt4Regs(uint[] data)
        {
            uint[] encrypted = new uint[4];
            UInt32 A = data[0], B = data[1], C = data[2], D = data[3];

            B += S[0];
            D += S[1];

            uint t, u, temp;

            for (int i = 1; i <= R; i++)
            {
                t = ROL((uint)((B * (2 * B + 1)) % mod), (int)LOG_W);
                u = ROL((uint)((D * (2 * D + 1)) % mod), (int)LOG_W);
                A = ROL(A ^ t, (int)u) + S[2 * i];
                C = ROL(C ^ u, (int)t) + S[2 * i + 1];
                temp = A;
                A = B;
                B = C;
                C = D;
                D = temp;
            }
            A += S[2 * R + 2];
            C += S[2 * R + 3];

            encrypted[0] = A;
            encrypted[1] = B;
            encrypted[2] = C;
            encrypted[3] = D;

            return encrypted;
        }

        public uint[] Decrypt4Regs(uint[] data)
        {
            uint[] dec = new uint[4];

            uint A = data[0], B = data[1], C = data[2], D = data[3];
            uint t, u, temp;

            C -= S[2 * R + 3];
            A -= S[2 * R + 2];
            for (int i = (int)R; i >= 1; i--)
            {
                temp = D;
                D = C;
                C = B;
                B = A;
                A = temp;

                u = ROL((uint)((D * (2 * D + 1)) % mod), (int)LOG_W);
                t = ROL((uint)((B * (2 * B + 1)) % mod), (int)LOG_W);
                C = ROR((uint)((C - S[2 * i + 1]) % mod), (int)t) ^ u;
                A = ROR((uint)((A - S[2 * i]) % mod), (int)u) ^ t;
            }
            D -= S[1];
            B -= S[0];

            dec[0] = A;
            dec[1] = B;
            dec[2] = C;
            dec[3] = D;
            return dec;
        }

        public char[] Encrypt8CharArray(char[] ulaz)
        {
            uint[] data = new uint[4];
            Buffer.BlockCopy(ulaz, 0, data, 0, 16);

            uint[] enc = Encrypt4Regs(data);

            char[] izlaz = new char[16];
            Buffer.BlockCopy(enc, 0, izlaz, 0, 16);
            return izlaz;

        }

        public char[] Decrypt8CharArray(char[] ulaz)
        {
            uint[] data = new uint[4];
            Buffer.BlockCopy(ulaz, 0, data, 0, 16);

            uint[] dec = Decrypt4Regs(data);

            char[] izlaz = new char[16];
            Buffer.BlockCopy(dec, 0, izlaz, 0, 16);

            return izlaz;
        }

        public string EncryptString(string input)
        {
            string enc = "";
            //napravimo da blok bude sve po 8
            while (input.Length % 8 != 0)
            {
                input += '\0';
            }

            char[] ceoString = input.ToCharArray();

            for (int i = 0; i < input.Length; i += 8)
            {
                char[] blockinput = new char[8];
                Buffer.BlockCopy(ceoString, i * 2, blockinput, 0, 16);
                // Mogli bi threadove ovde da dodamo da radi brze, za svaki blok po thread
                char[] blockoutput = Encrypt8CharArray(blockinput);
                enc += new String(blockoutput);
            }
            return enc;
        }

        public string DecodeString(string input)
        {
            string dec = "";
            //napravimo da blok bude sve po 8
            while (input.Length % 8 != 0) input += '\0';
            char[] ceoString = input.ToCharArray();

            for (int i = 0, j = 0; i < input.Length; i += 8, j++)
            {
                //zbog nekog razloga svaki drugi put u blockinput bude prazan string??
                if (j % 2 == 1)
                    continue;
                char[] blockinput = new char[8];
                Buffer.BlockCopy(ceoString, i * 2, blockinput, 0, 16);
                char[] blockoutput = Decrypt8CharArray(blockinput);
                dec += new String(blockoutput);
            }
            return dec;
        }

        public byte[] EncryptByteArray(byte[] input)
        {
            int missing = input.Length % (4 * sizeof(uint));
            missing = missing == 0 ? 0 : 4 * sizeof(uint) - missing;
            int len = input.Length + missing;

            byte[] inputExtended = new byte[len];
            Buffer.BlockCopy(input, 0, inputExtended, 0, input.Length);

            byte[] enc = new byte[len];

            uint[] data = new uint[4];

            for (int i = 0; i < len / (4 * sizeof(uint)); i++)
            {
                Buffer.BlockCopy(inputExtended, i * 4 * sizeof(uint), data, 0, 4 * sizeof(uint));
                uint[] rez = Encrypt4Regs(data);
                Buffer.BlockCopy(rez, 0, enc, i * 4 * sizeof(uint), 4 * sizeof(uint));
            }

            return enc;
        }

        public byte[] DecryptByteArray(byte[] input)
        {
            byte[] dec = new byte[input.Length];
            uint[] data = new uint[4];
            for (int i = 0; i < input.Length / (4 * sizeof(uint)); i++)
            {
                Buffer.BlockCopy(input, i * 4 * sizeof(uint), data, 0, 4 * sizeof(uint));
                uint[] rez = Decrypt4Regs(data);
                Buffer.BlockCopy(rez, 0, dec, i * 4 * sizeof(uint), 4 * sizeof(uint));
            }

            return dec;
        }

        public string EncryptStringFaster(string input)
        {
            string enc = "";
            while (input.Length % 8 != 0)
            {
                input += '\0';
            }

            char[] ceoString = input.ToCharArray();

            for (int i = 0; i < input.Length; i += 8)
            {
                uint[] blockinput = new uint[4];
                Buffer.BlockCopy(ceoString, i * 2, blockinput, 0, 16);
                uint[] blockoutput = Encrypt4Regs(blockinput);
                char[] rez = new char[8];
                Buffer.BlockCopy(blockoutput, 0, rez, 0, 16);
                enc += new String(rez);
            }
            return enc;
        }

        public string DecodeStringFaster(string input)
        {
            string dec = "";
            while (input.Length % 8 != 0) input += '\0';
            char[] ceoString = input.ToCharArray();

            for (int i = 0, j = 0; i < input.Length; i += 8, j++)
            {
                uint[] blockinput = new uint[4];
                Buffer.BlockCopy(ceoString, i * 2, blockinput, 0, 16);
                uint[] blockoutput = Decrypt4Regs(blockinput);
                char[] rez = new char[8];
                Buffer.BlockCopy(blockoutput, 0, rez, 0, 16);
                dec += new String(rez);
            }
            return dec;
        }

        public void EncryptTxtFile(string inputName, string outputName)
        {
        }

        public void DecryptTxtFile(string inputName, string outputName)
        {
        }
    }

}