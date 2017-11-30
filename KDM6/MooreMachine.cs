using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KDM
{
    public class MooreMachine: Machine
    {
        public MooreMachine(Machine machine) : base(machine.inputAlphabet,machine.outputAlphabet,machine.inputMatrix,machine.outputMatrix)
        {
            CheckFormat();
        }
        public MooreMachine(string fileName) : base(fileName)
        {
            CheckFormat();
        }

        private void CheckFormat()
        {
            if (inputAlphabet.Count != inputMatrix[0].Count || outputMatrix[0].Count != 1)
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
                int nextState = inputMatrix[currentState][index];
                string currentOutput = outputAlphabet[outputMatrix[nextState][0]];
                output += currentOutput + " ";
                Console.Write($"\n(s{currentState},{inputAlphabet[index]}) -> s{nextState}, {currentOutput}");
                currentState = nextState;
            }
            return output;
        }

    }
}
