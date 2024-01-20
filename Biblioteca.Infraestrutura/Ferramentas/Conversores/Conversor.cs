using AutoMapper;
using Biblioteca.Infraestrutura.Logs.Fabricas;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Ferramentas.Conversores
{
    public class Conversor<TOrigem, TDestino>
    {

        private static MapperConfiguration _Config;

        private static IMapper _Mapper;

        private static LogCustomizado _Log = new LogCustomizado("LogErroConverssor", new FabricaDeLogs());

        public Conversor()
        {
            _Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TOrigem, TDestino>();
                cfg.AllowNullCollections = true;
            });

            _Mapper = _Config.CreateMapper();
        }


        /// <summary>
        /// Converte um Objeto em DTO ou DTO em Objeto, basta inverter a ordem.
        /// </summary>
        /// <param name="obj">O Objeto que será convertido para o Tipo informado na Origem</param>
        /// <returns>Retorna um objeto carregado em conformidade com o Tipo Informado</returns>
        public TDestino ConvertaPara(TOrigem obj)
        {


            _Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TOrigem, TDestino>();
                cfg.AllowNullCollections = true;
            });

            _Log.LogInformation("Criando o Mapeador de Objetos");

            try
            {

                _Mapper = _Config.CreateMapper();
                _Log.LogInformation("Mapeador de Objetos Criado");

            }
            catch (Exception ex)
            {
                _Log.LogError(ex, "Erro ao criar Mapeador de Objetos", new { });

            }


            return _Mapper.Map<TDestino>(obj);

        }
    }
}
