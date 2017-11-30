using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KDM
{
    class MealyMachine: Machine, IMachine
    {
        public MealyMachine(Machine machine) : base(machine.inputAlphabet,machine.outputAlphabet,machine.inputMatrix,machine.outputMatrix)
        {
            CheckFormat();
        }
        public MealyMachine(string fileName) : base(fileName)
        {
            CheckFormat();
        }

        private void CheckFormat()
        {
            if (inputAlphabet.Count != inputMatrix[0].Count || inputAlphabet.Count != outputMatrix[0].Count)
            {
                throw new Exception("Wrong machine format");
            }
        }

        public string Run(string[] inputString)
        {
            if (!AreValid(inputString))
                throw new Exception("String is not valid, because it doesn't belong to alphabet");
            string output = "";
            for (int i = 0; i < inputString.Length; i++)
            {
                int index = inputAlphabet.FindIndex(x => x == inputString[i]);
                string currentOutput = outputAlphabet[outputMatrix[currentState][index]];
                output += currentOutput + " ";
                int nextState = inputMatrix[currentState][index];
                Console.Write($"\n(s{currentState},{inputAlphabet[index]}) -> s{nextState}, {currentOutput}");
                currentState = nextState;
            }
            return output;
        }

    }
}
