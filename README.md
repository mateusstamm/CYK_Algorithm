# **CYK Resulter - UTFPR - LFA 2023/1**

Projeto realizado como parte da disciplina de Linguagens Formais e Autômatos na UTFPR, ministrada pelo professor Evando Carlos Pessini. O objetivo do projeto é implementar um tradutor de cadeias utilizando o algoritmo CYK (Cocke-Younger-Kasami) para gramáticas na forma normal de Chomsky.

# Descrição do Projeto

O CYK Resulter é um programa que utiliza o algoritmo CYK para verificar se uma determinada cadeia de caracteres pode ser gerada por uma gramática na forma normal de Chomsky. O programa recebe como entrada um arquivo de gramática e uma cadeia, e retorna se a cadeia pode ser gerada pela gramática ou não.

# Funcionamento

- Clone o repositório ou faça o download do arquivo ZIP para o seu diretório local.
- Acesse a pasta raiz do projeto onde está localizado o arquivo .csproj.
- Execute o comando "dotnet run gramatica.txt [cadeia]" no terminal para compilar e executar o programa.
- ***Substitua gramatica.txt pelo caminho e nome do arquivo de gramática desejado (caso queira algum diferente do padrão), e [cadeia] pela cadeia que você deseja testar.


# Testes

Gramática:
- S -> AB|BC
- A -> BA|a
- B -> CC|b
- C -> AB|a

Cadeias / Resultado:
- abba  / True
- bccb  / False
- baaa  / True
- aabbb / True
- abc   / False

# Tecnologias utilizadas

- C# (linguagem de programação)
- .NET Core 7.0 (plataforma de desenvolvimento)