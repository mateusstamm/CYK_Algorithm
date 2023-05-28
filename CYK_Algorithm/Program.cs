using System;
using System.Collections.Generic;
using System.IO;

class CYKAlgorithm
{
    static Dictionary<string, List<string>> grammar = new Dictionary<string, List<string>>();

    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Uso: cyk <arquivo_gramatica> <cadeia>");
            return;
        }

        string grammarFile = args[0];
        string inputString = args[1];

        LoadGrammar(grammarFile);

        bool result = CYK(inputString);
        Console.WriteLine("Resultado: " + result);
    }

    static void LoadGrammar(string fileName)
    {
        try
        {
            string[] lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                string[] rule = line.Split(' ');
                string leftSide = rule[0].Trim();
                string[] productions = rule[1].Trim().Split('|');

                if (grammar.ContainsKey(leftSide))
                {
                    grammar[leftSide].AddRange(productions);
                }
                else
                {
                    grammar[leftSide] = new List<string>(productions);
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Arquivo de gramática não encontrado: " + fileName);
        }
    }

    static bool CYK(string inputString)
    {
        int n = inputString.Length;
        bool[,] table = new bool[n, n];

        // Preenchimento da tabela CYK
        for (int j = 0; j < n; j++)
        {
            foreach (KeyValuePair<string, List<string>> entry in grammar)
            {
                string nonTerminal = entry.Key;
                List<string> productions = entry.Value;

                foreach (string production in productions)
                {
                    if (production.Length == 1 && production[0] == inputString[j])
                    {
                        table[j, 0] = true;
                        break;
                    }
                }
            }
        }

        for (int j = 1; j < n; j++)
        {
            for (int i = 0; i < n - j; i++)
            {
                for (int k = 0; k < j; k++)
                {
                    for (int p = 0; p < grammar.Count; p++)
                    {
                        string nonTerminal = grammar.Keys.ElementAt(p);
                        List<string> productions = grammar[nonTerminal];

                        foreach (string production in productions)
                        {
                            if (production.Length == 2)
                            {
                                char leftSymbol = production[0];
                                char rightSymbol = production[1];

                                if (table[i, k] && table[i + k + 1, j - k - 1])
                                {
                                    if (grammar.ContainsKey(leftSymbol.ToString()) &&
                                        grammar.ContainsKey(rightSymbol.ToString()))
                                    {
                                        List<string> leftProductions = grammar[leftSymbol.ToString()];
                                        List<string> rightProductions = grammar[rightSymbol.ToString()];

                                        foreach (string leftProduction in leftProductions)
                                        {
                                            foreach (string rightProduction in rightProductions)
                                            {
                                                if (leftProduction.Length == 1 && rightProduction.Length == 1 &&
                                                    leftProduction[0] == inputString[i + k + 1] &&
                                                    rightProduction[0] == inputString[i])
                                                {
                                                    table[i, j] = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return table[0, n - 1];
    }
}
