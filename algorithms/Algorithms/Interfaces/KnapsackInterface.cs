using System;
namespace Algorithms.Interfaces
{
    class KnapsackInterface : AlgorithmInterface
    {
        private KnapsackCypher algorithm;
        public KnapsackInterface(int N, int M, int[] privateKey)
        {
            algorithm = new KnapsackCypher(N, M, privateKey);
        }
        public byte[] Encrypt(byte[] input)
        {
            int[] enc = algorithm.Encrypt(input);

            byte[] encb = new byte[enc.Length * sizeof(int)];
            Buffer.BlockCopy(enc, 0, encb, 0, enc.Length * sizeof(int));
            return encb;
        }
        public byte[] Decrypt(byte[] input)
        {
            int size = input.Length / sizeof(int);
            if (size < 1) size = 1;
            //if (input.Length % sizeof(int) != 0)
            // throw new Exception("Nije moguce dekodirati nesto sto je manje od 4 byte");
            int[] intInput = new int[size];
            Buffer.BlockCopy(input, 0, intInput, 0, size * sizeof(int));
            return algorithm.Decrypt(intInput);
        }
        public byte[] EncryptParallel(byte[] input)
        {
            return new byte[16];
        }
        public byte[] DecryptParallel(byte[] input)
        {
            return new byte[16];
        }
    }
}