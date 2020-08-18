O projeto consiste em uma Poc construída para avaliar o método de implementação do full-text search utilizando o entity framework core 3.1 e a estratégia de "code first".

- A estrutura do projeto está em .NET CORE 3.1.
- Foi utilizado o SQL Server como banco de dados 

Para executar o projeto é necessário:
- Ter o SDK do dotnet instalado
- Instalar a ferramenta de linha de comando do ef, utilizando o comando "dotnet tool install -g dotnet-ef"
- Alterar o arquivo appsettings.json e inserir a string de conexão correta para o seu servidor SQL
- Executar o comando "dotnet ef database update" no terminal
- Executar o comando "dotnet run" no terminal (ou executar via Visual Studio ou Visual Code)

A Pasta de migrations contém um arquivo com o sufixo "InitialCreate.cs" e possuirá o passo a passo dos comandos que  foram executados no banco de dados após rodar o comando "dotnet ef database update".

Durante os estudos foi constatado de que o EF Core não possui um suporte nativo para criação de índices full-text via migrations, por este motivo foi necessário alterar o arquivo de migration criado inicialmente adicionando alguns comandos.

O Catálogo criado foi configurado para não diferenciar acentuação das palavras, com a opção "WITH ACCENT_SENSITIVITY = OFF"

Além da criação do índice também optei por inserir uma stoplist customizada para o índice. As stoplists são listas que contém uma série de palavras que são irrelevantes para uma consulta e que o SQL Server pode desconsiderá-las no momento das comparações, como por exemplo "e", "de", "a", "um" etc. A stoplist foi criada utilizando uma lista padrão fornecida pelo SQL Server, mas por esta ser customizadas podemos adicionar ou remover palavras que julgarmos necessário.


A consulta SQL, chamada de full-text query, Utilizando-se dos benefícios do full-text índex, pode ser feita com os predicados "CONTAINS" e "FREETEXT" ou as funções "CONTAINSTABLE" e "FREETEXTTABLE", porém o EF Core somente suporta "CONTAINS" e "FREETEXT" e esse suporte ainda é parcial.

Na consulta exemplificada no controller de produtos, o primeiro parâmetro da função "Contains" refere-se à coluna que queremos filtrar e o segundo é a condição de busca. No SQL Server poderíamos especificar uma consulta por mais de uma coluna, porém isso não é possível nativamente pelo EF, por este motivo foi necessário implementar a criação de uma EF.Function customizada para trabalhar com múltiplas colunas sem degradar a performance. 






 


