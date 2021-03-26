using System;
using System.Collections.Generic;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            double result;
            bool active = true;
            List<string> test = new List<string>();

            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Welcome to this string calculator.");
            Console.WriteLine("Please write an equation containing regular operands +,-,*,/,^,(,)");
            Console.WriteLine("An example could be 5*3^(-4/5)+9/(1+5)");
            Console.WriteLine("To leave the program enter q to quit");
            Console.WriteLine("------------------------------------------------------------------");
            do
            {
                Console.Write("Enter your equation here: ");
                string equation = Console.ReadLine();
                CalculateString calcStr = new CalculateString();

                if (equation == "Q" || equation == "q")
                    break;

                try
                {
                    equation = equation.Replace(" ", string.Empty);
                    result = calcStr.Evaluate(equation);
                    Console.WriteLine($"The Answer to the equation {equation} is : {result}");
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    
                }

                foreach (string t in test)
                {
                    Console.WriteLine(t);
                }
            } while (active);
        }
    }
}
