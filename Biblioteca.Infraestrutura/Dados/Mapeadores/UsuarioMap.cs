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
        public void Configure(EntityTypeBuilder<Usuario> mapeie)
        {
            mapeie.ToTable("Usuarios");
            mapeie.HasKey(u => u.Id);
            mapeie.Property(u => u.Id).HasColumnType("INT").UseIdentityColumn();
            mapeie.Property(u => u.Codigo).HasColumnType("VARCHAR(100)");
            mapeie.Property(u => u.Nome).HasColumnType("VARCHAR(100)");
            mapeie.Property(u => u.Email).IsRequired().HasColumnType("VARCHAR(100)");
            mapeie.Property(u => u.Senha).IsRequired().HasColumnType("VARCHAR(100)");
            mapeie.Property(u => u.Permissao).HasConversion<int>().IsRequired();
           
        }
    }
}
