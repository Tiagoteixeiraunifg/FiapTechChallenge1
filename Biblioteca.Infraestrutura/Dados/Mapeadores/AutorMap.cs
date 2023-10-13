using Biblioteca.Negocio.Entidades.Autores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Mapeadores
{
    public class AutorMap : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> m)
        {
            m.ToTable("Autores");
            m.HasKey(u => u.Id);
            m.Property(u => u.Id).UseIdentityColumn().HasColumnName("Id");
            m.Property(u => u.Codigo).HasMaxLength(100);
            m.Property(u => u.Nome).HasMaxLength(100);
            m.Property(u => u.Telefone).HasMaxLength(20);
            m.Property(u => u.DataCriacao).HasColumnName("DataCriacao");
            m.Property(u => u.DataAtualizacao).HasColumnName("DataAtualizacao");
        }
    }
}
