using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Menu", Namespace = "a")]
    public class Menu : AccessObject
    {
        [DataMember(Name = "IdPai", EmitDefaultValue = false)]
        public int IdPai { get; set; }

        [DataMember(Name = "IdAplicacao", EmitDefaultValue = false)]
        public int IdAplicacao { get; set; }

        [DataMember(Name = "Url")]
        public string Url { get; set; }

        [DataMember(Name = "Target")]
        public string Target { get; set; }

        [DataMember(Name = "IdMensagem")]
        public int IdMensagem { get; set; }

        [DataMember(Name = "SubMenu")]
        public List<Menu> SubMenu { get; set; }

        [DataMember(Name = "Ordem")]
        public int Ordem { get; set; }

        [DataMember(Name = "Autenticacao")]
        public int Autenticacao { get; set; }

        [DataMember(Name = "Novo")]
        public int Novo { get; set; }

        [DataMember(Name = "SubMenusIds", EmitDefaultValue = false)]
        public int[] SubMenusIds { get; set; }

        
        public string VersaoMinima { get; set; }

        [DataMember(Name = "ExternalPagesUrl")]
        public string ExternalPagesUrl { get; set; }

        [DataMember(Name = "PermitePesquisa")]
        public bool PermitePesquisa { get; set; }
    }
}