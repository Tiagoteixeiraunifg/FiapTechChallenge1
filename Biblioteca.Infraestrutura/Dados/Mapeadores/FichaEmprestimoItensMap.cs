using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
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
    public class FichaEmprestimoItensMap : IEntityTypeConfiguration<FichaEmprestimoItem>
    {
        public void Configure(EntityTypeBuilder<FichaEmprestimoItem> m)
        {
            m.ToTable("FichaEmprestimoItens");
            m.HasKey(u => u.Id);
            m.Property(u => u.Id).UseIdentityColumn().HasColumnName("Id");
            m.Property(u => u.Codigo).HasMaxLength(100);

            m.HasOne(x => x.Livro).WithOne().HasForeignKey<FichaEmprestimoItem>(x => x.LivroId);

        }
    }
}
