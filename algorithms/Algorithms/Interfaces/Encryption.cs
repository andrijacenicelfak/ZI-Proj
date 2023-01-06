
namespace Algorithms.Interfaces
{
    class Encription
    {
        private AlgorithmInterface Algorithm { get; set; }

        public Encription(AlgorithmInterface algorithm)
        {
            Algorithm = algorithm;
        }

        public byte[] Encrypt(byte[] input)
        {
            return Algorithm.Encrypt(input);
        }
        public byte[] Decrypt(byte[] input)
        {
            return Algorithm.Decrypt(input);
        }

        public byte[] EncryptParallel(byte[] input, int numOfThreads)
        {
            int blockSize = input.Length / numOfThreads;
            byte[][] output = new byte[numOfThreads][];
            int fullLenght = 0;
            Parallel.For(0, numOfThreads, index =>
            {
                int bSize = blockSize;
                if (index == numOfThreads - 1 && input.Length > numOfThreads * blockSize)
                    bSize += input.Length - numOfThreads * blockSize;
                byte[] curr = new byte[bSize];
                Buffer.BlockCopy(input, index * blockSize, curr, 0, bSize);
                byte[] enc = Algorithm.Encrypt(curr);
                output[index] = new byte[enc.Length];
                Interlocked.Add(ref fullLenght, enc.Length);
                Buffer.BlockCopy(enc, 0, output[index], 0, enc.Length);
            });
            byte[] finalOutput = new byte[fullLenght];
            int currentFill = 0;
            foreach (byte[] b in output)
            {
                Buffer.BlockCopy(b, 0, finalOutput, currentFill, b.Length);
                currentFill += b.Length;
            }
            return finalOutput;
        }
        public byte[] DecryptParallel(byte[] input, int numOfThreads, bool removePadding = false)
        {
            //byte[] output = new byte[input.Length];
            byte[][] output = new byte[numOfThreads][];
            int fullLenght = 0;
            int blockSize = input.Length / numOfThreads;
            Parallel.For(0, numOfThreads, index =>
            {
                int bSize = blockSize;
                if (index == numOfThreads)
                    bSize += input.Length - numOfThreads * blockSize;

                byte[] curr = new byte[bSize];
                Buffer.BlockCopy(input, index * blockSize, curr, 0, bSize);
                byte[] dec = Algorithm.Decrypt(curr);
                output[index] = new byte[dec.Length];
                Interlocked.Add(ref fullLenght, dec.Length);
                Buffer.BlockCopy(dec, 0, output[index], 0, dec.Length);
            });
            byte[] finalOutput = new byte[fullLenght];
            int currentFill = 0;
            foreach (byte[] b in output)
            {
                Buffer.BlockCopy(b, 0, finalOutput, currentFill, b.Length);
                currentFill += b.Length;
            }
            if (removePadding)
            {
                //Remove padding
                int numOfPadding = 0;
                for (int i = 0; i < output.Length; i += 2)
                    if (finalOutput[i] == 0 & finalOutput[i + 1] == 0)
                        numOfPadding++;
                byte[] finalOutput2 = new byte[output.Length - numOfPadding * 2];
                for (int i = 0, j = 0; i < output.Length; i += 2)
                {
                    if (finalOutput[i] == 0 & finalOutput[i + 1] == 0)
                        continue;
                    finalOutput2[j] = finalOutput[i];
                    finalOutput2[j + 1] = finalOutput[i + 1];
                    j += 2;
                }
                return finalOutput2;
            }
            return finalOutput;
        }

        public void EncryptFile(string inputName, string outputName)
        {
            FileStream input = new FileStream(inputName, FileMode.Open, FileAccess.Read);
            FileStream output = new FileStream(outputName, FileMode.Create, FileAccess.Write);
            byte[] data = new byte[512];
            while (input.Read(data, 0, 512) > 0)
            {
                byte[] enc = Algorithm.Encrypt(data);
                output.Write(enc);
            }
            output.Close();
            input.Close();
        }

        public void DecryptFile(string inputName, string outputName)
        {
            FileStream input = new FileStream(inputName, FileMode.Open, FileAccess.Read);
            FileStream output = new FileStream(outputName, FileMode.Create, FileAccess.Write);
            byte[] data = new byte[512];
            while (input.Read(data, 0, 512) > 0)
            {
                byte[] enc = Algorithm.Decrypt(data);
                output.Write(enc);
            }
            output.Close();
            input.Close();
        }

        public void EncryptBMPFile(string inputName, string outputName)
        {
            FileStream input = new FileStream(inputName, FileMode.Open, FileAccess.Read);
            byte[] header = new byte[54];
            int count = input.Read(header, 0, 54);
            FileStream output = new FileStream(outputName, FileMode.Create, FileAccess.Write);
            output.Write(header, 0, 54);

            byte[] data = new byte[512];
            while (input.Read(data, 0, 512) > 0)
            {
                byte[] enc = Algorithm.Encrypt(data);
                output.Write(enc);
            }

            output.Close();
            input.Close();
        }

        public void DecryptBMPFile(string inputName, string outputName)
        {
            FileStream input = new FileStream(inputName, FileMode.Open, FileAccess.Read);
            byte[] header = new byte[54];
            int count = input.Read(header, 0, 54);
            FileStream output = new FileStream(outputName, FileMode.Create, FileAccess.Write);
            output.Write(header, 0, 54);

            byte[] data = new byte[512];
            while (input.Read(data, 0, 512) > 0)
            {
                byte[] enc = Algorithm.Decrypt(data);
                output.Write(enc);
            }

            output.Close();
            input.Close();
        }
    }
}