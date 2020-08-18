using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using fulltext.Models;

namespace fulltext.Context.Configuration
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");
            
            builder.HasKey(x=>x.Id);

            builder.Property(x=>x.Id)
                .IsRequired();
            
            builder.Property(x=>x.Descricao)
                .HasColumnType("VARCHAR(300)");

            builder.Property(x=>x.Observacao)
                .HasColumnType("VARCHAR(300)");

            builder.HasData(
                new Produto { Id = 1, Descricao = "Neurologia", Observacao = "a especialidade de neurologia irá cuidar da saúde do seu cérebro." },
                new Produto { Id = 2, Descricao = "Clínica médica", Observacao = "o médico que irá cuidar de você e te fazer os encaminhamentos necessários para os especialistas." },
                new Produto { Id = 3, Descricao = "PCR Covid19", Observacao = "Exame PCR para constatar se você está infectado pelo coronavírus. " },
                new Produto { Id = 4, Descricao = "Checkup", Observacao = "Um pacote criado com o intuito de fornecer todos os exames necessários para se fazer um check-up em apenas uma compra." },
                new Produto { Id = 5, Descricao = "Hemograma com plaquetas", Observacao = "" },
                new Produto { Id = 6, Descricao = "Gestantes", Observacao = "Pacote exclusivo para ser comprado por gestantes" },
                new Produto { Id = 7, Descricao = "Ouro", Observacao = "Assinatura mais top da plataforma, do tipo ouro que fornece 1 consulta por mês gratuitamente." },
                new Produto { Id = 8, Descricao = "Telemedicina", Observacao = "Consulta com o médico através de videochamada." }
            );
                
        }
    }
}