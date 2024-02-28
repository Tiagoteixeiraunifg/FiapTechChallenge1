using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Infraestrutura.Logs.Fabricas;
using Biblioteca.Negocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Biblioteca.Infraestrutura.Dados.Repositorios.Generico
{
    public class EFRepositorioGenerico<T> : IRepositorioGenerico<T> where T : EntidadeBase
    {
        private readonly ILogger _logger;

        protected  ApplicationDbContext _contexto;

        protected DbSet<T> _DbSet;

        public EFRepositorioGenerico(ApplicationDbContext contexto)
        {
            _contexto = contexto;
            _DbSet = _contexto.Set<T>();
            _logger = new LogCustomizado("LogRepositorioGenerico", new FabricaDeLogs());
        }

        public EFRepositorioGenerico(ILogger<EFRepositorioGenerico<T>> logger)
        {
            _logger = logger;
        }

        public T Altere(T Entidade)
        {
            var transaction = _contexto.Database.BeginTransaction();

            try
            {
                _DbSet.Update(Entidade);
                _contexto.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return Entidade;
        }

        public T Cadastre(T Entidade)
        {
            var transaction = _contexto.Database.BeginTransaction();

            try
            {
                _DbSet.Add(Entidade);
                _contexto.SaveChanges();
                transaction.Commit();
                _logger.LogInformation("Fez o cadastro da Entidade");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao cadastrar, executando o RollBack");
                transaction.Rollback();
                throw;
            }

            return Entidade;
        }

        public bool Delete(int Id)
        {

            try
            {
                var resp = _DbSet.FirstOrDefault(x => x.Id == Id);
                _DbSet.Remove(resp);
                _contexto.SaveChanges();
                Dispose();

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public DbSet<T> ObtenhaDbSet() 
        {
            return _DbSet;
        }

        public void Dispose() {

            _contexto.Dispose();
            GC.SuppressFinalize(this);
        }

        public T ObtenhaPorId(int Id)
        {
            return _DbSet.AsNoTracking().FirstOrDefault(x => x.Id == Id);
        }

        public IList<T> ObtenhaTodos()
        {
            return _DbSet.ToList();
        }

    }
}
