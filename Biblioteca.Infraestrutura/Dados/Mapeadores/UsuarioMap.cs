using Biblioteca.Negocio.Entidades.Usuarios;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Mapeadores
{

    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> m)
        {
            m.ToTable("Usuarios");
            m.HasKey(u => u.Id);
            m.Property(u => u.Id).HasColumnType("INT").UseIdentityColumn();
            m.Property(u => u.Codigo).HasColumnType("VARCHAR(100)");
            m.Property(u => u.Nome).HasColumnType("VARCHAR(100)");
            m.Property(u => u.Email).IsRequired().HasColumnType("VARCHAR(100)");
            m.Property(u => u.Senha).IsRequired().HasColumnType("VARCHAR(100)");
            m.Property(u => u.Permissao).HasConversion<int>().IsRequired();
        }
    }
}
