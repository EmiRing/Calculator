using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class CalculateString
    {
        private string[] _operators = { "+", "-", "/", "*", "^" };
        private Func<double, double, double>[] _operations =
        {
            (t1, t2) => t1 + t2,
            (t1, t2) => t1 - t2,
            (t1, t2) => t1 / t2,
            (t1, t2) => t1 * t2,
            (t1, t2) => Math.Pow(t1, t2)
        };

        public double Evaluate(string equation)
        {
            List<string> terms = getTerms(equation);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int termIndex = 0;

            while (termIndex < terms.Count)
            {
                string term = terms[termIndex];
                
                if (term == "(")
                {
                    string subExpression = getSubExpression(terms, ref termIndex);
                    operandStack.Push(Evaluate(subExpression));
                    continue;
                }
                if (term == ")")
                {
                    throw new ArgumentException("Parenthesis mismatch: Ending parentheses found without a starting parenthesis.");
                }

                if (Array.IndexOf(_operators, term) >= 0)
                {
                    while (operatorStack.Count > 0 && Array.IndexOf(_operators, terms) < Array.IndexOf(_operators, operatorStack.Peek()))
                    {
                        DoCalculation(ref operatorStack, ref operandStack);
                        /*string op = operatorStack.Pop();
                        double term2 = operandStack.Pop();
                        double term1 = operandStack.Pop();
                        if (op == "^" && term1 < 0 && term2 % 1 != 0)
                        {
                            throw new ArgumentException("Imaginary numbers are not supported.");
                        }
                        operandStack.Push(_operations[Array.IndexOf(_operators, op)](term1, term2));*/
                    }
                    operatorStack.Push(term);
                }
                else
                {
                    operandStack.Push(double.Parse(term));
                }

                termIndex++;
            }

            while (operatorStack.Count > 0)
            {
                DoCalculation(ref operatorStack, ref operandStack);
                /*string op = operatorStack.Pop();
                double term2 = operandStack.Pop();
                double term1 = operandStack.Pop();
                if (op == "^" && term1 < 0 && term2 % 1 != 0)
                {
                    throw new ArgumentException("Imaginary numbers are not supported.");
                }
                operandStack.Push(_operations[Array.IndexOf(_operators, op)](term1, term2));*/
            }

            return operandStack.Pop();
        }
        
        private string getSubExpression(List<string> terms, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parCount = 1;
            index++;

            while (index < terms.Count && parCount > 0)
            {
                string term = terms[index];
                if (terms[index] == "(")
                {
                    parCount++;
                }
                    
                if (terms[index] == ")")
                {
                    parCount--;
                } 

                if (parCount > 0)
                {
                    subExpr.Append(term);
                }

                index++;
            }

            if (parCount > 0)
            {
                throw new ArgumentException("Parenthesis mismatch: No closing parenthesis.");
            }

            return subExpr.ToString();
        }

        private void DoCalculation(ref Stack<string> operatorStack, ref Stack<double> operandStack)
        {
            string op = operatorStack.Pop();
            double term2 = operandStack.Pop();
            double term1 = operandStack.Pop();
            if (op == "^" && term1 < 0 && term2 % 1 != 0)
            {
                throw new ArgumentException("Imaginary numbers are not supported.");
            }
            operandStack.Push(_operations[Array.IndexOf(_operators, op)](term1, term2));
        }
        private List<string> getTerms(string equation)
        {
            string operators = "()^*/+-";
            List<string> terms = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in equation.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if (sb.Length > 0)
                    {
                        terms.Add(sb.ToString());
                        sb.Length = 0;
                        terms.Add(c.ToString());
                    }
                    else if ( c == '-' && (terms.Count == 0 || operators.IndexOf(terms.Last<string>()) >= 2 || operators.IndexOf(terms.Last<string>()) == 0))
                    {
                        sb.Append(c.ToString());
                    }
                    else if (c == '+' && (terms.Count == 0 || operators.IndexOf(terms.Last<string>()) >= 2 || operators.IndexOf(terms.Last<string>()) == 0))
                    {
                        continue;
                    }
                    else if ((c == '*' || c == '/') && (operators.IndexOf(terms.Last<string>()) >= 2 || operators.IndexOf(terms.Last<string>()) == 0 || terms.Count == 0))
                    {

                        throw new ArgumentException("Invalid use of operators!");
                    }
                    else
                    {
                        terms.Add(c.ToString());
                    }
                }
                else
                {
                    sb.Append(c.ToString());
                }
            }

            if (sb.Length > 0)
            {
                terms.Add(sb.ToString());
            }

            return terms;

        }
    }
}
