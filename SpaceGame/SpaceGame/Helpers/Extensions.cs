using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Helpers
{
    public static class Extensions
    {

        public static int NextRandom(this RandomNumberGenerator generator)
        {
            Span<byte> rndBytes = stackalloc byte[4];

            generator.GetBytes(rndBytes);

            return BitConverter.ToInt32(rndBytes);
        }

        public static int NextRandom(this RandomNumberGenerator generator, int min, int max)
        {
            Span<byte> rndBytes = stackalloc byte[4];

            generator.GetBytes(rndBytes);

            return Math.Max(BitConverter.ToInt32(rndBytes) % max, min);
        }

    }
}
