using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Entidades.Livros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Mapeadores
{
    public class LivroMap : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> m)
        {
            m.ToTable("Livros");
            m.HasKey(u => u.Id);
            m.Property<int>(u => u.Id).UseIdentityColumn().HasColumnName("Id");
            m.Property(u => u.Codigo).HasMaxLength(100);
            m.Property(u => u.Titulo).HasMaxLength(100);
            m.Property(u => u.SubTitulo).HasMaxLength(100);
            m.Property(u => u.Status).HasColumnType("INT");
            m.Property(u => u.QuantidadeEstoque).HasPrecision(14,2);
            m.Property(u => u.DataCriacao).HasColumnName("DataCriacao");
            m.Property(u => u.DataAtualizacao).HasColumnName("DataAtualizacao");
            m.HasIndex(u => u.EditoraId).IsClustered(false).IsUnique(false);
            
            m.HasOne(u => u.Editora).WithOne().HasForeignKey<Livro>(x => x.EditoraId);
            
            m.HasMany(u => u.Autores).WithOne(x => x.Livro).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
