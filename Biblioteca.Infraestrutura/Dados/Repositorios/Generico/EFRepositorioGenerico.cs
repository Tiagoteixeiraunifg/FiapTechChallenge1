using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Negocio.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Biblioteca.Infraestrutura.Dados.Repositorios.Generico
{
    public class EFRepositorioGenerico<T> : IRepositorioGenerico<T> where T : EntidadeBase
    {

        protected  ApplicationDbContext _contexto;

        protected DbSet<T> _DbSet;

        public EFRepositorioGenerico(ApplicationDbContext contexto)
        {
            _contexto = contexto;
            _DbSet = _contexto.Set<T>();
        }

        public T Altere(T Entidade)
        {
            _DbSet.Update(Entidade);
            _contexto.SaveChanges();
            return Entidade;
        }

        public T Cadastre(T Entidade)
        {
            
            _DbSet.Add(Entidade);
            _contexto.SaveChanges();
            return Entidade;
        }

        public bool Delete(int Id)
        {

            try
            {
                var resp = _DbSet.FirstOrDefault(x => x.Id == Id);
                _DbSet.Remove(resp);
                _contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public T ObtenhaPorId(int Id)
        {
            return _DbSet.FirstOrDefault(x => x.Id == Id);
        }

        public IList<T> ObtenhaTodos()
        {
            return _DbSet.ToList();
        }

    }
}
