using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace fulltext.Context
{
    public class CatalogoContext : DbContext
    {
        public CatalogoContext(DbContextOptions<CatalogoContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder
                .HasDbFunction(typeof(Funcoes)
                .GetMethod(nameof(Funcoes.ContainsIn)))
                .HasTranslation(args =>
                {
                    var arguments = args.ToList();

                    var a = (string)((SqlConstantExpression)arguments[0]).Value;
                    
                    arguments[0] = new SqlFragmentExpression(a);

                    return SqlFunctionExpression.Create("CONTAINS", arguments, typeof(bool), null);
                });
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }

    public class Funcoes
    {

        public static bool ContainsIn(string column, string expression)
            => false;
    }
}