using System;
using CAContext.Domain.Enums;
using Shared.Entities;

namespace CAContext.Domain.Entities
{
    public class ContribuicaoArquivo : Entity
    {      
        public int ContribuicaoId { get; set; }

        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public string Descricao { get; set; }

        public EnumTipoArquivo TipoArquivo { get; set; }

        public string TempoDuracao { get; set; }
    }
}