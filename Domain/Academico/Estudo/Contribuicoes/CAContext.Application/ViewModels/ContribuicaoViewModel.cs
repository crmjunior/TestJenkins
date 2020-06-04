using System;
using System.Collections.Generic;
using CAContext.Domain.Enums;
using Shared.Entities;

namespace CAContext.Application.ViewModels
{
    public class ContribuicaoViewModel : ViewModelValidation
    {
        public int Id { get; set; }

        public int Matricula { get; set; }

        public int ApostilaId { get; set; }

        public DateTime DataCriacao { get; set; }

        public string Descricao { get; set; }

        public bool Ativa { get; set; }

        public string Origem { get; set; }

        public bool Editado { get; set ; }

        public int NumeroCapitulo { get; set; }

        public string TrechoSelecionado { get; set; }

        public string CodigoMarcacao { get; set; }

        public bool AprovadoMedgrupo { get; set; }

        public string OrigemSubnivel { get; set; }

        public int AcademicoId { get; set; }

        public EnumTipoCategoria TipoCategoria { get; set; }

        public EnumTipoContribuicao TipoContribuicao { get; set; }

        public string Estado { get; set; }

        public EnumOpcaoPrivacidade OpcaoPrivacidade { get; set; }

        public virtual ICollection<ContribuicaoArquivoViewModel> Arquivos { get; set; }
    }
}