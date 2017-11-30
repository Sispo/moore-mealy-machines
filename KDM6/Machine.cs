using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KDM
{
    public class Machine
    {
        public List<string> inputAlphabet;
        public List<string> outputAlphabet;
        public List<List<int>> inputMatrix;
        public List<List<int>> outputMatrix;

        public int currentState = 0;

        public void setCurrent(int state)
        {
            this.currentState = state;
        }

        public Machine()
        {
            initLists();
        }

        public Machine(List<string> inputAlpha, List<string> outputAlpha, List<List<int>> inputM, List<List<int>> outputM)
        {
            inputAlphabet = inputAlpha;
            outputAlphabet = outputAlpha;
            inputMatrix = inputM;
            outputMatrix = outputM;
        }

        public Machine(string fileName)
        {
            initLists();

            using (StreamReader sr = new StreamReader(fileName))
            {
                inputAlphabet.AddRange(sr.ReadLine().Split(' '));
                outputAlphabet.AddRange(sr.ReadLine().Split(' '));

                int previousInputCount = 0;
                int previousOutputCount = 0;

                while (!sr.EndOfStream)
                {
                    string[] str = sr.ReadLine().Split('!');
                    List<int> buffer = new List<int>();
                    int count = 0;
                    foreach (string s in str[0].Split(' '))
                    {
                        count++;
                        buffer.Add(Int32.Parse(s));
                    }

                    if (previousInputCount != 0 && previousInputCount != count)
                    {
                        throw new Exception("Wrong machine format");
                    } else if (previousInputCount == 0)
                    {
                        previousInputCount = count;
                    }

                    inputMatrix.Add(buffer);

                    buffer = new List<int>();
                    count = 0;

                    foreach (string s in str[1].Split(' '))
                    {
                        count++;
                        buffer.Add(outputAlphabet.FindIndex(x => x == s));
                    }

                    if (previousOutputCount != 0 && previousOutputCount != count)
                    {
                        throw new Exception("Wrong machine format");
                    }
                    else if (previousOutputCount == 0)
                    {
                        previousOutputCount = count;
                    }

                    outputMatrix.Add(buffer);
                }
            }
        }

        void initLists()
        {
            inputAlphabet = new List<string>();
            outputAlphabet = new List<string>();
            inputMatrix = new List<List<int>>();
            outputMatrix = new List<List<int>>();
        }

        public string[] Split(string input)
        {
            List<string> splitted = new List<string>();
            List<string> hypoStrings = new List<string>();
            string currentString = "";

            for(int i = 0; i < input.Length; i++)
            {

                char current = input[i];
                currentString += current;
                hypoStrings = inputAlphabet.FindAll(x => x.Contains(currentString));

                if (hypoStrings.Count == 0)
                {
                    throw new Exception("Invalid string.");
                } else
                {
                    if (hypoStrings.Any(x => x == currentString))
                    {
                        splitted.Add(currentString);
                        currentString = "";
                    }
                }

                
            }

            return splitted.ToArray();
        }

        public bool AreValid(string[] s)
        {
            foreach (string str in s)
                if (!inputAlphabet.Contains(str))
                    return false;
            return true;
        }

        public string Show()
        {
            Console.Write("Input alphabet: ");
            foreach (string str in inputAlphabet)
                Console.Write(str + " ");
            Console.WriteLine();
            Console.Write("Output alphabet: ");
            foreach (string str in outputAlphabet)
                Console.Write(str + " ");
            Console.WriteLine();
            Console.WriteLine();


            for (int i = 0; i < outputMatrix.Count; i++)
            {

                Console.Write($"S{i}  |  ");
                for (int j = 0; j < inputMatrix[i].Count; j++)
                    Console.Write($"S{inputMatrix[i][j]} ");
                Console.Write($"  |  ");
                for (int j = 0; j < outputMatrix[i].Count; j++)
                    Console.Write(outputAlphabet[outputMatrix[i][j]] + " ");
                Console.WriteLine();
            }
            return base.ToString();
        }

        public Machine Minimize(Machine machine)
        {
            List<List<List<int>>> equivalenceLevels = new List<List<List<int>>>();
            List<List<int>> classes = new List<List<int>>();

            classes.Add(new List<int>());
            classes[0].Add(0);

            for (int i = 1; i < outputMatrix.Count; i++)
            {
                bool foundClass = false;
                for (int j = 0; j < classes.Count; j++)
                {
                    if (outputMatrix[classes[j][0]].SequenceEqual(outputMatrix[i]))
                    {
                        classes[j].Add(i);
                        foundClass = true;
                        break;
                    }
                }

                if (!foundClass)
                {
                    List<int> newClass = new List<int>();
                    newClass.Add(i);
                    classes.Add(newClass);
                }
            }

            equivalenceLevels.Add(classes);
            ShowEquivalanceLevel(classes);
            if (CanContinueMinimizing(classes))
            {
                do
                {
                    List<List<int>> newClasses = GetCopy(classes);
                    
                    for (int i = 0; i < classes.Count; i++)
                    {

                        List<int> current = classes[i];

                        if (current.Count > 1)
                        {
                            List<int> newCl = new List<int>();
                            for (int j = 1; j < current.Count; j++)
                            {

                                for (int l = 0; l < inputAlphabet.Count; l++)
                                {
                                    
                                    if (GetClassIndexFor(inputMatrix[current[0]][l], classes) != GetClassIndexFor(inputMatrix[current[j]][l], classes))
                                    {
                                        newClasses[i].Remove(current[j]);
                                        
                                        newCl.Add(current[j]);
                                        
                                        break;
                                    }
                                }

                            }
                            if (newCl.Count > 0)
                            {
                                newClasses.Add(newCl);
                            }
                        }


                    }
                    classes = newClasses;
                    equivalenceLevels.Add(classes);
                    ShowEquivalanceLevel(classes);
                } while (!AreLevelsEqual(equivalenceLevels[equivalenceLevels.Count - 1],equivalenceLevels[equivalenceLevels.Count - 2]));
            }

            List<List<int>> newInputMatrix = new List<List<int>>();
            List<List<int>> newOutputMatrix = new List<List<int>>();

            for (int i = 0; i < classes.Count; i++)
            {
                List<int> inputMatrixRow = new List<int>();
                for (int l = 0; l < inputAlphabet.Count; l++)
                {
                    inputMatrixRow.Add(GetClassIndexFor(inputMatrix[classes[i][0]][l], classes));
                }
                newInputMatrix.Add(inputMatrixRow);
                newOutputMatrix.Add(outputMatrix[classes[i][0]]);
            }


            return new Machine(inputAlphabet, outputAlphabet, newInputMatrix, newOutputMatrix);
        }

        public List<List<int>> GetCopy(List<List<int>> cl)
        {
            List<List<int>> newCl = new List<List<int>>();
            for(int i = 0; i < cl.Count; i++)
            {
                newCl.Add(new List<int>());
                for(int j = 0; j < cl[i].Count; j++)
                {
                    newCl[i].Add(cl[i][j]);
                }
            }
            return newCl;
        }
        public bool AreLevelsEqual(List<List<int>> lhs, List<List<int>> rhs)
        {
            if (lhs.Count == rhs.Count)
            {

                for(int i = 0; i < lhs.Count; i++)
                {
                    if (!lhs[i].SequenceEqual(rhs[i]))
                    {
                        return false;
                    }
                }

            } else
            {
                return false;
            }
            return true;
        }

        public void ShowEquivalanceLevel(List<List<int>> level)
        {
            string output = "\n";
            for(int i = 0; i < level.Count; i++)
            {
                string str = "{";
                List<int> classEq = level[i];

                for (int j = 0; j < classEq.Count; j++)
                {
                    int state = classEq[j];
                    str += state;
                    if (j != classEq.Count - 1)
                    {
                        str += ", ";
                    }
                }
                str += "}";
                if(i != level.Count - 1)
                {
                    str += ", ";
                }
                output += str;
            }
            Console.WriteLine(output);
        }

        public bool CanContinueMinimizing(List<List<int>> classes)
        {
            for (int i = 0; i < classes.Count; i++)
            {
                if (!(classes[i].Count == 1))
                {
                    return true;
                }
            }
            return false;
        }

        public int GetClassIndexFor(int state, List<List<int>> classes)
        {
            for (int i = 0; i < classes.Count; i++)
            {
                if (classes[i].Contains(state))
                {
                    return i;
                }
            }
            throw new Exception("Invalid classes.");
        }
    }
}
