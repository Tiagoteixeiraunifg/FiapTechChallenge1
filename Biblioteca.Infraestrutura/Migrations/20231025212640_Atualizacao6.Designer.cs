﻿// <auto-generated />
using System;
using Biblioteca.Infraestrutura.Dados.Contextos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Biblioteca.Infraestrutura.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231025212640_Atualizacao6")]
    partial class Atualizacao6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.Alunos.Aluno", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("Codigo")
                        .HasMaxLength(100)
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataCriacao");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Nome")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Telefone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Alunos", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.Autores.Autor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("Codigo")
                        .HasMaxLength(100)
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataCriacao");

                    b.Property<string>("Nome")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Telefone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Autores", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.Editoras.Editora", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cidade")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Cnpj")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<Guid>("Codigo")
                        .HasMaxLength(100)
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataCriacao");

                    b.Property<string>("Nome")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Editoras", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.FichaEmprestimos.FichaEmprestimoAluno", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AlunoId")
                        .HasColumnType("int");

                    b.Property<Guid>("Codigo")
                        .HasMaxLength(100)
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataCriacao");

                    b.Property<DateTime>("DataEmprestimo")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataEmprestimo");

                    b.Property<DateTime>("DataEntregaEmprestimo")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataVencimentoEmprestimo");

                    b.Property<string>("Observacoes")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int>("StatusEmprestimo")
                        .HasColumnType("INT");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasIndex("AlunoId")
                        .IsUnique();

                    b.HasIndex("UsuarioId")
                        .IsUnique();

                    b.ToTable("FichaEmprestimoAlunos", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.FichaEmprestimos.FichaEmprestimoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("Codigo")
                        .HasMaxLength(100)
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataStatusItem")
                        .HasColumnType("datetime2");

                    b.Property<int>("FichaEmprestimoAlunoId")
                        .HasColumnType("int");

                    b.Property<int>("LivroId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantidade")
                        .HasPrecision(14, 2)
                        .HasColumnType("decimal(14,2)");

                    b.Property<int>("StatusItem")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FichaEmprestimoAlunoId");

                    b.HasIndex("LivroId")
                        .IsUnique();

                    b.ToTable("FichaEmprestimoItens", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.Livros.Livro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("Codigo")
                        .HasMaxLength(100)
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataAtualizacao");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataCriacao");

                    b.Property<int>("EditoraId")
                        .HasColumnType("int");

                    b.Property<decimal>("QuantidadeEstoque")
                        .HasPrecision(14, 2)
                        .HasColumnType("decimal(14,2)");

                    b.Property<int>("Status")
                        .HasColumnType("INT");

                    b.Property<string>("SubTitulo")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Titulo")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("EditoraId")
                        .IsUnique();

                    b.ToTable("Livros", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.Livros.LivroAutores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AutorId")
                        .HasColumnType("int");

                    b.Property<Guid>("Codigo")
                        .HasMaxLength(100)
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LivroId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.HasIndex("LivroId");

                    b.ToTable("LivroAutores", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.Usuarios.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Nome")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<int>("Permissao")
                        .HasColumnType("int");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.FichaEmprestimos.FichaEmprestimoAluno", b =>
                {
                    b.HasOne("Biblioteca.Negocio.Entidades.Alunos.Aluno", "Aluno")
                        .WithOne()
                        .HasForeignKey("Biblioteca.Negocio.Entidades.FichaEmprestimos.FichaEmprestimoAluno", "AlunoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteca.Negocio.Entidades.Usuarios.Usuario", "Usuario")
                        .WithOne()
                        .HasForeignKey("Biblioteca.Negocio.Entidades.FichaEmprestimos.FichaEmprestimoAluno", "UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.FichaEmprestimos.FichaEmprestimoItem", b =>
                {
                    b.HasOne("Biblioteca.Negocio.Entidades.FichaEmprestimos.FichaEmprestimoAluno", "FichaEmprestimoAluno")
                        .WithMany("FichaEmprestimoItens")
                        .HasForeignKey("FichaEmprestimoAlunoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteca.Negocio.Entidades.Livros.Livro", "Livro")
                        .WithOne()
                        .HasForeignKey("Biblioteca.Negocio.Entidades.FichaEmprestimos.FichaEmprestimoItem", "LivroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FichaEmprestimoAluno");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.Livros.Livro", b =>
                {
                    b.HasOne("Biblioteca.Negocio.Entidades.Editoras.Editora", "Editora")
                        .WithOne()
                        .HasForeignKey("Biblioteca.Negocio.Entidades.Livros.Livro", "EditoraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Editora");
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.Livros.LivroAutores", b =>
                {
                    b.HasOne("Biblioteca.Negocio.Entidades.Autores.Autor", "Autor")
                        .WithMany()
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteca.Negocio.Entidades.Livros.Livro", "Livro")
                        .WithMany("Autores")
                        .HasForeignKey("LivroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.FichaEmprestimos.FichaEmprestimoAluno", b =>
                {
                    b.Navigation("FichaEmprestimoItens");
                });

            modelBuilder.Entity("Biblioteca.Negocio.Entidades.Livros.Livro", b =>
                {
                    b.Navigation("Autores");
                });
#pragma warning restore 612, 618
        }
    }
}
