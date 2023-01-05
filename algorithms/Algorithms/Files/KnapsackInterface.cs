
namespace Algorithms.Files
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
            int[] intInput = new int[input.Length / sizeof(int)];
            Buffer.BlockCopy(input, 0, intInput, 0, input.Length);
            return algorithm.Decrypt(intInput);
        }
    }
}