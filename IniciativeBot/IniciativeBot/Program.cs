
using IniciativeBot.Rolling;

RNG.SetSeed(15);

for (int i = 0; i < 20; i++)
{
    Console.Write(RNG.RollDice(20));
    Console.Write(", ");
}

RNG.SetSeed()