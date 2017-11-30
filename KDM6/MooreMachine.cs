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

        public MooreMachine(Machine machine)
        {
            if (machine.inputAlphabet.Count != machine.inputMatrix[0].Count || machine.outputMatrix[0].Count != 1)
            {
                throw new Exception("Wrong machine format");
            }

            inputAlphabet = machine.inputAlphabet;
            outputAlphabet = machine.outputAlphabet;
            inputMatrix = machine.inputMatrix;
            outputMatrix = machine.outputMatrix;
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
