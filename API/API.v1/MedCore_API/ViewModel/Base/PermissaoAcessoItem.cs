using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MedCoreAPI.ViewModel.Base
{
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

