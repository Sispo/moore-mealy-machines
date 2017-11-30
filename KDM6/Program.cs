using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KDM
{
    class Program
    {
        enum Machines { MealyMachineExample = 1, MealyMachineExample2, MooreMachineExample, MealyMachineMinimizationExample, MooreMachineMinimizationExample };
        static void Main(string[] args)
        {
            Console.WriteLine($"\n1) Defaults\n2) From file");

            bool defaults = Convert.ToInt32(Console.ReadLine()) == 1;

            if (defaults)
            {
                Console.WriteLine($"\n1) Mealy Machine Example 1\n2) Mealy Machine Example 2\n3) Moore Machine Example 1\n4) Mealy Machine Minimization Example\n5) Moore Machine Minimization Example");

                Machines selection = (Machines)Convert.ToInt32(Console.ReadLine());

                string fileName = "";
                bool shouldMinimize = false;
                bool isMachineMealy = true;

                switch (selection)
                {
                    case Machines.MealyMachineExample:
                        fileName = "../../../mealy.txt";
                        break;
                    case Machines.MealyMachineExample2:
                        fileName = "../../../mealy2.txt";
                        break;
                    case Machines.MooreMachineExample:
                        fileName = "../../../words.txt";
                        isMachineMealy = false;
                        break;
                    case Machines.MealyMachineMinimizationExample:
                        fileName = "../../../mealyMinimize.txt";
                        shouldMinimize = true;
                        break;
                    case Machines.MooreMachineMinimizationExample:
                        fileName = "../../../mooreMinimize.txt";
                        shouldMinimize = true;
                        isMachineMealy = false;
                        break;
                }

                Machine machine = isMachineMealy ? (Machine)new MealyMachine(fileName) : (Machine)new MooreMachine(fileName);

                machine.Show();

                if (shouldMinimize)
                {
                    Console.WriteLine($"\nMinimizing...\n");
                    machine.Minimize();
                    machine.Show();
                }

                if (isMachineMealy)
                {
                    MealyMachine mm = (MealyMachine)machine;
                    ShowResult(mm.Run(mm.Split(GetString())));
                }
                else
                {
                    MooreMachine mm = (MooreMachine)machine;
                    ShowResult(mm.Run(mm.Split(GetString())));
                }

            } else
            {
                Console.Write("\n1) Mealy Machine\n2) Moore Machine\n");
                bool isMachineMealy = Convert.ToInt32(Console.ReadLine()) == 1;
                Console.Write("\nEnter filename: ");
                string fileName = Console.ReadLine();

                Machine machine = new Machine(fileName);
                machine.Show();

                Console.Write("\nDo you want to minimize the machine ? (y/n): ");
                bool shouldMinimize = Console.ReadKey().KeyChar == 'y';

                if(shouldMinimize)
                {
                    machine.Minimize();
                    machine.Show();
                }

                if (isMachineMealy)
                {
                    MealyMachine mm = new MealyMachine(machine);
                    ShowResult(mm.Run(mm.Split(GetString())));
                }
                else
                {
                    MooreMachine mm = new MooreMachine(machine);
                    ShowResult(mm.Run(mm.Split(GetString())));
                }
            }
        }

        public static string GetString()
        {
            Console.Write("\nEnter string: ");
            return Console.ReadLine();
        }

        public static void ShowResult(string result)
        {
            Console.WriteLine("\nResult: " + result);
        }

    }
}
