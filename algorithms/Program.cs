using Algorithms;
using Algorithms.Interfaces;



/*
byte[] bytes = System.IO.File.ReadAllBytes("file.txt");

int[] privateKey = { 1, 2, 4, 9, 17, 34, 69, 140 };
int M = 313;
int N = 101;
Encription e = new Encription(new KnapsackInterface(N, M, privateKey));

byte[] enc = e.EncryptParallel(bytes, 8);


byte[] dec = e.DecryptParallel(enc, 4);

foreach (byte b in bytes)
    Console.Write(b + " ");
Console.WriteLine();
Console.WriteLine();

foreach (byte b in dec)
    Console.Write(b + " ");
Console.WriteLine();
*/
/*
TigerHash hash = new TigerHash();
hash.ComputeHash(bytes);
byte[] bhash1 = hash.FinalHashValue;

foreach (byte b in bhash1)
    Console.Write(b + " ");
Console.WriteLine();
*/
/*
int[] privateKey = { 1, 2, 4, 9, 17, 34, 69, 140 };
int M = 313;
int N = 101;
Encription file = new Encription(new KnapsackInterface(N, M, privateKey));
*/

Encryption file = new Encryption(new RC6CRTInterface("plaky"));
file.EncryptBMPFile("bmp_24.bmp", "bmp_24_enc.bmp");
file.DecryptBMPFile("bmp_24_enc.bmp", "bmp_24_dec.bmp");

/*
*/
/*
//RC6 CRT/Obican
RC6CRT rc = new RC6CRT("Pralinajemalamaca");

byte[] bytes = { 1, 2, 3, 4, 3, 2, 2, 3, 2, 3, 4, 1, 2, 8, 15, 14, 1, 1 };

foreach (byte bajt in bytes)
    Console.Write(bajt + " ");
Console.WriteLine();

byte[] enc = rc.EncryptByteArrayCRT(bytes);


foreach (byte bajt in enc)
    Console.Write(bajt + " ");
Console.WriteLine();


byte[] dec = rc.DecryptByteArrayCRT(enc);


foreach (byte bajt in dec)
    Console.Write(bajt + " ");
Console.WriteLine();
*/
// Knapsack
/*
int[] privateKey = { 1, 2, 4, 9, 17, 34, 69, 140 };
int M = 313;
int N = 101;
KnapsackCypher kc = new KnapsackCypher(N, M, privateKey);

string zaba = "ja sam mala zaba";

byte[] zabab = new byte[zaba.Length * sizeof(char)];

Buffer.BlockCopy(zaba.ToCharArray(), 0, zabab, 0, zaba.Length * sizeof(char));

foreach (byte b in zabab)
    Console.Write(b + " ");
Console.WriteLine();

int[] enc = kc.Encrypt(zabab);

foreach (int i in enc)
    Console.Write(i + " ");
Console.WriteLine();

byte[] dec = kc.Decrypt(enc);

foreach (byte b in dec)
    Console.Write(b + " ");
Console.WriteLine();


char[] decchar = new char[dec.Length / 2];
Buffer.BlockCopy(dec, 0, decchar, 0, dec.Length);

Console.WriteLine(new String(decchar));

/**/