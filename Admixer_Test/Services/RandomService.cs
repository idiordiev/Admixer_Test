using System;
using Admixer_Test.Interfaces;

namespace Admixer_Test.Services
{
    public class RandomService : IRandomService
    {
        private readonly Random _random;

        public RandomService()
        {
            _random = new Random();
        }

        public int GetRandomInt(int min, int max)
        {
            return _random.Next(min, max + 1);
        }
    }
}