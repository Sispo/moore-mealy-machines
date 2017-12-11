using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Machines
{
    public class MachineFileReader
    {
        public static Machine ReadMachineFromFile(string fileName)
        {
            var machine = new Machine();

            using (StreamReader sr = new StreamReader(fileName))
            {
                machine.inputAlphabet.AddRange(sr.ReadLine().Split(' '));
                machine.outputAlphabet.AddRange(sr.ReadLine().Split(' '));

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
                    }
                    else if (previousInputCount == 0)
                    {
                        previousInputCount = count;
                    }

                    machine.inputMatrix.Add(buffer);

                    buffer = new List<int>();
                    count = 0;

                    foreach (string s in str[1].Split(' '))
                    {
                        count++;
                        buffer.Add(machine.outputAlphabet.FindIndex(x => x == s));
                    }

                    if (previousOutputCount != 0 && previousOutputCount != count)
                    {
                        throw new Exception("Wrong machine format");
                    }
                    else if (previousOutputCount == 0)
                    {
                        previousOutputCount = count;
                    }

                    machine.outputMatrix.Add(buffer);
                }
            }

            return machine;
        }
    }
}
