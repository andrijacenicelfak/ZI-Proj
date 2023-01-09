using System;
using System.Collections.Generic;

namespace Algorithms
{
    class Bifid
    {
        private char[,] keyMatrix = new char[5, 5];

        private Dictionary<char, KeyValuePair<int, int>> map;
        private string Key { get; set; }
        public Bifid(string key)
        {
            Key = key.ToUpper();
            BuildKey();
            map = new Dictionary<char, KeyValuePair<int, int>>();
            FillMap();
        }

        private void FillMap()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    //Console.Write(keyMatrix[i, j] + " ");
                    map.Add(keyMatrix[i, j], new KeyValuePair<int, int>(i, j));
                }
            }
        }
        private void BuildKey()
        {
            char[] alphabet = ("abcdefghiklmnopqrstuvwxyz").ToUpper().ToCharArray();
            int index = 0;
            for (int k = 0; k < Key.Length; k++)
            {
                char current = Key[k];
                if (current == 'J')
                    current = 'I';
                if (current < 'A' || current > 'Z')
                    continue;
                int rind = (current - 'A');
                if (current >= 'J')
                    rind--;
                if (alphabet[rind] != (char)0)
                {
                    keyMatrix[index / 5, index % 5] = current;
                    alphabet[rind] = (char)0;
                    index++;
                }
            }
            if (index < 25)
            {
                for (int i = 0; i < 25; i++)
                {
                    //Console.Write(('A' + i) + ":" + alphabet[i] + ", ");
                    if (alphabet[i] == (char)0)
                        continue;
                    keyMatrix[index / 5, index % 5] = alphabet[i];
                    alphabet[i] = (char)0;
                    index++;
                }
            }
            /*
            //testing
            Console.WriteLine();
            for (int i = 0; i < 25; i++)
            {
                Console.Write(keyMatrix[i / 5, i % 5] + " ");
                if (i % 5 == 4)
                    Console.WriteLine();
            }
            */
        }
        public string Encrypt(string text)
        {
            text = text.ToUpper();
            int[] up = new int[text.Length];
            int[] down = new int[text.Length];
            int size = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char curr = text[i];
                if (curr < 'A' || curr > 'Z')
                    continue;
                if (curr == 'J')
                    curr = 'I';

                var pair = map[curr];
                up[size] = pair.Key;
                down[size] = pair.Value;
                size++;
            }
            int[] coords = new int[size * 2];
            Buffer.BlockCopy(up, 0, coords, 0, size * sizeof(int));
            Buffer.BlockCopy(down, 0, coords, size * sizeof(int), size * sizeof(int));
            string rez = "";
            for (int k = 0; k < size * 2; k += 2)
            {
                int i = coords[k], j = coords[k + 1];
                rez += keyMatrix[i, j];
            }
            return rez;
        }

        public string Decrypt(string text)
        {
            string rez = "";
            int[] coords = new int[text.Length * 2];
            int size = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char curr = text[i];
                if (curr < 'A' || curr > 'Z')
                    continue;
                var kyp = map[curr];
                coords[size++] = kyp.Key;
                coords[size++] = kyp.Value;
            }
            int[] up = new int[size / 2];
            int[] down = new int[size / 2];

            Buffer.BlockCopy(coords, 0, up, 0, size * sizeof(int) / 2);
            Buffer.BlockCopy(coords, size * sizeof(int) / 2, down, 0, size * sizeof(int) / 2);

            for (int k = 0; k < size / 2; k++)
            {
                rez += keyMatrix[up[k], down[k]];
            }
            return rez;
        }
    }
}