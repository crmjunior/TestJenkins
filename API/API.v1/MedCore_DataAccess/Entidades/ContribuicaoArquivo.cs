using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ContribuicaoArquivo", Namespace = "a")]
    public class ContribuicaoArquivo
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "ContribuicaoID", EmitDefaultValue = false)]
        public int ContribuicaoID { get; set; }

        [DataMember(Name = "DataCriacao", EmitDefaultValue = false)]
        public DateTime DataCriacao { get; set; }

        [DataMember(Name = "Tipo", EmitDefaultValue = false)]
        public EnumTipoArquivoContribuicao Tipo { get; set; }

        [DataMember(Name = "Arquivo", EmitDefaultValue = false)]
        public string Arquivo { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "BitAtivo", EmitDefaultValue = false)]
        public bool BitAtivo { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "Time", EmitDefaultValue = false)]
        public string Time { get; set; }
    }
}