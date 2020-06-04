using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [NotMapped]
    [DataContract(Name = "PermissaoAcessoItem", Namespace = "a")]
    public class PermissaoAcessoItem
    {
        [DataMember(Name = "IdOrdemDeVenda")]
        public int IdOrdemDeVenda { get; set; }
        [DataMember(Name = "Mensagem")]
        public string Mensagem { get; set; }
        [DataMember(Name = "PermiteAcesso")]
        public int PermiteAcesso { get; set; }
    }
}