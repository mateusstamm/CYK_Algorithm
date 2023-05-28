using System;
using System.Collections.Generic;
using System.IO;

class CYKAlgorithm
{
    // Dicionário para armazenar a gramática
    static Dictionary<string, List<string>> grammar = new Dictionary<string, List<string>>();

    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Uso: dotnet run <arquivo_gramatica> <cadeia>");
            return;
        }

        string grammarFile = args[0];
        string inputString = args[1];

        // Carrega a gramática a partir do arquivo
        LoadGrammar(grammarFile);

        // Executa o algoritmo CYK na cadeia de entrada
        bool result = CYK(inputString);
        Console.WriteLine("Resultado: " + result);
    }

    static void LoadGrammar(string fileName)
    {
        try
        {
            // Lê as linhas do arquivo de gramática
            string[] lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                // Divide a linha em parte esquerda (não terminal) e produções
                string[] rule = line.Split(' ');
                string leftSide = rule[0].Trim();
                string[] productions = rule[1].Trim().Split('|');

                if (grammar.ContainsKey(leftSide))
                {
                    // Se a parte esquerda já está no dicionário, adiciona as produções a ela
                    grammar[leftSide].AddRange(productions);
                }
                else
                {
                    // Se a parte esquerda não está no dicionário, cria uma nova entrada para ela
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
                    // Verifica se a produção é um terminal e coincide com o caractere de entrada
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
                    // Cria uma lista com os não terminais da gramática
                    List<string> nonTerminals = new List<string>(grammar.Keys);
                    for (int p = 0; p < nonTerminals.Count; p++)
                    {
                        string nonTerminal = nonTerminals[p];
                        List<string> productions = grammar[nonTerminal];

                        foreach (string production in productions)
                        {
                            if (production.Length == 2)
                            {
                                char leftSymbol = production[0];
                                char rightSymbol = production[1];

                                // Verifica se a tabela CYK contém os valores necessários para essa produção
                                if (table[i, k] && table[i + k + 1, j - k - 1])
                                {
                                    // Verifica se os símbolos esquerdo e direito estão presentes na gramática
                                    if (grammar.ContainsKey(leftSymbol.ToString()) &&
                                        grammar.ContainsKey(rightSymbol.ToString()))
                                    {
                                        List<string> leftProductions = grammar[leftSymbol.ToString()];
                                        List<string> rightProductions = grammar[rightSymbol.ToString()];

                                        foreach (string leftProduction in leftProductions)
                                        {
                                            foreach (string rightProduction in rightProductions)
                                            {
                                                // Verifica se as produções da esquerda e da direita correspondem aos caracteres de entrada
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