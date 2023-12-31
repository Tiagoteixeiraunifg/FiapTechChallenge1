﻿using Biblioteca.Negocio.Entidades.Livros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.Autores
{
    public class Autor : EntidadeBase
    {
        public Guid Codigo { get; set; }
        
        public string Nome { get; set; }

        public string Telefone { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
