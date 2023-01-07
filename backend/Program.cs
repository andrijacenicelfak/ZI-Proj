using Algorithms;
using KnapsackCipher = Algorithms.KnapsackCypher;
using Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

int[] privateKey = { 1, 2, 4, 9, 17, 34, 69, 140 };
int M = 313;
int N = 101;

KnapsackCipher knapsackCipher = new KnapsackCipher(N, M, privateKey);
int[] publicKey = knapsackCipher.publicKey;

app.MapGet("/", () => "Hello World!");

app.MapGet("/key", () => new { key = publicKey });

app.MapPost("/send", (KnapsackData kdata) =>
{
    byte[] dec = knapsackCipher.Decrypt(kdata.data);
    char[] carr = new char[dec.Length / 2];
    Buffer.BlockCopy(dec, 0, carr, 0, dec.Length);
    string s = new String(carr);
    Console.WriteLine(s);
});

app.Run();
