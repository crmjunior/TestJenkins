using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ComboDeclaracao", Namespace = "a")]
    public class ComboDeclaracao
    {
        [DataMember(Name = "Ano")]
        public int Ano { get; set; }

        [DataMember(Name = "ListaProdutos")]
        public List<ListaProdutosDTO> ListaProdutos { get; set; }

        [DataMember(Name = "Email")]
        public string EmailAluno { get; set; }

    }
}