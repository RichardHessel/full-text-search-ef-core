using Microsoft.EntityFrameworkCore.Migrations;

namespace fulltext.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "VARCHAR(300)", nullable: true),
                    Observacao = table.Column<string>(type: "VARCHAR(300)", nullable: true) 
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            // Cria o catálogo, que é um agrupador lógico de índices, para o banco de dados
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT CATALOG Vitrine_Catalog WITH ACCENT_SENSITIVITY = OFF;", 
                suppressTransaction: true
            );

            // Cria uma lista de stopwords (palavras que não devem ser consideradas pelo SQL nas comparações) 
            // baseado em uma lista padrão de sistema, fornecida pelo SQL SERVER
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT STOPLIST custom_stoplist FROM SYSTEM STOPLIST;",
                suppressTransaction: true
            );

            // Cria o índice fulltext, especificando quais colunas serão indexadas e em qual linguagem os textos
            // serão escritos (no caso o código 1046 é o LCID do português - brasil) e indica que será utilizada a stoplist
            // customizada criada anteriormente
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON DB_Catalogo.dbo.Produtos " +
                    " ( " +
                        "Descricao " +
                        "LANGUAGE 1046, " +
                        "Observacao " +
                        "LANGUAGE 1046 " +
                    ") " +
                "KEY INDEX PK_Produtos " + 
                "ON Vitrine_Catalog WITH STOPLIST = custom_stoplist",
                suppressTransaction: true
            );

            // faz um populate no banco de dados com alguns registros para testes.
            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Descricao", "Observacao" },
                values: new object[,]
                {
                    { 1, "Neurologia", "a especialidade de neurologia irá cuidar da saúde do seu cérebro." },
                    { 2, "Clínica médica", "o médico que irá cuidar de você e te fazer os encaminhamentos necessários para os especialistas." },
                    { 3, "PCR Covid19", "Exame PCR para constatar se você está infectado pelo coronavírus. " },
                    { 4, "Checkup", "Um pacote criado com o intuito de fornecer todos os exames necessários para se fazer um check-up em apenas uma compra." },
                    { 5, "Hemograma com plaquetas", "" },
                    { 6, "Gestantes", "Pacote exclusivo para ser comprado por gestantes" },
                    { 7, "Ouro", "Assinatura mais top da plataforma, do tipo ouro que fornece 1 consulta por mês gratuitamente." },
                    { 8, "Telemedicina", "Consulta com o médico através de videochamada." }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");
        }
    }
}
