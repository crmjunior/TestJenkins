using MedCore_DataAccess.Entidades;
using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class ContribuicaoDTO
    {
        public int ContribuicaoId { get; set; }

        public int ClientId { get; set; }

        public int? MedGrupoID { get; set; }

        public string Descricao { get; set; }

        public string Estado { get; set; }

        public DateTime DataCriacao { get; set; }

        public string NomeAluno { get; set; }

        public int? TipoContribuicao { get; set; }

        public string SiglaAluno { get; set; }

        public string EspecialidadeAluno { get; set; }

        public string CodigoMarcacao { get; set; }

        public bool? Dono { get; set; }

        public string Data { get; set; }

        public string CursoAluno { get; set; }

        public bool? Editada { get; set; }

        public string Origem { get; set; }

        public string OrigemSubnivel { get; set; }

        public int? NumeroCapitulo { get; set; }

        public int? ApostilaId { get; set; }

        public bool? BitAprovacaoMedgrupo { get; set; }

        public string TrechoSelecionado { get; set; }

        public IList<ContribuicaoArquivo> Arquivos { get; set; }

        public bool? Arquivada { get; set; }

        public bool? AprovarMaisTarde { get; set; }

        public bool? Encaminhada { get; set; }

        public bool? IsVideo { get; set; }

        public bool? IsAudio { get; set; }

        public bool? IsImagem { get; set; }

        public bool? IsMedGrupo { get; set; }

        public IEnumerable<int> ProfessoresEncaminhados { get; set; }

        public IEnumerable<int> Interacoes { get; set; }

        public int? TipoCategoria { get; set; }

        public TipoOpcaoPrivacidade OpcaoPrivacidade { get; set; }
    }
}