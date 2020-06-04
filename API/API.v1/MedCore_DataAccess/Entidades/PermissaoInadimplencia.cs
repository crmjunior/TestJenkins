using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "PermissaoInadimplencia", Namespace = "a")]
    public class PermissaoInadimplencia
    {
        [DataMember(Name = "PermiteAcesso")]
        public int PermiteAcesso { get; set; }

        [DataMember(Name = "Mensagem")]
        public string Mensagem { get; set; }

        [DataMember(Name = "IdOrdemDeVenda")]
        public int IdOrdemDeVenda { get; set; }

        [DataMember(Name = "LstIdOrdemDeVenda")]
        public List<int> LstIdOrdemDeVenda { get; set; }

        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "IdAplicacao", EmitDefaultValue = false)]
        public int IdAplicacao { get; set; }

        [DataMember(Name = "LstOrdemVendaMsg")]
        public List<PermissaoAcessoItem> LstOrdemVendaMsg { get; set; }

        public enum StatusAcesso
        {
            Negado = 0,
            Permitido = 1,
            ExAluno = 2
        }

    }
}