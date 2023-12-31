﻿using Biblioteca.Negocio.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces
{
    public interface IRepositorioGenerico<T> : IDisposable where T : EntidadeBase
    {
        IList<T> ObtenhaTodos();

        T ObtenhaPorId(int Id);

        T Cadastre(T Entidade);

        T Altere(T Entidade);

        bool Delete(int Id);

        DbSet<T> ObtenhaDbSet();
    }
}
