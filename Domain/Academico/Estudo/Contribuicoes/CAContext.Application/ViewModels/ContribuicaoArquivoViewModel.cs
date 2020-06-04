using System;
using CAContext.Domain.Enums;
using Shared.Entities;

namespace CAContext.Application.ViewModels
{
    public class ContribuicaoArquivoViewModel : ViewModelValidation
    {
        public int Id { get; set; }
        
        public int ContribuicaoId { get; set; }

        public DateTime DataCriacao { get; set; }

        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public string Descricao { get; set; }

        public EnumTipoArquivo TipoArquivo { get; set; }

        public string TempoDuracao { get; set; }
    }
}