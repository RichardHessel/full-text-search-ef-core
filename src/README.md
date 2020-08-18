O projeto consiste em uma Poc construída para avaliar o método de implementação do full-text search utilizando o entity framework core 3.1 e a estratégia de "code first".

- A estrutura do projeto está em .NET CORE 3.1.
- não possui solution pois foi criado utilizando exclusivamente a CLI do dotnet.
- Foi utilizado o SQL Server como banco de dados 

Para executar o projeto é necessário:
- Ter o SDK do dotnet instalado
- Instalar a ferramenta de linha de comando do ef, utilizando o comando "dotnet tool install -g dotnet-ef"
- Alterar o arquivo appsettings.json e inserir a string de conexão correta para o seu servidor SQL
- Executar o comando "dotnet ef database update" no terminal
- Executar o comando "dotnet run" no terminal

ps. Alguns passos não precisarão ser executados caso esteja utilizando o visual studio. 

A Pasta de migrations contém um arquivo com o sufixo "InitialCreate.cs" e possuirá o passo a passo dos comandos que  foram executados no banco de dados após rodar o comando "dotnet ef database update".

Durante os estudos foi constatado de que o EF Core não possui um suporte nativo para criação de índices full-text via migrations, por este motivo foi necessário alterar o arquivo de migration criado inicialmente adicionando alguns comandos:

<IMG  src="blob:https://dev.azure.com/398059c7-7625-4a4a-a395-129f14d4fd1c"  alt="image.png"/>

O Catálogo criado foi configurado para não diferenciar acentuação das palavras, com a opção "WITH ACCENT_SENSITIVITY = OFF"

Além da criação do índice também optei por inserir uma stoplist customizada para o índice. As stoplists são listas que contém uma série de palavras que são irrelevantes para uma consulta e que o SQL Server pode desconsiderá-las no momento das comparações, como por exemplo "e", "de", "a", "um" etc. A stoplist foi criada utilizando uma lista padrão fornecida pelo SQL Server, mas por esta ser customizadas podemos adicionar ou remover palavras que julgarmos necessário.


A consulta SQL, chamada de full-text query, Utilizando-se dos benefícios do full-text índex, pode ser feita com os predicados "CONTAINS" e "FREETEXT" ou as funções "CONTAINSTABLE" e "FREETEXTTABLE", porém o EF Core somente suporta "CONTAINS" e "FREETEXT" e esse suporte ainda é parcial.

<IMG  src="blob:https://dev.azure.com/199c4cdb-2695-4d2b-8e5d-5fbbdf673369"  alt="image.png"/>

No exemplo acima foi utilizado o recurso de funções do EF, no caso a função "CONTAINS".
Como pode ser observado o primeiro parâmetro da função refere-se à coluna que queremos filtrar e o segundo é a condição de busca. No SQL Server poderíamos especificar uma consulta por mais de uma coluna, porém isso não é possível nativamente pelo EF, seria necessário implementar a criação de uma função customizada ou utilizar RawSQL para trabalhar com múltiplas colunas sem degradar a performance. 
O mesmo serve para a função "FREETEXT".

Para o exemplo exibido foi utilizado o termo "FORMSOF(INFLECTIONAL, termo)". O uso de uma expressão INFLECTIONAL irá desconsiderar diferenças nas palavras como, singular x plural, tempo verbal, referência feminina ou masculina do termo, por exemplo:
- um registro na tabela que possui o dado "médico" seria retornado tanto se a consulta fosse feita utilizando a palavra chave "médico" ou "médica" como filtro. 
- um registro que possui o dado "cuidando" poderia ser encontrado se fosse utilizado a palavra chave "cuidar". 

Neste ponto, poderíamos utilizar este mesmo tipo de filtragem com o freetext, porém ele já faria de forma nativa, sem precisar especificar a condição "FORMSOF", porém a função não possui a possibilidade de trabalhar com prefixos, o que o CONTAINS consegue fazer de forma efetiva, como exemplificado no print, após o termo condicional **OR**. 

Nas implementações das funcionalidades devemos analisar algumas condições:
- Queremos fazer uma busca utilizando prefixo? ou seja, o usuário pode digitar parte da palavra e queremos retornar correspondências? **O Contains parece se encaixar melhor nesse cenário.**
- Queremos fazer buscas utilizando inflexão, por sinônimos ou receber uma sequencia de palavras (que podem ser consideradas como um OR na consulta) sem essa necessidade de filtrar utilizando prefixos? **O freetext parece a melhor solução pois faz tudo isso nativamente, sem precisarmos escrever qualquer tipo de código a mais.**





 


