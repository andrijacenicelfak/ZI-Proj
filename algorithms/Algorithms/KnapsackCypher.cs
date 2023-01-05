namespace Algorithms
{
    class KnapsackCypher
    {
        private int N { get; set; }
        private int N_1 { get; set; }
        private int M { get; set; }

        private int[] privateKey;

        public int[] publicKey;

        public KnapsackCypher(int N, int M, int[] privateKey)
        {
            this.N = N;
            this.M = M;
            this.privateKey = privateKey;

            //TODO check if values are valid
            publicKey = new int[8];
            GeneratePublicKey();
        }

        private void GeneratePublicKey()
        {
            for (int i = 0; i < 8; i++)
            {
                publicKey[i] = (privateKey[i] * N) % M;
                // Console.Write(publicKey[i] + " ");
            }
            // Console.WriteLine();
            N_1 = 1;
            while ((N * N_1) % M != 1) N_1++;
            // Console.WriteLine(N_1);
        }

        public int[] Encrypt(byte[] bytes)
        {
            int index = 0;
            int[] rez = new int[bytes.Length];
            foreach (byte b in bytes)
            {
                int cr = 0;
                byte v = 1;
                for (int i = 7; i >= 0; i--)
                {
                    if ((b & v) > 0)
                    {
                        cr += publicKey[i];
                    }
                    v = (byte)(v << 1);
                }
                rez[index++] = cr;
            }
            return rez;
        }

        public static int[] EncryptWithKey(byte[] bytes, byte[] publicKey)
        {
            int index = 0;
            int[] rez = new int[bytes.Length];
            foreach (byte b in bytes)
            {
                int cr = 0;
                byte v = 1;
                for (int i = 7; i >= 0; i--)
                {
                    if ((b & v) > 0)
                    {
                        cr += publicKey[i];
                    }
                    v = (byte)(v << 1);
                }
                rez[index++] = cr;
            }
            return rez;
        }

        public byte[] Decrypt(int[] crpt)
        {
            int index = 0;
            byte[] bytes = new byte[crpt.Length];
            foreach (int i in crpt)
            {
                int curr = (i * N_1) % M;
                int j = 7;
                byte v = 1;
                byte r = 0;
                while (curr > 0 && j >= 0)
                {
                    if (curr >= privateKey[j])
                    {
                        //Console.WriteLine(curr + " : " + " - " + privateKey[j] + "\nr : " + r + " ->" + (r | v));
                        curr -= privateKey[j];
                        r = (byte)(r | v);
                    }
                    v = (byte)(v << 1);
                    j--;
                }
                bytes[index++] = r;
            }
            return bytes;
        }

    }
}