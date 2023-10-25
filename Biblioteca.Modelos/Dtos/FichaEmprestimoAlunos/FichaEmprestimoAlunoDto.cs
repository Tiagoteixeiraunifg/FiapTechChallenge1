using Biblioteca.Negocio.Entidades;
using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using Biblioteca.Negocio.Utilidades.Conversores;
using Biblioteca.Negocio.Utilidades.Extensoes;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Negocio.Validacoes.FichaEmprestimoAlunos;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Text.Json.Serialization;


namespace Biblioteca.Negocio.Dtos.FichaEmprestimoAlunos
{
    public class FichaEmprestimoAlunoDto : EntidadeBase
    {

        private Conversor<FichaEmprestimoAlunoDto, FichaEmprestimoAluno> _Conversor;

        private InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> _InconsistenciaDeValidacoes;

        public FichaEmprestimoAlunoDto()
        {
            _Conversor = new Conversor<FichaEmprestimoAlunoDto, FichaEmprestimoAluno>();
            FichaEmprestimoItens = new List<FichaEmprestimoItem>();
        }


        /// <summary>
        /// Código ùnico da operação em formato Guid
        /// </summary>
        public Guid Codigo { get; set; }

        /// <summary>
        /// O Usuário responsável pelo processo de emprestimo do Livro
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// O Aluno que está solicitando o/os livro(s) emprestado(s)
        /// </summary>
        public int AlunoId { get; set; }

        /// <summary>
        /// As observações diversas
        /// </summary>
        public string Observacoes { get; set; }

        /// <summary>
        /// O Status do Emprestimo de Forma Textual
        /// </summary>
        public string StatusEmprestimoDescricao { get; set; }


        /// <summary>
        /// Propriedade será carreda antes de converter o Dto para a Entidade de Destino
        /// </summary>
        [JsonIgnore]
        public FichaEmprestimoAlunoStatusEnum StatusEmprestimo { get; set; }


        /// <summary>
        /// Os Itens a serem emprestados para o Aluno (Lista de Ids de Livros)
        /// </summary>
        public IList<FichaEmprestimoAlunoItensDto> FichaEmprestimoItensDto { get; set; }



        [JsonIgnore]
        public IList<FichaEmprestimoItem> FichaEmprestimoItens { get; set; }


        /// <summary>
        /// Data do Emprestimo
        /// </summary>
        public DateTime DataEmprestimo { get; set; }

        /// <summary>
        /// Data máxima para final do emprestimo
        /// </summary>
        public DateTime DataVencimentoEmprestimo { get; set; }


        public bool ValideCadastroFicha() 
        {
            _InconsistenciaDeValidacoes = new FichaEmprestimoAlunoValidador().ValideCadastroFicha(ObtenhaEntidade());
            return _InconsistenciaDeValidacoes.EhValido();
        }

        public bool ValideFinalizacaoFicha() 
        {
            _InconsistenciaDeValidacoes = new FichaEmprestimoAlunoValidador().ValideFinalizacaoFicha(ObtenhaEntidade());
            return _InconsistenciaDeValidacoes.EhValido();
        }

        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> RetornarInconsistencias() => _InconsistenciaDeValidacoes;

        private void PreenchaStatusDoEmprestimoPelaDescricaoDoDto() 
        {
            if (StatusEmprestimoDescricao.PossuiValor()) 
            {
                foreach (FichaEmprestimoAlunoStatusEnum item in Enum.GetValues(typeof(FichaEmprestimoAlunoStatusEnum)))
                {
                    if (item.ObtenhaDescricao().Equals(StatusEmprestimoDescricao)) 
                    {
                        StatusEmprestimo = item;
                    }
                }
                
            }
        }

        private void PreenchaListaDeItensPeloDto() 
        {
            if (FichaEmprestimoItensDto.PossuiValor() && FichaEmprestimoItensDto.PossuiLinhas()) 
            {
                foreach (var item in FichaEmprestimoItensDto)
                {
                    FichaEmprestimoItens.Add(
                        new FichaEmprestimoItem() 
                        {
                            Codigo = Guid.NewGuid(),
                            LivroId = item.LivroId,
                            StatusItem = item.StatusItem,
                            DataStatusItem = DateTime.Now,
                            Quantidade = item.Quantidade,
                        }
                        
                        );
                }
            }
        }

        public FichaEmprestimoAluno ObtenhaEntidade() 
        {
            PreenchaStatusDoEmprestimoPelaDescricaoDoDto();
            PreenchaListaDeItensPeloDto();

            return _Conversor.ConvertaPara(this);
        }


    }
}
