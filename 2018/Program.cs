﻿using System.Collections.Generic;
using System.Threading.Tasks;
using _2018.Days;
using Common;

namespace _2018
{
    public class Program
    {
        public static async Task Main()
        {            
            var solver = new Solver(10000);
            await solver.Solve(
                //new Day01(), new Day02(), new Day03(), new Day04(),
                //new Day05(), new Day06(), new Day07(), new Day08(),
                //new Day09(), new Day10, new Day11(), new Day12(),
                new Day13()
                );
        }
    }
}
