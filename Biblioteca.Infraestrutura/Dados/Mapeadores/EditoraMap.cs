using Biblioteca.Negocio.Entidades.Editoras;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Mapeadores
{
    public class EditoraMap : IEntityTypeConfiguration<Editora>
    {
        public void Configure(EntityTypeBuilder<Editora> m)
        {
            m.ToTable("Editoras");
            m.HasKey(u => u.Id);
            m.Property<int>(u => u.Id).UseIdentityColumn().HasColumnName("Id");
            m.Property(u => u.Codigo).HasMaxLength(100);
            m.Property(u => u.Nome).HasMaxLength(100);
            m.Property(u => u.Cnpj).HasMaxLength(25);
            m.Property(u => u.Cidade).HasMaxLength(100);
            m.Property(u => u.DataCriacao).HasColumnName("DataCriacao");
            m.Property(u => u.DataAtualizacao).HasColumnName("DataAtualizacao");
        }
    }
}
