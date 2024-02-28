using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
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
    public class FichaEmprestimoAlunoMap : IEntityTypeConfiguration<FichaEmprestimoAluno>
    {
        public void Configure(EntityTypeBuilder<FichaEmprestimoAluno> m)
        {
            m.ToTable("FichaEmprestimoAlunos");
            m.HasKey(u => u.Id);
            m.Property(u => u.Id).UseIdentityColumn().HasColumnName("Id");
            m.Property(u => u.Codigo).HasMaxLength(100);
            m.Property(u => u.StatusEmprestimo).HasColumnType("INT");
            m.Property(u => u.Observacoes).HasMaxLength(4000);
            m.Property(u => u.DataEmprestimo).HasColumnName("DataEmprestimo");
            m.Property(u => u.DataEntregaEmprestimo).HasColumnName("DataVencimentoEmprestimo");
            m.Property(u => u.DataCriacao).HasColumnName("DataCriacao");
            m.Property(u => u.DataAtualizacao).HasColumnName("DataAtualizacao");
            m.HasIndex(u => u.AlunoId).IsClustered(false).IsUnique(false);
            m.HasIndex(u => u.UsuarioId).IsClustered(false).IsUnique(false);

            m.HasOne(x => x.Aluno).WithOne().HasForeignKey<FichaEmprestimoAluno>(x => x.AlunoId).OnDelete(DeleteBehavior.NoAction);

            m.HasOne(x => x.Usuario).WithOne().HasForeignKey<FichaEmprestimoAluno>(x => x.UsuarioId).OnDelete(DeleteBehavior.NoAction);

            m.HasMany(x => x.FichaEmprestimoItens).WithOne(x => x.FichaEmprestimoAluno).HasForeignKey(x => x.FichaEmprestimoAlunoId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
