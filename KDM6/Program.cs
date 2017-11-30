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
        enum Machines { M1 = 1, M2, MealyMinimize, MoorMinimize, Finish, MealyMinimize2, MealyMinimize3, Lab11 };
        static void Main(string[] args)
        {
            while(true)
            {
                string result = "";

                Console.WriteLine($"\n1) M1\n2) M2\n3) mealyMinimize\n4) moorMinimize\n5) Finish\n6) 2\n7) Meely Min 3\n8) Lab 11");

                Machines selection = (Machines)Convert.ToInt32(Console.ReadLine());

                switch (selection)
                {
                    case Machines.M1:

                        MealyMachine m1 = new MealyMachine(new Machine("m1.txt"));
                        m1.Show();

                        result = m1.Run(m1.Split(GetString()));

                        break;
                    case Machines.M2:

                        MealyMachine m2 = new MealyMachine(new Machine("m2.txt"));
                        m2.Show();

                        result = m2.Run(m2.Split(GetString()));

                        break;
                    case Machines.MealyMinimize:

                        MealyMachine mealyToMin = new MealyMachine(new Machine("mMinimize.txt"));
                        mealyToMin.Show();
                        MealyMachine newm = new MealyMachine(mealyToMin.Minimize(mealyToMin));
                        newm.Show();

                        result = newm.Run(newm.Split(GetString()));

                        break;
                    case Machines.MoorMinimize:

                        MooreMachine machineMoorToMin = new MooreMachine(new Machine("moorMinimize.txt"));
                        machineMoorToMin.Show();
                        MooreMachine newmoor = new MooreMachine(machineMoorToMin.Minimize(machineMoorToMin));
                        newmoor.Show();

                        result = newmoor.Run(newmoor.Split(GetString()));

                        break;
                    case Machines.Finish:
                        return;
                    case Machines.MealyMinimize2:

                        MealyMachine mealy2ToMin = new MealyMachine(new Machine("m2Minimize.txt"));
                        mealy2ToMin.Show();
                        result = mealy2ToMin.Run(mealy2ToMin.Split(GetString()));
                        Console.WriteLine("\nResult: " + result);
                        MealyMachine newm2 = new MealyMachine(mealy2ToMin.Minimize(mealy2ToMin));
                        newm2.Show();

                        result = newm2.Run(newm2.Split(GetString()));
                        break;
                    case Machines.MealyMinimize3:

                        MealyMachine mealy3ToMin = new MealyMachine(new Machine("m3Minimize.txt"));
                        mealy3ToMin.Show();
                        result = mealy3ToMin.Run(mealy3ToMin.Split(GetString()));
                        Console.WriteLine("\nResult: " + result);
                        MealyMachine newm3 = new MealyMachine(mealy3ToMin.Minimize(mealy3ToMin));
                        newm3.Show();

                        result = newm3.Run(newm3.Split(GetString()));
                        break;
                    case Machines.Lab11:

                        Console.OutputEncoding = System.Text.Encoding.Unicode;
                        Console.InputEncoding = System.Text.Encoding.Unicode;

                        MooreMachine lab11moore = new MooreMachine(new Machine("lab11.txt"));
                        lab11moore.Show();
                        MooreMachine minimizedLab11moore = new MooreMachine(lab11moore.Minimize(lab11moore));
                        minimizedLab11moore.Show();

                        result = minimizedLab11moore.Run(GetString().Split(' '));

                        //Console.OutputEncoding = System.Text.Encoding.Default;
                        //Console.InputEncoding = System.Text.Encoding.Default;

                        break;
                }

                Console.WriteLine("\nResult: " + result);
            }
        }

        public static string GetString()
        {
            Console.Write("\nEnter string: ");
            return Console.ReadLine();
        }

    }
}
