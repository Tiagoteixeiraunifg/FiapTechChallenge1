using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Entidades.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Mapeadores
{
    public class LivroAutoresMap : IEntityTypeConfiguration<LivroAutores>
    {
        public void Configure(EntityTypeBuilder<LivroAutores> m)
        {
            m.ToTable("LivroAutores");
            m.HasKey(u => u.Id);
            m.Property(u => u.Id).UseIdentityColumn().HasColumnName("Id");
            m.Property(u => u.Codigo).HasMaxLength(100);

            m.HasOne(x => x.Livro).WithMany(x => x.Autores).HasForeignKey(x => x.LivroId);

        }
    }
}
