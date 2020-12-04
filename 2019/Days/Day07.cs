﻿using Common;
using Common.Days;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2019.Days
{
    public class Day07 : IDay
    {
        public async Task<(string, string)> Solve(string day)
        {
            var instructions = await InputHandler.GetInputByCommaSeparationAsync(day);
            var program = instructions.Select(int.Parse).ToList();

            int[] validNumbersOne = { 0, 1, 2, 3, 4 };
            int[] validNumbersTwo = { 5, 6, 7, 8, 9 };


            int[] testNumbers = { 9, 7, 8, 5, 6 };
            var testPerms = new List<IEnumerable<int>> { testNumbers };
            var testInstructions = new List<int> {
                3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,
                -5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,
                53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10
            };

            var result1 = GetMaxOutput(GetPermutations(validNumbersOne, 5).ToList(), program);
            
            var result2 = GetMaxOutputWithFeedbackLoop(GetPermutations(validNumbersTwo, 5).ToList(), program);
            //var result2 = GetMaxOutputWithFeedbackLoop(testPerms.ToList(), testInstructions);

            //var result2 = string.Empty;

            return (result1, result2);
        }

        private static string GetMaxOutputWithFeedbackLoop(List<IEnumerable<int>> combinations, List<int> program)
        {
            var maxOutput = 0;
            while (combinations.Count > 0)
            {
                var programOutput = 0;
                var setting = combinations.ElementAt(0);

                var computers = new List<IntCodeComputer>
                {
                    new IntCodeComputer(program),
                    new IntCodeComputer(program),
                    new IntCodeComputer(program),
                    new IntCodeComputer(program),
                    new IntCodeComputer(program)
                };
                
                // Start the 5 computers
                for (var j = 0; j < computers.Count; j++)
                {
                    computers[j].Input(setting.ElementAt(j), programOutput);
                    programOutput = computers[j].Run();
                }

                // Feedback loop
                while (computers[4].ComputerState != ComputerState.Stopped)
                {
                    for (var j = 0; j < computers.Count; j++)
                    {
                        computers[j].Input(programOutput);
                        programOutput = computers[j].Run();
                    }
                }

                if (maxOutput < programOutput)
                {
                    maxOutput = programOutput;
                }

                combinations.Remove(setting);
            }

            return maxOutput.ToString();
        }

        private static string GetMaxOutput(List<IEnumerable<int>> combinations, List<int> program)
        {
            var maxOutput = 0;
            while (combinations.Count > 0)
            {
                var programOutput = 0;
                var setting = combinations.ElementAt(0);

                for (var j = 0; j < 5; j++)
                {
                    var computer = new IntCodeComputer(program);
                    computer.Input(setting.ElementAt(j), programOutput);
                    programOutput = computer.Run();
                }

                if (maxOutput < programOutput)
                {
                    maxOutput = programOutput;
                }

                combinations.Remove(setting);
            }

            return maxOutput.ToString();
        }

        private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1).SelectMany(t => list.Where(o => !t.Contains(o)),
                (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}
