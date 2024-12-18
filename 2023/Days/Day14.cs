﻿using Common;
using Common.Coordinates;

namespace _2023.Days
{
    public class Day14 : IDay
    {
        public async Task<(string, string, string)> Solve()
        {
            var input = await InputHandler.GetInputByLineAsync(nameof(Day14));
            var coordiantes = Utils.GenerateCoordinates(input);

            TiltRocks(coordiantes, 1);
            var southEdge = coordiantes.Max(x => x.Key.Y);
            var totalWeightPartOne = coordiantes.Where(x => x.Value.Equals('O')).Sum(x => CalculateWeight(x.Key, southEdge + 1));

            // Reset before part 2 by tilting south
            TiltRocks(coordiantes, 3);

            long numberOfCycles = 1000000000;

            var cache = new Dictionary<string, long>();
            var cyclesNeeded = numberOfCycles;

            for (long k = 1; k <= numberOfCycles; k++)
            {
                TiltRocks(coordiantes, 1);
                TiltRocks(coordiantes, 2);
                TiltRocks(coordiantes, 3);
                TiltRocks(coordiantes, 4);

                var cacheKey = new string(coordiantes.Select(x => x.Value).ToArray());
                if (!cache.TryAdd(cacheKey, k))
                {
                    var start = cache[cacheKey];
                    var end = k;
                    var cycleSize = end - start;

                    // fast forward
                    var rest = (numberOfCycles - start) % cycleSize;
                    k = numberOfCycles - rest;
                }
            }

            var totalWeightPartTwo = coordiantes.Where(x => x.Value.Equals('O')).Sum(x => CalculateWeight(x.Key, southEdge + 1));

            return (nameof(Day14), totalWeightPartOne.ToString(), totalWeightPartTwo.ToString());
        }

        private static int CalculateWeight(Coordinate c, int southEdge) => southEdge - c.Y;

        private static void TiltRocks(Dictionary<Coordinate, char> state, int direction)
        {
            var rocks = state.Where(x => x.Value.Equals('O'));
            switch (direction)
            {
                case 1:
                    rocks = rocks.OrderBy(x => x.Key.Y).ThenBy(x => x.Key.X).ToList();
                    break;
                case 2:
                    rocks = rocks.OrderBy(x => x.Key.X).ThenBy(x => x.Key.Y).ToList();
                    break;
                case 3:
                    rocks = rocks.OrderByDescending(x => x.Key.Y).ThenBy(x => x.Key.X).ToList();
                    break;
                case 4:
                    rocks = rocks.OrderByDescending(x => x.Key.X).ThenByDescending(x => x.Key.Y).ToList();
                    break;
            }

            foreach (var rock in rocks)
            {
                var next = rock.Key.GetNextCoordinate(direction);

                var prev = rock.Key;
                while (state.TryGetValue(next, out var nextValue))
                {
                    //Move rock in correct direction
                    if (nextValue == '.')
                    {
                        state[next] = 'O';
                        state[prev] = '.';
                        prev = next;
                        next = next.GetNextCoordinate(direction);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
