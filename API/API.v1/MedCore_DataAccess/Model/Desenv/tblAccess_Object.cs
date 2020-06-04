using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Object
    {
        public tblAccess_Object()
        {
            tblAccess_MenuProduto = new HashSet<tblAccess_MenuProduto>();
            tblAccess_PermissionObject = new HashSet<tblAccess_PermissionObject>();
            tblAcess_Object_Validity = new HashSet<tblAcess_Object_Validity>();
            tblBanners = new HashSet<tblBanners>();
            tblCronogramaPrateleira = new HashSet<tblCronogramaPrateleira>();
        }

        public int intObjectId { get; set; }
        public int intObjectTypeId { get; set; }
        public string txtNome { get; set; }
        public int intOrdem { get; set; }
        public int intPaiId { get; set; }
        public int intEmployeeID { get; set; }
        public int intApplicationId { get; set; }
        public DateTime dteCriacao { get; set; }
        public DateTime? dteUltimaAlteracao { get; set; }
        public bool bitAtivo { get; set; }
        public bool bitAcessoEspecial { get; set; }
        public int? intObjectLogTipo { get; set; }
        public bool bitPermiteOffline { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblAccess_ObjectType intObjectType { get; set; }
        public virtual ICollection<tblAccess_MenuProduto> tblAccess_MenuProduto { get; set; }
        public virtual ICollection<tblAccess_PermissionObject> tblAccess_PermissionObject { get; set; }
        public virtual ICollection<tblAcess_Object_Validity> tblAcess_Object_Validity { get; set; }
        public virtual ICollection<tblBanners> tblBanners { get; set; }
        public virtual ICollection<tblCronogramaPrateleira> tblCronogramaPrateleira { get; set; }
    }
}
